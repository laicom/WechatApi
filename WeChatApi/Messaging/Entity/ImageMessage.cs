using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class ImageMessage : MessageBase, IIncomeMessage
    {
        public virtual string PicUrl { get; set; }
        public virtual Int64 MsgId { get; set; }
        public virtual string MediaId { get; set; }

        public MessageTypes GetMsgType()
        {
            return MessageTypes.Image;
        }
    }
}
