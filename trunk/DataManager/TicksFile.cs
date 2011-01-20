using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;

using OpenWealth.Simple;

namespace OpenWealth.DataManager
{
    public class TicksFile
    {
        static ILog l = Core.GetLogger(typeof(TicksFile).FullName);
        ReaderWriterLock _lock = new System.Threading.ReaderWriterLock();

        public readonly ISymbol symbol;
        public readonly string fileName;

        // Дата начала и окончания периода, она не может менятся
        public readonly int startDateTime;
        public readonly int endDateTime;

        public bool Include(int dt)
        {
            return ((dt >= startDateTime) && (dt <= endDateTime));
        }

        public int Count { get; private set; }

#if FileTest
        internal List<IBar> ONLY4TEST_notGarbage = null;
#endif

        public TicksFile(string fileName, ISymbol symbol, int startDate, int endDate)
        {
            this.symbol = symbol;
            this.startDateTime = startDate;
            this.endDateTime = endDate;
            this.fileName = fileName;
            Count = 0;
        }

        public TicksFile(string fileName)
        {
            this.fileName = fileName;
            if (!File.Exists(fileName))
            {
                l.Error("Файл не существует" + fileName);
                return;
            }
            using (StreamReader sr = new StreamReader(fileName, new System.Text.UTF8Encoding()))
            {
                if ((sr.ReadLine() != CaptionLine1) || (sr.ReadLine() != CaptionLine2))
                {
                    l.Error("Не верный формат файла " + fileName);
                    return;
                }

                symbol = Core.Data.GetSymbol(sr.ReadLine());

                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                startDateTime = DateTime2Int.Int( DateTime.ParseExact(sr.ReadLine(), "yyyy.MM.dd", provider));
                endDateTime = DateTime2Int.Int( DateTime.ParseExact(sr.ReadLine(), "yyyy.MM.dd", provider).AddDays(1).AddSeconds(-1) );

                Count = int.Parse(sr.ReadLine());
            }
        }

        TicksFile(string fileName, string symbol, int startDate, int endDate, int count)
        {
            this.fileName = fileName;
            this.symbol = Core.Data.GetSymbol(symbol);
            this.startDateTime = startDate;
            this.endDateTime = endDate;
            this.Count = count;
        }

        public static TicksFile LoadFromAllTicksFileList(string dataDir, StreamReader sr)
        {
            string fileName = Path.Combine(dataDir, sr.ReadLine());
            if (!File.Exists(fileName))
            {
                l.Error("Файл не существует " + fileName);
                return null;
            }
            if ((sr.ReadLine() != CaptionLine1) || (sr.ReadLine() != CaptionLine2))
            {
                l.Error("Не верный формат файла " + fileName);
                return null;
            }

            string symbol = sr.ReadLine();

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime startDate = DateTime.ParseExact(sr.ReadLine(), "yyyy.MM.dd", provider);
            DateTime endDate = DateTime.ParseExact(sr.ReadLine(), "yyyy.MM.dd", provider).AddDays(1).AddSeconds(-1);

            int count = int.Parse(sr.ReadLine());

            return new TicksFile(fileName, symbol, DateTime2Int.Int( startDate), DateTime2Int.Int(endDate), count);
        }

        WeakReference _ticks = new WeakReference(null);
        internal IList<IBar> ticks
        {
            private get
            {
                List<IBar> result = null;
                _lock.AcquireReaderLock(10000);
                try
                {
                    if (_ticks == null)
                    {
                        l.Info("Попытка получить ticks, когда _ticks==null");
                        return null;
                    }
                    result = _ticks.Target as List<IBar>;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }

                if (result != null)
                    return result;

                _lock.AcquireWriterLock(10000);
                try
                {                    
                    result = _ticks.Target as List<IBar>;
                    if (result == null)
                    {
                        result = Load();
                        if (result.Count > 0)
                            lastBar = result[result.Count - 1];
                        else
                            lastBar = null;
                        _ticks = new WeakReference(result);
                    }
                }
                finally
                {
                    _lock.ReleaseWriterLock();
                }
                return result;
            }
            set
            {
                _lock.AcquireWriterLock(10000);
                try
                {
                    Count = value.Count;
                    if (Count > 0)
                        lastBar = value[Count - 1];
                    else
                        lastBar = null;
                    _ticks = new WeakReference(value);
                    changeFrom = 0;
                    TicksFileSaver.Add4Save(this, value);
                }
                finally
                {
                    _lock.ReleaseWriterLock();
                }
            }
        }

        public const string FileExt = @".ticks";
        const string CaptionLine1 = @"http://OpenWealth.ru/TickFileFormat.html";
        const string CaptionLine2 = @"ver1";
        const int CaptionLength = 250;

