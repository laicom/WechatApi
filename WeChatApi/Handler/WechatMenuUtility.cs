/********
 * 
 *  Description:
 *  WechatMenuUtility 菜单更新工具
 * Usage:  
 * 
 *  Create By 赖国欣(guoxin.lai@gmail.com)
 *  
 *  
 *  Revision History:
 *  Date                  Who                 What
 *  2014-12          guoxin.lai           Created
 * 
 ********/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    public class WechatMenuUtility : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                var action = context.Request.QueryString["a"];
                if (string.IsNullOrWhiteSpace(action))
                {
                    context.Response.Write("require [a] pramameter.a=create or delete");
                    context.Response.Write(DefaultMsg);
                    return;
                }
                var menuFile = context.Request.QueryString["f"];
                if (string.IsNullOrEmpty(menuFile))
                {
                    menuFile = "wxmenu.js";
                }
                menuFile = context.Server.MapPath(menuFile);
                if (action == "create")
                {
                    if (!System.IO.File.Exists(menuFile))
                    {
                        context.Response.Write("wxmenu file not found");
                        context.Response.Write(DefaultMsg);
                        return;
                    }
                    var menuJson = System.IO.File.ReadAllText(menuFile, System.Text.Encoding.UTF8);
                    var mm = new MenuManage();
                    var result = mm.CreateMenu(menuJson);
                    context.Response.Write("menu create result:" + result);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format("error:" + ex.Message + "--" + ex.StackTrace));
            }
            context.Response.Write(DefaultMsg);
        }

        const string DefaultMsg = @"<hr />WeChatApi Create menu utility. Create by Laiguoxin(guoxin.lai@gmail.com,http://www.9499.net) 2014-12 reference:https://github.com/laicom/WechatApi.git";
    }
}
