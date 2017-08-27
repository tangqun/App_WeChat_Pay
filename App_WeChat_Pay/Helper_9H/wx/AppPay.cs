using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace WxPayAPI
{
    public class AppPay
    {
        public string body { get; set; }

        public string out_trade_no { get; set; }

        public int period { get; set; }

        public int total_fee { get; set; }

        public string appid { get; set; }

        public string mch_id { get; set; }

        public string spbill_create_ip { get; set; }

        public string notify_url { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string key)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", body);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            //data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //data.SetValue("time_expire", DateTime.Now.AddMinutes(period).ToString("yyyyMMddHHmmss"));
            data.SetValue("trade_type", "APP");
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            data.SetValue("spbill_create_ip", spbill_create_ip);
            data.SetValue("notify_url", notify_url);

            WxPayData result = WxPayApi.UnifiedOrder(data, key);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }
    }
}
