using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading;
using OpenWealth;

namespace TEST_bot_BackTestHost
{
    class BotHost : IBotHost
    {
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

        public IBars Ticks { get; private set; }
        public IBar LastTick { get; private set; }

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

        public int DT { get; private set; }
        public DateTime Now { get { return DateTime2Int.DateTime(DT); } }

        bool stopFlag, newBarFlag;
        public void Start()
        {
            stopFlag = false;
            newBarFlag = true;

            LastBar = Bars.Get(startDT);
            if (LastBar == null)
                LastBar = Bars.First;

            LastTick = Ticks.Get(startDT);
            if (LastTick == null)
                LastTick = Ticks.First;

            DT = startDT;
            DateTime Day = DateTime2Int.DateTime(DT).Date;

            while ((!stopFlag) && (LastTick.DT <= endDT))
            {
                // Рассчет исполнения активных заявок 
                throw new NotImplementedException();  onDeal!!!!

                #region  Вызов событий onTick onBar
                EventHandler<BarsEventArgs> onTickHandler = onTick;
                if (onTickHandler != null)
                    onTickHandler(this, new BarsEventArgs(Ticks, LastTick));

                if (newBarFlag)
                {
                    EventHandler<BarsEventArgs> onBarHandler = onBar;
                    if (onBarHandler != null)
                        onBarHandler(this, new BarsEventArgs(Bars, LastBar));
                    newBarFlag = false;
                }
                #endregion

                // Получаю следующий тик
                IBar newLastTick = Ticks.GetNext(LastTick);

                #region Inc(DT);// В зависимости от того, существуют ли подписчики на onSec и onNewDay увиличиваю DT инкриментом или значением из нового тика

                EventHandler<EventArgs> onSecHandler = onSec;
                EventHandler<EventArgs> onNewDayHandler = onNewDay;

                if ((onSecHandler != null) || (onNewDayHandler != null))
                    while (newLastTick.DT > DT)
                    {
                        DT += 1;
                        onSecHandler(this, null);
                        if (Day != DateTime2Int.DateTime(DT).Date)
                        {
                            onNewDayHandler(this, null);
                            Day = DateTime2Int.DateTime(DT).Date;
                        }
                    }
                else
                    DT = newLastTick.DT;

                #endregion

                LastTick = newLastTick;
                if (LastTick.DT > LastBar.EndDT)
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
