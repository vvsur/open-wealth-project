using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий Менеджера данных
    /// Менеджер данных является единым способом получения объектов IMarket, IScale, ISymbol, IBars
    /// 
    /// TODO надо переделать, чтобы в данном интерфейсе были только GetMarket, GetMarkets, GetScale. 
    ///      GetSymbol - должно вызываться в IMarket
    ///      GetBars - должно вызываться в ISymbol
    ///      т.е. цепочка такая, здесь получил IMarket, или зарегил новый IMarket
    ///                          далее в IMarket получил или зарегил ISymbol
    ///                          а уже в ISymbol делаешь GetBars
    /// </summary>

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