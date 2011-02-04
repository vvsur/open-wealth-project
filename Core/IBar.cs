using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий общие сведения о "свече"
    /// Если свеча описывает тики, то open=high=low=close и DT=EndDT
    /// Пример реализации тиков OpenWealth.Simple.Tick
    /// Пример реализации полноценной свечи OpenWealth.Simple.Bar
    /// </summary>
    public interface IBar
    {
        // Время начала свечи, преобразуется к int c помощью DateTime2Int.Int для увеличения быстродействия и уменьшения занимаемой памяти
        int DT { get; }
        // Время окончания свечи (сравнивая с текущим можно определить, закрыта ли свеча)
        int EndDT { get; }
        // Получает время начала свечи в виде DateTime
        DateTime GetDateTime();
        int Number { get; }
        float Open { get; }
        float High { get; }
        float Low { get; }
        float Close { get; }
        int Volume { get; }
    }
}