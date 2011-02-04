using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Simple
{
    /// <summary>
    /// Реализации полноценной свечи
    /// Для тиков лучше использовать Tick
    /// </summary>
    public class Bar : IBar
    {
        private readonly static DateTime dt1970 = new DateTime(1970, 1, 1);

        public int DT { get; protected set; }
        public int EndDT { get; protected set; }
        public int Number { get; protected set; }
        public float Open { get; protected set; }
        public float High { get; protected set; }
        public float Low { get; protected set; }
        public float Close { get; protected set; }
        public int Volume { get; protected set; }
        public DateTime GetDateTime()
        {
            return dt1970 + new TimeSpan(DT * TimeSpan.TicksPerSecond);
        }
        
        /*        public Bar(int dt, int endDT, int number, float open, float high, float low, float close, int volume)
        {
            this.DT = dt;
            this.EndDT = endDT;
            this.Number = number;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }
        */

        public Bar(int dt, int endDT, int number, float open, float high, float low, float close, int volume)
        {
            this.DT = dt;
            this.EndDT = endDT;
            this.Number = number;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }

        public override string ToString()
        {
            return string.Concat("Bar ", GetDateTime().ToString("yyyyMMss hhmmss"), " ", Number, " ", Open, " ", High, " ", Low, " ", Close, " ", Volume);
        }
    }
}
