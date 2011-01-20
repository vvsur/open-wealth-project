using System;
using System.Collections.Generic;

namespace OpenWealth.DataManager
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

        public override string ToString()
        {
            if (Name != string.Empty)
                return Name;
            else
                return "NoName";
        }

        IList<ISymbol> symbols = new List<ISymbol>();
        public IEnumerable<ISymbol> GetSymbols()
        {
            return symbols;
        }

        public ISymbol GetSymbol(string name)
        {
            ISymbol newSymbol = null;

            lock (symbols)
            {
                foreach (ISymbol s in symbols)
                    if (s.Name == name)
                        return s;

                newSymbol = new Symbol(this, name);
                symbols.Add(newSymbol);
            }

            EventHandler<EventArgs> changeSymbols = ChangeSymbols;
            if (changeSymbols != null)
                changeSymbols(this, null);

            return newSymbol;
        }

        public event EventHandler<EventArgs> ChangeSymbols;
    }
}
