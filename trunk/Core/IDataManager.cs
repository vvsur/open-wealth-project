using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IDataManager
    {
        //TODO как регистрируются провайдеры данных? 

        //TODO IBars Get(ISymbol symbol, IScale scale, DateTime from, DateTime to, int maxCount);
        //TODO IBars Get(ISymbol symbol, IScale scale, int count);
        IBars GetBars(ISymbol symbol, IScale scale);
        IBars GetBars(string marketName, string symbol, ScaleEnum scale, int interval);

        IEnumerable<ISymbol> Symbols { get; }
        ISymbol GetSymbol(string marketName, string name);
        ISymbol GetSymbol(string marketNameDotName);
        // TODO событие новый символ
        // TODO событие новый Bars

        IScale GetScale(ScaleEnum scale, int interval, DateTime beginning);
        IScale GetScale(ScaleEnum scale, int interval);

        IMarket GetMarket(string name);
        IEnumerable<IMarket> Markets { get; }
        // TODO событие новый рынок
    }
}