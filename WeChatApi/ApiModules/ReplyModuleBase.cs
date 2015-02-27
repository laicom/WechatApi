/********
 * 
 *  Description:
 *  WeChat(微信）消息回应基础模块
 *  
 *  Author 赖国欣(guoxin.lai@gmail.com)
 *  
 *
 *  Create Date   2013-7-21
 * 
 ********/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Nlab.WeChatApi
{
    internal delegate  bool TParser(string input,out object output) ;

    public abstract class ReplyModuleBase : IReplyHandleModule
    {
        protected ConcurrentDictionary<string, string> Parameters = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, DateTime> DuplicateMsgTab = new ConcurrentDictionary<string, DateTime>();


        public virtual void SetParameters(string name, string value,Type type=null)
        {
            if (string.IsNullOrEmpty(value))
                return;
            var prop = this.GetType().GetProperty(name);
            if (prop != null)
            {
                //var v = ParameterConvert(value, prop.PropertyType);
                prop.SetValue(this,value, null);
            }
            else
                Parameters.TryAdd(name, value);
        }

        private IEnumerable<string> _matchList;
        private IEnumerable<string> MatchesList
        {
            get
            {
                if (_matchList == null)
                {
                    if (MatchText != null)
                        _matchList= MatchText.ToLower().Split(';');
                    else
                        _matchList = new List<string>();
                }
                return _matchList;
            }
        }


        /// <summary>
        /// 设置匹配的键值，以分号号分隔
        /// </summary>
        public virtual string MatchText { get; set; }

        private string _msgType = "text|event";
        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual string MatchType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public virtual string StartWith { get; set; }

        
        /// <summary>
        /// 非重复消息判定
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual bool IsDuplicated(IIncomeMessage msg)
        {
            var key = string.Format("{0}{1}{2:yyMMddHHmmss}", msg.MsgType, msg.FromUserName, msg.CreateTime);
            DateTime dt;
            if (!DuplicateMsgTab.TryGetValue(key, out dt))
            {
                DuplicateMsgTab.TryAdd(key, DateTime.Now);
                CleanDuplicateCacheData();
                return false;
            }
            return true;
        }

        static DateTime LastCleanTime = DateTime.Now;
        private void CleanDuplicateCacheData()
        {
            if (DateTime.Now.Subtract(LastCleanTime).TotalSeconds < 30)
                return;
            var keys = DuplicateMsgTab.Keys;
            var expTime = DateTime.Now.AddSeconds(-60);
            DateTime t;
            foreach (var k in keys)
            {
                if (DuplicateMsgTab[k] < expTime)
                    DuplicateMsgTab.TryRemove(k,out t);
            }
        }

        public virtual bool Predict(IIncomeMessage msg)
        {
            return
                //!IsDuplicated(msg) &&
            MatchType.Contains(msg.MsgType) &&
            (
                (
                    MatchesList != null && IsMatchs(msg)
                )
                || IsMatchStartWith(msg)
                //(StartWith != null && msg.IsText() && (msg as TextMessage).Content.StartsWith(StartWith))
            );
        }

        private bool IsMatchStartWith(IIncomeMessage msg)
        {
            if (string.IsNullOrEmpty(StartWith) || !msg.IsText())
                return false;
            var spl = StartWith.Split(',', ';');
            var content = (msg as TextMessage).Content;
            foreach (var s in spl)
            {
                if (content.StartsWith(s))
                    return true;
            }
            return false;
        }

        private const string WidthCast = "*";
        private bool IsMatchs(IIncomeMessage msg)
        {
            if (msg.MsgType == "text")
                return MatchesList.Contains((msg as TextMessage).Content.ToLower()) || MatchText == WidthCast;
            if (msg.GetMsgType() == MessageTypes.MenuClick)
                return MatchesList.Contains((msg as MenuClickEventMessage).EventKey.ToLower());
            if (msg.GetMsgType() == MessageTypes.QRScan)
                return MatchesList.Contains((msg as QrScanEventMessage).EventKey.ToLower());
            if (msg.MsgType == "event")
                return MatchType.Contains((msg as EventMessage).Event);
            return false;
        }

        public abstract IReplyMessage Reply(IIncomeMessage msg);
    }
}
