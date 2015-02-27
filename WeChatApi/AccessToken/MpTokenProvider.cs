/*****
 * 服务号、订阅号 Access Token
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
using System.Collections.Concurrent;
using System.Configuration;
using Newtonsoft.Json;

namespace Nlab.WeChatApi
{

    /// <summary>
    /// Access Token provider for subscribe/service public account
    /// 服务号、订阅号的实现
    /// </summary>
    public class MpTokenProvider : AccesTokenProviderBase
    {
        private static object syncObj = new object();

        private AccessToken currentToken;

        public override AccessToken GetToken()
        {
            if (currentToken == null || currentToken.HasExpired)
            {
                lock (syncObj)
                {
                    if (currentToken == null || currentToken.HasExpired)
                    {
                        RefreshTotken();
                    }
                }
            }
            //currentToken.LastAccess = DateTime.Now;
            return currentToken;
        }

        public virtual void RefreshTotken()
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppId, AppSecret);
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "get";

            var resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var resultJson = reader.ReadToEnd();
                var resultObj =JsonConvert.DeserializeObject<ResultEntity>(resultJson);
                if (resultObj.ErrCode != 0)
                {
                    throw new WechatApiException(resultObj.ErrCode,resultObj.ErrMsg);
                }
                else
                {
                    var token = JsonConvert.DeserializeObject<AccessToken>(resultJson);
                    token.LastAccess = DateTime.Now;
                    currentToken = token;
                }
            }
        }
    }
}
