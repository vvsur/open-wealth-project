using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Описание сущьности рынок (секция биржи)
    /// На рынок есть ссылка из ISymbol
    /// </summary>
    public interface IMarket
    {
        string Name { get; }
        IEnumerable<ISymbol> GetSymbols();
        ISymbol GetSymbol(string name);
        IDictionary<string, Object> Ext { get; }

        event EventHandler<EventArgs> ChangeSymbols;
    }
}
