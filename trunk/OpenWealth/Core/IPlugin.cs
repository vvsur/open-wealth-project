using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IPlugin
    {
        void Init();
        string name { get; }
        bool isDataSource { get; }
    }
}
