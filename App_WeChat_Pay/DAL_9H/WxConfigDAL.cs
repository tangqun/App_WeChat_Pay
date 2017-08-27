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
    public class WxConfigDAL : IWxConfigDAL
    {
        public WxConfigModel GetByAppId(int appId)
        {
            string sql = "SELECT * FROM `wxconfig` WHERE AppId=@AppId";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@AppId", appId));
            if (dr != null)
            {
                WxConfigModel model = new WxConfigModel();
                model.Id = dr["Id"].ToInt();
                model.WxAPPID = dr["WxAPPID"].ToString();
                model.MCHID = dr["MCHID"].ToString();
                model.KEY = dr["KEY"].ToString();
                model.APPSECRET = dr["APPSECRET"].ToString();
                model.SSLCERT_PATH = dr["SSLCERT_PATH"].ToString();
                model.SSLCERT_PASSWORD = dr["SSLCERT_PASSWORD"].ToString();
                model.NOTIFY_URL = dr["NOTIFY_URL"].ToString();
                model.IP = dr["IP"].ToString();
                model.CallBackUrl = dr["CallBackUrl"].ToString();
                model.AppId = dr["AppId"].ToInt();
                return model;
            }
            return null;
        }
    }
}
