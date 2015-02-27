/********
 * 
 *  Description:
 *  WeChat(微信）消息推送,pt客服接口
 *  
 *  Author 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Create Date   2014-7-3
 * 
 ********/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Nlab.WeChatApi
{

    public class MessagePusher
    {
        public string Token { get; set; }
        
        public MessagePusher() { }

        public MessagePusher(string token)
        {
            Token = token; 
        }

        public bool PushMessage<T>(T msg) where T: IPushMessage
        {
            if(string.IsNullOrEmpty(Token))
                Token = AccessTokenManager.Current.GetToken().Value;
            var jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Token;

            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";

            //req.Headers.Add("contentType", "text/json; charset=utf-8");
            var stream = req.GetRequestStream();
            var buffer = Encoding.UTF8.GetBytes(jsonstr);
            stream.Write(buffer, 0, buffer.Length);

            var resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var resultJson = reader.ReadToEnd();
                var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PushResultEntity>(resultJson);
                if (resultObj.ErrCode != 0)
                    throw new WechatApiException(resultObj.ErrCode, "PushMessage Failed  -- " + resultObj.ErrMsg);
                return true;
            }

        }


        class PushResultEntity
        {
            [Newtonsoft.Json.JsonProperty("errcode")]
            public int ErrCode { get; set; }

            [Newtonsoft.Json.JsonProperty("errmsg")]
            public string ErrMsg { get; set; }
        }
    }
}
