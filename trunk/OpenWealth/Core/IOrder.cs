using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public enum BuySellEnum { Buy, Sell };
    public enum OrderStatusEnum { New, Sending, Active, Canceling, Cancel };
    public enum OrderTypeEnum { Market, Limited };

    interface IOrder
    {
        //свойства
        OrderStatusEnum OrderStatus { get; }
        IEnumerable<IOrder> Deals { get; }
        void Send();  // а зачем???? создание не обозначает отправку?
        void Cancel();  // а зачем???? создание не обозначает отправку?
        bool isActive();

    }
}
