/*****
 * Access Token Manager
 * 
 *Author: 赖国欣(guoxin.lai@gmail.com) 
 * Created : 2014-07-08
 * updated:
 *      2014-11-15 支持企业号接入
 * *****/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Nlab.WeChatApi.Config;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// Access Token Manager
    /// </summary>
    public class AccessTokenManager
    {
        private static ConcurrentDictionary<string, IAccessTokenProvider> managerTab = new ConcurrentDictionary<string, IAccessTokenProvider>();
        private static object syncObj = new object();

        /// <summary>
        /// 获取当前应用的AccessToken，依赖于当前AppSetting的配置
        /// </summary>
        public static IAccessTokenProvider Current
        {
            get
            {
                var appId = Config.WeChatConfigManager.WechatAppId;
                IAccessTokenProvider provider;
                managerTab.TryGetValue(appId, out provider);
                if (provider != null) return provider;

                var appSecret = Config.WeChatConfigManager.WeChatAppSecret;
                var isCorperation = Config.WeChatConfigManager.IsCorperation;
                return Create(appId,appSecret,isCorperation);
            }
        }

        public static IAccessTokenProvider Create(string appId, string appSecret,bool isCorperation)
        {
            IAccessTokenProvider provider;
            managerTab.TryGetValue(appId, out provider);

            if (provider == null)
            {
                lock (syncObj)
                {
                    if (provider == null)
                    {
                        if (isCorperation) //企业号
                            provider = new QyTokenProvider();
                        else
                            provider = new MpTokenProvider();

                        managerTab[appId] = provider;
                        provider.AppId = appId;
                        provider.AppSecret = appSecret;
                    }
                }
            }
            return provider;           
        }
 

    }

}
