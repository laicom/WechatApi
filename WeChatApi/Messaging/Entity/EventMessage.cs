using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class EventMessage : MessageBase, IIncomeMessage
    {
        public virtual string Event { get; set; }


        public virtual MessageTypes GetMsgType()
        {
            return MessageTypes.Event;
        }
    }
}
