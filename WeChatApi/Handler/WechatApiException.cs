using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// WechatApi Exception
    /// </summary>
    public class WechatApiException :Exception
    {
        public WechatApiException(string message) : base(message){ }
        public WechatApiException(int code, string message) : base(message) { this.ErrorCode = code; }
        public int ErrorCode { get; set; }
    }
}
