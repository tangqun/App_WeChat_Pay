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
    public class WxConfigBLL : IWxConfigBLL
    {
        private IWxConfigDAL wxConfigDAL = new WxConfigDAL();

        public WxConfigModel GetByAppId(int appId)
        {
            return wxConfigDAL.GetByAppId(appId);
        }
    }
}
