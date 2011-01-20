using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading;

namespace OpenWealth.DataManager
{
    public class TicksFiles
    {
        static readonly ILog l = Core.GetLogger(typeof(TicksFiles).FullName);
        static List<TicksFile> allTicksFileList = null;
        static ReaderWriterLock staticLock = new ReaderWriterLock();

        List<TicksFile> ticksFileList = new List<TicksFile>();
        ReaderWriterLock _lock = new ReaderWriterLock();
        
        readonly ISymbol symbol;
        readonly static string dataDir;

        static TicksFiles()
        {
            ISettingsHost sh = Core.GetGlobal("SettingsHost") as ISettingsHost;
            if (sh != null)
            {
                dataDir = sh.Get("DataDir", string.Empty);
                if (dataDir == string.Empty)
                {
                    dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "OpenWealth");
                    sh.Set("DataDir", dataDir);
                }
                if (!Directory.Exists(dataDir))
                {
                    l.Debug("Создаю каталог для тиков " + dataDir);
                    Directory.CreateDirectory(dataDir);
                }
            }
        }

        static internal void Init() // инициирую allTicksFileList
        {
            staticLock.AcquireWriterLock(1000);
            try
            {
                if (allTicksFileList == null)
                {
                    allTicksFileList = new List<TicksFile>();

                    if (File.Exists(Path.Combine(dataDir, "CaptionOfAllFiles.txt")))
                    {
                        using (StreamReader sr = new StreamReader(Path.Combine(dataDir, "CaptionOfAllFiles.txt"), new System.Text.UTF8Encoding()))
                        {
                            while (!sr.EndOfStream)
                                allTicksFileList.Add(TicksFile.LoadFromAllTicksFileList(dataDir, sr));
                        }
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(dataDir);
                        FileInfo[] files = di.GetFiles("*" + TicksFile.FileExt);
                        foreach (FileInfo f in files)
                            allTicksFileList.Add(new TicksFile(f.FullName));
                    }
                }
            }
            finally
            {
                staticLock.ReleaseWriterLock();
            }
        }

