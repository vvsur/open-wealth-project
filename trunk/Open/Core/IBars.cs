using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий набор свечь по конкретному инструменту и конкретному периоду
    /// </summary>
    public interface IBars// : System.Collections.IEnumerable
    {
        ISymbol symbol { get; }
        IScale scale { get; }

        void Add(IDataProvider system, IBar bar);
        void Change(IDataProvider system, IBar bar);

     //   IBar this[int i] { get; }
        int Count { get; }

        IBar First { get; }
        IBar Last { get; }
        IBar GetPrevious(IBar bar);
        IBar GetNext(IBar bar);

        System.Threading.ReaderWriterLock Lock { get; }

        event EventHandler<BarsEventArgs> NewBarEvent;
        event EventHandler<BarsEventArgs> ChangeBarEvent;
    }

    /// <summary>
    /// Параметры события, содержащие IBar
    /// </summary>
    public class BarsEventArgs : EventArgs
    {
        public IBar bar { get; private set; }
        public BarsEventArgs(IBar bar)
        {
            this.bar = bar;
        }
    }

}
