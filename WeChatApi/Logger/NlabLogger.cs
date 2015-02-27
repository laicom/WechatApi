/*
 * plate text logger
 * author Laiguoxin<guoxin.lai@gmail.com>
 * Create date:  2014-9-28
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Nlab.WeChatApi.Logger
{
    class NlabLogger
    {

        private static string _logFilePath;
        private static DateTime nextDate;
        private static bool accessDeny = false;
        private static bool displayInConsole = System.Environment.UserInteractive;
        public static void Log(string msg)
        {
            if (displayInConsole)
                Console.WriteLine("{1:HH:mm:ss}{0}", msg,DateTime.Now);
            if (accessDeny) return; //无法写日志，不再执行
            try
            {
                if (_logFilePath == null || DateTime.Now > nextDate)
                {
                    string fileName = string.Format(@"{0}{1:yyMMdd}.log", "NlabWechatErr", DateTime.Now);
                    string filePath;
                    if(HttpContext.Current!=null)
                        filePath = HttpContext.Current.Server.MapPath(@".\log");
                    else
                        filePath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    _logFilePath = Path.Combine(filePath, fileName);
                    nextDate = DateTime.Now.Date.AddDays(1);
                }
                using (FileStream fs = new FileStream(_logFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("HH:mm:ss ") + msg + "\r\n");
                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch (IOException)
            {
                accessDeny = true;
            }
            catch (UnauthorizedAccessException)
            {
                accessDeny = true;
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("log error:{0}--{1}",ex.Message,ex.StackTrace));
             }
        }

        public static void LogError(Exception ex, string msg=null)
        {
            var m=string.Format("{0} \r\n     {1}\r\n           {2}",ex.Message,ex.StackTrace,msg);
            Log(m);
        }
    }
}
