/********
 * 
 *  Description: 
 *  WeChatChainsHandler
 *  WeChat(微信）可配置插件式消息处理
 *  
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Revision History:
 *  Date                  Who                 What
 *  
 * 
 ********/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Nlab.WeChatApi.Config;
using System.Web;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// WeChat message process in chains model.
    /// will load reply modules from web.config
    /// </summary>
    public class WeChatConfigurableChainsHandler : WeChatChainsHandler, System.Configuration.IConfigurationSectionHandler
    {

        private List<IReplyHandleModule> LoadReplyModules()
        {
            var config = WeChatConfigManager.Current;

            var ModuleCol = config.ReplyModules;

            var moduleList = new List<IReplyHandleModule>();

            foreach (ReplySettingCollectionElement item in config.ReplySettings)
            {
                var typeName = ModuleCol[item.ModuleName].Type;
                var type = System.Type.GetType(typeName);
                var module = type.Assembly.CreateInstance(type.FullName) as ReplyModuleBase;
                if (module == null)
                    throw new Exception("Load reply module fail for " + item.ModuleName);
                foreach (ParameterCollectionElement prm in item.Parameters)
                {
                    module.SetParameters(prm.Name, prm.Value);
                }
                module.SetParameters("MatchText", item.MatchText);
                module.SetParameters("StartWith", item.StartWith);
                module.SetParameters("MatchType", item.MatchType,typeof(MessageTypes));
                moduleList.Add(module);
            }
            return moduleList;
        }

        //static List<IReplyHandleModule> replyModules;
        protected override ReplyHandleChains CreateReplyChain()
        {
            //if(replyModules==null)
              var  replyModules = LoadReplyModules();
            return ReplyHandleChains.Create(replyModules);
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            ReplyHandles = null;
            return null;
        }

        public override void ProcessRequest(System.Web.HttpContext context)
        {
            base.ProcessRequest(context);
            CheckManualStateRefresh(context);
        }
        private const string RefreshConfigKey="wechat_refresh_flag";
        private static bool WechatRefreshFlag = false;

        public override bool IsReusable
        {
            get
            {
                return false;
                //if (WechatRefreshFlag)
                //{
                //    WechatRefreshFlag = false;
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
            }
        }


        private void CheckManualStateRefresh(HttpContext context)
        {
            if (context.Request.QueryString["refresh"] == "true")
            {
                WechatRefreshFlag = true;
            }
        }

    }
}
