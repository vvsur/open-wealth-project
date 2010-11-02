using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace OpenWealth.Data
{
    public class Data : IDataManager, IPlugin, IDescription
    {
        static ILog l = Core.GetLogger(typeof(Data).FullName);

        public Data()
        {
            if (Core.GetGlobal("data") != null)
            {
                //l.Error("переменная DataStorage уже установленна");
            }
            Core.SetGlobal("data", this);
        }

        //public IBars Get(ISymbol symbol, IScale scale, DateTime from, DateTime to, int maxCount);
        //public IBars Get(ISymbol symbol, IScale scale, int count);

        List<IMarket> markets = new List<IMarket>();
        public IMarket GetMarket(string name)
        {
            lock (markets)
            {
                foreach (IMarket m in markets)
                    if (m.Name == name)
                        return m;

                IMarket market = new Market(name);
                markets.Add(market);
                return market;
            }
        }

        public IEnumerable<IMarket> Markets { get { return markets; } }

        Dictionary<string, IBars> barss = new Dictionary<string, IBars>();
        public IBars GetBars(ISymbol symbol, IScale scale)
        {
            if (l.IsDebugEnabled)
                l.Debug("GetBars " + symbol + " " + scale);
            lock (barss)
            {
                string key = symbol + "." + scale;
                if (barss.ContainsKey(key))
                    return barss[key];

                IBars newBars;
                if ((scale.scaleType == ScaleEnum.tick) && (scale.interval == 1))
                    newBars = new Ticks(symbol, scale);
                else
                    newBars = new AggregateBars(this, symbol, scale);

                barss.Add(key, newBars);
                return newBars;
            }
        }

        public IBars GetBars(string marketName, string symbol, ScaleEnum scale, int interval)
        {
            return GetBars(GetSymbol(marketName, symbol), GetScale(scale, interval));
        }

        Dictionary<string, ISymbol> symbols = new Dictionary<string, ISymbol>();

        public IEnumerable<ISymbol> Symbols { get { return symbols.Values; } }
        public ISymbol GetSymbol(string marketName, string name)
        {
            if ((marketName == null) || (name == null))
            {
                l.Error("GetSymbol ((marketName == null) || (name == null))");
                throw new ArgumentNullException("GetSymbol ((marketName == null) || (name == null))");
            }

            lock (symbols)
            {
                string nameDotMarketName = name + "." + marketName;
                if (symbols.ContainsKey(nameDotMarketName))
                    return symbols[nameDotMarketName];
                ISymbol newSymbol = new Symbol(GetMarket(marketName), name);
                symbols.Add(nameDotMarketName, newSymbol);
                return newSymbol;
            }
        }

        public ISymbol GetSymbol(string marketNameDotName)
        {
            string[] split = marketNameDotName.Split('.');
            if (split.Length != 2)
            {
                l.Info("Не могу распарсить название бумаги "+ marketNameDotName);
                GetSymbol(string.Empty, marketNameDotName);
                return null;
            }
            return GetSymbol(split[1], split[0]);
        }

        List<IScale> scales =new List<IScale>();
        public IScale GetScale(ScaleEnum scale, int interval, DateTime beginning)
        {
            foreach (IScale s in scales)
                if ((s.beginning == beginning) && (s.scaleType == scale) && (s.interval == interval))
                    return s;

            IScale newS = new Scale(scale, interval, beginning);
            scales.Add(newS);
            return newS;
        }
        public IScale GetScale(ScaleEnum scale, int interval)
        {
            return GetScale(scale, interval, DateTime.MinValue);
        }

        #region Реализация интерфейса IPlugin

        public void Init() { } // всё что нужно, сделано в конструкторе
        
        #endregion Реализация интерфейса IPlugin

        #region Реализация интерфейса IDescription
        public string Name { get { return "Модуль Данные"; } }
        public string Description { get { return "Получает данные от провайдеров и отдает желающим. Не сохраняет данные."; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion Реализация интерфейса IDescription

    }
}
