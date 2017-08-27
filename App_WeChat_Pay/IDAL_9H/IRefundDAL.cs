using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL_9H
{
    public interface IRefundDAL
    {
        RefundModel GetByOutRefundNo(string outRefundNo);

        bool Insert(int appId, string unionUserId, string outTradeNo, string tradeNo, string outRefundNo, string refundNo, int totalFee, int refundFee, int realRefundFee, string refundTime, int rState, string refundState);

        bool Update(string outRefundNo, string refundNo, int realRefundFee, int rState, string refundState);
    }
}
