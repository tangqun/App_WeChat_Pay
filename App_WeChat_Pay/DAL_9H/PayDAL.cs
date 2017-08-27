using Helper_9H;
using IDAL_9H;
using Model_9H;
using Model_9H.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_9H
{
    public class PayDAL : IPayDAL
    {
        public bool Insert(string unionUserId, string outTradeNo, int totalFee, string body, int appId)
        {
            string sql = "INSERT INTO `pay` (`AppId`, `UnionUserId`, `OutTradeNo`, `TotalFee`, `Body`, `PayType`, `TradeNo`, `RealFee`, `PayTime`, `PayState`, `TradeState`, `RefundFee`, `RefundTime`, `CreateTime`, `UpdateTime`) VALUES (@AppId, @UnionUserId, @OutTradeNo, @TotalFee, @Body, @PayType, '', 0, '1970-01-01', '1', '', 0, '1970-01-01', NOW(), '1970-01-01');";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AppId",appId),
                new MySqlParameter("@UnionUserId", unionUserId),
                new MySqlParameter("@OutTradeNo", outTradeNo),
                new MySqlParameter("@TotalFee", totalFee),
                new MySqlParameter("@Body", body),
                new MySqlParameter("@PayType",(int)PayTypeEnum.无),
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public bool Update(string outTradeNo, int payType, string tradeNo, int realFee, DateTime payTime, int payState, string tradeState)
        {
            string sql = "UPDATE `pay` SET PayType=@PayType, TradeNo=@TradeNo, RealFee=@RealFee, PayTime=@PayTime, PayState=@PayState, TradeState=@TradeState, UpdateTime=NOW() WHERE OutTradeNo=@OutTradeNo";
            MySqlParameter[] parameters = {
                new MySqlParameter("@PayType", payType),
                new MySqlParameter("@TradeNo", tradeNo),
                new MySqlParameter("@RealFee", realFee),
                new MySqlParameter("@PayTime", payTime),
                new MySqlParameter("@PayState", payState),
                new MySqlParameter("@TradeState", tradeState),
                new MySqlParameter("@OutTradeNo", outTradeNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public bool Update(string tradeNo, int refundFee)
        {
            string sql = "UPDATE `pay` SET RefundFee=RefundFee+@RefundFee WHERE TradeNo=@TradeNo";
            MySqlParameter[] parameters = {
                new MySqlParameter("@RefundFee", refundFee),
                new MySqlParameter("@TradeNo", tradeNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public bool Update(string outTradeNo, DateTime refundTime, int payState, string tradeState)
        {
            string sql = "UPDATE `pay` SET RefundTime=@RefundTime, PayState=@PayState, TradeState=@TradeState, UpdateTime=NOW() WHERE OutTradeNo=@OutTradeNo";
            MySqlParameter[] parameters = {
                new MySqlParameter("@RefundTime", refundTime),
                new MySqlParameter("@PayState", payState),
                new MySqlParameter("@TradeState", tradeState),
                new MySqlParameter("@OutTradeNo", outTradeNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public bool Update(string outTradeNo, int refundFee, DateTime refundTime, int payState, string tradeState)
        {
            string sql = "UPDATE `pay` SET RefundFee=RefundFee+@RefundFee, RefundTime=@RefundTime, PayState=@PayState, TradeState=@TradeState, UpdateTime=NOW() WHERE OutTradeNo=@OutTradeNo";
            MySqlParameter[] parameters = {
                new MySqlParameter("@RefundFee", refundFee),
                new MySqlParameter("@RefundTime", refundTime),
                new MySqlParameter("@PayState", payState),
                new MySqlParameter("@TradeState", tradeState),
                new MySqlParameter("@OutTradeNo", outTradeNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public PayModel GetByOutTradeNo(string outTradeNo)
        {
            string sql = "SELECT * FROM `pay` WHERE OutTradeNo=@OutTradeNo";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@OutTradeNo", outTradeNo));
            return EntityToModel(dr);
        }

        public PayModel GetByTradeNo(string tradeNo)
        {
            string sql = "SELECT * FROM `pay` WHERE TradeNo=@TradeNo";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@TradeNo", tradeNo));
            return EntityToModel(dr);
        }

        private PayModel EntityToModel(DataRow dr)
        {
            if (dr != null)
            {
                PayModel model = new PayModel();
                model.Id = dr["Id"].ToInt();
                model.AppId = dr["AppId"].ToInt();
                model.UnionUserId = dr["UnionUserId"].ToString();
                model.OutTradeNo = dr["OutTradeNo"].ToString();
                model.TotalFee = dr["TotalFee"].ToInt();
                model.Body = dr["Body"].ToString();
                model.PayType = dr["PayType"].ToInt();
                model.TradeNo = dr["TradeNo"].ToString();
                model.RealFee = dr["RealFee"].ToInt();
                model.PayTime = dr["PayTime"].ToDateTime();
                model.PayState = dr["PayState"].ToInt();
                model.TradeState = dr["TradeState"].ToString();

                model.RefundFee = dr["RefundFee"].ToInt();
                model.RefundTime = dr["RefundTime"].ToDateTime();

                model.CreateTime = dr["CreateTime"].ToDateTime();
                model.UpdateTime = dr["UpdateTime"].ToDateTime();
                return model;
            }
            return null;
        }
    }
}
