using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class SubscribeEventMessage : EventMessage
    {

        public virtual String EventKey { get; set; }

        public override MessageTypes GetMsgType()
        {
            return MessageTypes.Subscribe;
        }
    }
}
