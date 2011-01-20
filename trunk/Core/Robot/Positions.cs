using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public class Positions
    {
        List<Position> list = new List<Position>();
        public void AddDeal(IDeal deal)
        {
            lock (list)
            {
                Get(deal.Symbol).Value += (int)deal.BuySell * deal.Value;
            }
        }

        public void AddOrder(IOrder order)
        {
            lock (list)
            {

                //Get(deal.Symbol).Value += (int)deal.BuySell * deal.Value;
                throw new NotImplementedException();


            }
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        Position Get(ISymbol symbol)
        {
            lock (list)
            {
                foreach (Position p in list)
                    if (p.Symbol == symbol)
                        return p;
                Position result = new Position(symbol);
                list.Add(result);
                return result;
            }
        }
    }

    public class Position
    {
        public Position(ISymbol symbol)
        {
            this.Symbol= symbol;
            Value = 0;
            ActiveBuy = 0;
            ActiveSell = 0;
        }

        public readonly ISymbol Symbol;

        public int Value { get; internal set; }
        public int ActiveBuy { get; internal set; }
        public int ActiveSell { get; internal set; }
    }
}
