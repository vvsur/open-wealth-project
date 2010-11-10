using System;
using System.Collections.Generic;

namespace OpenWealth.Simple
{
    /// <summary>
    /// Тик, реализующий IBar
    /// </summary>
    public class Tick : IBar
    {
        public DateTime dt { get; protected set; }
        public Int64 number { get; protected set; }
        public double open { get { return m_Price; } }
        public double high { get { return m_Price; } }
        public double low { get { return m_Price; } }
        public double close { get { return m_Price; } }
        public int volume { get; protected set; }
        public IDictionary<string, Object> ext { get; private set; }

        double m_Price;

        public Tick(DateTime dt, Int64 number, Double price, int volume)
        {
            this.dt = dt;
            this.number = number;
            m_Price = price;
            this.volume = volume;
            ext = new Dictionary<string, Object>();
        }
        public Tick(DateTime dt, Int64 number, Double price, int volume, IDictionary<string, Object> ext)
        {
            this.dt = dt;
            this.number = number;
            m_Price = price;
            this.volume = volume;
            this.ext = ext;
        }

        public override string ToString()
        {
            return string.Concat("Tick ", number, " ", dt, " ", m_Price, " ", volume);
        }

        #region Lock
        System.Threading.ReaderWriterLock m_lock = new System.Threading.ReaderWriterLock();
        public System.Threading.ReaderWriterLock Lock { get { return m_lock; } }
        #endregion Lock
    }
}
