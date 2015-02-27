using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class TextMessage : MessageBase, IIncomeMessage
    {
        public virtual string Content { get; set; }
        public virtual Int64 MsgId { get; set; }


        public MessageTypes GetMsgType()
        {
            return MessageTypes.Text;
        }
    }
}
