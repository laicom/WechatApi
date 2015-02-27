/********
 * 
 *  Description:
 *  WeChat(微信）服务对接处理流程基础框架
 *  
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Revision History:
 *  Date                  Who                 What
 *  2013-07-30          guoxin.lai          Created
 *  2014-11-15          guoxin.lai          Update for Encrypt mode & enterprise account
 * 
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Nlab.WeChatApi.Config;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// WeChat message core process and base mode handler.
    /// you should inherit this class then override the WeChatHandler_OnReceiveXXXXXX to implement your application interact with wechat. take it easy & enjoy!
    /// 
    /// </summary>
    public abstract class WeChatHandlerBase : System.Web.IHttpHandler
    {
        public WeChatHandlerBase()
        {
            OnReceiveTextMessage += new Func<TextMessage, IReplyMessage>(WeChat_OnReceiveTextMessage);
            OnReceiveEventMessage += new Func<EventMessage, IReplyMessage>(WeChat_OnReceiveEventMessage);
            OnReceiveImageMessage += new Func<ImageMessage, IReplyMessage>(WeChat_OnReceiveImageMessage);
            OnReceiveVoiceMessage += new Func<VoiceMessage, IReplyMessage>(WeChat_OnReceiveVoiceMessage);
            OnReceiveVideoMessage += new Func<VideoMessage, IReplyMessage>(WeChat_OnReceiveVideoMessage);
            OnReceiveLinkMessage += new Func<LinkMessage, IReplyMessage>(WeChat_OnReceiveLinkMessage);
            OnReceiveLocationMessage += new Func<LocationEventMessage, IReplyMessage>(WeChat_OnReceiveLocationMessage);
            OnReceiveQrScanMessage += new Func<QrScanEventMessage, IReplyMessage>(WeChat_OnReceiveQrScanMessage);
            OnReceiveMenuClickMessage += new Func<MenuClickEventMessage, IReplyMessage>(WeChat_OnReceiveMenuClickMessage);
            OnReceiveSubscribeMessage += new Func<SubscribeEventMessage, IReplyMessage>(WeChat_OnReceiveSubscribeMessage);
            OnReceiveUnSubscribeMessage += new Func<UnSubscribeEventMessage, IReplyMessage>(WeChat_OnReceiveUnSubscribeMessage);

            OnProcessMessageError += new Func<IIncomeMessage, Exception,IReplyMessage>(WeChat_OnProcessMessageError);
        }


        #region abstract method

        protected abstract IReplyMessage WeChat_OnReceiveTextMessage(TextMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveImageMessage(ImageMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveVoiceMessage(VoiceMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveVideoMessage(VideoMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveLocationMessage(LocationEventMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveLinkMessage(LinkMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveEventMessage(EventMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveQrScanMessage(QrScanEventMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveMenuClickMessage(MenuClickEventMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveSubscribeMessage(SubscribeEventMessage msg);
        protected abstract IReplyMessage WeChat_OnReceiveUnSubscribeMessage(UnSubscribeEventMessage msg);

        /// <summary>
        /// 异常处理记日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual IReplyMessage WeChat_OnProcessMessageError(IIncomeMessage msg, Exception e)
        {
            string errMsg = string.Empty;
            if (msg != null)
                errMsg = string.Format("Process {0} msg failed from:[{1}],", msg.MsgType, msg.FromUserName);
            Nlab.WeChatApi.Logger.NlabLogger.LogError(e, errMsg);
            return null;
        }


        #endregion

        /// <summary>
        /// to get wechat token from web.config appsetting, you can override it
        /// </summary>
        protected virtual string WeChatToken
        {
            get { return WeChatConfigManager.WeChatToken; }
        }

        /// <summary>
        /// to get EncodingAESKey from web.config appsetting, you can override it
        /// </summary>
        protected virtual string EncodingAESKey
        {
            get { return WeChatConfigManager.EncodingAESKey; }
        }

        /// <summary>
        /// to get WeChatAppId from web.config appsetting, you can override it
        /// </summary>
        protected virtual string WechatAppId
        {
            get { return WeChatConfigManager.WechatAppId; }
        }

        /// <summary>
        /// Implement IHttpHandler.IsReusable
        /// </summary>
        public virtual bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// Implement IHttpHandler.ProcessRequest
        /// </summary>
        /// <param name="context"></param>
        public virtual void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                var method = context.Request.HttpMethod.ToUpper();
                if (method == "POST")
                    WeChatMessagerHandle(context);
                else if (method == "GET")
                    WeChatAuthenticationHandle(context);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex);
                //context.Response.StatusCode = 500;
                //context.Response.Write("Exception :" + ex.Message);
                if (OnProcessMessageError != null)
                    OnProcessMessageError(null, ex);
            }
        }

        #region 事件定义

        /// <summary>
        /// 微信验证，一般不需要处理
        /// </summary>
        public event Action<string, string, string, string> OnAuthen;

        /// <summary>
        /// 收到信息，传递原始xml，一般不需要处理
        /// </summary>
        public event Action<string> OnReceiveRequest;

        /// <summary>
        /// 完成请求的响应，传递收到和回复的xml，一般不需要处理
        /// </summary>
        public event Action<string,string> OnCompleteResponse;


        //以下为针对性的信息处理
        public event Func<TextMessage, IReplyMessage> OnReceiveTextMessage;
        public event Func<ImageMessage, IReplyMessage> OnReceiveImageMessage;
        public event Func<VoiceMessage, IReplyMessage> OnReceiveVoiceMessage;
        public event Func<VideoMessage, IReplyMessage> OnReceiveVideoMessage;
        public event Func<LocationEventMessage, IReplyMessage> OnReceiveLocationMessage;
        public event Func<QrScanEventMessage, IReplyMessage> OnReceiveQrScanMessage;
        public event Func<MenuClickEventMessage, IReplyMessage> OnReceiveMenuClickMessage;
        public event Func<SubscribeEventMessage, IReplyMessage> OnReceiveSubscribeMessage;
        public event Func<UnSubscribeEventMessage, IReplyMessage> OnReceiveUnSubscribeMessage;
        public event Func<LinkMessage, IReplyMessage> OnReceiveLinkMessage;
        public event Func<EventMessage, IReplyMessage> OnReceiveEventMessage;

        //异常处理
        public event Func<IIncomeMessage, Exception,IReplyMessage> OnProcessMessageError;

        /// <summary>
        /// 统一的消息处理过程
        /// </summary>
        public event Func<IIncomeMessage, IReplyMessage> OnReceiveMessage;

        /// <summary>
        /// 异常处理
        /// </summary>
        public event Action<Exception> OnError;

    #endregion

        /// <summary>
        /// 服务认证过程
        /// </summary>
        /// <param name="context"></param>
        private void WeChatAuthenticationHandle(System.Web.HttpContext context)
        {
            string signature = context.Request["signature"],
                    timestamp = context.Request["timestamp"],
                    nonce = context.Request["nonce"],
                    echostr = context.Request["echostr"],
                    encryptType = context.Request["encrypt_type"] 
                    ;

            if (string.IsNullOrEmpty(signature)) //企业号
            {
                signature = context.Request["msg_signature"];
                if (!string.IsNullOrEmpty(signature))
                {
                    var cryptTool = new Nlab.WeChatApi.Utilities.MessageCrypt(WeChatToken, EncodingAESKey, WechatAppId);
                    string decryptEcho;
                    var rst = cryptTool.VerifyURL(signature, timestamp, nonce, echostr, ref echostr);
                    if (rst != 0)
                        throw new WechatApiException(rst, "VerifyURL failed");
                    context.Response.Write(echostr);
                    return;
                }
            }
            //context.Request.Path
            else // 
                if (!string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce) && !string.IsNullOrEmpty(echostr))
            {
                if (OnAuthen != null)
                    OnAuthen(signature, timestamp, nonce, echostr);
                var authArray = new string[] { WeChatToken, timestamp, nonce };
                Array.Sort(authArray);
                var authStr = string.Join("", authArray);
                var authStrSha1 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(authStr, "sha1").ToLower();
                if (signature == authStrSha1)
                {
                    context.Response.Write(echostr);
                    return;
                }
                else
                {
                    context.Response.Write("Invalid Token.");
                }
            }
            context.Response.Write("WeChatApi Create by Laiguoxin(guoxin.lai@gmail.com,http://www.9499.net) 2013-7 reference:https://github.com/laicom/WechatApi.git");
        }

        /// <summary>
        /// 消息处理过程
        /// </summary>
        /// <param name="context"></param>
        private void WeChatMessagerHandle(System.Web.HttpContext context)
        {
            var inputStream = context.Request.InputStream;
            var inputBuffer = new Byte[inputStream.Length];
            inputStream.Read(inputBuffer, 0, inputBuffer.Length);
            var requetContent = Encoding.UTF8.GetString(inputBuffer);
            //if (OnReceiveRequest != null)
            //    OnReceiveRequest(requetContent);

            Utilities.MessageCrypt cryptTool = null;
            string signature = context.Request["signature"],
                msgSignature= context.Request["msg_signature"],
            timestamp = context.Request["timestamp"],
            nonce = context.Request["nonce"],
            //echostr = context.Request["echostr"],
            encryptType = context.Request["encrypt_type"]
            ;

            bool isEncrypt = WeChatConfigManager.IsCorperation || (!string.IsNullOrEmpty(encryptType) && encryptType == "aes");

#region 加密模式解密处理
            if (isEncrypt)
            {
                cryptTool = new Nlab.WeChatApi.Utilities.MessageCrypt(WeChatToken, EncodingAESKey, WechatAppId);
                requetContent = cryptTool.DecryptMsg(msgSignature, timestamp, nonce, requetContent);

            }
#endregion
            if (OnReceiveRequest != null)
                OnReceiveRequest(requetContent);

            var msgBag = XmlToDict(requetContent);
            var responseContent = ProcessMessageReceive(msgBag);
            if (!string.IsNullOrEmpty(responseContent))
            {
                if (OnCompleteResponse != null)
                    OnCompleteResponse(requetContent, responseContent);
                if (cryptTool != null) //加密模式，进行数据加密
                    responseContent=cryptTool.EncryptMsg(responseContent, timestamp, nonce);
                context.Response.Write(responseContent);
            }
        }

        /// <summary>
        /// 原始xml转换
        /// </summary>
        /// <param name="rawXml"></param>
        /// <returns></returns>
        private Dictionary<string, string> XmlToDict(string rawXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(rawXml);
            var nodes = doc.FirstChild.ChildNodes;
            var msgBag = new Dictionary<string, string>();
            foreach (XmlNode node in nodes)
            {
                msgBag.Add(node.Name, node.InnerText);
            }
            return msgBag;
        }

        /// <summary>
        /// 消息处理流程
        /// </summary>
        /// <param name="msgBag"></param>
        /// <returns></returns>
        protected virtual string ProcessMessageReceive(Dictionary<string, string> msgBag)
        {
            IReplyMessage reply = null;
            IIncomeMessage msg = null;
            if (!msgBag.ContainsKey("MsgType"))
            {
                throw new WechatApiException(-3, "received unexpected message type.");
            }
            try
            {
                switch (msgBag["MsgType"])
                {
                    case "text":
                        {
                            msg = MessageHelper.Create<TextMessage>(msgBag);
                            reply = OnReceiveTextMessage(msg as TextMessage);
                            break;
                        }
                    case "image":
                        {
                            msg = MessageHelper.Create<ImageMessage>(msgBag);
                            reply = OnReceiveImageMessage(msg as ImageMessage);
                            break;
                        }
                    case "voice":
                        {
                            msg = MessageHelper.Create<VoiceMessage>(msgBag);
                            reply = OnReceiveVoiceMessage(msg as VoiceMessage);
                            break;
                        }
                    case "video":
                        {
                            msg = MessageHelper.Create<VideoMessage>(msgBag);
                            reply = OnReceiveVideoMessage(msg as VideoMessage);
                            break;
                        }
                    //case "location":
                    //    {
                    //        msg = MessageHelper.Create<LocationMessage>(msgBag);
                    //        reply = OnReceiveLocationMessage(msg as LocationMessage);
                    //        break;
                    //    }
                    case "link":
                        {
                            msg = MessageHelper.Create<LinkMessage>(msgBag);
                            reply = OnReceiveLinkMessage(msg as LinkMessage);
                            break;
                        }
                    case "event":
                        {
                            var eventType = msgBag["Event"].ToLower();
                            switch (eventType)
                            {
                                case "click":
                                case "view":
                                    msg = MessageHelper.Create<MenuClickEventMessage>(msgBag);
                                    reply = OnReceiveMenuClickMessage(msg as MenuClickEventMessage);
                                    break;
                                case "subscribe":
                                    msg = MessageHelper.Create<SubscribeEventMessage>(msgBag);
                                    reply = OnReceiveSubscribeMessage(msg as SubscribeEventMessage);
                                    break;
                                case "unsubscribe":
                                    msg = MessageHelper.Create<UnSubscribeEventMessage>(msgBag);
                                    reply = OnReceiveUnSubscribeMessage(msg as UnSubscribeEventMessage);
                                    break;
                                case "scan":
                                    msg = MessageHelper.Create<QrScanEventMessage>(msgBag);
                                    reply = OnReceiveQrScanMessage(msg as QrScanEventMessage);
                                    break;
                                case "location":
                                    msg = MessageHelper.Create<LocationEventMessage>(msgBag);
                                    reply = OnReceiveLocationMessage(msg as LocationEventMessage);
                                    break;
                                default:
                                    msg = MessageHelper.Create<EventMessage>(msgBag);
                                    reply = OnReceiveEventMessage(msg as EventMessage);
                                    break;
                            }
                            break;
                        }
                }
                if (reply == null && msg != null && OnReceiveMessage != null)
                    reply = OnReceiveMessage(msg);

                if (reply != null)
                {
                    switch (reply.MsgType)
                    {
                        case "text":
                            {
                                return MessageHelper.TransReply(reply as ReplyTextMessage);
                            }
                        case "music":
                            {
                                return MessageHelper.TransReply(reply as ReplyMusicMessage);
                            }
                        case "news":
                            {
                                return MessageHelper.TransReply(reply as ReplyNewsMessage);
                            }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                if (OnProcessMessageError != null)
                    OnProcessMessageError(msg, ex);
                return string.Empty;
            }
        }
    }
}
