using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.Data
{
    public class AggregateBars : IBars
    {
        static ILog l = Core.GetLogger(typeof(AggregateBars).FullName);

        public ISymbol symbol { get; private set; }
        public IScale scale { get; private set; }
        List<IBar> m_bars = new List<IBar>();
        IBars m_TickBars;

        public AggregateBars(IData data, ISymbol symbol, IScale scale)
        {
            l.Debug("Создаем AggregateBars для " + symbol + " " + scale);
            if (scale.scaleType != ScaleEnum.sec)
                throw new NotImplementedException("поддерживаются только ТАЙМ фреймы");

            this.symbol = symbol;
            this.scale = scale;
            m_TickBars = data.GetBars(symbol, data.GetScale(ScaleEnum.tick, 1));
            m_TickBars.Lock.AcquireReaderLock(1000);
            try
            {
                foreach (IBar bar in m_TickBars)
                    m_TickBars_NewBarEvent(m_TickBars, new BarsEventArgs(bar));
                m_TickBars.NewBarEvent += new EventHandler<BarsEventArgs>(m_TickBars_NewBarEvent);
                m_TickBars.ChangeBarEvent += new EventHandler<BarsEventArgs>(m_TickBars_ChangeBarEvent);
            }
            finally
            {
                m_TickBars.Lock.ReleaseReaderLock();
            }
        }        

        public DateTime TimeAlignment(DateTime dt)
        {
            if (scale.scaleType != ScaleEnum.sec)
                throw new InvalidOperationException("Для scaleType отличного от ScaleEnum.sec вызов данного метода ошибочен");

            TimeSpan delta = dt - scale.beginning;
            Int64 sec = (Int64)delta.TotalSeconds;
            sec = (sec / scale.interval) * scale.interval;

            delta = new TimeSpan(0, (int)(sec / 60), (int)(sec % 60));

            DateTime result = scale.beginning + delta;
            l.Debug("Округлил время " + dt + " до " + result);
            return result;
        }

        public IBar FindBar(DateTime dt)
        {
            if (!Lock.IsWriterLockHeld)
                Lock.AcquireReaderLock(1000);
            try
            {
                DateTime alignmentDT = TimeAlignment(dt);
                foreach (IBar bar in m_bars)
                    if (alignmentDT == bar.dt)
                    {
                        l.Debug("Найден бар " + bar.number);
                        return bar;
                    }
                l.Debug("Бар не найден");
                return null;
            }
            finally
            {
                if (!Lock.IsWriterLockHeld)
                    Lock.ReleaseReaderLock();
            }
        }

        IBar m_LastTick;

        void m_TickBars_NewBarEvent(object sender, BarsEventArgs e) // TODO возможная оптимизация (слишком часто вызываю TimeAlignment(e.bar.dt))
        {
            l.Debug("m_TickBars_NewBarEvent новый тик " + e.bar.number);
            Lock.AcquireWriterLock(1000);
            try
            {
                if (m_LastTick == e.bar)
                    return;
                m_LastTick = e.bar;

                AggregateBar bar = FindBar(e.bar.dt) as AggregateBar;

                if (bar == null)
                {
                    l.Debug("m_TickBars_NewBarEvent Создаю новый бар " + TimeAlignment(e.bar.dt));
                    bar = new AggregateBar(TimeAlignment(e.bar.dt), e.bar.number, e.bar.close, e.bar.close, e.bar.close, e.bar.close, e.bar.volume);

                    m_bars.Add(bar);

                    EventHandler<BarsEventArgs> ev = NewBarEvent;
                    if (ev != null)
                        ev(this, new BarsEventArgs(bar));

                    ev = ChangeBarEvent;
                    if (ev != null)
                        ev(this, new BarsEventArgs(bar));

                    return;
                }

                l.Debug("m_TickBars_NewBarEvent Добавляю тик в бар");

                if ((m_LastTick != null) &&
                        ((m_LastTick.dt > e.bar.dt)
                        || ((m_LastTick.dt == e.bar.dt) && (m_LastTick.number > e.bar.number))))
                {
                    l.Debug("Тики пришли не по порядку. Пересчитываю весь бар");
                    RecalcBar(bar);
                }
                else
                    bar.AddTick(e.bar);

                EventHandler<BarsEventArgs> changeBarEvent = ChangeBarEvent;
                if (changeBarEvent != null)
                    changeBarEvent(this, new BarsEventArgs(bar));
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }

        void RecalcBar(AggregateBar bar)
        {
            l.Debug("!!!!!!!!!!!!!!!!!     RecalcBar " + bar.number.ToString());
            bar.Lock.AcquireWriterLock(1000);
            try
            {
                bar.Clear();
                m_TickBars.Lock.AcquireReaderLock(1000);
                try
                {
                    foreach (IBar tick in m_TickBars)
                        if (TimeAlignment(tick.dt) == bar.dt)
                            bar.AddTick(tick);
                }
                finally
                {
                    m_TickBars.Lock.ReleaseReaderLock();
                }
            }
            finally
            {
                bar.Lock.ReleaseWriterLock();
            }
        }

        void m_TickBars_ChangeBarEvent(object sender, BarsEventArgs e)
        {
            l.Fatal("Не ожидаю, что тиковые бары могут меняться");
            throw new NotImplementedException("Не ожидаю, что тиковые бары могут меняться");
        }

        public IBar this[int i] { get { return m_bars[i]; } }

        public void Add(IPlugin system, IBar bar)
        {
            Lock.AcquireWriterLock(1000);
            try
            {
                m_bars.Add(bar);
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }

            EventHandler<BarsEventArgs> e = NewBarEvent;
            if (e != null)
                e(this, new BarsEventArgs(bar));

        }

        public void Change(IPlugin system, IBar bar)
        {
            if (m_bars.Contains(bar))
            {
                EventHandler<BarsEventArgs> e = ChangeBarEvent;
                if (e != null)
                    e(this, new BarsEventArgs(bar));
            }
            else
                Add(system, bar);
        }

        public int Count { get { return m_bars.Count; } }

        public event EventHandler<BarsEventArgs> NewBarEvent;
        public event EventHandler<BarsEventArgs> ChangeBarEvent;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }
        public BarEnum GetEnumerator()
        {
            return new BarEnum(m_bars);
        }

        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }       
        #endregion Lock


    }
}