        static internal void Save()  // сохраняю allTicksFileList
        {
            staticLock.AcquireReaderLock(1000);
            try
            {
                if (allTicksFileList != null)
                {
                    using (FileStream fs = new FileStream(Path.Combine(dataDir, "CaptionOfAllFiles.txt"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                        foreach (TicksFile f in allTicksFileList)
                            if (f.Count > 0)
                            {
                                byte[] buff = encoding.GetBytes(Path.GetFileName(f.fileName) + "\r\n");
                                fs.Write(buff, 0, buff.Length);

                                buff = f.fileCaption;
                                fs.Write(buff, 0, buff.Length);
                            }
                    }
                }
            }
            finally
            {
                staticLock.ReleaseWriterLock();
            }
        }
        
        public TicksFiles(ISymbol symbol)
        {
            if (symbol.ToString().Length > 100)
                throw new ArgumentException("Длина названия символа не должна превышать 100 символов");

            this.symbol = symbol;

            staticLock.AcquireReaderLock(int.MaxValue);
            try
            {
                _lock.AcquireWriterLock(1000);
                try
                {
                    foreach (TicksFile f in allTicksFileList)
                        if (f.symbol == symbol)
                            ticksFileList.Add(f);
                }
                finally
                {
                    _lock.ReleaseWriterLock();
                }
            }
            finally
            {
                staticLock.ReleaseReaderLock();
            }        
        }

        public void Add(IBar bar)
        {
            GetFile(bar.DT).Add(bar);
        }

        public void Change(IBar bar)
        {
            GetFile(bar.DT).Change(bar);
        }

        public void Delete(IBar bar)
        {
            GetFile(bar.DT).Delete(bar);
        }

        public IBar Last
        {
            get
            {
                _lock.AcquireReaderLock(1000);
                try
                {
                    TicksFile lastFileWithData = null;
                    foreach (TicksFile f in ticksFileList)
                        if ((lastFileWithData == null) || ((lastFileWithData.endDateTime < f.endDateTime) && (f.Count > 0)))
                            lastFileWithData = f;
                    if (lastFileWithData == null)
                        return null;
                    return lastFileWithData.Last;
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
                _lock.AcquireReaderLock(1000);
                try
                {
                    TicksFile firstFileWithData = null;
                    foreach (TicksFile f in ticksFileList)
                        if ((firstFileWithData == null) || ((firstFileWithData.startDateTime > f.startDateTime) && (f.Count > 0)))
                            firstFileWithData = f;
                    if (firstFileWithData == null)
                        return null;
                    return firstFileWithData.First;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }
            }
        }

        public IBar GetPrevious(IBar bar)
        {
            TicksFile currentFile = GetFile(bar.DT);
            _lock.AcquireReaderLock(1000);
            try
            {                
                IBar result = currentFile.GetPrevious(bar);
                if (result == null)
                {
                    TicksFile previusFile = null;
                    foreach (TicksFile f in ticksFileList)
                        if ((f.startDateTime < currentFile.startDateTime) && ((previusFile == null) || ((previusFile.startDateTime < f.startDateTime) && (f.Count > 0))))
                            previusFile = f;
                    if(previusFile!=null)
                        return previusFile.Last;
                }
                return result;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public IBar GetNext(IBar bar)
        {
            TicksFile currentFile = GetFile(bar.DT);
            _lock.AcquireReaderLock(1000);
            try
            {                
                IBar result = currentFile.GetNext(bar);
                if (result == null)
                {
                    TicksFile nextFile = null;
                    foreach (TicksFile f in ticksFileList)
                        if ((f.startDateTime > currentFile.startDateTime) && ((nextFile == null) || ((nextFile.startDateTime > f.startDateTime) && (f.Count > 0))))
                            nextFile = f;
                    if (nextFile != null)
                        return nextFile.First;
                }
                return result;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public IBar Get(int dt)
        {
            return GetFile(dt).Get(dt);
        }

        public int Count
        {
            get
            {
                int result = 0;
                foreach (TicksFile f in ticksFileList)
                    result += f.Count;
                return result;
            }
        }

/*        public bool BarExists(long number, out int index)
        {
            for (index = 0; index < tickList.Count; ++index)
            {
                if (tickList[index].number == number)
                    return true;
                else
                    if (tickList[index].number > number)
                        return false;
            }
            return false;
        }
*/
/*        internal bool ExistsFileStartInPeriod(DateTime from, DateTime to)
        {
            _lock.AcquireReaderLock(1000);
            try
            {
                foreach (TicksFile f in ticksFileList)
                    if ((f.startDate >= from) && (f.startDate <= to))
                        return true;
                return false;
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }
*/


        TicksFile file_GetFileCache = null;

        TicksFile GetFile(int dt)
        {
            _lock.AcquireReaderLock(1000);
            try
            {
                if ((file_GetFileCache!=null)&&(file_GetFileCache.Include(dt)))
                    return file_GetFileCache;

                foreach (TicksFile f in ticksFileList)
                    if (f.Include(dt))
                    {
                        file_GetFileCache = f;
                        return f;
                    }

                LockCookie lc = _lock.UpgradeToWriterLock(1000);
                try
                {
                    DateTime startDate = new DateTime(DateTime2Int.DateTime(dt).Year, DateTime2Int.DateTime(dt).Month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                    string fileName = Path.Combine(dataDir, symbol + " " + startDate.ToString("yyyyMM") + TicksFile.FileExt);
                    //Создаю уникальное имя для файла, чтобы не затереть данные
                    int i = 1;
                    while (File.Exists(fileName))
                    {
                        l.Error("Не уникальное имя файла с тиками " + fileName);
                        fileName = Path.Combine(dataDir, symbol + " " + startDate.ToString("yyyyMM") + "[" + i + "]" + TicksFile.FileExt);
                        ++i;
                    }

                    TicksFile newTickFile = new TicksFile(
                        fileName,
                        symbol,
                        DateTime2Int.Int(startDate),
                        DateTime2Int.Int(endDate));

                    ticksFileList.Add(newTickFile);

                    staticLock.AcquireWriterLock(10000);
                    try { allTicksFileList.Add(newTickFile); }
                    finally { staticLock.ReleaseLock(); }

                    file_GetFileCache = newTickFile;
                    return newTickFile;
                }
                finally
                {
                    _lock.DowngradeFromWriterLock(ref lc);
                }
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        // Объеденяет файлы
        public void Concat(int count = 100*1024*1024/Simple.Tick.Size)
        {
            _lock.AcquireWriterLock(10000);
            try
            {
                List<TicksFile> newTicksFileList = new List<TicksFile>();

                if (Count <= count)
                {
                    //Объединяю всё в один файл
                    newTicksFileList.Add(
                        forConcatCreateFile(DateTime.MinValue, DateTime.MaxValue, string.Empty, Count)
                        );
                }
                else
                {
                    // Как минимум разбиваю по годам
                    IBar bar = First;
                    int year = bar.GetDateTime().Year;
                    int lastYear = Last.GetDateTime().Year;
                    while (year <= lastYear)
                    {
                        // расчитываю кво баров в данном году
                        int barCountOfYear = 0;
                        int nextYear = DateTime2Int.Int(new DateTime(year + 1, 1, 1));
                        while ((bar != null)&&(bar.DT < nextYear))
                        {
                            ++barCountOfYear;
                            bar = GetNext(bar);
                        }
                        if (barCountOfYear <= count)
                        {
                            //Объединяю год в один файл
                            if (barCountOfYear > 0)
                                newTicksFileList.Add(
                                    forConcatCreateFile(new DateTime(year, 1, 1), new DateTime(year + 1, 1, 1).AddSeconds(-1), " yyyy", barCountOfYear)
                                    );
                        }
                        else
                        {
                            //Разбивыаю по месяцам
                            for (int i = 1; i <= 12; ++i)
                            {
                                TicksFile tf = forConcatCreateFile(new DateTime(year, i, 1), new DateTime(year, i, 1).AddMonths(1).AddSeconds(-1), " yyyyMM", (barCountOfYear / 12 * 2));
                                if (tf.Count > 0)
                                    newTicksFileList.Add(tf);
                            }
                        }
                        ++year;
                    }
                }
                // Удаляю все существующие файлы
                staticLock.AcquireWriterLock(10000);
                try
                {
                    foreach (TicksFile tf in ticksFileList)
                        if (!newTicksFileList.Contains(tf))
                        {
                            tf.Delete();
                            allTicksFileList.Remove(tf); // TODO возможно здесь баг, т.к. после Concat файл CaptionOfAllFiles.txt был больше 15M, а после пересоздания стал 800K
                        }

                    ticksFileList = newTicksFileList;
                    foreach (TicksFile tf in ticksFileList)
                        if (!allTicksFileList.Contains(tf))
                            allTicksFileList.Add(tf);
                }
                finally
                {
                    staticLock.ReleaseWriterLock();
                }
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        TicksFile forConcatCreateFile(DateTime from, DateTime till, string maskForDateTime, int count)
        {
            int intTill = DateTime2Int.Int(till);
            int intFrom = DateTime2Int.Int(from);

            // Проверяю, возможно данный файл уже существует
            foreach (TicksFile tf in ticksFileList)
                if ((tf.startDateTime == intFrom) && (tf.endDateTime == intTill))
                    return tf;

            // ну а если нет, то создаю
            IBar bar = Get(DateTime2Int.Int(from));
            if (bar==null)
                bar = First;
                
            IList<IBar> ticks = new List<IBar>(count);
            while ((bar != null) && (bar.DT <= intTill))
            {
                if (bar.DT>=intFrom)
                    ticks.Add(bar);
                bar = GetNext(bar);
            }

            string fileName = string.Empty;

            if(maskForDateTime==string.Empty)
                fileName = Path.Combine(dataDir, symbol + TicksFile.FileExt);
            else
                fileName = Path.Combine(dataDir, symbol + from.ToString(maskForDateTime) + TicksFile.FileExt);

            //  Создаю уникальное имя для файла, чтобы не затереть данные
            int i = 1;
            while (File.Exists(fileName))
            {
                l.Info("Не уникальное имя файла с тиками " + fileName);
                fileName = Path.Combine(dataDir, symbol + from.ToString(maskForDateTime) + "[" + i + "]" + TicksFile.FileExt);
                ++i;
            }

            TicksFile result = new TicksFile(fileName, symbol, intFrom, intTill);
            result.ticks = ticks;
            return result;
        }
    }

}
