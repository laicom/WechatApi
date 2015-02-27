using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nlab.WeChatApi
{
    public class PushNewsMessage : IPushMessage
    {
        private string _msgType = "news";
        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty("msgtype")]
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

        [JsonProperty("news")]
        public NewsEntity News { get; set; }



        public class NewsEntity
        {
            [JsonProperty("articles")]
            public List<Article> Articles { get; set; }
        }
    }
}
