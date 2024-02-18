using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discoursify.Models
{
    public class StaticContent
    {
        public static void init()
        {
            HttpContext.Current.Session["USER_ROLE"] = "";
            HttpContext.Current.Session["USER_NAME"] = "";
            HttpContext.Current.Session["USER_EMAIL"] = "";
        }
    }
}