using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IBars : System.Collections.IEnumerable
    {
        ISymbol symbol { get; }
        IScale scale { get; }

        void Add(IPlugin system, IBar bar);
        void Change(IPlugin system, IBar bar);

        IBar this[int i] { get; }
        int Count { get; }

        System.Threading.ReaderWriterLockSlim Lock { get; }

        event EventHandler<BarsEventArgs> NewBarEvent;
        event EventHandler<BarsEventArgs> ChangeBarEvent;
    }

    public class BarsEventArgs : EventArgs
    {
        public IBar bar { get; private set; }
        public BarsEventArgs(IBar bar)
        {
            this.bar = bar;
        }
    }

}
