using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    [Serializable]
    public class AliConfigModel
    {
        public int Id { get; set; }
        public string PARTNER { get; set; }
        public string KEY { get; set; }
        public string NOTIFY_URL { get; set; }
        public string SELLER_ID { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string CallBackUrl { get; set; }
        public int AppId { get; set; }
        public string Refund_Url { get; set; }
        public string RefundToBank_Url { get; set; }
    }
}
