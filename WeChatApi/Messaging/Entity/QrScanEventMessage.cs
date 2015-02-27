using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class QrScanEventMessage : EventMessage
    {
        public virtual String EventKey { get; set; }
        public virtual String Ticket { get; set; }

        public override MessageTypes GetMsgType()
        {
            return MessageTypes.QRScan;
        }
    }
}
