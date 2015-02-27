using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class UnSubscribeEventMessage : EventMessage
    {

        public override MessageTypes GetMsgType()
        {
            return MessageTypes.UnSubscribe;
        }
    }
}
