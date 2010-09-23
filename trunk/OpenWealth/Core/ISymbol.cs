using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface ISymbol
    {
        string name { get; }
        IDictionary<string, Object> ext { get; }
    }
}
