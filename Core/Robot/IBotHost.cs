using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IBotHost
    {
        BotParam GetParam(string name, double minValue, double maxValue, double defaultValue);

        ISymbol Symbol { get; }
        IScale Scale { get; }
        int MaxPosition { get; }
        IBot Robot { get; }

        void BuyAtMarket(int qty);
        void SellAtMarket(int qty);
        void BuyAtLimit(int qty, float limit);
        void SellAtLimit(int qty, float limit);

        IList<IOrder> GetAllOrders();
        IList<IOrder> GetActiveOrders();
        IList<IDeal> GetMyDeal();

        Position Pos { get; }

        // текущее время
        int DT { get; }
        DateTime Now { get; }

        // событие, возникающее в момент когда бар полностью сформирован
        event EventHandler<BarsEventArgs> onBar;
        event EventHandler<BarsEventArgs> onTick;
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