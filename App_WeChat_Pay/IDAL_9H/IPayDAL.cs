using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL_9H
{
    public interface IPayDAL
    {
        bool Insert(string authorizerAppID, string openID, string outTradeNo, int totalFee, string body, DateTime dt);

        bool Update(string outTradeNo, int payType, string tradeNo, int realFee, string tradeState, int payState, DateTime payTime);

        // 支付宝更新退款金额
        //bool Update(string tradeNo, int refundFee);

        // 支付宝更新订单转入退款
        //bool Update(string outTradeNo, DateTime refundTime, int payState, string tradeState);

        // 微信同步处理，更新订单转入退款，更新退款金额
        //bool Update(string outTradeNo, int refundFee, DateTime refundTime, int payState, string tradeState);

        PayModel GetByOutTradeNo(string outTradeNo);

        PayModel GetByTradeNo(string tradeNo);
    }
}
