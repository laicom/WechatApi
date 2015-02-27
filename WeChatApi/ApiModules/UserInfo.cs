using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nlab.WeChatApi
{
    public class UserInfo
    {
        [JsonProperty("subscribe")]
        public virtual bool Subscribe { get; set; }

        [JsonProperty("openid")]
        public virtual string Openid { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [JsonProperty("sex")]
        public virtual string Sex { get; set; }

        [JsonProperty("city")]
        public virtual string City { get; set; }

        [JsonProperty("nickname")]
        public virtual string NickName { get; set; }

        [JsonProperty("country")]
        public virtual string Country { get; set; }


        [JsonProperty("province")]
        public virtual string Province { get; set; }

        public virtual string Address
        {
            get
            {
                return string.Format("{0}{1}{2}", Country, Province, City);
            }
        }


        [JsonProperty("language")]
        public virtual string Language { get; set; }


        [JsonProperty("headimgurl")]
        public virtual string Headimgurl { get; set; }


        [JsonProperty("subscribe_time")]
        public virtual int SubscribeTime { get; set; }


    }
}
