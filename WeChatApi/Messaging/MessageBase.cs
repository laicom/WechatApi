/********
 * 
 *  WeChat message base(entity)
 *  
 *  Author 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Create Date   2013-7-3
 * 
 ********/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 接收方微信号
        /// </summary>
        public virtual string ToUserName { get; set; }
        /// <summary>
        /// 发送方微信号
        /// </summary>
        public virtual string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// (未知)扩展的消息内容
        /// </summary>
        public Dictionary<string, string> ExtValue { get; set; }

    }
}
