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
    public class PayController : ApiController
    {
        private IPayBLL payBLL = new PayBLL();

        [HttpGet]
        public RESTfulModel UnifiedOrder(string authorizerAppID = "", string openID = "", string outTradeNo = "", int totalFee = 0, string body = "")
        {
            return payBLL.UnifiedOrder(authorizerAppID, openID, outTradeNo, totalFee, body);
        }
    }
}
