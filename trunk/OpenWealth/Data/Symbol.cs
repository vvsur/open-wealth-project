using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Data
{
    public class Symbol : ISymbol
    {
        public string name { get; private set; }
        //public IEnumerable<string> synonyms { get; private set; }
        internal Symbol(string name)
        {
            this.name = name;
            this.ext = new Dictionary<string, Object>();
        }
        public IDictionary<string, Object> ext { get; private set; }
    }
}
