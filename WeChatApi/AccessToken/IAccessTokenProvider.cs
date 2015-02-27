using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    /// <summary>
    /// IAccessTokenProvider
    /// </summary>
    public interface IAccessTokenProvider
    {
        AccessToken GetToken();
        void TryGetToken(Action<string> successCallback,Action<WechatApiException> failedCallback);
        string AppId { get; set; }
        string AppSecret { get; set; }
    }

    public interface IAccessTokenPersistenceProvider : IAccessTokenProvider
    {
        void SaveToken(AccessToken token);
    }


    public abstract class AccesTokenProviderBase : IAccessTokenProvider
    {

        public abstract AccessToken GetToken();

        public virtual void TryGetToken(Action<string> successCallback, Action<WechatApiException> failedCallback)
        {
            try
            {
                var token = GetToken();
                successCallback(token.Value);
            }
            catch (WechatApiException wxEx)
            {
                failedCallback(wxEx);
            }
            catch (Exception ex)
            {
                var wxEx = new WechatApiException(-1, "get token failed:" + ex.Message + "--" + ex.StackTrace);
                failedCallback(wxEx);
            }
        }
        public virtual string AppId{get;set;}
        public virtual string AppSecret{get;set;}
    }
}
