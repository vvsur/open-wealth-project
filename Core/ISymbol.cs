using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий символ (инструмент, Контракт FORTS, Ценную бумагу и т.п.)
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Наименование символа
        /// </summary>
        string Name { get; }    
        /// <summary>
        /// Рынок, на которов данный инструмент торгуется
        /// </summary>
        IMarket Market { get; }
        /// <summary>
        /// Дополнительные параметры
        /// </summary>
        IDictionary<string, Object> Ext { get; }
        /// <summary>
        /// Возврящает значение "НазваниеРынка.НазваниеИнструмента"
        /// </summary>
        string ToString();
    }
}
