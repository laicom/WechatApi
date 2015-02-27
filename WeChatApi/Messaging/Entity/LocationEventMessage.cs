using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class LocationEventMessage : EventMessage
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public virtual double Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public virtual double Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public virtual double Precision { get; set; }

        public override MessageTypes GetMsgType()
        {
            return MessageTypes.Location;
        }
    }
}
