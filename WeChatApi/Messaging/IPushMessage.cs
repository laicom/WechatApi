using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public interface IPushMessage
    {
            /// <summary>
            /// message type
            /// </summary>
            string MsgType { get; set; }

            /// <summary>
            /// 接收方微信号
            /// </summary>
            string ToUserName{ get; set; }
     }
}
