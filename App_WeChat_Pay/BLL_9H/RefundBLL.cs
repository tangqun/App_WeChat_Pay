using DAL_9H;
using IBLL_9H;
using IDAL_9H;
using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_9H
{
    public class RefundBLL : IRefundBLL
    {
        private IRefundDAL refundDAL = new RefundDAL();

        public RefundModel GetByOutRefundNo(string outRefundNo)
        {
            return refundDAL.GetByOutRefundNo(outRefundNo);
        }

        public bool Insert(int appId, string unionUserId, string outTradeNo, string tradeNo, string outRefundNo, string refundNo, int totalFee, int refundFee, int realRefundFee, string refundTime, int rState, string refundState)
        {
            return refundDAL.Insert(appId, unionUserId, outTradeNo, tradeNo, outRefundNo, refundNo, totalFee, refundFee, realRefundFee, refundTime, rState, refundState);
        }

        public bool Update(string outRefundNo, string refundNo, int realRefundFee, int rState, string refundState)
        {
            return refundDAL.Update(outRefundNo, refundNo, realRefundFee, rState, refundState);
        }
    }
}
