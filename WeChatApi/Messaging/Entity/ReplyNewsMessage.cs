using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 回复图文消息
    /// </summary>
    public class ReplyNewsMessage : MessageBase, IReplyMessage
    {
        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图,限制为10条以内
        /// </summary>
        public virtual List<Article> Articles { get; set; }
        /// <summary>
        /// 位0x0001被标志时，星标刚收到的消息
        /// </summary>
        public virtual int FuncFlag { get; set; }


        public MessageTypes GetMsgType()
        {
            return MessageTypes.News;
        }
    }
    public class Article
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }


        private string _url, _picUrl;
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        [JsonProperty("url")]
        public virtual string Url
        {
            get { return _url; }
            set { _url = value.Resolve(); }
        }

        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
        /// </summary>
        [JsonProperty("picurl")]
        public virtual string PicUrl
        {
            get { return _picUrl; }
            set { _picUrl = value.Resolve(); }
        }
    }

}
