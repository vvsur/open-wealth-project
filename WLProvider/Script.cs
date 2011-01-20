using System;
using System.Collections.Generic;
using System.Text;

using WealthLab;

namespace OpenWealth.WLProvider
{
    public abstract class WealthScript : WealthLab.WealthScript
    {
        static ILog l = Core.GetLogger(typeof(WealthScript).FullName);

        new void PrintDebug(string msg)
        {
            l.Debug(msg);
            base.PrintDebug(msg);
        }

        protected int firstValidBar = -1;
        protected int bar = -1;

        protected override void Execute()
        {
            ResetFlag();
            Init();

            if (firstValidBar == -1) 
                PrintDebug("Значение firstValidBar не определено");
            else
                if (IsStreaming)
                {
                    bar = Bars.Count - 1;
                    if (bar >= firstValidBar)
                        onBar();
                }
                else
                    for (bar = firstValidBar; bar < Bars.Count; bar++)
                        onBar();
        }

        protected abstract void Init(); // выполняется перед циклом в Execute (в бою будет вызываться на каждый бар)
        protected abstract void onBar(); // выполняется на каждом шаге цикла в Execute (в бою будет вызываться на новый bar)

        #region Реализация торговых сигналов
        public WealthLab.Position BuyAtLimit(double limitPrice) { return BuyAtLimit(limitPrice, string.Empty); }
        public WealthLab.Position BuyAtLimit(double limitPrice, string signalName)
        {
            PrintDebug(Alerts.Count);
            WealthLab.Position result = base.BuyAtLimit(bar + 1, limitPrice, signalName);
            PrintDebug(Alerts.Count);
            return result;
        }

        public WealthLab.Position ShortAtLimit(double limitPrice) { return ShortAtLimit(limitPrice, string.Empty); }
        public WealthLab.Position ShortAtLimit(double limitPrice, string signalName)
        {
            PrintDebug(Alerts.Count);
            WealthLab.Position result = base.ShortAtLimit(bar + 1, limitPrice, signalName);
            PrintDebug(Alerts.Count);
            return result;
        }

        public bool CoverAtLimit(WealthLab.Position pos, double limitPrice) { return CoverAtLimit(pos, limitPrice, string.Empty); }
        public bool CoverAtLimit(WealthLab.Position pos, double limitPrice, string signalName)
        {
            PrintDebug(Alerts.Count);
            bool result = base.CoverAtLimit(bar + 1, pos, limitPrice, signalName);
            PrintDebug(Alerts.Count);
            return result;
        }

        public bool SellAtLimit(WealthLab.Position pos, double limitPrice) { return SellAtLimit(pos, limitPrice, string.Empty); }
        public bool SellAtLimit(WealthLab.Position pos, double limitPrice, string signalName)
        {
            PrintDebug(Alerts.Count);
            bool result = base.SellAtLimit(bar + 1, pos, limitPrice, signalName);
            PrintDebug(Alerts.Count);
            return result;
        }

        public bool ExitAtLimit(WealthLab.Position pos, double limitPrice) { return ExitAtLimit(pos, limitPrice, string.Empty); }
        public bool ExitAtLimit(WealthLab.Position pos, double limitPrice, string signalName)
        {
            PrintDebug(Alerts.Count);
            bool result = base.ExitAtLimit(bar + 1, pos, limitPrice, signalName);
            PrintDebug(Alerts.Count);
            return result;
        }

        #endregion Реализация торговых сигналов

        #region Переопределение логики торговых сигналов WealthLab

        void ResetFlag()
        {
            atClosePrintDebugFlag = true;
            atMarketPrintDebugFlag = true;
            atStop = true;
            atTrailingStop = true;
            splitPositionPrintDebugFlag = true;
            barDebugFlag = true;
        }

        bool atClosePrintDebugFlag = true;
        public new Position BuyAtClose(int bar)
        {
            if (atClosePrintDebugFlag)
            {
                atClosePrintDebugFlag = false;
                PrintDebug("AtClose команды не поддерживаются в OpenWealth. Объясните мне рыночный смысл данных команд!");
            }
            return null;
        }
        public new Position BuyAtClose(int bar, string signalName) { return BuyAtClose(0); }
        public new Position ShortAtClose(int bar) { return BuyAtClose(0); }
        public new Position ShortAtClose(int bar, string signalName) { return BuyAtClose(0); }
        public new bool CoverAtClose(int bar, Position pos) { BuyAtClose(0); return false; }
        public new bool CoverAtClose(int bar, Position pos, string signalName) { BuyAtClose(0); return false; }
        public new bool ExitAtClose(int bar, Position pos) { BuyAtClose(0); return false; }
        public new bool ExitAtClose(int bar, Position pos, string signalName) { BuyAtClose(0); return false; }
        public new bool SellAtClose(int bar, Position pos) { BuyAtClose(0); return false; }
        public new bool SellAtClose(int bar, Position pos, string signalName) { BuyAtClose(0); return false; }

        bool atMarketPrintDebugFlag = true;
        public new Position BuyAtMarket(int bar)
        {
            if (atMarketPrintDebugFlag)
            {
                atMarketPrintDebugFlag = false;
                PrintDebug("AtMarket команды пока не поддерживаются в OpenWealth. Планы по их реализации есть. Пока предлагаю использовать лимитированную, в которой указывать цену на 5% хуже текущей.");
            }
            return null;
        }
        public new Position BuyAtMarket(int bar, string signalName) { return BuyAtMarket(0); }
        public new Position ShortAtMarket(int bar) { return BuyAtMarket(0); }
        public new Position ShortAtMarket(int bar, string signalName) { return BuyAtMarket(0); }
        public new bool CoverAtMarket(int bar, Position pos) { BuyAtMarket(0); return false; }
        public new bool CoverAtMarket(int bar, Position pos, string signalName) { BuyAtMarket(0); return false; }
        public new bool ExitAtMarket(int bar, Position pos) { BuyAtMarket(0); return false; }
        public new bool ExitAtMarket(int bar, Position pos, string signalName) { BuyAtMarket(0); return false; }
        public new bool SellAtMarket(int bar, Position pos) { BuyAtMarket(0); return false; }
        public new bool SellAtMarket(int bar, Position pos, string signalName) { BuyAtMarket(0); return false; }

