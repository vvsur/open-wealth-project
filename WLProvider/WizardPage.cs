using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OpenWealth.WLProvider
{
    public partial class WizardPage : UserControl
    {
        public WizardPage()
        {
            InitializeComponent();
        }
        public void Initialize()
        {
            // Clear entered symbols
            txtSymbols.Clear(); //TODO Загрузка настроек

            StringBuilder sb = new StringBuilder();
            IDataManager data = Core.GetGlobal("data") as IDataManager;
            if (data != null)
            {
                IEnumerable<IMarket> ms = data.GetMarkets();
                foreach (IMarket m in ms)
                {
                    IEnumerable<ISymbol> symbols = m.GetSymbols();
                    foreach (ISymbol symbol in symbols)
                    {
                        sb.Append(symbol);
                        sb.Append(" ");
                    }
                }
            }

            txtSymbols.Text = sb.ToString().Trim();
        }

        public string Symbols() { return txtSymbols.Text; }

    }
}
