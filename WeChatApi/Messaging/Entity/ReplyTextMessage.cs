using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 消息回复
    /// </summary>
    public class ReplyTextMessage : MessageBase, IReplyMessage
    {
        /// <summary>
        /// 回复的消息内容,长度不超过2048字节
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 位0x0001被标志时，星标刚收到的消息
        /// </summary>
        public virtual int FuncFlag { get; set; }


        public MessageTypes GetMsgType()
        {
            return MessageTypes.Text;
        }
    }
}
