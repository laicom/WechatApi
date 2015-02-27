/********
 * 
 *  Description:
 *  WeChatApi 配置管理
 *  
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Revision History:
 *  Date                  Who                 What
 *  2013-07-30          guoxin.lai          Created
 * 
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Nlab.WeChatApi.Config
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class WeChatConfigManager
    {
        private static readonly string CnfigName = "WeChat";


        //private static Lazy<WeChatConfigSection> _configSection = new Lazy<WeChatConfigSection>(() =>
        //    {
        //        return ConfigurationManager.GetSection(CnfigName) as WeChatConfigSection;
        //    }
        //    );

        public static WeChatConfigSection Current
        {
            get
            {
                var config = ConfigurationManager.GetSection(CnfigName) as WeChatConfigSection;
                if (config == null)
                    throw new ConfigurationErrorsException("WeChat configuration section not found");
                return config;
            }
        }


        /// <summary>
        /// to get wechat token from web.config appsetting, you can override it
        /// </summary>
        public static string WeChatToken
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["WeChatAuthenToken"]; }
        }

        /// <summary>
        /// to get EncodingAESKey from web.config appsetting, you can override it
        /// </summary>
        public static string EncodingAESKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["EncodingAESKey"]; }
        }

        /// <summary>
        /// to get EncodingAESKey from web.config appsetting, you can override it
        /// </summary>
        public static string WeChatAppSecret
        {
            get { return ConfigurationManager.AppSettings["WeChatAppSecret"]; }
        }
        /// <summary>
        /// to get WeChatAppId from web.config appsetting, you can override it
        /// </summary>
        public static string WechatAppId
        {
            get 
            {
                var corpId = System.Configuration.ConfigurationManager.AppSettings["WeChatCorpId"];
                if (!string.IsNullOrEmpty(corpId))
                    return corpId;
                return System.Configuration.ConfigurationManager.AppSettings["WeChatAppId"]; 
            }
        }

        /// <summary>
        /// 企业应用Id
        /// </summary>
        public static string CorperationAgentId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CorperationAgentId"];
            }
        }

        /// <summary>
        /// JsApiList
        /// </summary>
        public static string JsApiList
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["JsApiList"];
            }
        }


        /// <summary>
        /// JsApiDebug
        /// </summary>
        public static bool JsApiDebug
        {
            get
            {
                bool result = false;
                var cfgValue= System.Configuration.ConfigurationManager.AppSettings["JsApiDebug"];
                bool.TryParse(cfgValue, out result);
                return result;
            }
        }


        /// <summary>
        /// Is corperation account
        /// </summary>
        public static bool IsCorperation
        {
            get
            {
                var corpId= System.Configuration.ConfigurationManager.AppSettings["WeChatCorpId"];
                return !string.IsNullOrEmpty(corpId);
            }
        }

        public static string GetTemplate(string templateName)
        {
            var tmp = Current.Templates.GetItemByKey(templateName);
            if (tmp == null)
                return string.Empty;
            if (!string.IsNullOrEmpty(tmp.Value))
                return tmp.Value;
            else if (!string.IsNullOrEmpty(tmp.Filepath))
            {
                if (File.Exists(tmp.Filepath))
                {
                    return File.ReadAllText(tmp.Filepath);
                }
                else
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        var filePath = System.Web.HttpContext.Current.Server.MapPath(tmp.Filepath);
                        if (File.Exists(filePath))
                        {
                            return File.ReadAllText(filePath);
                        }
                    }
                    //
                }
            }
            return string.Empty;
        }
    }
}
