using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// 回复音乐消息
    /// </summary>
    public class ReplyMusicMessage : MessageBase, IReplyMessage
    {
        /// <summary>
        /// 音乐链接
        /// </summary>
        public virtual string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public virtual string HQMusicUrl { get; set; }

        /// <summary>
        /// 缩略图的媒体id，通过上传多媒体文件，得到的id
        /// </summary>
        public virtual string ThumbMediaId { get; set; }

        ///// <summary>
        ///// 位0x0001被标志时，星标刚收到的消息
        ///// </summary>
        //public virtual int FuncFlag { get; set; }

        public virtual string Title { get; set; }
        public virtual string Description { get; set; }


        public MessageTypes GetMsgType()
        {
            return MessageTypes.Music;
        }
    }
}
