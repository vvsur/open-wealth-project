using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IDeal
    {
        ISymbol Symbol { get; }
        IAccount Account { get; }
        BuySellEnum BuySell{ get; }
        float Price { get; }
        int Value { get; }
        IOrder Order { get; }
    }
}
