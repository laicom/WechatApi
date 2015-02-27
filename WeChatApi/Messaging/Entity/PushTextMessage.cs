using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nlab.WeChatApi
{
    public class PushTextMessage : IPushMessage
    {
        private string _msgType = "text";
        /// <summary>
        /// 消息类型
        /// </summary>
        [Newtonsoft.Json.JsonProperty("msgtype")]
        public virtual string MsgType
        {
            get { return _msgType; }
            set { ; }
        }

        /// <summary>
        /// 接收方微信号
        /// </summary>
        [JsonProperty("touser")]
        public virtual string ToUserName { get; set; }

        [JsonProperty("text")]
        public TextEntity Text { get; set; }

        public class TextEntity
        {
            [JsonProperty("content")]
            public string Content { get; set; }
        }
    }
}
