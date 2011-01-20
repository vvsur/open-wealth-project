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

        ISymbol GetSymbol(string marketName, string name);
        ISymbol GetSymbol(string marketNameDotSymbolName);
        void DelSymbol(ISymbol symbol);

        IScale GetScale(ScaleEnum scale, int interval, int beginning);
        IScale GetScale(ScaleEnum scale, int interval);

        IMarket GetMarket(string name);
        IEnumerable<IMarket> GetMarkets();

        // TODO передавать в событии ссылку на добавляемый рынок
        // TODO событие новый Bars
        event EventHandler<EventArgs> ChangeMarkets;
    }
}