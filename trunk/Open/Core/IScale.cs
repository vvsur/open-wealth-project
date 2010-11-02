using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public enum ScaleEnum { undefined = 0, tick = 1, sec = 2, volume = 3, month = 4 } // TODO расширить данный enum

    public interface IScale
    {
        ScaleEnum scaleType { get; }
        int interval { get; }
        DateTime beginning { get; }
        string ToString();
        bool CanConvertTo(IScale scale);
        TimeSpan ToTimeSpan();
    }
}
