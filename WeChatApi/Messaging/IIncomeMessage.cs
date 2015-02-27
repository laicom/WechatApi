using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// in coming message interface
    /// </summary>
    public interface IIncomeMessage : IWeChatMessage
    {
            /// <summary>
            /// (未知)扩展的消息内容
            /// </summary>
            Dictionary<string, string> ExtValue { get; set; }
     }
}
