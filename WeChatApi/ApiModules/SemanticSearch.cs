/********
 * 
 *  Description:
 *  语义理解接口
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *  Revision History:
 *  Date                  Who                 What
 *  2014-11-16          guoxin.lai          Created
 * 
 ********/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Nlab.WeChatApi
{
    public class SemanticSearch
    {
        /// <summary>
        /// 语义理解接口
        /// 
        /// </summary>
        /// <param name="q">查询</param>
        /// <returns> 返回理解理解查询结果（json）</returns>
        public string GetSemantic(SemanticSearchEntity q)
        {
            var token = AccessTokenManager.Current.GetToken();
            var url = string.Format("https://api.weixin.qq.com/semantic/semproxy/search?access_token={0}", token.Value);

            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "post";

            var reqStream = req.GetRequestStream();
            var inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(q);
            var reqBuffer = System.Text.Encoding.UTF8.GetBytes(inputJson);
            reqStream.Write(reqBuffer, 0, reqBuffer.Length);

            var resp = req.GetResponse() as HttpWebResponse;
            var contentType = resp.ContentType;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                return reader.ReadToEnd();
            }
        }


        public class SemanticSearchEntity
        {
            [Newtonsoft.Json.JsonProperty("query")]
            public String Query { get; set; }

            [Newtonsoft.Json.JsonProperty("category")]
            public string Category { get; set; }

            [Newtonsoft.Json.JsonProperty("latitude")]
            public float Latitude { get; set; }

            [Newtonsoft.Json.JsonProperty("longitude")]
            public string Longitude { get; set; }

            [Newtonsoft.Json.JsonProperty("city")]
            public string City { get; set; }

            [Newtonsoft.Json.JsonProperty("region")]
            public string Region { get; set; }

            [Newtonsoft.Json.JsonProperty("appid")]
            public string AppId { get; set; }
        }
    }
}
