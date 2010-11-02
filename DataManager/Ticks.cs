using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.Data
{
    public class Ticks : IBars
    {
        static ILog l = Core.GetLogger(typeof(Ticks).FullName);

        public ISymbol symbol { get; protected set; }
        public IScale scale { get; protected set; }
        public Ticks(ISymbol symbol, IScale scale)
        {
            l.Debug("Создаем Bars для " + symbol + " " + scale);
            this.symbol = symbol;
            this.scale = scale;
        }
        
        List<IBar> bars = new List<IBar>();

        IBar m_LastBar;
        public void Add(IDataProvider system, IBar bar)
        {
            if (l.IsDebugEnabled)
                l.Debug("Новый бар " + bar);

            Lock.AcquireWriterLock(1000);
            try
            {
                if ((m_LastBar == null) || (m_LastBar.number < bar.number))
                {
                    m_LastBar = bar;
                    bars.Add(bar);
                }
                else
                    if (m_LastBar.number == bar.number)
                        return;
                    else
                    {
                        int index;
                        if (!BarExists(bar.number, out index))
                            bars.Insert(index, bar);
                        else
                            return;
                    }
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }

            EventHandler<BarsEventArgs> e = NewBarEvent;
            if (e != null)
                e(this, new BarsEventArgs(bar));
        }

        public bool BarExists(long number, out int index)
        {
            index = 0;
            foreach(IBar bar in bars)
            {
                if (bar.number == number)
                    return true;
                else
                    if (bar.number > number)
                        return false;
                ++index;
            }
            return false;
        }

        public void Change(IDataProvider system, IBar bar)
        {
            if (bars.Contains(bar))
            {
                EventHandler<BarsEventArgs> e = ChangeBarEvent;
                if (e != null)
                    e(this, new BarsEventArgs(bar));
            }
            else
                Add(system, bar);
        }

        /*System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }
        public BarEnum GetEnumerator()
        {
            return new BarEnum(bars);
        }
        */
        // TODO Вынисти одинаковые методы Ticks и AggregateBars в общего предка
        public IBar First
        {
            get
            {
                Lock.AcquireReaderLock(1000);
                try
                {
                    if (bars.Count > 0)
                    {
                        l.Debug("GetFirst " + bars[0]);
                        return bars[0];
                    }
                    else
                    {
                        l.Debug("GetFirst null");
                        return null;
                    }
                }
                finally
                {
                    Lock.ReleaseReaderLock();
                }
            }
        }
        public IBar Last
        {
            get
            {
                Lock.AcquireReaderLock(1000);
                try
                {
                    if (bars.Count > 0)
                    {
                        l.Debug("GetLast " + bars[bars.Count - 1]);
                        return bars[bars.Count - 1];
                    }
                    else
                    {
                        l.Debug("GetLast null");
                        return null;
                    }
                }
                finally
                {
                    Lock.ReleaseReaderLock();
                }
            }
        }
        public IBar GetPrevious(IBar bar)
        {
            Lock.AcquireReaderLock(1000);
            try
            {
                for (int find = bars.Count - 1; find > 0; --find)
                    if (bars[find] == bar)
                        return bars[find - 1];
                l.Info("GetPrevious бар не найден, вернул null");
                return null;
            }
            finally
            {
                Lock.ReleaseReaderLock();
            }
        }
        // Ввел в качестве кэша, при профилировании
        int lastGetNextFind = 0;
        public IBar GetNext(IBar bar)
        {
            Lock.AcquireReaderLock(1000);
            try
            {
                // Надеюсь, что с последнего GetNext bar не изменился 
                if ((lastGetNextFind <= bars.Count - 1) && (bars[lastGetNextFind] == bar))
                    if ((++lastGetNextFind) == bars.Count)
                    {
                        lastGetNextFind = 0;
                        return null;
                    }
                    else
                        return bars[lastGetNextFind];
                // Воспользоваться кэшем lastGetNextFind не удалось, ищу по всему массиву
                // TODO Вообще этот поиск можно ускорить, т.к. массив отсартирован
                for (int find = bars.Count - 2; find >= 0; --find)
                    if (bars[find] == bar)
                    {
                        lastGetNextFind = find+1;
                        return bars[lastGetNextFind];
                    }
                l.Info("GetNext бар не найден, вернул null");
                return null;
            }
            finally
            {
                Lock.ReleaseReaderLock();
            }
        }

        public IBar this[int i] { get { return bars[i]; } }
        public int Count { get { return bars.Count; } }

        public event EventHandler<BarsEventArgs> NewBarEvent;
        public event EventHandler<BarsEventArgs> ChangeBarEvent;

        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }
        #endregion Lock

    }
}
