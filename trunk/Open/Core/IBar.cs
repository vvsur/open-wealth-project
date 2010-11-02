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
        DateTime dt { get; }
        Int64 number { get; }
        Double open { get; }
        Double high { get; }
        Double low { get; }
        Double close { get; }
        int volume { get; }
        IDictionary<string, Object> ext { get; }

        System.Threading.ReaderWriterLock Lock { get; }
    }
}