        internal byte[] fileCaption
        {
            get
            {
                string str = CaptionLine1 + "\r\n" + CaptionLine2 + "\r\n" +
                    symbol.ToString() + "\r\n" +
                    DateTime2Int.DateTime(startDateTime).ToString("yyyy.MM.dd") + "\r\n" +
                    DateTime2Int.DateTime(endDateTime).ToString("yyyy.MM.dd") + "\r\n"+
                    Count+"\r\n";
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] result = encoding.GetBytes(str);
                if (result.Length > CaptionLength)
                {
                    l.Fatal("Длинна заголовка файла получилась больше максимально возможной " + CaptionLength + " " + result);
                }
                return result;
            }
        }
        // Данная переменная указывает с какой записи значения изменились (нужно чтобы не переписывать весь список на каждый новый тик)
        internal int changeFrom = int.MaxValue;

        List<IBar> Load()
        {
            _lock.AcquireWriterLock(1000);
            try
            {
                if (!File.Exists(fileName))
                {
                    return new List<IBar>(10000);
                }
                List<IBar> data = null;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long length = fs.Length;
                    long position = CaptionLength;
                    if (length <= CaptionLength)
                        return new List<IBar>(10000);
                    if (DateTime2Int.Int(DateTime.Now) < endDateTime)
                        data = new List<IBar>((int)(2 * (length - position) / Tick.Size));
                    else
                        data = new List<IBar>((int)(1.1 * (length - position) / Tick.Size));

                    fs.Position = CaptionLength;
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        while (position < length)
                        {
                            data.Add(new Tick(br));
                            position += Tick.Size;
                        }
                    }
                }
                Count = data.Count;
                changeFrom = int.MaxValue;
                return data;
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        internal void Delete()
        {
            _lock.AcquireWriterLock(1000);
            try
            {
                changeFrom = int.MaxValue;
                Count = 0;
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        internal void Save(IList<IBar> data)
        {
            _lock.AcquireReaderLock(1000);
            try
            {
                if (changeFrom == int.MaxValue)
                    return;

                int from = int.MaxValue;
                LockCookie lc = _lock.UpgradeToWriterLock(1000);
                try
                {
                    from = changeFrom;
                    changeFrom = int.MaxValue;
                }
                finally
                {
                    _lock.DowngradeFromWriterLock(ref lc);
                }

                if (Count == 0)
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    return;
                }
                
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    // Записываю заголовок файла
                    byte[] fc = fileCaption;
                    fs.Write(fc, 0, fc.Length);
                    // Записываю данные, причем начинаю с изменившихся
                    fs.Position = CaptionLength + from * Tick.Size;

                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        for (int i = from; i < data.Count; ++i)
                            if (data[i] is Tick)
                                ((Tick)data[i]).WriteTo(bw);
                            else
                                (new Tick(data[i])).WriteTo(bw);
                        fs.SetLength(CaptionLength + data.Count * Tick.Size);
                    }
                }
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }

        }

        IBar lastBar = null;
        public void Add(IBar bar)
        {
            IList<IBar> data = ticks;

            // пытаюсь получить блокировку в течении 10 минут, скидывая Warn и Error если это дело затягивается
            int count = 0;
            bool exceptionFlag = true;
            while ((exceptionFlag) && (count < (10 * 60)))
            {
                exceptionFlag = false;
                try
                {
                    _lock.AcquireWriterLock(1000);
                }
                catch (ApplicationException)
                {
                    exceptionFlag = true;
                    ++count;
                    if (count > 10)
                        if (count > 60)
                            l.Error("Не могу получить блокировку для записи тика в течении " + count + " сек");
                        else
                            l.Warn("Не могу получить блокировку для записи тика в течении " + count + " сек");
                }
            }
            
            try
            {
                if ((lastBar == null) || (lastBar.DT < bar.DT) || ((lastBar.DT == bar.DT) && (lastBar.Number < bar.Number)))
                {
                    if (changeFrom > data.Count) changeFrom = data.Count;

                    lastBar = bar;
                    data.Add(bar);
                }
                else
                {
                    if (lastBar.Number == bar.Number)
                        return;
                    else
                    {
                        int index;
                        if (!BarExists(bar, out index))
                        {
                            data.Insert(index, bar);
                            if (changeFrom > index) changeFrom = index;
                        }
                        else
                            return;
                    }
                }
                ++Count;
#if FileTest
                    ONLY4TEST_notGarbage = data;
#else
                    TicksFileSaver.Add4Save(this, data);
#endif
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void Change(IBar bar)
        {
            l.Error("Тики не могут изменяться");
            return;
            /*
            _lock.AcquireWriterLock(1000);
            try
            {
                List<IBar> data = ticks;
                int index;
                if (BarExists(bar.number, out index))
                {
                    data.RemoveAt(index);
                    if (changeFrom > index)
                        changeFrom = index;
                    if (firstChange == DateTime.MaxValue) firstChange = DateTime.Now;
                    lastChange = DateTime.Now;

                }
                else
                    l.Error("Не найден тик, который следоволо заменить, просто добовляю " + bar);
                Add(bar);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
             */
        }

        public void Delete(IBar bar)
        {
            IList<IBar> data = ticks;

            _lock.AcquireWriterLock(1000);
            try
            {
                int index;
                if (BarExists(bar, out index))
                {
                    data.RemoveAt(index);

                    if (changeFrom > index) changeFrom = index;

                    --Count;
                    TicksFileSaver.Add4Save(this, data);
                }
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public IBar Last
        {
            get
            {
                IList<IBar> data = ticks;

                _lock.AcquireReaderLock(1000);
                try
                {
                    int c = data.Count;
                    if (c > 0)
                        return data[data.Count - 1];
                    else
                        return null;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }
            }
        }

        public IBar First
        {
            get
            {
                IList<IBar> data = ticks;

                _lock.AcquireReaderLock(1000);
                try
                {
                    if (data.Count > 0)
                        return data[0];
                    else
                        return null;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }
            }
        }

        public IBar GetPrevious(IBar bar)
        {
            IList<IBar> data = ticks;

            _lock.AcquireReaderLock(10000);
            try
            {
                int index;
                if (BarExists(bar, out index))
                    if (index > 0)
                        return data[index-1];
                return null;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }

        }

        IBar lastBar_CacheForGetNext = null;
        int lastIndex_CacheForGetNext = -1;

        public IBar GetNext(IBar bar)
        {
            if (bar == null)
                return null;

            IList<IBar> data = ticks;

            _lock.AcquireReaderLock(10000);
            try
            {
                if ((lastBar_CacheForGetNext != null) && (bar.Number == lastBar_CacheForGetNext.Number) && (bar.DT == lastBar_CacheForGetNext.DT)
                    &&
                    (data.Count > lastIndex_CacheForGetNext) && (bar.Number == data[lastIndex_CacheForGetNext].Number) && (bar.DT == data[lastIndex_CacheForGetNext].DT))
                {
                    ++lastIndex_CacheForGetNext;
                    if (lastIndex_CacheForGetNext < data.Count)
                    {
                        lastBar_CacheForGetNext = data[lastIndex_CacheForGetNext];
                        return data[lastIndex_CacheForGetNext];
                    }
                    else
                        return null;
                }

                int index;
                if (BarExists(bar, out index))
                {
                    ++index;
                    if (index < data.Count)
                    {
                        lastBar_CacheForGetNext = data[index];
                        lastIndex_CacheForGetNext = index;
                        return data[index];
                    }
                }
                return null;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public IBar Get(int dt)
        {
            IList<IBar> data = ticks;

            _lock.AcquireReaderLock(1000);
            try
            {
                if ((data.Count == 0) || ((data[0].DT > dt)))
                    return null;
                if (data[data.Count - 1].DT < dt)
                    return data[data.Count - 1];
                for (int i = 1; i < data.Count; ++i)  // TODO можно оптимизирывать по скорости
                    if (data[i].DT > dt)
                        return data[i - 1];
                return null;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        bool BarExists(IBar bar, out int index) // TODO надо потестить
        {            
            IList<IBar> data = ticks;

            bool needLock = !((_lock.IsReaderLockHeld) || (_lock.IsWriterLockHeld));

            if (needLock)
                _lock.AcquireReaderLock(10000);
            try
            {
                index = 0;

                if (data.Count == 0)
                    return false;

                int findFrom = 0;
                int findTo   = data.Count-1;

                if (data[0].DT > bar.DT)
                    return false;

                if (data[findTo].DT < bar.DT)
                {
                    index = data.Count;
                    return false;
                }

                int loopCount = data.Count;
                int delta = findTo - findFrom;
                while ((delta > 1) && (data[index].DT != bar.DT))
                {
                    if ((data[index].DT == bar.DT) && (data[index].Number == bar.Number))
                        return true;

                    int step = (int)((double)delta * ((double)(bar.DT - data[findFrom].DT) / (double)(data[findTo].DT - data[findFrom].DT)));
                    if ((step <= 0) || (step >= delta)) step = 1;

                    index = findFrom + step;

                    if (data[index].DT < bar.DT)
                        findFrom = index;
                    else
                        findTo = index;

                    delta = findTo - findFrom;

                    --loopCount;
                    if (loopCount == 0)
                        break;
                }

                if ((data[findFrom].DT > bar.DT) || (data[findTo].DT < bar.DT) || (loopCount == 0))
                    throw new Exception("Похоже я ошибся в алгоритме BarExists");

                // Пробегаюсь по заданной секунде, в поисках нужной сделки
                int saveIndex = index;
                while ((index >= 0)&&(data[index].DT >= bar.DT))
                    if (data[index].Number == bar.Number)
                        return true;
                    else
                        --index;

                index = saveIndex+1;
                while ((index < data.Count)&&(data[index].DT <= bar.DT))
                    if (data[index].Number == bar.Number)
                        return true;
                    else
                        ++index;

                return false;
            }
            finally
            {
                if (needLock)
                    _lock.ReleaseReaderLock();
            }
        }
    }
}