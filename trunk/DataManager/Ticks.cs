using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.DataManager
{
    public class Ticks : IBars
    {
        static ILog l = Core.GetLogger(typeof(Ticks).FullName);

        public ISymbol symbol { get; protected set; }
        public IScale scale { get; protected set; }
        public Ticks(ISymbol symbol)
        {
            l.Debug("Создаем Ticks для " + symbol);
            this.symbol = symbol;
            this.scale = scale;

            ticksFileList = new TicksFiles(symbol);
        }

        internal TicksFiles ticksFileList;

        public void Add(IDataProvider system, IBar bar)
        {
            if (l.IsDebugEnabled)
                l.Debug("Новый бар " + bar);

            ticksFileList.Add(bar);

            EventHandler<BarsEventArgs> e = NewBarEvent;
            if (e != null)
                e(this, new BarsEventArgs(this,bar));
        }

        public void Change(IDataProvider system, IBar bar)
        {
            l.Error("Тики не могут меняться?");

            EventHandler<BarsEventArgs> ev = ChangeBarEvent;
            if (ev != null)
                ev(this, new BarsEventArgs(this,bar));
        }

        public void Delete(IDataProvider system, IBar bar) { ticksFileList.Delete(bar); }

        public IBar Get(int dt) { return ticksFileList.Get(dt); }

        public IBar First { get { return ticksFileList.First; } }

        public IBar Last { get { return ticksFileList.Last; } }

        public IBar GetPrevious(IBar bar) { return ticksFileList.GetPrevious(bar); }

        public IBar GetNext(IBar bar)  { return ticksFileList.GetNext(bar); }

        public int Count { get { return ticksFileList.Count; } }

        public event EventHandler<BarsEventArgs> NewBarEvent;
        public event EventHandler<BarsEventArgs> ChangeBarEvent;

        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }
        #endregion Lock

    }
}
