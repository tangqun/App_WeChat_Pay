using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WxPayAPI
{
    public class AppNotify : Notify
    {
        public AppNotify(HttpRequestBase request, HttpResponseBase response)
            : base(request, response)
        {

        }

        public virtual void ProcessNotify(WxPayData wxPayData)
        {

        }
    }
}
