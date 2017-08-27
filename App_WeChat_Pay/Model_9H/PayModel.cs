using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    [Serializable]
    public class PayModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string UnionUserId { get; set; }
        public string OutTradeNo { get; set; }
        public int TotalFee { get; set; }
        public string Body { get; set; }
        public int PayType { get; set; }
        public string TradeNo { get; set; }
        public int RealFee { get; set; }
        public DateTime PayTime { get; set; }
        public int PayState { get; set; }
        public string TradeState { get; set; }

        public int RefundFee { get; set; }
        public DateTime RefundTime { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
