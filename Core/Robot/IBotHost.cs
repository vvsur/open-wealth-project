using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Классы данного интерфейса необходимы для запуска роботов (IBot)
    /// 
    /// </summary>
    public interface IBotHost
    {
        BotParam GetParam(string name, double minValue, double maxValue, double defaultValue);
        IDictionary<string, BotParam> Params { get; }

        ISymbol Symbol { get; }
        IScale Scale { get; }

        IBars Bars { get; }
        IBar LastBar { get; }

        int MaxPosition { get; }
        IBot Robot { get; }

        void BuyAtMarket(int qty);
        void SellAtMarket(int qty);
        void BuyAtLimit(int qty, float limit);
        void SellAtLimit(int qty, float limit);

        IList<IOrder> AllOrders { get; }
        IList<IOrder> ActiveOrders { get; }
        IList<IDeal> MyDeal { get; }

        Position Pos { get; }

        // текущее время в роботе надо использовать данное время, а не системное, т.к. возможен бэктестинг
        int DT { get; }
        DateTime Now { get; }

        // время старта робота (для бэктестинга время первого бара в тестироемой выборке)
        int startDT { get; }
        // время, когда робот должен быть остановлен (например, конец дня, или int.maxValue), для бактестинга последний бар в тестируемой выборке
        int endDT { get; }

        void Start();
        void Stop();

        /// <summary>
        /// событие, возникающее в момент когда бар полностью сформирован
        /// </summary>
        event EventHandler<BarsEventArgs> onBar;
        event EventHandler<DealEventArgs> onDeal;
        event EventHandler<EventArgs> onSec;
        event EventHandler<EventArgs> onNewDay;
    }

    public class DealEventArgs : EventArgs
    {
        public IDeal deal { get; private set; }
        public DealEventArgs(IDeal deal)
        {
            this.deal = deal;
        }
    }

    public class OrderEventArgs : EventArgs
    {
        public IOrder order { get; private set; }
        public OrderEventArgs(IOrder order)
        {
            this.order = order;
        }
    }
}
