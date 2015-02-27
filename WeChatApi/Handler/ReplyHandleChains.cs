/*
 * 被动式消息回复处理链式结构体
 * author Laiguoxin<guoxin.lai@gmail.com>
 * Create date:  2013-7-28
  */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// <![CDATA[
    /// 处理链式结构体
    /// Usage: 
    ///     MessageHandleChains.Create(handler list);
    ///  OR   MessageHandleChains.Create().Next(handler1).Next(handler2)............
    ///  OR   MessageHandleChains.Create().Next<Type1>().Next<Type2>()............
    ///     ]]>
    /// </summary>
    public class ReplyHandleChains 
    {
        public static ReplyHandleChains Create()
        {
            return new ReplyHandleChains();
        }

        public static ReplyHandleChains Create(IEnumerable<IReplyHandleModule> handlers)
        {
            return new ReplyHandleChains(handlers);
        }        

        private int index=0;
        private List<IReplyHandleModule> handlerList;
        private ReplyHandleChains()
        {
            handlerList = new List<IReplyHandleModule>();
        }

        private ReplyHandleChains(IEnumerable<IReplyHandleModule> handlers)
        {
            handlerList = new List<IReplyHandleModule>(handlers);
        }

        public ReplyHandleChains Next(IReplyHandleModule handler)
        {
            if (handler == null)
                throw new ArgumentNullException();
            handlerList.Add(handler);
            return this;
        }

        public ReplyHandleChains Next<T>() where T : IReplyHandleModule, new()
        {
            return Next(new T());
        }

        public ReplyHandleChains Next<T>(string matches) where T : ReplyModuleBase, new()
        {
            var t = new T();
            t.MatchText = matches;
            return Next(t);
        }

        public IReplyMessage Reply(IIncomeMessage msg)
        {
            if (index >= handlerList.Count)
            {
                index = 0;
                return null;
            }
            var current = handlerList[index]; 
                index++;
            IReplyMessage reply=null;
            if (current.Predict(msg))
            {
                reply = current.Reply(msg);
            }
            if (reply == null)
                return Reply(msg);
            index = 0;
            return reply;
        }
     }
}
