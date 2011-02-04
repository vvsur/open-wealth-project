using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// tick - тиковый "тайм" фрейм
    /// sec - временной "тайм" фрейм
    /// volume - в данный момент не поддерживается
    /// month  - в данный момент не поддерживается
    /// </summary>
    public enum ScaleEnum { undefined = 0, tick = 1, sec = 2, volume = 3, month = 4 } // TODO расширить данный enum

    /// <summary>
    /// Интерфейс, описывающий интервалы свечей
    /// 
    /// Например для минуток 
    ///    scaleType = sec
    ///    interval = 60
    ///    
    /// beginning используется, например, если необходимо чтобы часовые свечи начинались в 10:30 а не в 10:00
    /// </summary>
    public interface IScale
    {
        ScaleEnum scaleType { get; }
        int interval { get; }
        int beginning { get; }
        string ToString();
        bool CanConvertTo(IScale scale);
        TimeSpan ToTimeSpan();
    }
}
