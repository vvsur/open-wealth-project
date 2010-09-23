using System;
using System.Collections.Generic;
using System.Text;

using WealthLab;

namespace OpenWealth.WLProvider
{
    class StreamingProvider : StreamingDataProvider
    {
        static ILog l = Core.GetLogger(typeof(StreamingProvider).FullName);
        IData data;
        bool m_Connect = false;

        public StreamingProvider()
        {
            // инициализация ядра и плагинов
            l.Debug("StreamingProvider()");
            if (Core.SingleInit())
            {
                Core.Init(
                    System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(
                            System.Reflection.Assembly.GetExecutingAssembly().Location
                                                       )
                        , "OWPlugin")
                      );
                Core.LoadPlugin(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Core.LoadPlugin();
                Core.InitPlugin();
            }
            this.data = Core.GetGlobal("data") as IData;
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
        public override StaticDataProvider GetStaticProvider()
        {
            l.Debug("GetStaticProvider");
            return new StaticProvider();
        }

        protected override void Subscribe(string symbol)
        {
            l.Debug("Subscribe " + symbol);
            IBars bars = data.GetBars(data.GetSymbol(symbol), data.GetScale(ScaleEnum.tick, 1));
            bars.NewBarEvent += new EventHandler<BarsEventArgs>(bars_NewBarEvent);
        }

        protected override void UnSubscribe(string symbol)
        {
            l.Debug("UnSubscribe " + symbol);
            IBars bars = data.GetBars(data.GetSymbol(symbol), data.GetScale(ScaleEnum.tick, 1));
            bars.NewBarEvent -= new EventHandler<BarsEventArgs>(bars_NewBarEvent);
        }

        void bars_NewBarEvent(object sender, BarsEventArgs e)
        {
            l.Debug("bars_NewBarEvent ");
            if (!m_Connect)
                return;

            Quote q = new Quote();

            q.TimeStamp = e.bar.dt;

            q.Ask = e.bar.close;
            q.Bid = e.bar.close;
            q.Open = e.bar.close;
            q.PreviousClose = 0;
            q.Price = e.bar.close;
            q.Size = e.bar.volume;
            q.Symbol = (sender as IBars).symbol.name;

            //Hearbeat(q.TimeStamp); // Зачем нужен данный метод?

            //UpdateMiniBar(q, q.Price, q.Price, q.Price);
            UpdateQuote(q); // не устанавливает 
        }


        #region Descriptive

        public override string Description { get { return "Provides realtime stock data from OpenWealth"; } }
        public override string FriendlyName { get { return "OpenWealth"; } }
        public override System.Drawing.Bitmap Glyph { get { return OpenWealth.WLProvider.Properties.Resources.Image1; } }
        public override bool IsConnected { get { return m_Connect; } }
        public override bool StreamingAtDisconnect { get { return !m_Connect; } }
        public override string URL { get { return "http://openwealth.ru/"; } }

        #endregion Descriptive





        /* в FidelityStreamingProvider не реализованно

        public IConnectionStatus ConnectionStatus { get; }
        public IDataHost DataHost { get; }

        public void ClearRequests(IStreamingUpdate requestor);
        public virtual void DisconnectStreaming();
        public List<string> GetSymbolsSubscribed(IStreamingUpdate requestor);
        public virtual void Initialize(IDataHost dataHost);
        public bool IsSymbolStreaming(string symbol, IStreamingUpdate requestor);
        public static void SetBadTickFilterSettings(bool badTickFilter, double threshold);
        public void Subscribe(string symbol, IStreamingUpdate streamingUpdate);
        public void UnSubscribe(string symbol, IStreamingUpdate streamingUpdate);

        #region реализация интерфейса IStreamingUpdate
        public void Hearbeat(DateTime timeStamp);
        public void UpdateMiniBar(Quote q, double open, double high, double low);
        public void UpdateQuote(Quote q);
        #endregion реализация интерфейса IStreamingUpdate
 */ 
    }
}



/*
        public void UpdateMiniBar(Quote q, double open, double high, double low)
        {
            using (Dictionary<StreamingRequest, int>.KeyCollection.Enumerator enumerator = d.Keys.GetEnumerator())
            {
                StreamingRequest current;
            Label_0030:
                if (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    current.Request.UpdateMiniBar(q, open, high, low);
                    goto Label_0030;
                }
            }
        }


 
        public void UpdateQuote(Quote q)
        {
            if (q.TimeStamp == DateTime.MinValue)
                return;
            else
            {
                if (!h)
                {
                    this.a(q);
                    return;
                }
                this.a(q);
                this.g[q.Symbol] = q;
            }
        }

 
        private void a(string A_0)
        {
            Quote quote;
            string[] strArray;
            int num;
            int num2;
            string[] strArray3;
            double num3;
            FidelityStreamingProvider.a a;
            int num6;
            FidelityStreamingProvider.a a2;
            DateTime time;
            TimeSpan span;
            int num7;

            quote = new Quote();
            string[] strArray2 = A_0.Split(new char[] { ',' })[0].Split(new char[] { '=' });
            quote.Symbol = strArray2[1];
            strArray = A_0.Split(new char[] { '~' });
            num = 0;
            num2 = 1;

            if (1 < strArray.Length)
            {
                strArray3 = strArray[num2].Split(new char[] { '=' });
                if (!(strArray3[0] == "FpsTradePriceLast"))
                {
                    num7 = 15;
                }
                else
                {
                    quote.Price = double.Parse(strArray3[1]);
                    num7 = 0x29;
                }
                goto Label_0005;
            }
            else
            {
                num7 = 0x35;
            }
            goto Label_0005;
        Label_0233:
            num2++;

            if (num2 < strArray.Length)
            {
                strArray3 = strArray[num2].Split(new char[] { '=' });
                if (!(strArray3[0] == "FpsTradePriceLast"))
                {
                    num7 = 15;
                }
                else
                {
                    quote.Price = double.Parse(strArray3[1]);
                    num7 = 0x29;
                }
                goto Label_0005;
            }
            else
            {
                num7 = 0x35;
            }
            goto Label_0005;


        Label_049A:
            num7 = 0x31;
            goto Label_0005;
        Label_051E:
            num++;
            num7 = 1;
            goto Label_0005;
        Label_07B2:
            time = DateTime.Now;
            time = this.r.ConvertLocalTimeToNative(time);
            if (quote.TimeStamp <= time)
            {
                base.UpdateQuote(quote);
                return;
            }
            span = (TimeSpan)(quote.TimeStamp - time);// если текущее время 
            if (span.TotalHours <= 5.0)
            {
                base.UpdateQuote(quote);
            }
            return;

        Label_0005:
            switch (num7)
            {
                case 0:
                case 1:
                case 0x25:
                case 0x26:
                case 0x27:
                case 0x2a:
                case 0x33:
                case 0x37:
                    goto Label_0233;

                case 4:
                    f.Add(quote.Symbol, quote.Price);
                    num7 = 0x2f;
                    goto Label_0005;


                case 7:
                    if (!(strArray3[0] == "FpsTradeSize"))
                    {
                        num7 = 0x2d;
                    }
                    else
                    {
                        num7 = 0x2c;
                    }
                    goto Label_0005;


                case 10:
                case 0x2f:
                    num++;
                    num7 = 0x27;
                    goto Label_0005;

                case 12:
                    goto Label_051E;


                case 15:
                    if (!(strArray3[0] == "FpsTradeVolTotal"))
                    {
                        num7 = 7;
                    }
                    else
                    {
                        num7 = 0x1c;
                    }
                    goto Label_0005;

                case 0x10:
                    if (!(strArray3[0] == "FpsAskPrice"))
                    {
                        num7 = 0x15;
                    }
                    else
                    {
                        num7 = 0x2b;
                    }
                    goto Label_0005;

                case 0x11:
                    if (!(strArray3[0] == "FpsTradePriceClose"))
                    {
                        num7 = 0x10;
                    }
                    else
                    {
                        quote.PreviousClose = double.Parse(strArray3[1]);
                        if (g.ContainsKey(quote.Symbol))
                        {
                            g[quote.Symbol] = quote.PreviousClose;
                            num7 = 0x1a;
                        }
                        else
                        {
                            num7 = 0x13;
                        }
                    }
                    goto Label_0005;

                case 0x12:
                    quote.Bid = double.Parse(strArray3[1]);
                    num7 = 0x33;
                    goto Label_0005;

                case 0x13:
                    g.Add(quote.Symbol, quote.PreviousClose);
                    num7 = 0x2e;
                    goto Label_0005;

                case 20:
                    a.a = a.b;
                    a.b = num3 / 100.0;
                    a.c = a.b - a.a;
                    this.q[quote.Symbol] = a;
                    num7 = 0x2a;
                    goto Label_0005;

                case 0x15:
                    if (!(strArray3[0] == "FpsBidPrice"))
                    {
                        goto Label_0233;
                    }
                    num7 = 0x12;
                    goto Label_0005;

                case 0x16:
                    if (num6 >= 0xb3b0)
                    {
                        goto Label_051E;
                    }
                    num7 = 0x22;
                    goto Label_0005;

                case 0x18:
                    quote.Price = f[quote.Symbol];
                    num7 = 0x19;
                    goto Label_0005;

                case 0x19:
                    if (!g.ContainsKey(quote.Symbol))
                    {
                        goto Label_049A;
                    }
                    num7 = 0x34;
                    goto Label_0005;

                case 0x1a:
                case 0x2e:
                    num++;
                    num7 = 0x37;
                    goto Label_0005;


                case 0x1c:
                    num7 = 0x23;
                    goto Label_0005;

                case 0x1d:
                    quote.TimeStamp = quote.TimeStamp.AddSeconds(1.0);
                    quote.Size = a2.c;
                    a2.c = 0.0;
                    this.q[quote.Symbol] = a2;
                    goto Label_07B2;

                case 30:
                    if (num <= 0)
                    {
                        return;
                    }
                    num7 = 0x18;
                    goto Label_0005;

                case 0x20:
                    num7 = 0x16;
                    goto Label_0005;

                case 0x22:
                    this.k.Change(num6, 0xea60);
                    num7 = 12;
                    goto Label_0005;

                case 0x23:
                    if (strArray.Length != 2)
                    {
                        goto Label_0233;
                    }
                    num7 = 50;
                    goto Label_0005;

                case 0x24:
                    if (((num6 <= 0xfa0) ? 0 : 1) != 0)
                    {
                    }
                    num7 = 0x20;
                    goto Label_0005;

                case 40:
                    if (!this.q.TryGetValue(quote.Symbol, out a))
                    {
                        a.a = 0.0;
                        a.b = num3 / 100.0;
                        a.c = num3;
                        this.q.Add(quote.Symbol, a);
                        num7 = 0;
                    }
                    else
                    {
                        num7 = 20;
                    }
                    goto Label_0005;

                case 0x29:
                    if (f.ContainsKey(quote.Symbol))
                    {
                        f[quote.Symbol] = quote.Price;
                        num7 = 10;
                    }
                    else
                    {
                        num7 = 4;
                    }
                    goto Label_0005;

                case 0x2b:
                    quote.Ask = double.Parse(strArray3[1]);
                    num7 = 0x25;
                    goto Label_0005;

                case 0x2c:
                    quote.Size = double.Parse(strArray3[1]);
                    num++;
                    num7 = 0x26;
                    goto Label_0005;

                case 0x2d:
                    if (!(strArray3[0] == "FpsTradeTimeLast"))
                    {
                        num7 = 0x11;
                    }
                    else
                    {
                        num7 = 0x36;
                    }
                    goto Label_0005;

                case 0x30:
                    goto Label_049A;

                case 0x31:
                    if (!this.q.TryGetValue(quote.Symbol, out a2))
                    {
                        goto Label_07B2;
                    }
                    num7 = 0x1d;
                    goto Label_0005;

                case 50:
                    num3 = double.Parse(strArray3[1]);
                    a = new FidelityStreamingProvider.a();
                    num7 = 40;
                    goto Label_0005;

                case 0x34:
                    quote.PreviousClose = g[quote.Symbol];
                    num7 = 0x30;
                    goto Label_0005;

                case 0x35:
                    num7 = 30;
                    goto Label_0005;

                case 0x36:
                    {
                        quote.TimeStamp = DateTime.Parse(strArray3[1]);
                        int second = quote.TimeStamp.Second;
                        int num5 = (60 - second) + 5;
                        num6 = num5 * 0x3e8;
                        num7 = 0x24;
                        goto Label_0005;
                    }
            }
        }
 
 */

