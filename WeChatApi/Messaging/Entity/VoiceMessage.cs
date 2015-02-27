using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class VoiceMessage : MessageBase, IIncomeMessage
    {
        public virtual Int64 MsgId { get; set; }
        public virtual string MediaId { get; set; }
        public virtual string Format { get; set; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public virtual string Recognition { get; set; }

        public MessageTypes GetMsgType()
        {
            return MessageTypes.Voice;
        }
    }
}
