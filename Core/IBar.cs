using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий общие сведения о "свече"
    /// Если свеча описывает тики, то open=high=low=close
    /// Пример реализации тиков OpenWealth.Simple.Tick
    /// Пример реализации полноценной свечи OpenWealth.Simple.Bar
    /// </summary>
    public interface IBar
    {
        int DT { get; }
        DateTime GetDateTime();
        int Number { get; }
        float Open { get; }
        float High { get; }
        float Low { get; }
        float Close { get; }
        int Volume { get; }
    }
}