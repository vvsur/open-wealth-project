using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IMarket
    {
        string Name { get; }
        IDictionary<string, Object> Ext { get; }
    }
}
