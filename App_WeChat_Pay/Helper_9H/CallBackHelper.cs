using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helper_Pay
{
    public class CallBackHelper
    {
        public static void Post_Wx(string url, string para)
        {
            try
            {
                LogHelper.Info_wx("回调参数记录: " + para);
                HttpWebRequest request_callback = (HttpWebRequest)HttpWebRequest.Create(url);
                request_callback.Method = "POST";
                request_callback.ContentType = "application/json";
                byte[] bytes_callback = System.Text.Encoding.UTF8.GetBytes(para);
                Stream reqest_callback_stream = request_callback.GetRequestStream();
                reqest_callback_stream.Write(bytes_callback, 0, bytes_callback.Length);
                reqest_callback_stream.Close();
                HttpWebResponse response_callback = (HttpWebResponse)request_callback.GetResponse();
                StreamReader streamReader_callback = new StreamReader(response_callback.GetResponseStream(), Encoding.UTF8);
                string responseStr_callback = streamReader_callback.ReadToEnd();
                if (responseStr_callback != null)
                {
                    LogHelper.Info_wx(responseStr_callback);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("微信回调失败(" + url + "): ", ex);
            }
        }

        public static void Post_Ali(string url, string para)
        {
            try
            {
                LogHelper.Info_ali("回调参数记录: " + para);
                HttpWebRequest request_callback = (HttpWebRequest)HttpWebRequest.Create(url);
                request_callback.Method = "POST";
                request_callback.ContentType = "application/json";
                byte[] bytes_callback = System.Text.Encoding.UTF8.GetBytes(para);
                Stream reqest_callback_stream = request_callback.GetRequestStream();
                reqest_callback_stream.Write(bytes_callback, 0, bytes_callback.Length);
                reqest_callback_stream.Close();
                HttpWebResponse response_callback = (HttpWebResponse)request_callback.GetResponse();
                StreamReader streamReader_callback = new StreamReader(response_callback.GetResponseStream(), Encoding.UTF8);
                string responseStr_callback = streamReader_callback.ReadToEnd();
                if (responseStr_callback != null)
                {
                    LogHelper.Info_ali(responseStr_callback);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("支付宝回调失败(" + url + "): ", ex);
            }
        }
    }
}
