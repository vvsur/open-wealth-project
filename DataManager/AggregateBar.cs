using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace OpenWealth.Data
{
    public class AggregateBar : OpenWealth.Simple.Bar, IBar
    {
        static ILog l = Core.GetLogger(typeof(AggregateBar).FullName);

        public void AddTick(IBar tick)
        {
            if(l.IsDebugEnabled)
                l.Debug("AddTick Добавляю тик " + tick);
            Lock.AcquireWriterLock(1000);
            try
            {
                if (Double.IsNaN(open))
                {
                    open = tick.open;
                    high = tick.high;
                    low = tick.low;
                    close = tick.close;
                }
                else
                {
                    this.close = tick.close;
                    if (this.high < tick.high)
                        this.high = tick.high;
                    if (this.low > tick.low)
                        this.low = tick.low;
                }
                this.volume += tick.volume;
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }

        public void Clear()
        {
            open = Double.NaN;
            high = Double.MinValue;
            low = Double.MaxValue;
            close = Double.NaN;
            volume = 0;
        }

        public AggregateBar(DateTime dt, Int64 number,
            Double open, Double high, Double low, Double close, int volume)
            : base(dt, number, open, high, low, close, volume)
        {            
        }
    }
}
