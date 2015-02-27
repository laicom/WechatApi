/*****
 * Wechat Access to userinfo fucntion
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
    public class UserInfoAccess
    {
        public UserInfo GetUserInfo(string userOpenId)
        {
            var token = AccessTokenManager.Current.GetToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token.Value, userOpenId);

            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "get";

            var resp = req.GetResponse() as HttpWebResponse;
            UserInfo user = null; 
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var resultJson = reader.ReadToEnd();
                var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultEntity>(resultJson);
                if (resultObj.ErrCode != 0)
                    throw new Exception(string.Format("GetUserInfo Failed  -- [{0}]:{1}", resultObj.ErrCode, resultObj.ErrMsg));
                else
                {
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo>(resultJson);
                }
            }
            return user;
        }

        /// <summary>
        /// //拉取用户列表(openid 列表)
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<string> GetAllUserIdList()
        {
            var token = AccessTokenManager.Current.GetToken();

            IEnumerable<string> result = new List<string>();

            var nextOpentId = string.Empty;
            for (var i = 0; i < 100; i++) 
            {
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", token.Value, nextOpentId);

                var req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "get";

                var resp = req.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    var resultJson = reader.ReadToEnd();
                    var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultEntity>(resultJson);
                    if (resultObj.ErrCode != 0)
                        throw new Exception(string.Format("GetAllUserIdList Failed  -- [{0}]:{1}", resultObj.ErrCode, resultObj.ErrMsg));
                    else
                    {
                        var userResult = Newtonsoft.Json.JsonConvert.DeserializeObject<UserIdListResultEntity>(resultJson);
                        if (string.IsNullOrEmpty(userResult.next_openid))
                            break;
                        else
                            nextOpentId = userResult.next_openid;
                        result=result.Concat(userResult.Data.OpenId);
                    }
                }
            }
            return result.ToList();
        }

        class UserIdListResultEntity
        {
            public int Total { get; set; }
            public int Count { get; set; }
            public UserIdListDataEntity Data{get;set;}

            public class UserIdListDataEntity
            {
                public List<string> OpenId { get; set; }
            }

            public string next_openid { get; set; }
        }
    }
}
