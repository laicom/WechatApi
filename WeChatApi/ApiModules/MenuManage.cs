/*****
 * Wechat media content operation
 * 
 *Author: 赖国欣(guoxin.lai@gmail.com) 
 * Created : 2014-07-02
 * *****/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Nlab.WeChatApi
{
    public class MenuManage
    {
        public string CreateMenu(string menuJson)
        {
            var token = AccessTokenManager.Current.GetToken();
            string url;
            if (Config.WeChatConfigManager.IsCorperation)
            {
                url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/create?access_token={0}&agentid={1}", token.Value, Config.WeChatConfigManager.CorperationAgentId);
            }
            else
            {
                url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", token.Value);
            }
            var req = WebRequest.Create(url) as HttpWebRequest;

            req.Method = "post";
            req.ContentType = "text/json";
            var stream = req.GetRequestStream();
            var buffer = Encoding.UTF8.GetBytes(menuJson);
            stream.Write(buffer, 0, buffer.Length);

            var resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var result = reader.ReadToEnd();
                //var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PushResultEntity>(resultJson);
                Logger.NlabLogger.Log("Wechat menu create result:" + result);
                return result;
            }
        }

  
    }
}
