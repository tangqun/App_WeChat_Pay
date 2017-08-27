using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H
{
    [Serializable]
    public class WxConfigModel
    {
        public int Id { get; set; }
        public string WxAPPID { get; set; }
        public string MCHID { get; set; }
        public string KEY { get; set; }
        public string APPSECRET { get; set; }
        public string SSLCERT_PATH { get; set; }
        public string SSLCERT_PASSWORD { get; set; }
        public string NOTIFY_URL { get; set; }
        public string IP { get; set; }
        public string CallBackUrl { get; set; }
        public int AppId { get; set; }
    }
}
