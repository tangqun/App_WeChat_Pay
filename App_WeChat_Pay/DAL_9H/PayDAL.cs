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
        public bool Insert(string authorizerAppID, string openID, string outTradeNo, int totalFee, string body, DateTime dt)
        {
            string sql =
                @"INSERT INTO `pay`
                            (`authorizer_appid`,
                             `openid`,
                             `out_trade_no`,
                             `total_fee`,
                             `body`,
                             `pay_type`,
                             `trade_no`,
                             `real_fee`,
                             `trade_state`,
                             `pay_state`,
                             `pay_time`,
                             `refund_fee`,
                             `refund_time`,
                             `create_time`,
                             `update_time`)
                VALUES (@authorizer_appid,
                        @openid,
                        @out_trade_no,
                        @total_fee,
                        @body,
                        @pay_type,
                        '',
                        0,
                        '',
                        @pay_state,
                        @pay_time,
                        0,
                        @refund_time,
                        @create_time,
                        @update_time);";
            MySqlParameter[] parameters = {
                new MySqlParameter("@authorizer_appid", authorizerAppID),
                new MySqlParameter("@openid", openID),
                new MySqlParameter("@out_trade_no", outTradeNo),
                new MySqlParameter("@total_fee", totalFee),
                new MySqlParameter("@body", body),
                new MySqlParameter("@pay_type", (int)PayTypeEnum.默认值),
                new MySqlParameter("@pay_state", (int)PayStateEnum.未支付),
                new MySqlParameter("@pay_time", dt),
                new MySqlParameter("@refund_time", dt),
                new MySqlParameter("@create_time", dt),
                new MySqlParameter("@update_time", dt),
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public bool Update(string outTradeNo, int payType, string tradeNo, int realFee, string tradeState, int payState, DateTime payTime)
        {
            string sql =
                @"UPDATE `pay`
                SET `pay_type` = @pay_type,
                  `trade_no` = @trade_no,
                  `real_fee` = @real_fee,
                  `trade_state` = @trade_state,
                  `pay_state` = @pay_state,
                  `pay_time` = @pay_time,
                  `update_time` = @update_time
                WHERE `out_trade_no` = @out_trade_no;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@pay_type", payType),
                new MySqlParameter("@trade_no", tradeNo),
                new MySqlParameter("@real_fee", realFee),
                new MySqlParameter("@trade_state", tradeState),
                new MySqlParameter("@pay_state", payState),
                new MySqlParameter("@pay_time", payTime),
                new MySqlParameter("@update_time", payTime),
                new MySqlParameter("@out_trade_no", outTradeNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        //public bool Update(string tradeNo, int refundFee)
        //{
        //    string sql = "UPDATE `pay` SET RefundFee=RefundFee+@RefundFee WHERE TradeNo=@TradeNo";
        //    MySqlParameter[] parameters = {
        //        new MySqlParameter("@RefundFee", refundFee),
        //        new MySqlParameter("@TradeNo", tradeNo)
        //    };
        //    return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        //}

        //public bool Update(string outTradeNo, DateTime refundTime, int payState, string tradeState)
        //{
        //    string sql = "UPDATE `pay` SET RefundTime=@RefundTime, PayState=@PayState, TradeState=@TradeState, UpdateTime=NOW() WHERE OutTradeNo=@OutTradeNo";
        //    MySqlParameter[] parameters = {
        //        new MySqlParameter("@RefundTime", refundTime),
        //        new MySqlParameter("@PayState", payState),
        //        new MySqlParameter("@TradeState", tradeState),
        //        new MySqlParameter("@OutTradeNo", outTradeNo)
        //    };
        //    return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        //}

        //public bool Update(string outTradeNo, int refundFee, DateTime refundTime, int payState, string tradeState)
        //{
        //    string sql = "UPDATE `pay` SET RefundFee=RefundFee+@RefundFee, RefundTime=@RefundTime, PayState=@PayState, TradeState=@TradeState, UpdateTime=NOW() WHERE OutTradeNo=@OutTradeNo";
        //    MySqlParameter[] parameters = {
        //        new MySqlParameter("@RefundFee", refundFee),
        //        new MySqlParameter("@RefundTime", refundTime),
        //        new MySqlParameter("@PayState", payState),
        //        new MySqlParameter("@TradeState", tradeState),
        //        new MySqlParameter("@OutTradeNo", outTradeNo)
        //    };
        //    return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        //}

        public PayModel GetByOutTradeNo(string outTradeNo)
        {
            string sql = "SELECT * FROM `pay` WHERE `out_trade_no` = @out_trade_no";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@out_trade_no", outTradeNo));
            return EntityToModel(dr);
        }

        public PayModel GetByTradeNo(string tradeNo)
        {
            string sql = "SELECT * FROM `pay` WHERE `trade_no` = @trade_no";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@trade_no", tradeNo));
            return EntityToModel(dr);
        }

        private PayModel EntityToModel(DataRow dr)
        {
            if (dr != null)
            {
                PayModel model = new PayModel();
                model.ID = dr["id"].ToInt();
                model.AuthorizerAppID = dr["authorizer_appid"].ToString();
                model.OpenID = dr["openid"].ToString();
                model.OutTradeNo = dr["out_trade_no"].ToString();
                model.TotalFee = dr["total_fee"].ToInt();
                model.Body = dr["body"].ToString();
                model.PayType = dr["pay_type"].ToInt();
                model.TradeNo = dr["trade_no"].ToString();
                model.RealFee = dr["real_fee"].ToInt();
                model.TradeState = dr["trade_state"].ToString();
                model.PayState = dr["pay_state"].ToInt();
                model.PayTime = dr["pay_time"].ToDateTime();

                model.RefundFee = dr["refund_fee"].ToInt();
                model.RefundTime = dr["refund_time"].ToDateTime();

                model.CreateTime = dr["create_time"].ToDateTime();
                model.UpdateTime = dr["update_time"].ToDateTime();
                return model;
            }
            return null;
        }
    }
}
