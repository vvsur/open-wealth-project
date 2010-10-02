using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
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
