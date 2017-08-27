using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL_9H
{
    public interface IPayBLL
    {
        RESTfulModel UnifiedOrder(string authorizerAppID, string openID, string outTradeNo, int totalFee, string body);
        RESTfulModel WXUnifiedOrder(string outTradeNo, string ip, int period, string tradeType, string openid);
        string WXReceiveNotify(string authorizerAppID, string xml_from);
        RESTfulModel WXOrderQuery(string outTradeNo);
    }
}
