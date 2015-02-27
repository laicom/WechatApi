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
    public class MediaDataOp
    {
        public GetMediaResultEntity GetMedia(string mediaId)
        {
            var token = AccessTokenManager.Current.GetToken();
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Value, mediaId);

            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "get";

            var resp = req.GetResponse() as HttpWebResponse;
            var contentType = resp.ContentType;
            var result = new GetMediaResultEntity() { ErrCode = 0, ContentType=contentType };

            //audio/amr ,text/plain , image/jpg , video/mp4 ,
            switch (contentType)
            {
                case "text/plain":
                    using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                    {
                        var resultJson = reader.ReadToEnd();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetMediaResultEntity>(resultJson);
                        return result;
                    }
                default:
                    var stream = resp.GetResponseStream();
                    using (var ms = new MemoryStream())
                    {
                        for (int b = stream.ReadByte(); b != -1; b = stream.ReadByte())
                        {
                            ms.WriteByte((byte)b);
                        }
                        var buffer = new byte[(int)ms.Length];
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        ms.Read(buffer, 0, buffer.Length);
                        result.Data = buffer;
                        var ext=contentType.Split('/')[1];
                        result.FileName = string.Format("{0}.{1}", mediaId, ext);
                        return result;
                    }
            }
        }

  
        public class GetMediaResultEntity
        {
            [Newtonsoft.Json.JsonProperty("errcode")]
            public int ErrCode { get; set; }

            [Newtonsoft.Json.JsonProperty("errmsg")]
            public string ErrMsg { get; set; }

            public string ContentType { get; set; }
            public byte[] Data { get; set; }
            public string FileName { get; set; }
        }
    }
}
