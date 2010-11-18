using System.Drawing;
using WealthLab;

namespace TestProvider
{
    public class StreamingProvider : StreamingDataProvider
    {
        public override string Description { get { return "OpenWealth.Test.StreamingProvider"; } }
        public override string FriendlyName { get { return "OpenWealthTestStreamingProvider"; } }
        public override Bitmap Glyph { get { return new Bitmap(16, 16); } }

        public override bool IsConnected { get { return true; } }
        protected override void Subscribe(string symbol)
        {
            TestBars.NewBarEvent+=new System.EventHandler<BarEventArgs>(TestBars_NewBarEvent);
        }

        protected override void UnSubscribe(string symbol)
        {
            TestBars.NewBarEvent -= new System.EventHandler<BarEventArgs>(TestBars_NewBarEvent);
        }

        void TestBars_NewBarEvent(object sender, BarEventArgs e)
        {
            Quote q = new Quote();

            q.TimeStamp = TestBars.bars.Date[e.barIndex];

            q.Ask = TestBars.bars.Close[e.barIndex];
            q.Bid = TestBars.bars.Close[e.barIndex];
            q.Open = TestBars.bars.Close[e.barIndex];
            q.PreviousClose = TestBars.bars.Close[e.barIndex];
            q.Price = TestBars.bars.Close[e.barIndex];
            q.Size = 1;
            q.Symbol = "TestSymbol";
            
            //Hearbeat(q.TimeStamp); // Why do I need this method?
            //UpdateMiniBar(q, q.Price, q.Price, q.Price);
            UpdateQuote(q); // What's wrong?
        }
        public override StaticDataProvider GetStaticProvider()
        {
            return new StaticProvider();
        }
    }
}