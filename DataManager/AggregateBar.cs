using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.DataManager
{
    public class AggregateBar : OpenWealth.Simple.Bar, IBar
    {
        static ILog l = Core.GetLogger(typeof(AggregateBar).FullName);

        public void AddTick(IBar tick)
        {
            if (l.IsDebugEnabled)
                l.Debug("AddTick Добавляю тик " + tick);

            if (Double.IsNaN(Open))
            {
                Open = tick.Open;
                High = tick.High;
                Low = tick.Low;
                Close = tick.Close;
            }
            else
            {
                this.Close = tick.Close;
                if (this.High < tick.High)
                    this.High = tick.High;
                if (this.Low > tick.Low)
                    this.Low = tick.Low;
            }
            this.Volume += tick.Volume;
        }

        public void Clear()
        {
            Open = float.NaN;
            High = float.MinValue;
            Low = float.MaxValue;
            Close = float.NaN;
            Volume = 0;
        }

        public AggregateBar(int dt, int endDT, int number,
            float open, float high, float low, float close, int volume)
            : base(dt, endDT, number, open, high, low, close, volume)
        {            
        }

        public AggregateBar(int dt, int endDT, int number,
            float price, int volume)
            : base(dt, endDT, number, price, price, price, price, volume)
        {
        }
    }
}
