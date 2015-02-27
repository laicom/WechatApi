/*
 * 消息解密程序，微信官方代码提供，稍作改进
 *Author: 赖国欣(guoxin.lai@gmail.com) 
 * Created : 2014-07-02
 * 
 
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Nlab.WeChatApi.Utilities
{
    class MessageCrypt
    {

        string m_sToken;
        string m_sEncodingAESKey;
        string m_sAppID;
        enum WXBizMsgCryptErrorCode
        {
            WXBizMsgCrypt_OK = 0,
            WXBizMsgCrypt_ValidateSignature_Error = -40001,
            WXBizMsgCrypt_ParseXml_Error = -40002,
            WXBizMsgCrypt_ComputeSignature_Error = -40003,
            WXBizMsgCrypt_IllegalAesKey = -40004,
            WXBizMsgCrypt_ValidateAppid_Error = -40005,
            WXBizMsgCrypt_EncryptAES_Error = -40006,
            WXBizMsgCrypt_DecryptAES_Error = -40007,
            WXBizMsgCrypt_IllegalBuffer = -40008,
            WXBizMsgCrypt_EncodeBase64_Error = -40009,
            WXBizMsgCrypt_DecodeBase64_Error = -40010
        };

        //构造函数
        // @param sToken: 公众平台上，开发者设置的Token
        // @param sEncodingAESKey: 公众平台上，开发者设置的EncodingAESKey
        // @param sAppID: 公众帐号的appid
        public MessageCrypt(string sToken, string sEncodingAESKey, string sAppID)
        {
            m_sToken = sToken;
            m_sAppID = sAppID;
            m_sEncodingAESKey = sEncodingAESKey;
        }


        // 检验消息的真实性，并且获取解密后的明文
        // @param sMsgSignature: 签名串，对应URL参数的msg_signature
        // @param sTimeStamp: 时间戳，对应URL参数的timestamp
        // @param sNonce: 随机串，对应URL参数的nonce
        // @param sPostData: 密文，对应POST请求的数据
        // @param sMsg: 解密后的原文，当return返回0时有效
        // @return: 成功0，失败返回对应的错误码
        public string DecryptMsg(string sMsgSignature, string sTimeStamp, string sNonce, string sPostData)
        {
            if (m_sEncodingAESKey.Length != 43)
            {
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey, "DecryptMsg failed,Unexpected EncodingAESKey"); 
            }
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(sPostData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ParseXml_Error, "DecryptMsg failed in ParseXml Error");
            }
            //verify signature
            int ret = 0;
            ret = VerifySignature(m_sToken, sTimeStamp, sNonce, sEncryptMsg, sMsgSignature);
            if (ret != 0)
                throw new WechatApiException(ret, "DecryptMsg failed in VerifySignature");

            //decrypt
            string cpid = "", msgDecrypt;
            try
            {
                msgDecrypt = Cryptography.AES_decrypt(sEncryptMsg, m_sEncodingAESKey, ref cpid);
            }
            catch (FormatException)
            {
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error, "DecryptMsg failed, DecodeBase64 Error");
            }
            catch (Exception)
            {
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error, "DecryptMsg failed,DecryptAES Error");
            }
            if (cpid != m_sAppID)
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error, "DecryptMsg failed,Validate Appid");
            return msgDecrypt;
        }


        //将企业号回复用户的消息加密打包
        // @param sReplyMsg: 企业号待回复用户的消息，xml格式的字符串
        // @param sTimeStamp: 时间戳，可以自己生成，也可以用URL参数的timestamp
        // @param sNonce: 随机串，可以自己生成，也可以用URL参数的nonce
        // @param sEncryptMsg: 加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,
        //						当return返回0时有效
        // return：成功0，失败返回对应的错误码
        public string EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce)
        {
            if (m_sEncodingAESKey.Length != 43)
            {
                throw new  WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey, "Unexpected  EncodingAESKey");
            }
            string raw = "";
            try
            {
                raw = Cryptography.AES_encrypt(sReplyMsg, m_sEncodingAESKey, m_sAppID);
            }
            catch (Exception)
            {
                throw new WechatApiException((int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_EncryptAES_Error, "EncryptMsg  falied");
            }
            string MsgSigature = "";
            int ret = 0;
            ret = GenarateSinature(m_sToken, sTimeStamp, sNonce, raw, ref MsgSigature);
            if (0 != ret)
                throw new WechatApiException(ret, "GenarateSinature  falied");
            //sEncryptMsg = "";

            //string EncryptLabelHead = "<Encrypt><![CDATA[";
            //string EncryptLabelTail = "]]></Encrypt>";
            //string MsgSigLabelHead = "<MsgSignature><![CDATA[";
            //string MsgSigLabelTail = "]]></MsgSignature>";
            //string TimeStampLabelHead = "<TimeStamp><![CDATA[";
            //string TimeStampLabelTail = "]]></TimeStamp>";
            //string NonceLabelHead = "<Nonce><![CDATA[";
            //string NonceLabelTail = "]]></Nonce>";
            //sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
            //sEncryptMsg = sEncryptMsg + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
            //sEncryptMsg = sEncryptMsg + TimeStampLabelHead + sTimeStamp + TimeStampLabelTail;
            //sEncryptMsg = sEncryptMsg + NonceLabelHead + sNonce + NonceLabelTail;
            //sEncryptMsg += "</xml>";

            return string.Format(encryptXmlFormat, raw, MsgSigature, sTimeStamp, sNonce);
        }
        private static readonly string encryptXmlFormat = @"<xml><Encrypt><![CDATA[{0}]]></Encrypt><MsgSignature><![CDATA[{1}]]></MsgSignature><TimeStamp><![CDATA[{2}]]></TimeStamp><Nonce><![CDATA[{3}]]></Nonce></xml>";

        public class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
        //Verify Signature
        private static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
        {
            string hash = "";
            int ret = 0;
            ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return ret;
            //System.Console.WriteLine(hash);
            if (hash == sSigture)
                return 0;
            else
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateSignature_Error;
            }
        }

        //验证URL
        // @param sMsgSignature: 签名串，对应URL参数的msg_signature
        // @param sTimeStamp: 时间戳，对应URL参数的timestamp
        // @param sNonce: 随机串，对应URL参数的nonce
        // @param sEchoStr: 随机串，对应URL参数的echostr
        // @param sReplyEchoStr: 解密之后的echostr，当return返回0时有效
        // @return：成功0，失败返回对应的错误码
        public int VerifyURL(string sMsgSignature, string sTimeStamp, string sNonce, string sEchoStr, ref string sReplyEchoStr)
        {
            int ret = 0;
            if (m_sEncodingAESKey.Length != 43)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
            }
            ret = VerifySignature(m_sToken, sTimeStamp, sNonce, sEchoStr, sMsgSignature);
            if (0 != ret)
            {
                return ret;
            }
            sReplyEchoStr = "";
            string cpid = "";
            try
            {
                sReplyEchoStr = Cryptography.AES_decrypt(sEchoStr, m_sEncodingAESKey, ref cpid); //m_sCorpID);
            }
            catch (Exception)
            {
                sReplyEchoStr = "";
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
            }
            if (cpid != m_sAppID)
            {
                sReplyEchoStr = "";
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateAppid_Error;
            }
            return 0;
        }


        public static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, ref string sMsgSignature)
        {
            ArrayList AL = new ArrayList();
            AL.Add(sToken);
            AL.Add(sTimeStamp);
            AL.Add(sNonce);
            AL.Add(sMsgEncrypt);
            AL.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
            }
            sMsgSignature = hash;
            return 0;
        }

    }
}
