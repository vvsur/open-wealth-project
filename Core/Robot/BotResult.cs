using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public class BotResult
    {
        public BotResult(bool IsTest, DateTime StartDateTime, string DLL, string Name, IEnumerable<BotParam> BotParams, ISymbol Symbol, IScale Scale, int From)
        {
            this.IsTest = IsTest;
            this.StartDateTime = StartDateTime;
            this.DLL = DLL;
                this.Name = Name;
             this.Symbol = Symbol;
                 this.Scale = Scale;
            this.From = From;

            foreach (BotParam bp in BotParams)
                this.BotParams.Add(new BotParam(bp));
        }

        #region идентификация инструмента

        public readonly ISymbol Symbol;
        public readonly IScale Scale;
        public readonly int From;

        int till = int.MinValue;
        public int Till
        {
            get { return till; }
            set
            {
                if (readOnly)
                    throw new InvalidOperationException("RobotResult находится в состоянии ReadOnly");
                till = value;
            }
        }

        #endregion идентификация инструмента

        #region идентификация робота

        public readonly string DLL;
        public readonly string Name;
        public readonly IList<BotParam> BotParams = new List<BotParam>();

        #endregion идентификация робота

        #region результат торговли

        public readonly IList<IDeal> Deals = new List<IDeal>();  // надеюсь менять сделки в результате никто не дадумается :)
        public void AddDeal(IDeal deal)
        {
            lock (Deals)
            {
                Deals.Add(deal);
            }
        }

        double profit = double.NaN;
        double commisions = double.NaN;

        public double Profit
        {
            get { return profit; }
            set
            {
                if (readOnly)
                    throw new InvalidOperationException("RobotResult находится в состоянии ReadOnly");
                profit = value;
            }
        }

        public double Commisions
        {
            get { return commisions; }
            set
            {
                if (readOnly)
                    throw new InvalidOperationException("RobotResult находится в состоянии ReadOnly");
                commisions = value;
            }
        }

        #endregion результат торговли

        #region идентификация теста

        public readonly bool IsTest; // торговля шла реально, или тестово
        public readonly DateTime StartDateTime; // время старта теста

        int testDuration = -1;
        /// <summary>
        /// продолжительность работы теста (секунд)
        /// </summary> 
        public int TestDuration 
        { 
            get {return testDuration;}
            set 
            {
                if (readOnly)
                    throw new InvalidOperationException("RobotResult находится в состоянии ReadOnly");
                testDuration = value;
            }
        }


        #endregion идентификация теста

        bool readOnly = false;

        public BotResult(string xlmFile)
        {
            throw new NotImplementedException();
        }

        public void Save(string xlmFile)
        {
            throw new NotImplementedException();
        }
    }
}