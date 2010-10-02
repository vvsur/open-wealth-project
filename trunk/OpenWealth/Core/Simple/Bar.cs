using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Simple
{
    public class Bar : IBar
    {
        public DateTime dt { get; protected set; }
        public Int64 number { get; protected set; }
        public Double open { get; protected set; }
        public Double high { get; protected set; }
        public Double low { get; protected set; }
        public Double close { get; protected set; }
        public int volume { get; protected set; }
        public IDictionary<string, Object> ext { get; private set; }

        public Bar(DateTime dt, Int64 number, 
            Double open, Double high, Double low, Double close, int volume)
        {
            this.dt = dt;
            this.number = number;
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            this.volume = volume;
            ext = new Dictionary<string, Object>();
        }
        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }
        #endregion Lock
    }
}
