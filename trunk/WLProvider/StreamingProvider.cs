using System;
using System.Collections.Generic;
using System.Text;

using WealthLab;

namespace OpenWealth.WLProvider
{
    public class StreamingProvider : StreamingDataProvider
    {
        static ILog l = Core.GetLogger(typeof(StreamingProvider).FullName);
        IDataManager data;
        bool m_Connect = false;

        public StreamingProvider()
        {
            this.data = Core.GetGlobal("data") as IDataManager;
            if (this.data == null)
                throw new Exception("Не найден модуль data");
        }

        public override void ConnectStreaming(IConnectionStatus connStatus)
        {
            l.Debug("ConnectStreaming");
            m_Connect = true;
        }
        public override void DisconnectStreaming(IConnectionStatus connStatus)
        {
            l.Debug("DisconnectStreaming");
            m_Connect = false;
        }

        StaticProvider staticProvider = new StaticProvider();
        public override StaticDataProvider GetStaticProvider()
        {
            l.Debug("GetStaticProvider");
            return staticProvider;
        }

        protected override void Subscribe(string symbol)
        {
            l.Debug("Subscribe " + symbol);
            IBars bars = data.GetBars(data.GetSymbol(symbol), data.GetScale(ScaleEnum.tick, 1));            
            bars.NewBarEvent += bars_NewBarEvent;
        }

        protected override void UnSubscribe(string symbol)
        {
            if (symbol == string.Empty)
                l.Debug("UnSubscribe symbol == string.Empty");
            else
            {
                l.Debug("UnSubscribe " + symbol);
                IBars bars = data.GetBars(data.GetSymbol(symbol), data.GetScale(ScaleEnum.tick, 1));
                bars.NewBarEvent -= bars_NewBarEvent;
            }
        }

        void bars_NewBarEvent(object sender, BarsEventArgs e)
        {
            l.Debug("bars_NewBarEvent ");
            if (!m_Connect)
                return;

            Quote q = new Quote();

            q.Symbol = (sender as IBars).symbol.ToString();
            q.TimeStamp = e.bar.dt;
            q.Ask = e.bar.close;
            q.Bid = e.bar.close;
            q.Open = e.bar.close;
            q.PreviousClose = e.bar.close;
            q.Price = e.bar.close;
            q.Size = e.bar.volume;

            UpdateQuote(q);
        }
/* static MarketInfo marketInfo = new MarketInfo();

        public override MarketInfo GetMarketInfo(string symbol)
        {
            return marketInfo;
        }
*/

        #region Descriptive

        public override string Description { get { return "Provides realtime stock data from OpenWealth"; } }
        public override string FriendlyName { get { return "OpenWealth"; } }
        public override System.Drawing.Bitmap Glyph { get { return OpenWealth.WLProvider.Properties.Resources.Image1; } }
        public override bool IsConnected { get { return m_Connect; } }
        public override bool StreamingAtDisconnect { get { return !m_Connect; } }
        public override string URL { get { return "http://openwealth.ru/"; } }

        #endregion Descriptive

    }
}
/*
using WealthLab.Extensions.Attribute;

// Information for the Extension Manager
[assembly: ExtensionInfo(
    ExtensionType.Provider,
    "Yahoo WMS",
    "Yahoo! Finance Static, Real-Time and Fundamental",
    "Yahoo! Finance provides end-of-day data for U.S. and world equities, mutual funds, indices and futures.",
    "1.3.4.0",
    "Wealth-Lab Management Software",
    "WealthLab.DataProviders.Yahoo.Resources.Yahoo.png",
    ExtensionLicence.Freeware,
    new string[] { "WealthLab.DataProviders.Yahoo.dll", 
                   "WealthLab.DataProviders.Yahoo.pdb"},
    PublisherUrl = @"http://www.wealth-lab.com")]
*/