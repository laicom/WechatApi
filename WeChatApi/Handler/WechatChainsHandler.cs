/********
 * 
 *  Description: 
 *  WeChatChainsHandler
 *  WeChat(微信）公众平台接入链式信息处理基础类
 *  
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Revision History:
 *  Date                  Who                 What
 *  
 * 
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// WeChat message process in chains model.
    /// Usage just to provide you handle chains
    /// </summary>
    public abstract class WeChatChainsHandler : WeChatHandlerBase 
    {
        public WeChatChainsHandler() :base()
        {
            OnReceiveMessage += new Func<IIncomeMessage, IReplyMessage>(WeChat_OnReceiveMessage);
        }


        protected ReplyHandleChains ReplyHandles;
        protected abstract ReplyHandleChains CreateReplyChain();

        protected virtual IReplyMessage WeChat_OnReceiveMessage(IIncomeMessage msg)
        {
            if (ReplyHandles == null)
            {
                ReplyHandles = CreateReplyChain();
            }
            return ReplyHandles.Reply(msg);
        }


        protected override IReplyMessage WeChat_OnReceiveTextMessage(TextMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveImageMessage(ImageMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveLocationMessage(LocationEventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveLinkMessage(LinkMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveEventMessage(EventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveQrScanMessage(QrScanEventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveMenuClickMessage(MenuClickEventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveSubscribeMessage(SubscribeEventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveUnSubscribeMessage(UnSubscribeEventMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveVoiceMessage(VoiceMessage msg)
        {
            return null;
        }

        protected override IReplyMessage WeChat_OnReceiveVideoMessage(VideoMessage msg)
        {
            return null;
        }
    }
}
