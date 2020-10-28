using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Common
{
    public static class CommonConstants
    {
        public  const string USER_SESSION = "USER_SESSION";
        public static string CartSession = "CartSession";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";

        public static string CurrentCulture { set; get; }
    }
}