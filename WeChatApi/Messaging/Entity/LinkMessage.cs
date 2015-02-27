using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class LinkMessage : MessageBase,IIncomeMessage
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public virtual Int64 MsgId { get; set; }


        public MessageTypes GetMsgType()
        {
            return MessageTypes.Link;
        }
    }
}
