using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper_Pay
{
    public class WxConfigHelper
    {
        private string appid;
        private string appsecret;
        private string notify_url;
        private string mchid;
        private string key;
        public string AppId
        {
            get { return appid; }
        }
        public string AppSecret
        {
            get { return appsecret; }
        }
        public string Notify_Url
        {
            get { return notify_url; }
        }
        public string MchId
        {
            get { return mchid; }
        }
        public string Key
        {
            get { return key; }
        }

        // 微信开放平台
        public static string IP = ConfigurationManager.AppSettings["IP"];
        public static string Log_Level = ConfigurationManager.AppSettings["Log_Level"];

        public WxConfigHelper(int appId)
        {
            if (appId == 1)
            {
                this.appid = ConfigurationManager.AppSettings["AppId_ZhongChao_Open"];
                this.appsecret = ConfigurationManager.AppSettings["AppSecret_ZhongChao_Open"];
                this.notify_url = ConfigurationManager.AppSettings["Notify_Url_ZhongChao_Open"];

                this.mchid = ConfigurationManager.AppSettings["MchId_ZhongChao"];
                this.key = ConfigurationManager.AppSettings["Key_ZhongChao"];
            }
        }
    }
}
