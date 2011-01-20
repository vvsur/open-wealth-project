using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public interface IBot : IDescription
    {
        void Init(IBotHost BotHost);
        void Start();
        void Stop();
    }
}
