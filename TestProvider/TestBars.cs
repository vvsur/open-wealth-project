using System;

using WealthLab;

namespace TestProvider
{
    public static class TestBars
    {
        static TestBars()
        {
            bars = new Bars("TestSymbol", BarScale.Second, 1);
            for (int i = 0; i < 10; ++i)
                NewTick(null, null);

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new System.Timers.ElapsedEventHandler(NewTick);
            t.Interval = 250;
            t.Enabled = true;
        }

        static int secNum = 0;
        static TimeFor timeFor = TimeFor.Open;

        static void NewTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            double price;
            switch (timeFor)
            {
                case TimeFor.Open:
                    price = secNum + 2;
                    timeFor = TimeFor.Low;
                    bars.Add(new DateTime(2010, 10, 10, 0, 0, secNum), price, price, price, price, 1);
                    break;
                case TimeFor.Low:
                    price = secNum + 1;
                    timeFor = TimeFor.High;
                    bars.Low[secNum] = price;
                    bars.Close[secNum] = price;
                    bars.Volume[secNum] = 2;
                    break;
                case TimeFor.High:
                    price = secNum + 4;
                    timeFor = TimeFor.Close;
                    bars.High[secNum] = price;
                    bars.Close[secNum] = price;
                    bars.Volume[secNum] = 3;
                    break;
                case TimeFor.Close:
                    price = secNum + 3;
                    timeFor = TimeFor.Open;
                    bars.Close[secNum] = price;
                    bars.Volume[secNum] = 4;
                    ++secNum;
                    break;
            }

            if (NewBarEvent != null)
                NewBarEvent(null, new BarEventArgs(bars.Count-1));            
        }

        public static Bars bars { get; private set; }
        public static event EventHandler<BarEventArgs> NewBarEvent;
    }

    enum TimeFor { Open, Low, High, Close };

    public class BarEventArgs : EventArgs
    {
        public int barIndex;
        public BarEventArgs(int barIndex)
        {
            this.barIndex = barIndex;
        }
    }    
}
