using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OpenWealth.Simple
{
    public partial class SelectSymbol : UserControl
    {
        public SelectSymbol()
        {
            InitializeComponent();
        }

        public IList<ISymbol> GetSelectedSymbols()
        {
            IList<ISymbol> checkedSymbols = new List<ISymbol>();
            for (int i = 0; i < symbolCheckedListBox.Items.Count; ++i)
            {
                ISymbol s = symbolCheckedListBox.Items[i] as ISymbol;
                if ((symbolCheckedListBox.GetItemChecked(i)) && (s != null))
                    checkedSymbols.Add(s);
            }
            return checkedSymbols;
        }

        private void SelectSymbol_Load(object sender, EventArgs e)
        {
            if(!this.DesignMode)
            {
                Core.Data.ChangeMarkets += new EventHandler<EventArgs>(Data_ChangeMarkets);
                Data_ChangeMarkets(this, null);
            }
        }

        IList<IMarket> checkedMarkets = new List<IMarket>();        

        void Data_ChangeMarkets(object sender, EventArgs e)
        {
            marketCheckedListBox.BeginUpdate();
            marketCheckedListBox.Items.Clear();
            foreach (IMarket market in Core.Data.GetMarkets()) // TODO надо лочить
                marketCheckedListBox.Items.Add(market, checkedMarkets.Contains(market));
            marketCheckedListBox.EndUpdate();
        }

        void Market_ChangeSymbols(object sender, EventArgs e)
        {
            IList<ISymbol> checkedSymbols = GetSelectedSymbols();

            symbolCheckedListBox.BeginUpdate();

            symbolCheckedListBox.Items.Clear();
            for (int i = 0; i < marketCheckedListBox.Items.Count; ++i)
            {
                IMarket m = marketCheckedListBox.Items[i] as IMarket;
                if ((m != null) && (checkedMarkets.Contains(m)))
                {
                    IEnumerable<ISymbol> symbols = m.GetSymbols(); // TODO надо лочить
                    foreach (ISymbol s in symbols)
                        symbolCheckedListBox.Items.Add(s, checkedSymbols.Contains(s));
                }
            }
            symbolCheckedListBox.EndUpdate();
        }

        private void marketCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            IMarket m = marketCheckedListBox.Items[e.Index] as IMarket;
            if (m != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (!checkedMarkets.Contains(m))
                    {
                        checkedMarkets.Add(m);
                        m.ChangeSymbols += Market_ChangeSymbols;
                    }
                }
                else
                {
                    checkedMarkets.Remove(m);
                    m.ChangeSymbols -= Market_ChangeSymbols;
                }
            }
            Market_ChangeSymbols(m, null);
        }

        private void allMarketButton_Click(object sender, EventArgs e)
        {
            bool Checked = (marketCheckedListBox.Items.Count != marketCheckedListBox.CheckedItems.Count);

            marketCheckedListBox.BeginUpdate();
            for (int i = 0; i < marketCheckedListBox.Items.Count; ++i)
                marketCheckedListBox.SetItemChecked(i, Checked);
            marketCheckedListBox.EndUpdate();
        }

        private void allSymbolButton_Click(object sender, EventArgs e)
        {
            bool Checked = (symbolCheckedListBox.Items.Count != symbolCheckedListBox.CheckedItems.Count);

            symbolCheckedListBox.BeginUpdate();
            for (int i = 0; i < symbolCheckedListBox.Items.Count; ++i)
                symbolCheckedListBox.SetItemChecked(i, Checked);
            symbolCheckedListBox.EndUpdate();
        }

    }
}
