using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading;
using OpenWealth;

namespace TEST_bot_BackTestHost
{
    class BotHost : IBotHost
    {
        static ILog l = Core.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Params +

        IDictionary<string, BotParam> botParams = new Dictionary<string, BotParam>();
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
        IDictionary<string, BotParam> Params { get { return botParams; } }

        #endregion

        public int startDT { get; private set; }
        public int endDT { get; private set; }

        public ISymbol Symbol { get; private set; }
        public IScale Scale { get; private set; } 
        public int MaxPosition { get; private set; }

        public IBot Robot { get; private set; }

        public IBars Bars { get; private set; }
        public IBar LastBar { get; private set; }

        public IBars ticks { get; private set; }
        public IBar lastTick { get; private set; }

        #region Выставление заявок

        IList<IOrder> allOrders = new List<IOrder>();
        IList<IOrder> activeOrders = new List<IOrder>();

        public void BuyAtMarket(int qty)
        {
            IOrder order = new Order();
            allOrders.Add(order);
            activeOrders.Add(order);
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

        #endregion

        public IList<IOrder> AllOrders
        {
            get
            {
                return allOrders;
            }
        }

        public IList<IOrder> ActiveOrders
        {
            get
            {
                return activeOrders;
            }
        }

        public IList<IDeal> MyDeal
        {
            get
            {
                throw new NotImplementedException();
            }            
        }

        public Position Pos { get; private set; }

        public int DT { get; private set; }
        public DateTime Now { get { return DateTime2Int.DateTime(DT); } }

        bool stopFlag, newBarFlag;
        public void Start()
        {
            stopFlag = false;
            newBarFlag = true;

            lastTick = ticks.Get(startDT);
            if (lastTick == null)
                lastTick = ticks.First;

            if ((lastTick == null) || (lastTick.DT > endDT))
            {
                l.Info("Нет данных, для данного временного периода");
                return;
            }

            LastBar = Bars.Get(lastTick.DT);
            if (LastBar == null)
            {
                l.Error("Тик есть, а бара нет :(");
                return;
            }

            DT = startDT;
            DateTime day = DateTime2Int.DateTime(DT).Date;

            while ((!stopFlag) && (LastTick != null) && (LastBar != null) && (LastTick.DT <= endDT))
            {
                // Рассчет исполнения активных заявок 
                !!!

                #region  Вызов события onBar
                if (newBarFlag)
                {
                    EventHandler<BarsEventArgs> onBarHandler = onBar;
                    if (onBarHandler != null)
                        onBarHandler(this, new BarsEventArgs(Bars, LastBar));
                    newBarFlag = false;
                }
                #endregion

                // Получаю следующий тик
                IBar newLastTick = ticks.GetNext(lastTick);

                #region Inc(DT);// В зависимости от того, существуют ли подписчики на onSec и onNewDay увиличиваю DT инкриментом или значением из нового тика

                EventHandler<EventArgs> onSecHandler = onSec;
                EventHandler<EventArgs> onNewDayHandler = onNewDay;

                int toDT = endDT;
                if (newLastTick != null)
                    toDT = newLastTick.DT;

                if ((onSecHandler != null) || (onNewDayHandler != null))
                {
                    while (toDT > DT)
                    {
                        DT += 1;
                        onSecHandler(this, null);
                        if (day != DateTime2Int.DateTime(DT).Date)
                        {
                            onNewDayHandler(this, null);
                            day = DateTime2Int.DateTime(DT).Date;
                        }
                    }
                }
                else
                    DT = toDT;

                #endregion

                lastTick = newLastTick;
                if ((lastTick != null) && (lastTick.DT > LastBar.EndDT))
                {
                    LastBar = Bars.GetNext(LastBar);
                    newBarFlag = true;
                }
            }
        }
        public void Stop()
        {
            stopFlag = true;
        }

        public event EventHandler<BarsEventArgs> onBar;
        public event EventHandler<BarsEventArgs> onTick;
        public event EventHandler<DealEventArgs> onDeal;
        public event EventHandler<EventArgs> onSec;
        public event EventHandler<EventArgs> onNewDay;
     }
}
