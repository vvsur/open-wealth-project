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
        public void Add(IPlugin system, IBar bar)
        {
            l.Debug("Новый бар " + bar.number);

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

        public void Change(IPlugin system, IBar bar)
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }
        public BarEnum GetEnumerator()
        {
            return new BarEnum(bars);
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
