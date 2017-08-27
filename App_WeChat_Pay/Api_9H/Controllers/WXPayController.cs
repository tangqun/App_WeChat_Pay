using BLL_9H;
using IBLL_9H;
using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api_9H.Controllers
{
    public class WXPayController : ApiController
    {
        private IPayBLL payBLL = new PayBLL();

        [HttpGet]
        public RESTfulModel UnifiedOrder(string outTradeNo = "", string ip = "", int period = 15, string tradeType = "JSAPI", string openID = "")
        {
            return payBLL.WXUnifiedOrder(outTradeNo, ip, period, tradeType, openID);
        }

        [HttpGet]
        public string ReceiveNotify(string id)
        {
            return payBLL.WXReceiveNotify(id, Request.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public RESTfulModel OrderQuery(string outTradeNo = "")
        {
            return payBLL.WXOrderQuery(outTradeNo);
        }
    }
}
