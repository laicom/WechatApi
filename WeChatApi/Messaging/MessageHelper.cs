/********
 * 
 *  Description:
 *  WeChatApi Message Helper
 *  
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Revision History:
 *  Date                  Who                 What
 *  2013-07          guoxin.lai          Created
 * 
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// the message factory
    /// </summary>
    public static class MessageHelper
    {
        internal static T Create<T>(Dictionary<string, string> messageBag) where T : IIncomeMessage, new()
        {
            var t = new T();
            var props= typeof(T).GetProperties();
            foreach (var p in props)
            {
                if (messageBag.ContainsKey(p.Name))
                {
                    var value=messageBag[p.Name];
                    messageBag.Remove(p.Name);
                    p.SetValue(t,ValueConvert(p.PropertyType, value), null);
                }
            }
            t.ExtValue = messageBag;
            return t;
        }
        private static  object ValueConvert(Type t,string value)
        {
            switch(t.Name)
            {
                case "Int32":
                    return Int32.Parse(value);
                case "Int64":
                    return Int64.Parse(value);
                case "Double":
                    return Double.Parse(value);
                case "Single":
                    return Single.Parse(value);
                case "DateTime":
                    var s= int.Parse(value);
                    return new DateTime(1970, 1, 1).AddSeconds(s).ToLocalTime();
                default:
                    return value;
            }
        }

        public static ReplyTextMessage CreateTextReply(IIncomeMessage msgReplyTo, string content, int funcFlag = 0)
        {
            return new ReplyTextMessage()
            {
                Content = content,
                CreateTime = DateTime.Now,
                FromUserName = msgReplyTo.ToUserName,
                ToUserName = msgReplyTo.FromUserName,
                FuncFlag = funcFlag,
                MsgType = "text"
            };
        }

        public static ReplyMusicMessage CreateMusicReply(IIncomeMessage msgReplyTo, string title, string description, string musicUrl, string musicHQUrl, string thumbMediaId)
        {
            return new ReplyMusicMessage()
            {
                MusicUrl = musicUrl,
                HQMusicUrl = musicHQUrl,
                CreateTime = DateTime.Now,
                FromUserName = msgReplyTo.ToUserName,
                ToUserName = msgReplyTo.FromUserName,
                //FuncFlag = funcFlag,
                 ThumbMediaId=thumbMediaId,
                MsgType = "music",
                 Title=title,
                  Description=description
            };
        }


        public static ReplyNewsMessage CreateNewsReply(IIncomeMessage msgReplyTo, List<Article> articles)
        {
            return new ReplyNewsMessage()
            {
                Articles = articles,
                CreateTime = DateTime.Now,
                FromUserName = msgReplyTo.ToUserName,
                ToUserName = msgReplyTo.FromUserName,
                MsgType = "news"
            };
        }

        public static PushNewsMessage CreatePushMessageFromTemplate(string toUser, List<Article> articles)
        {
            var news = new PushNewsMessage.NewsEntity() { Articles = articles };
            return new PushNewsMessage() { News = news, ToUserName=toUser };
        }

        /// <summary>
        /// create article list for news reply from jsontemplate(in app config setting)
        /// </summary>
        /// <param name="configTemplateName">template config item name</param>
        /// <param name="namedItemsToRepace"></param>
        /// <param name="repaceValues"></param>
        /// <returns></returns>
        public static List<Article> CreateArticleListFromConfigTemplate(string configTemplateName, string[] namedItemsToRepace = null, string[] repaceValues = null)
        {
            var jsonTemplate = Nlab.WeChatApi.Config.WeChatConfigManager.GetTemplate(configTemplateName);
            return CreateArticleListFromTemplate(jsonTemplate,namedItemsToRepace,repaceValues);
        }

        /// <summary>
        /// create article list for news reply from jsontemplate
        /// </summary>
        /// <param name="jsonTemplate"></param>
        /// <param name="NamedItemsToRepace"></param>
        /// <param name="RepaceValues"></param>
        /// <returns></returns>
        public static List<Article> CreateArticleListFromTemplate( string jsonTemplate,string[] namedItemsToRepace=null,string[] repaceValues=null)
        {
            if (namedItemsToRepace != null && repaceValues != null && namedItemsToRepace.Length == repaceValues.Length)
            {
                for(var i=0;i<namedItemsToRepace.Length;i++)
                {
                    jsonTemplate = jsonTemplate.Replace(namedItemsToRepace[i], repaceValues[i]);
                }
            }
            return JsonConvert.DeserializeObject<List<Article>>(jsonTemplate);
        }

        private static readonly string textTemplate = @"
<xml>
 <ToUserName><![CDATA[{0}]]></ToUserName>
 <FromUserName><![CDATA[{1}]]></FromUserName>
 <CreateTime>{2}</CreateTime>
 <MsgType><![CDATA[{3}]]></MsgType>
 <Content><![CDATA[{4}]]></Content>
 <FuncFlag>{5}</FuncFlag>
 </xml>
";
        internal static string TransReply(ReplyTextMessage msg)
        {
            return string.Format(textTemplate,
                msg.ToUserName,
                msg.FromUserName,
                msg.CreateTime.ToWeChatTimeStamp(),
                msg.MsgType,
                msg.Content,
                msg.FuncFlag
                );
        }


        private static readonly string musicTemplate = @"
 <xml>
 <ToUserName><![CDATA[{0}]]></ToUserName>
 <FromUserName><![CDATA[{1}]]></FromUserName>
 <CreateTime>{2}</CreateTime>
 <MsgType><![CDATA[{3}]]></MsgType>
 <Music>
 <Title><![CDATA[{4}]]></Title>
 <Description><![CDATA[{5}]]></Description>
 <MusicUrl><![CDATA[{6}]]></MusicUrl>
 <HQMusicUrl><![CDATA[{7}]]></HQMusicUrl>
<ThumbMediaId><![CDATA[{8}]]></ThumbMediaId>
 </Music>
 </xml>
";

        internal static string TransReply(ReplyMusicMessage msg)
        {
            return string.Format(musicTemplate,
                msg.ToUserName,
                msg.FromUserName,
                msg.CreateTime.ToWeChatTimeStamp(),
                msg.MsgType,
                msg.Title,
                msg.Description,
                msg.MusicUrl.Resolve(),
                msg.HQMusicUrl.Resolve(),
                msg.ThumbMediaId
                );
        }

        private static readonly string newsHeadTemplate = @"
 <xml>
 <ToUserName><![CDATA[{0}]]></ToUserName>
 <FromUserName><![CDATA[{1}]]></FromUserName>
 <CreateTime>{2}</CreateTime>
 <MsgType><![CDATA[{3}]]></MsgType>
 <ArticleCount>{4}</ArticleCount>
<Articles>
";

        private static readonly string newsArticleTemplate = @"
<item>
 <Title><![CDATA[{0}]]></Title> 
 <Description><![CDATA[{1}]]></Description>
 <PicUrl><![CDATA[{2}]]></PicUrl>
 <Url><![CDATA[{3}]]></Url>
 </item>
";

        private static readonly string newsFootTemplate = @"
 </Articles>
 <FuncFlag>{0}</FuncFlag>
 </xml> 
";
        internal static string TransReply(ReplyNewsMessage msg)
        {
            var xb = new StringBuilder(2048);
            xb.AppendFormat(newsHeadTemplate,
                msg.ToUserName,
                msg.FromUserName,
                msg.CreateTime.ToWeChatTimeStamp(),
                msg.MsgType,
                msg.Articles.Count
                );
            foreach (var item in msg.Articles)
            {
                xb.AppendFormat(newsArticleTemplate,
                    item.Title,
                    item.Description,
                    item.PicUrl.Resolve(),
                    item.Url.Resolve()
                    );
            }
            xb.AppendFormat(newsFootTemplate, msg.FuncFlag);
            return xb.ToString();
        }

        /// <summary>
        /// 时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static int ToWeChatTimeStamp(this DateTime dt)
        {
            return (int)dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Resove url path append with server host
        /// </summary>
        /// <param name="urlToResolve"></param>
        /// <returns></returns>
        internal static string Resolve(this string urlToResolve, Uri hostUri = null)
        {
            if (hostUri == null)
            {
                var context = HttpContext.Current;
                if (context == null)
                    return urlToResolve;
                // your can pass uri by context use key WechatResolveUri, to replace the context request url when needed
                else if (context.Items.Contains("WechatResolveUri")) 
                    hostUri = context.Items["WechatResolveUri"] as Uri;
                else
                    hostUri = context.Request.Url;
            }
            if (!string.IsNullOrEmpty(urlToResolve) && urlToResolve.StartsWith("/", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                return string.Format("{0}://{1}:{2}{3}", hostUri.Scheme, hostUri.Host, hostUri.Port, urlToResolve);
            }
            return urlToResolve;
        }
    }
}
