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
        IDictionary<string, Object> Ext { get; }
    }
}
