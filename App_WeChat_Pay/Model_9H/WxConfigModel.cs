using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    public class WXConfigModel
    {
        public int ID { get; set; }
        public string AuthorizerAppID { get; set; }
        public string MCHID { get; set; }
        public string KEY { get; set; }
        public string SSLCERT_PATH { get; set; }
        public string SSLCERT_PASSWORD { get; set; }
        public string NOTIFY_URL { get; set; }
    }
}
