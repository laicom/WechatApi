using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class VideoMessage : MessageBase, IIncomeMessage
    {
        public virtual Int64 MsgId { get; set; }
        public virtual string MediaId { get; set; }
        public virtual string Format { get; set; }

        public MessageTypes GetMsgType()
        {
            return MessageTypes.Video;
        }
    }
}
