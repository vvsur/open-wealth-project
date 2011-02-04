using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.DataManager
{
    public class AggregateBars : IBars
    {
        static ILog l = Core.GetLogger(typeof(AggregateBars).FullName);

        public ISymbol symbol { get; private set; }
        public IScale scale { get; private set; }
        List<IBar> bars = new List<IBar>();
        IBars m_TickBars;

        string debKey = string.Empty;

        public AggregateBars(IDataManager data, ISymbol symbol, IScale scale)
        {
            l.Info("Создаем AggregateBars для " + symbol + " " + scale);
            if (scale.scaleType != ScaleEnum.sec)
                throw new NotImplementedException("поддерживаются только ТАЙМ фреймы");

            this.symbol = symbol;
            this.scale = scale;
            this.debKey = "(" + symbol + "." + scale + ") ";
            m_TickBars = data.GetBars(symbol, data.GetScale(ScaleEnum.tick, 1));
            m_TickBars.Lock.AcquireReaderLock(1000);
            try
            {
                IBar bar = m_TickBars.First;
                while (bar != null)
                {
                    m_TickBars_NewBarEvent(m_TickBars, new BarsEventArgs(this,bar));
                    bar = m_TickBars.GetNext(bar);
                }
                m_TickBars.NewBarEvent += new EventHandler<BarsEventArgs>(m_TickBars_NewBarEvent);
                m_TickBars.ChangeBarEvent += new EventHandler<BarsEventArgs>(m_TickBars_ChangeBarEvent);
            }
            finally
            {
                m_TickBars.Lock.ReleaseReaderLock();
            }
        }        

        public int TimeAlignment(int dt)
        {
            if (scale.scaleType != ScaleEnum.sec)
                throw new InvalidOperationException("Для scaleType отличного от ScaleEnum.sec вызов данного метода ошибочен");

            int periods = (dt - scale.beginning) / scale.interval;

            int result = scale.beginning + periods * scale.interval;

            if (l.IsDebugEnabled)
                l.Debug(debKey + "Округлил время " + DateTime2Int.DateTime(dt) + " до " + DateTime2Int.DateTime(result));
            return result;
        }

        public IBar FindBar(int dt)
        {
            if (!Lock.IsWriterLockHeld)
                Lock.AcquireReaderLock(1000);
            try
            {
                int alignmentDT = TimeAlignment(dt);
                IBar bar = this.Last;
                while (bar != null)
                {
                    if (alignmentDT == bar.DT)
                    {
                        if (l.IsDebugEnabled)
                            l.Debug(debKey+"Найден бар " + bar.Number);
                        return bar;
                    }
                    if (alignmentDT > bar.DT) // если искомое время больше времени в баре, то можно закругляться
                        break;
                    bar = this.GetPrevious(bar);
                }
                l.Debug(debKey+"Бар не найден");
                return null;
            }
            finally
            {
                if (!Lock.IsWriterLockHeld)
                    Lock.ReleaseReaderLock();
            }
        }

        IBar m_LastTick = null;

        void m_TickBars_NewBarEvent(object sender, BarsEventArgs e) // TODO возможная оптимизация (слишком часто вызываю TimeAlignment(e.bar.dt))
        {
            if(l.IsDebugEnabled)
                l.Debug(debKey+"m_TickBars_NewBarEvent новый тик " + e.bar);
            Lock.AcquireWriterLock(1000);
            try
            {
                AggregateBar bar = FindBar(e.bar.DT) as AggregateBar;

                if (bar == null)
                {
                    int timeAlignment = TimeAlignment(e.bar.DT);

                    if (l.IsDebugEnabled)
                        l.Debug(debKey + "m_TickBars_NewBarEvent Создаю новый бар " + DateTime2Int.DateTime(timeAlignment) );

                    bar = new AggregateBar(timeAlignment, timeAlignment + scale.interval - 1, e.bar.Number, e.bar.Close, e.bar.Close, e.bar.Close, e.bar.Close, e.bar.Volume);

                    bars.Add(bar);

                    EventHandler<BarsEventArgs> ev = NewBarEvent;
                    if (ev != null)
                        ev(this, new BarsEventArgs(this,bar));
                }
                else
                {
                    l.Debug(debKey + "m_TickBars_NewBarEvent Добавляю тик в бар");

                    if ((m_LastTick != null) &&
                            ((m_LastTick.DT > e.bar.DT)
                            || ((m_LastTick.DT == e.bar.DT) && (m_LastTick.Number > e.bar.Number))))
                    {
                        l.Debug(debKey + "Тики пришли не по порядку. Пересчитываю весь бар");
                        RecalcBar(bar);
                    }
                    else
                        bar.AddTick(e.bar);

                    m_LastTick = e.bar;

                    EventHandler<BarsEventArgs> changeBarEvent = ChangeBarEvent;
                    if (changeBarEvent != null)
                        changeBarEvent(this, new BarsEventArgs(this,bar));
                }
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }

        void RecalcBar(AggregateBar bar)
        {
            l.Info("RecalcBar " + bar.Number);
            Lock.AcquireWriterLock(1000);
            try
            {
                bar.Clear();
                m_TickBars.Lock.AcquireReaderLock(1000);
                try
                {
                    IBar tick = m_TickBars.First;
                    while (tick != null)
                    {
                        if (TimeAlignment(tick.DT) == bar.DT)
                            bar.AddTick(tick);
                        tick = m_TickBars.GetNext(tick);
                    }

                }
                finally
                {
                    m_TickBars.Lock.ReleaseReaderLock();
                }
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }

        void m_TickBars_ChangeBarEvent(object sender, BarsEventArgs e)
        {
            l.Fatal("Не ожидаю, что тиковые бары могут меняться");
            throw new NotImplementedException("Не ожидаю, что тиковые бары могут меняться");
        }

        public IBar this[int i] { get { return bars[i]; } }

        public void Add(IDataProvider system, IBar bar)
        {
            Lock.AcquireWriterLock(1000);
            try
            {
                bars.Add(bar);
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }

            EventHandler<BarsEventArgs> e = NewBarEvent;
            if (e != null)
                e(this, new BarsEventArgs(this, bar));
        }

        public void Change(IDataProvider system, IBar bar)
        {
            if (bars.Contains(bar))
            {
                EventHandler<BarsEventArgs> e = ChangeBarEvent;
                if (e != null)
                    e(this, new BarsEventArgs(this, bar));
            }
            else
                Add(system, bar);
        }

        public int Count { get { return bars.Count; } }

        public event EventHandler<BarsEventArgs> NewBarEvent;
        public event EventHandler<BarsEventArgs> ChangeBarEvent;

        /* System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }
        public BarEnum GetEnumerator()
        {
            return new BarEnum(m_bars);
        }
        */

        public IBar First
        {
            get
            {
                Lock.AcquireReaderLock(1000);
                try
                {
                    if (bars.Count > 0)
                    {
                        l.Debug(debKey+"GetFirst " + bars[0]);
                        return bars[0];
                    }
                    else
                    {
                        l.Debug(debKey+"GetFirst null");
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
                        if(l.IsDebugEnabled)
                            l.Debug(debKey+"GetLast " + bars[bars.Count - 1]);
                        return bars[bars.Count - 1];
                    }
                    else
                    {
                        l.Debug(debKey+"GetLast null");
                        return null;
                    }
                }
                finally
                {
                    Lock.ReleaseReaderLock();
                }
            }
        }
        // Ввел в качестве кэша, при профилировании
        int lastGetPreviousFind = 0;
        public IBar GetPrevious(IBar bar)
        {
            Lock.AcquireReaderLock(1000);
            try
            {
                // Надеюсь, что с последнего GetPrevious bar не изменился 
                if ((lastGetPreviousFind <= bars.Count - 1) && (lastGetPreviousFind >= 0) && (bars[lastGetPreviousFind] == bar))
                    if ((--lastGetPreviousFind) == -1)
                    {
                        lastGetPreviousFind = bars.Count-1;
                        return null;
                    }
                    else
                        return bars[lastGetPreviousFind];
                // Воспользоваться кэшем lastGetPreviousFind не удалось, ищу по всему массиву
                // TODO Вообще этот поиск можно ускорить, т.к. массив отсартирован
                for (int find = bars.Count - 1; find > 0; --find)
                    if (bars[find] == bar)
                    {
                        lastGetPreviousFind = find - 1;
                        return bars[lastGetPreviousFind];
                    }
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
                        lastGetNextFind = find + 1;
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

        public bool BarExists(long number, out int index)
        {
            m_lock.AcquireReaderLock(1000);
            try
            {
                for (index = 0; index < bars.Count; ++index)
                {
                    if (bars[index].Number == number)
                        return true;
                    else
                        if (bars[index].Number > number)
                            return false;
                }
                return false;
            }
            finally
            {
                m_lock.ReleaseReaderLock();
            }
        }

        public void Delete(IDataProvider system, IBar bar)
        {
            m_lock.AcquireWriterLock(1000);
            try
            {
                int index;
                if (BarExists(bar.Number, out index))
                    bars.RemoveAt(index);
            }
            finally
            {
                m_lock.ReleaseWriterLock();
            }
        }

        public IBar Get(int dt)
        {
            m_lock.AcquireReaderLock(1000);
            try
            {
                if ((bars.Count == 0)||((bars[0].DT > dt)))
                    return null;
                if (bars[bars.Count - 1].DT < dt)
                    return bars[bars.Count - 1];
                for (int i = 1; i < bars.Count; ++i)
                    if (bars[i].DT > dt)
                        return bars[i-1];
                return null;
            }
            finally
            {
                m_lock.ReleaseReaderLock();
            }
        }

        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }       
        #endregion Lock
    }
}