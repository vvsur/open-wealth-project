using System;
using System.Collections;
using System.Collections.Generic;
using OpenWealth;

namespace TEST_bot_BackTestHost
{
    public class Bot : IBot
    {
        private readonly static ILog l = Core.GetLogger(typeof(Bot));

        public string Name        { get { return "testBot"; } }
        public string Description { get { return "testBot"; } }
        public string URL         { get { return null; } }

        IBotHost bh = null;
        public void Init(IBotHost BotHost)
        {
            if (this.bh != null)
                l.Error("Повторная инициализация робота.");

            this.bh = BotHost;
        }

        public void Start()
        {
            if (bh != null)
            {
                bh.onBar += new EventHandler<BarsEventArgs>(onBar);
            }            
        }

        public void Stop()
        {
            if (bh != null)
            {
                bh.onBar -= new EventHandler<BarsEventArgs>(onBar);
                IList<IOrder> activeOrders = bh.GetActiveOrders();
                foreach (IOrder order in activeOrders)
                    order.Cancel();
            }
        }

        void onBar(object sender, BarsEventArgs e)
        {
            IBar now = e.bar;
            IBar pre = e.bars.GetPrevious(e.bar);
            if ((now != null) && (pre != null))
            {
                if (now.Close > pre.Close)
                    bh.BuyAtMarket(bh.MaxPosition - bh.Pos.Value - bh.Pos.ActiveBuy);
                if (now.Close < pre.Close)
                    bh.SellAtMarket(bh.MaxPosition + bh.Pos.Value - bh.Pos.ActiveSell);
            }
        }
    }
}
