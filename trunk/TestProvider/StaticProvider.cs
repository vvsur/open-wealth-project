using System;
using System.Collections.Generic;

using System.Drawing;
using WealthLab;
using System.Windows.Forms;

namespace TestProvider
{
    public class StaticProvider : StaticDataProvider
    {
        public override string Description { get { return  "OpenWealth.Test.StaticProvider"; } }
        public override string FriendlyName { get { return "OpenWealth.Test.StaticProvider"; } }
        public override Bitmap Glyph { get { return new Bitmap(16, 16); } }

        public override DataSource CreateDataSource()
        {
            DataSource ds = new DataSource(this);

            ds.DSString = "TestSymbol";
            ds.Scale = BarScale.Second;
            ds.BarInterval = 1;

            return ds;
        }

        public override void PopulateSymbols(DataSource ds, List<string> symbols)
        {
            symbols.Clear();
            symbols.Add("TestSymbol");
        }

        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            return TestBars.bars;
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return true;
        }
        
        public override UserControl WizardFirstPage()
        {
            return new UserControl();
        }

        public override UserControl WizardNextPage(System.Windows.Forms.UserControl currentPage)
        {
            return null;
        }

        public override UserControl WizardPreviousPage(System.Windows.Forms.UserControl currentPage)
        {
            return null;
        }
    }
}