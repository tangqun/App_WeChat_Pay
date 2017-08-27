using DAL_9H;
using IBLL_9H;
using IDAL_9H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL_9H
{
    public class RetBLL:IRetBLL
    {
        private IRetDAL retDAL = new RetDAL();

        public string GetByCode(int code)
        {
            return retDAL.GetByCode(code);
        }
    }
}
