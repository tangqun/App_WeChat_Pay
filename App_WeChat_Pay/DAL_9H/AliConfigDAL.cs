using Helper_9H;
using IDAL_9H;
using Model_9H;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_9H
{
    public class AliConfigDAL : IAliConfigDAL
    {
        public AliConfigModel GetByAppId(int appId)
        {
            string sql = "SELECT * FROM `aliconfig` WHERE AppId=@AppId";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@AppId", appId));
            if (dr != null)
            {
                AliConfigModel model = new AliConfigModel();
                model.Id = dr["Id"].ToInt();
                model.PARTNER = dr["PARTNER"].ToString();
                model.KEY = dr["KEY"].ToString();
                model.SELLER_ID = dr["SELLER_ID"].ToString();
                model.PrivateKey = dr["PrivateKey"].ToString();
                model.PublicKey = dr["PublicKey"].ToString();
                model.NOTIFY_URL = dr["NOTIFY_URL"].ToString();
                model.CallBackUrl = dr["CallBackUrl"].ToString();
                model.AppId = dr["AppId"].ToInt();
                model.Refund_Url = dr["Refund_Url"].ToString();
                model.RefundToBank_Url = dr["RefundToBank_Url"].ToString();
                return model;
            }
            return null;
        }
    }
}
