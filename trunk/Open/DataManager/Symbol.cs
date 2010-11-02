using System;
using System.Collections.Generic;

namespace OpenWealth.Data
{
    public class Symbol : ISymbol
    {
        public string Name { get; private set; }        
        public IMarket Market { get; private set; }

        internal Symbol(IMarket market, string name)
        {
            this.Name = name;
            Market = market;            
            this.Ext = new Dictionary<string, Object>();
        }
        public IDictionary<string, Object> Ext { get; private set; }

        public override string ToString()
        {
            if (Market.Name == string.Empty)
                return Name;
            else
                return Name + "." + Market.Name;
        }

    }
}
