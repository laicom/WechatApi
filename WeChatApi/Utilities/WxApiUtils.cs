

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public static class WxApiUtils
    {
        /// <summary>
        /// TimeStamp string means total seconds escape from epoc standard times
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTimeStamp(this DateTime dt)
        {
            return ((int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
        }

        /// <summary>
        /// get current timestamp
        /// TimeStamp string means total seconds escape from epoc standard times
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }


        /// <summary>
        /// 字节传换成16进制表示的字符串
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <returns></returns>
        public static string BytesToHexString(this byte[] data)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (byte b in data)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// 16进制表示的字符串换成字节
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("the string is  invalid hexString.");
            var len = hexString.Length / 2;
            var bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bytes;
        }


        /// <summary>
        /// 列表输出为特定字符分隔的字符串
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="splitChar">分隔符，默认为逗号</param>
        /// <returns></returns>
        public static string ListToString<T>(IEnumerable<T> list, char splitChar = ',')
        {
            int index = 0, len = list.Count();
            StringBuilder sb = new StringBuilder(len * 32);
            foreach (var item in list)
            {
                sb.Append(item);
                if (++index < len) sb.Append(splitChar);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 列表输出为特定字符分隔的字符串
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="splitChar">分隔符，默认为逗号</param>
        /// <returns></returns>
        public static string ListToString<T>(IEnumerable<T> list, string splitChars)
        {
            int index = 0, len = list.Count();
            StringBuilder sb = new StringBuilder(len * 32);
            foreach (var item in list)
            {
                sb.Append(item);
                if (++index < len) sb.Append(splitChars);
            }
            return sb.ToString();
        }
    }
}
