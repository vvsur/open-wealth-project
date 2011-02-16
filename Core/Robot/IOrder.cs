using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public enum BuySellEnum { Sell = -1, NotDefine = 0, Buy = 1 };
    public enum OrderStatusEnum { New, Sending, Active, Canceling, Cancel, Error };
    public enum OrderTypeEnum { Market, Limited };

    public interface IOrder
    {
        IOrder Update(OrderStatusEnum OrderStatus, Int64 Number, float Price, int Volue, int QTY);
        void AddDeal(IDeal deal);

        void Cancel();
        void Move(float Price, int Volue);

        bool isActive { get; }

        IEnumerable<IOrder> Deals { get; }
        OrderStatusEnum OrderStatus { get; }
        Int64 Number { get; }
        float Price { get; }
        int Volue { get; }
        int QTY { get; }
        IEnumerable<IOrder> Deals { get; }

        IOrder PrevOrderState { get; }

        IDictionary<string, object> Ext { get; }
    }
}