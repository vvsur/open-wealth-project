using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading;
using OpenWealth;

namespace TEST_bot_BackTestHost
{
    class BotHost : IBotHost
    {
        Dictionary<string, BotParam> botParams = new Dictionary<string, BotParam>();
        public BotParam GetParam(string name, double minValue, double maxValue, double defaultValue)
        {
            BotParam result;
            if (!botParams.TryGetValue(name, out result))
            {
                result = new BotParam(name, minValue, maxValue, defaultValue);
                botParams.Add(name, result);
            }
            return result;
        }

        // Время первого и последнего bar'а для теста
        int endDT;
        int startDT;

        public ISymbol Symbol { get; private set; }
        public IScale Scale { get; private set; } // слово new - это минус в сторону partial
        public int MaxPosition { get; private set; }

        public IBot Robot { get; private set; }

        public void BuyAtMarket(int qty)
        {
            throw new NotImplementedException();
        }
        public void SellAtMarket(int qty)
        {
            throw new NotImplementedException();
        }
        public void BuyAtLimit(int qty, float limit)
        {
            throw new NotImplementedException();
        }
        public void SellAtLimit(int qty, float limit)
        {
            throw new NotImplementedException();
        }

        public IList<IOrder> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public IList<IOrder> GetActiveOrders()
        {
            throw new NotImplementedException();
        }

        public IList<IDeal> GetMyDeal()
        {
            throw new NotImplementedException();
        }

        public Position Pos { get; private set; }

        // текущее время
        public int DT { get { throw new NotImplementedException(); } }
        public DateTime Now { get { return DateTime2Int.DateTime(DT); } }

        // событие, возникающее в момент когда бар полностью сформирован
        public event EventHandler<BarsEventArgs> onBar;
        public event EventHandler<BarsEventArgs> onTick;
        public event EventHandler<DealEventArgs> onDeal;
        public event EventHandler<EventArgs> onSec;
        public event EventHandler<EventArgs> onNewDay;
     }
}
