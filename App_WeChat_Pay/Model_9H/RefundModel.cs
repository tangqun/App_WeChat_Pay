using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    public class RefundModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string OutTradeNo { get; set; }
        public string TradeNo { get; set; }
        public string OutRefundNo { get; set; }
        public string RefundNo { get; set; }
        public int TotalFee { get; set; }
        public int RefundFee { get; set; }
        public int RealRefundFee { get; set; }
        public int RState { get; set; }
        public string RefundState { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
