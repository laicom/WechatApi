/*
 * 消息回复处理接口
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
    /// 消息回复处理接口
    /// </summary>
    public interface IReplyHandleModule
    {
        /// <summary>
        /// 决定是否处理回复消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Predict(IIncomeMessage msg);


        /// <summary>
        /// 生成回复
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        IReplyMessage Reply(IIncomeMessage msg);
    }
}
