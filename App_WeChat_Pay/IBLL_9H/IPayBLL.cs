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
        RESTfulModel UnifiedOrder(string authorizerAppID, string openID, int totalFee, string body);
        RESTfulModel OrderQuery(string outTradeNo);
        RESTfulModel WXUnifiedOrder(string outTradeNo, string ip, int period, string tradeType);
        string WXReceiveNotify(string authorizerAppID, string xml_from);
        RESTfulModel WXOrderQuery(string outTradeNo);
    }
}
