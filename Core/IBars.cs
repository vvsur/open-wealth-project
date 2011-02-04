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
        void Delete(IDataProvider system, IBar bar);

        int Count { get; }

        IBar First { get; }
        IBar Last { get; }
        /// <summary>
        /// Возвращает бар, с указанным временем, или ближайший бар с меньшим временем
        /// Если найдено несколько баров с указанным временем, то возвращает последний из найденных
        /// Если дата первого бара больше, чем dt, то возвращается null
        /// </summary>
        IBar Get(int dt);
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
        public IBars bars { get; private set; }
        public IBar bar { get; private set; }
        public BarsEventArgs(IBars bars, IBar bar)
        {
            this.bars = bars;
            this.bar = bar;
        }
    }
}
