using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Helper_Pay
{
    public class WxPayHelper
    {

        public static string MakeSign(SortedDictionary<string, object> sortedDictionary, string key)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in sortedDictionary)
            {
                if (pair.Value == null)
                {
                    WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');

            //在string后加入API KEY
            buff += "&key=" + key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(buff));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        public static string ToXml(SortedDictionary<string, object> sortedDictionary)
        {
            //数据为空时不能转化为xml格式
            if (0 == sortedDictionary.Count)
            {
                WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData数据为空!");
                throw new WxPayException("WxPayData数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in sortedDictionary)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData字段数据类型错误!");
                    throw new WxPayException("WxPayData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        public static bool CheckSign(SortedDictionary<string, object> sortedDictionary, string key)
        {
            //如果没有设置签名，则跳过检测
            object obj;
            sortedDictionary.TryGetValue("sign", out obj);
            if (obj == null)
            {
                WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData签名不存在!");
                throw new WxPayException("WxPayData签名不存在!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if (sortedDictionary["sign"] == null || sortedDictionary["sign"].ToString() == "")
            {
                WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData签名存在但不合法!");
                throw new WxPayException("WxPayData签名存在但不合法!");
            }

            //获取接收到的签名
            string return_sign = sortedDictionary["sign"].ToString();

            //在本地计算新的签名
            string cal_sign = MakeSign(sortedDictionary, key);

            if (cal_sign == return_sign)
            {
                return true;
            }

            WxLogHelper.Error(typeof(WxPayHelper).ToString(), "WxPayData签名验证错误!");
            throw new WxPayException("WxPayData签名验证错误!");
        }
    }

    public class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}
