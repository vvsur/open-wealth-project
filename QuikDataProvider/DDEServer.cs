using System;
using NDde.Server;

namespace OpenWealth.QuikDataProvider.DDE
{
    public class DDEServer : DdeServer
    {
        static ILog l = Core.GetLogger(typeof(DDEServer).FullName);

        public delegate void ConnectionEventDelegate(NDde.Server.DdeConversation conv);

        public delegate void PokeEventDelegate(
            NDde.Server.DdeConversation conv, string item, byte[] data, int format);

        public DDEServer(string service)
            : base(service)
        {
            l.Debug("DDEServer " + service);
        }

        protected override void OnAfterConnect(NDde.Server.DdeConversation conversation)
        {
            base.OnAfterConnect(conversation);
            l.Debug(String.Format("Topic : {0} has been connected!", conversation.Topic));
        }

        protected override void OnDisconnect(NDde.Server.DdeConversation conversation)
        {
            base.OnDisconnect(conversation);
            l.Debug(String.Format("Topic : {0} has been disconnected!", conversation.Topic));
        }


        protected override PokeResult OnPoke(NDde.Server.DdeConversation conversation, string item, byte[] data, int format)
        {
            l.Debug("OnPoke " + conversation.Topic);

            switch (conversation.Topic)
            {
                case "[ALL_DEAL]ALL_DEAL":
                    AllDealQueue.Add(data);
                    break;
            }
            return PokeResult.Processed;
        }

        
    }
}
