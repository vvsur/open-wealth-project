using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    interface IOrderManager
    {
        IOrder New(IAccount account, IBot robot, ISymbol symbol, OrderTypeEnum orderType, BuySellEnum buySell, double price);
        IEnumerable<IOrder> GetOrders(IAccount account, IBot robot, ISymbol symbol, OrderTypeEnum orderType, BuySellEnum buySell, OrderStatusEnum orderStatus, bool isActive);
        IEnumerable<IDeal> GetDeals(IAccount account, IBot robot, ISymbol symbol, BuySellEnum buySell);
        void GetPositions(IAccount account, IBot robot, ISymbol symbol);
    }
}
