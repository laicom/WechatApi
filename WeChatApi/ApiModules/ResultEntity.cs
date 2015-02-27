using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class ResultEntity
    {
        [Newtonsoft.Json.JsonProperty("errcode")]
        public int ErrCode { get; set; }

        [Newtonsoft.Json.JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
    }
}
