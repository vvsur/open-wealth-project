using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IBacktestBotHost : IBotHost
    {
        void Init();
        void Stop();

        BotResult Result { get; }
    }
}
