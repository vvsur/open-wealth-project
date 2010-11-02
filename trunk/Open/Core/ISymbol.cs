using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface ISymbol
    {
        string Name { get; }      
        IMarket Market { get; }
        IDictionary<string, Object> Ext { get; }
        string ToString();
    }
}
