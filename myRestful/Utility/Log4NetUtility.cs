using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace myRestful.Utility
{
    public class Log4NetUtility
    {
        //這裡是設定檔的位置，實際寫出的log檔在log4net.config裡面
        public static String GetLog4netConfigPath()
        {
            return ConfigurationManager.AppSettings["Log4NetConfigPath"];
        }
       
    }
}