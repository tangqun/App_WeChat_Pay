using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    public class RedPackModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string UnionUserId { get; set; }
        public string Mch_BillNo { get; set; }
        public string TxNo { get; set; }
        public int SendState { get; set; }
        public string TxState { get; set; }
        public string SendType { get; set; }
        public string Type { get; set; }
        public int Total_Num { get; set; }
        public int Total_Amount { get; set; }
        public string Fail_Reason { get; set; }
        public string Send_Time { get; set; }
        public string Refund_Time { get; set; }
        public int Refund_Amount { get; set; }
        public string Wishing { get; set; }
        public string Remark { get; set; }
        public string Act_Name { get; set; }
        public string OpenId { get; set; }
        public string Single_Amount { get; set; }
        public string Rev_Time { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
