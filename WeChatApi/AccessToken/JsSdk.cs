/*****
 * a utilities for JsSdk generate ticket,signature and wx.config
 * 
 *Author: 赖国欣(guoxin.lai@gmail.com) 
 * Created : 2015-01-28
 * *****/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web;
using Nlab.WeChatApi.Config;
using System.Diagnostics;

namespace Nlab.WeChatApi
{
    public class JsSdk
    {
        private static object syncObj = new object();

        private static JsApiTicket currentTicket;

        public JsApiTicket GetTicket()
        {
            if (currentTicket == null || currentTicket.HasExpired)
            {
                lock (syncObj)
                {
                    if (currentTicket == null || currentTicket.HasExpired)
                    {
                        RefreshTicket();
                    }
                }
            }
            //currentTicket.LastAccess = DateTime.Now;
            return currentTicket;
        }


        const string jsConfigFmt = @"appId: '{0}', timestamp:{1},nonceStr: '{2}',signature: '{3}',jsApiList: ['{4}'], debug: {5}";
        const string allJsInterface = "onMenuShareTimeline,onMenuShareAppMessage,onMenuShareQQ,onMenuShareWeibo,startRecord,stopRecord,onVoiceRecordEnd,playVoice,pauseVoice,stopVoice,onVoicePlayEnd,uploadVoice,downloadVoice,chooseImage,previewImage,uploadImage,downloadImage,translateVoice,getNetworkType,openLocation,getLocation,hideOptionMenu,showOptionMenu,hideMenuItems,showMenuItems,hideAllNonBaseMenuItem,showAllNonBaseMenuItem,closeWindow,scanQRCode,chooseWXPay,openProductSpecificView,addCard,chooseCard,openCard";
        const string StringToSignFmt = @"jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
        const string defaultNonce = "create_by_nlab_wechat_api_jssdk";

        /// <summary>
        /// 返回wx.config,即wx.config({value})的value部分内容
        /// 需要web.config的appsetting里配置好WeChatAppSecret, WeChatAppId
        /// 可选配置：JsApiList，JsApiDebug
        /// </summary>
        /// <param name="url"></param>
        /// <param name="apiList"></param>
        /// <param name="debug"></param>
        /// <param name="appId"></param>
        /// <param name="nonceStr"></param>
        /// <returns>return value format：appId: '{0}', timestamp:{1},nonceStr: '{2}',signature: '{3}',jsApiList: '[{4}]', debug: {5}</returns>
        public string GetWxConfig(string url = null, string apiList = null, bool? debug = null, string nonceStr = null)
        {
            var ticket = GetTicket();

            var appId = WeChatConfigManager.WechatAppId;
            if (string.IsNullOrEmpty(nonceStr))
            {
                nonceStr = defaultNonce;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = HttpContext.Current.Request.Url.AbsoluteUri;
            }
            //url = url.ToLower();
            var timeStamp = ticket.LastAccess.GetTimeStamp();
            var strToSign = string.Format(StringToSignFmt, ticket.Value, nonceStr, timeStamp, url);
            var signature = Utilities.Cryptography.Sha1Hash(strToSign);
            System.Diagnostics.Debug.WriteLine("ticket:{0}\r\n ts:{1}\r\n str:{2}\r\n sign:{3}", ticket.Value, timeStamp, strToSign, signature);
            if (string.IsNullOrEmpty(apiList))
            {
                apiList = WeChatConfigManager.JsApiList;
            }
            if (string.IsNullOrEmpty(apiList))
            {
                apiList = allJsInterface;//Enum.GetName(typeof(JsApi));
            }
            var apiArray = apiList.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            apiList = WxApiUtils.ListToString(apiArray, "','");
            if (!debug.HasValue)
            {
                debug = WeChatConfigManager.JsApiDebug;
            }
            var result = string.Format(jsConfigFmt, appId, timeStamp, nonceStr, signature, apiList, debug.Value.ToString().ToLower());
            Debug.WriteLine("wx.config:" + result);
            return result;
        }

        public void RefreshTicket()
        {
            var token = AccessTokenManager.Current.GetToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token.Value);
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "get";

            var resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                var resultJson = reader.ReadToEnd();
                var resultObj = JsonConvert.DeserializeObject<ResultEntity>(resultJson);
                if (resultObj.ErrCode != 0)
                {
                    throw new WechatApiException(resultObj.ErrCode, resultObj.ErrMsg);
                }
                else
                {
                    var ticket = JsonConvert.DeserializeObject<JsApiTicket>(resultJson);
                    ticket.LastAccess = DateTime.Now;
                    currentTicket = ticket;
                }
            }
        }
    }

    /// <summary>
    /// JsApiTicket Entity
    /// </summary>
    public class JsApiTicket
    {
        [JsonProperty("ticket")]
        public string Value { get; set; }

        public DateTime LastAccess { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public bool HasExpired
        {
            get
            {
                return DateTime.Now.Subtract(LastAccess) > TimeSpan.FromSeconds(ExpiresIn + 3);
            }
        }
    }
}
