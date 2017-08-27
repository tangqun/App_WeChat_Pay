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
    public class RedPackBLL : IRedPackBLL
    {
        private IRedPackDAL redPackDAL = new RedPackDAL();

        public bool Insert(int appId, string unionUserId, string mch_BillNo, string wishing, string remark, string act_name, string openid)
        {
            return redPackDAL.Insert(appId, unionUserId, mch_BillNo, wishing, remark, act_name, openid);
        }

        public RedPackModel GetByMchBillNo(string mch_billno)
        {
            return redPackDAL.GetByMchBillNo(mch_billno);
        }

        public bool Update(string mch_billno, string txNo, int sendState, string txState, string sendType, string type, int total_num, int total_amount, string fail_reason, string send_time, string refund_time, int refund_amount, string single_amount, string rev_time)
        {
            return redPackDAL.Update(mch_billno, txNo, sendState, txState, sendType, type, total_num, total_amount, fail_reason, send_time, refund_time, refund_amount, single_amount, rev_time);
        }
    }
}
