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

            IData data = Core.GetGlobal("data") as IData;
            if (data != null)
                foreach (ISymbol symbol in data.symbols)
                    txtSymbols.Text += symbol.name + " ";
            txtSymbols.Text = txtSymbols.Text.Trim();
        }

        public string Symbols() { return txtSymbols.Text; }

    }
}
