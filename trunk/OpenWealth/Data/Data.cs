using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Data
{
    public class Data : IData, IPlugin
    {
        public Data()
        {
            _symbols = new List<ISymbol>();

            if (Core.GetGlobal("data") != null)
            {
                //l.Error("переменная DataStorage уже установленна");
            }
            Core.SetGlobal("data", this);
        }

        //public IBars Get(ISymbol symbol, IScale scale, DateTime from, DateTime to, int maxCount);
        //public IBars Get(ISymbol symbol, IScale scale, int count);

        List<IBars> barss = new List<IBars>();
        public IBars GetBars(ISymbol symbol, IScale scale)
        {
            lock (barss)
            {
                foreach (IBars bars in barss)
                    if ((bars.symbol == symbol) && (bars.scale == scale))
                        return bars;

                IBars newBars;
                if ((scale.scaleType == ScaleEnum.tick) && (scale.interval == 1))
                    newBars = new Ticks(symbol, scale);
                else
                    newBars = new AggregateBars(this, symbol, scale);

                barss.Add(newBars);
                return newBars;
            }
        }

        public IBars GetBars(string symbol, ScaleEnum scale, int interval)
        {
            return GetBars(GetSymbol(symbol), GetScale(scale, interval));
        }

        List<ISymbol> _symbols;
        public IEnumerable<ISymbol> symbols { get { return _symbols; } }
        public ISymbol GetSymbol(string name)
        {
            foreach (ISymbol symbol in _symbols)
                if (symbol.name == name)
                    return symbol;
            ISymbol newSymbol = new Symbol(name);
            _symbols.Add(newSymbol);
            return newSymbol;
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

        public string name { get { return "Модуль Данные"; } }

        public bool isDataSource { get { return false; } }
        
        #endregion Реализация интерфейса IPlugin
    }
}
