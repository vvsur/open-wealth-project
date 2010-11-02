using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    interface IOrderManager
    {
        IOrder New(IAccount account, IRobot robot, ISymbol symbol, OrderTypeEnum orderType, BuySellEnum buySell, double price);
        IEnumerable<IOrder> GetOrders(IAccount account, IRobot robot, ISymbol symbol, OrderTypeEnum orderType, BuySellEnum buySell, OrderStatusEnum orderStatus, bool isActive);
        IEnumerable<IDeal> GetDeals(IAccount account, IRobot robot, ISymbol symbol, BuySellEnum buySell);
        void GetPositions(IAccount account, IRobot robot, ISymbol symbol);
    }
}
