using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IData
    {
        //TODO IBars Get(ISymbol symbol, IScale scale, DateTime from, DateTime to, int maxCount);
        //TODO IBars Get(ISymbol symbol, IScale scale, int count);
        IBars GetBars(ISymbol symbol, IScale scale);
        IBars GetBars(string symbol, ScaleEnum scale, int interval);

        IEnumerable<ISymbol> symbols { get; }
        ISymbol GetSymbol(string name);
        // TODO событие новый символ
        // TODO событие новый Bars

        IScale GetScale(ScaleEnum scale, int interval, DateTime beginning);
        IScale GetScale(ScaleEnum scale, int interval);
    }
}