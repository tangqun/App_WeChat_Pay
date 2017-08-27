using Model_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL_9H
{
    public interface IRedPackBLL
    {
        bool Insert(int appId, string unionUserId, string mch_BillNo, string wishing, string remark, string act_name, string openid);
        RedPackModel GetByMchBillNo(string mch_billno);
        bool Update(string mch_billno, string txNo, int sendState, string txState, string sendType, string type, int total_num, int total_amount, string fail_reason, string send_time, string refund_time, int refund_amount, string single_amount, string rev_time);
    }
}
