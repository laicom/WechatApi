/********
 * 
 *  Description:
 *  WeChat 消息(静态)扩展处理
 *  
 *  Author 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Create Date   2013-8-3
 * 
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 消息(静态)扩展处理
    /// </summary>
    public static class MessageExtent
    {
        public static bool IsEvent(this IWeChatMessage msg)
        {
            return  msg.MsgType == "event";
        }

        public static bool IsText(this IWeChatMessage msg)
        {
            return msg.MsgType == "text";
        }

        public static bool IsLink(this IWeChatMessage msg)
        {
            return msg.MsgType == "link";
        }


        public static bool IsLocation(this IWeChatMessage msg)
        {
            return msg.MsgType == "event" && (msg as EventMessage).Event == "location";
        }

        public static bool IsImage(this IWeChatMessage msg)
        {
            return msg.MsgType == "image";
        }

        public static bool IsMusic(this IWeChatMessage msg)
        {
            return msg.MsgType == "music";
        }

        public static bool IsSubscribe(this IWeChatMessage msg)
        {
            return msg.MsgType == "event" && (msg as EventMessage).Event== "subscribe";
        }

        public static bool IsNews(this IWeChatMessage msg)
        {
            return msg.MsgType == "news";
        }

        public static bool ContentContains(this IWeChatMessage msg,string s)
        {
            return msg.MsgType == "text" && (msg as TextMessage).Content.Contains(s);
        }


        public static bool ContentEquals(this IWeChatMessage msg, string s)
        {
            return msg.MsgType == "text" && (msg as TextMessage).Content.Equals(s);
        }

        public static T RamdonGet<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);
            var rnd = new System.Random();
            var i= rnd.Next(list.Count);
            return list[i];
        }
    }
}
