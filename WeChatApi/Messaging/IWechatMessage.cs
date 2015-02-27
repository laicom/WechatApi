/********
 * 
 *  Description:
 *  WeChat message base interface
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
    public interface IWeChatMessage
    {
            /// <summary>
            /// message type
            /// </summary>
            string MsgType { get; set; }

            /// <summary>
            /// 接收方微信号
            /// </summary>
            string ToUserName { get; set; }
            /// <summary>
            /// 发送方微信号
            /// </summary>
            string FromUserName { get; set; }
            /// <summary>
            /// 消息创建时间
            /// </summary>
            DateTime CreateTime { get; set; }

            MessageTypes GetMsgType();
     }
}
