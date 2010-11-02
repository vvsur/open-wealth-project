using System;
using System.Collections.Generic;

namespace OpenWealth.Data
{
    public class Market : IMarket
    {
        public string Name { get; private set; }

        internal Market(string name)
        {
            this.Name = name;
            this.Ext = new Dictionary<string, Object>();
        }
        public IDictionary<string, Object> Ext { get; private set; }
    }
}
