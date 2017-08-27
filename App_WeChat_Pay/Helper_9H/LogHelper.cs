using log4net;
using System;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace Helper_9H
{
    public class LogHelper
    {
        private static readonly ILog logError = LogManager.GetLogger("logError");
        private static readonly ILog logInfo = LogManager.GetLogger("logInfo");
        private static readonly ILog logInfo_wx = LogManager.GetLogger("logInfo_wx");
        private static readonly ILog logInfo_ali = LogManager.GetLogger("logInfo_ali");
        private static readonly ILog logInfo_redpack = LogManager.GetLogger("logInfo_redpack");

        public static void Error(Exception ex)
        {
            string output = "时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "异常信息：" + ex.Message + "\r\n" + "堆栈信息：" + ex.StackTrace + "\r\n\r\n";
            logError.Error(output);
        }

        public static void Info(string title, string msg)
        {
            string output = "时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "标题：" + title + "\r\n" + "内容：" + msg + "\r\n\r\n";
            logInfo.Info(output);
        }

        public static void Info_wx(string title, string msg)
        {
            string output = "时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "标题：" + title + "\r\n" + "内容：" + msg + "\r\n\r\n";
            logInfo_wx.Info(output);
        }

        public static void Info_ali(string title, string msg)
        {
            string output = "时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "标题：" + title + "\r\n" + "内容：" + msg + "\r\n\r\n";
            logInfo_ali.Info(output);
        }

        public static void Info_redpack(string title, string msg)
        {
            string output = "时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + "标题：" + title + "\r\n" + "内容：" + msg + "\r\n\r\n";
            logInfo_redpack.Info(output);
        }
    }
}
