/*****
 * 企业号 Access Token
 * 
 *Author: 赖国欣(guoxin.lai@gmail.com) 
 * Created : 2014-07-02
 * *****/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 企业号Access Token
    /// </summary>
    public class QyTokenProvider : AccesTokenProviderBase
    {
        private static object syncObj = new object();

        public override AccessToken GetToken()
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", AppId, AppSecret);
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "get";

            var resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var resultJson = reader.ReadToEnd();
                var resultObj = JsonConvert.DeserializeObject<ResultEntity>(resultJson);
                if (resultObj.ErrCode != 0)
                {
                    throw new WechatApiException(resultObj.ErrCode, resultObj.ErrMsg);
                }
                else
                {
                    var token = JsonConvert.DeserializeObject<AccessToken>(resultJson);
                    token.LastAccess = DateTime.Now;
                    return token;
                }
            }
        }

    }
}
