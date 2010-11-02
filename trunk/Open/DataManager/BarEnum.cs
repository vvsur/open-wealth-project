using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Data
{
    public class BarEnum : System.Collections.IEnumerator
    {
        public IList<IBar> m_bars;
        int position = -1;

        public BarEnum(IList<IBar> bars)
        {
            m_bars = bars;
        }

        public bool MoveNext()
        {
            position++;
            return (position < m_bars.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public IBar Current
        {
            get
            {
                try
                {
                    return m_bars[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
