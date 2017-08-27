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
    public class WXConfigDAL : IWXConfigDAL
    {
        public WXConfigModel GetByAuthorizerAppID(string authorizerAppID)
        {
            string sql = "SELECT * FROM `wxconfig` WHERE `authorizer_appid` = @authorizer_appid";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@authorizer_appid", authorizerAppID));
            return EntityToModel(dr);
        }

        private WXConfigModel EntityToModel(DataRow dr)
        {
            if (dr != null)
            {
                WXConfigModel model = new WXConfigModel();
                model.ID = dr["id"].ToInt();
                model.AuthorizerAppID = dr["authorizer_appid"].ToString();
                model.MCHID = dr["mch_id"].ToString();
                model.KEY = dr["key"].ToString();
                model.SSLCERT_PATH = dr["sslcert_path"].ToString();
                model.SSLCERT_PASSWORD = dr["sslcert_password"].ToString();
                model.NOTIFY_URL = dr["notify_url"].ToString();
                return model;
            }
            return null;
        }
    }
}
