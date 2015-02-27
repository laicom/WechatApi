/*****
 * Wechat Access Token manager
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
    /// Access Token Entity
    /// </summary>
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }

        public DateTime LastAccess { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public bool HasExpired
        {
            get
            {
                return DateTime.Now.Subtract(LastAccess) > TimeSpan.FromSeconds(ExpiresIn+3);
            }
        }
    }

}