        bool atStop = true;
        public new Position BuyAtStop(int bar, double stopPrice)
        {
            if (atStop)
            {
                atStop = false;
                PrintDebug("AtStop команды пока не поддерживаются в OpenWealth. Планы по их реализации есть.");
            }
            return null;
        }
        public new Position BuyAtStop(int bar, double stopPrice, string signalName) { return BuyAtStop(0, 0); }
        public new Position ShortAtStop(int bar, double stopPrice) { return BuyAtStop(0, 0); }
        public new Position ShortAtStop(int bar, double stopPrice, string signalName) { return BuyAtStop(0, 0); }
        public new bool CoverAtStop(int bar, Position pos, double stopPrice) { BuyAtStop(0, 0); return false; }
        public new bool CoverAtStop(int bar, Position pos, double stopPrice, string signalName) { BuyAtStop(0, 0); return false; }
        public new bool ExitAtStop(int bar, Position pos, double price) { BuyAtStop(0, 0); return false; }
        public new bool ExitAtStop(int bar, Position pos, double price, string signalName) { BuyAtStop(0, 0); return false; }
        public new bool SellAtStop(int bar, Position pos, double stopPrice) { BuyAtStop(0, 0); return false; }
        public new bool SellAtStop(int bar, Position pos, double stopPrice, string signalName) { BuyAtStop(0, 0); return false; }

        bool atTrailingStop = true;
        public new bool CoverAtTrailingStop(int bar, Position pos, double stopPrice)
        {
            if (atTrailingStop)
            {
                atTrailingStop = false;
                PrintDebug("AtTrailingStop команды пока не поддерживаются в OpenWealth. Планы по их реализации есть.");
            }
            return false;
        }
        public new bool CoverAtTrailingStop(int bar, Position pos, double stopPrice, string signalName) { return CoverAtTrailingStop(0, null, 0); }
        public new bool CoverAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct) { return CoverAtTrailingStop(0, null, 0); }
        public new bool CoverAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct, string signalName) { return CoverAtTrailingStop(0, null, 0); }
        public new bool ExitAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct) { return CoverAtTrailingStop(0, null, 0); }
        public new bool ExitAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct, string signalName) { return CoverAtTrailingStop(0, null, 0); }
        public new bool SellAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct) { return CoverAtTrailingStop(0, null, 0); }
        public new bool SellAtAutoTrailingStop(int bar, Position pos, double triggerPct, double profitReversalPct, string signalName) { return CoverAtTrailingStop(0, null, 0); }
        public new bool ExitAtTrailingStop(int bar, Position pos, double stopPrice) { return CoverAtTrailingStop(0, null, 0); }
        public new bool ExitAtTrailingStop(int bar, Position pos, double stopPrice, string signalName) { return CoverAtTrailingStop(0, null, 0); }
        public new bool SellAtTrailingStop(int bar, Position pos, double stopPrice) { return CoverAtTrailingStop(0, null, 0); }
        public new bool SellAtTrailingStop(int bar, Position pos, double stopPrice, string signalName) { return CoverAtTrailingStop(0, null, 0); }

        bool splitPositionPrintDebugFlag = true;
        public new Position SplitPosition(Position position, double percentToRetain)
        {
            if (splitPositionPrintDebugFlag)
            {
                splitPositionPrintDebugFlag = false;
                PrintDebug("SplitPosition пока не поддерживаются в OpenWealth. Планы по их реализации есть.");
            }
            return null;
        }

        bool barDebugFlag = true;
        public new Position BuyAtLimit(int bar, double limitPrice)
        {
            if (barDebugFlag)
            {
                barDebugFlag = false;
                PrintDebug("В стратегии OpenWealth не надо указывать bar совершения сделки. Все сделки происходят на следующем баре.");
            }
            return null;
        }
        public new Position BuyAtLimit(int bar, double limitPrice, string signalName) { return BuyAtLimit(0, 0); }
        public new Position ShortAtLimit(int bar, double limitPrice) { return BuyAtLimit(0, 0); }
        public new Position ShortAtLimit(int bar, double limitPrice, string signalName) { return BuyAtLimit(0, 0); }
        public new bool CoverAtLimit(int bar, Position pos, double limitPrice) { BuyAtLimit(0, 0); return false; }
        public new bool CoverAtLimit(int bar, Position pos, double limitPrice, string signalName) { BuyAtLimit(0, 0); return false; }
        public new bool ExitAtLimit(int bar, Position pos, double price) { BuyAtLimit(0, 0); return false; }
        public new bool ExitAtLimit(int bar, Position pos, double price, string signalName) { BuyAtLimit(0, 0); return false; }
        public new bool SellAtLimit(int bar, Position pos, double limitPrice) { BuyAtLimit(0, 0); return false; }
        public new bool SellAtLimit(int bar, Position pos, double limitPrice, string signalName) { BuyAtLimit(0, 0); return false; }

        #endregion Переопределение логики торговых сигналов WealthLab
    }
}