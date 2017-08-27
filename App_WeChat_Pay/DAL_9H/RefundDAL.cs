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
    public class RefundDAL : IRefundDAL
    {
        public RefundModel GetByOutRefundNo(string outRefundNo)
        {
            string sql = "SELECT * FROM `refund` WHERE OutRefundNo = @OutRefundNo;";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@OutRefundNo", outRefundNo));
            if (dr != null)
            {
                RefundModel model = new RefundModel();
                model.Id = dr["Id"].ToInt();
                model.AppId = dr["AppId"].ToInt();
                model.OutTradeNo = dr["OutTradeNo"].ToString();
                model.TradeNo = dr["TradeNo"].ToString();
                model.OutRefundNo = dr["OutRefundNo"].ToString();
                model.RefundNo = dr["RefundNo"].ToString();
                model.TotalFee = dr["TotalFee"].ToInt();
                model.RefundFee = dr["RefundFee"].ToInt();
                model.RealRefundFee = dr["RealRefundFee"].ToInt();
                model.RState = dr["RState"].ToInt();
                model.RefundState = dr["RefundState"].ToString();
                model.CreateTime = dr["CreateTime"].ToDateTime();
                model.UpdateTime = dr["UpdateTime"].ToDateTime();
                return model;
            }
            return null;
        }


        public bool Insert(int appId, string unionUserId, string outTradeNo, string tradeNo, string outRefundNo, string refundNo, int totalFee, int refundFee, int realRefundFee, string refundTime, int rState, string refundState)
        {
            string sql = @"INSERT INTO `jhpay`.`refund`
                                                (`AppId`,
                                                 `UnionUserId`,
                                                 `OutTradeNo`,
                                                 `TradeNo`,
                                                 `OutRefundNo`,
                                                 `RefundNo`,
                                                 `TotalFee`,
                                                 `RefundFee`,
                                                 `RealRefundFee`,
                                                 `RefundTime`,
                                                 `RState`,
                                                 `RefundState`,
                                                 `CreateTime`,
                                                 `UpdateTime`)
                                    VALUES (@AppId,
                                            @UnionUserId,
                                            @OutTradeNo,
                                            @TradeNo,
                                            @OutRefundNo,
                                            @RefundNo,
                                            @TotalFee,
                                            @RefundFee,
                                            @RealRefundFee,
                                            @RefundTime,
                                            @RState,
                                            @RefundState,
                                            NOW(),
                                            NOW());";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AppId", appId),
                new MySqlParameter("@UnionUserId", unionUserId),
                new MySqlParameter("@OutTradeNo", outTradeNo),
                new MySqlParameter("@TradeNo", tradeNo),
                new MySqlParameter("@OutRefundNo", outRefundNo),
                new MySqlParameter("@RefundNo", refundNo),
                new MySqlParameter("@TotalFee", totalFee),
                new MySqlParameter("@RefundFee", refundFee),
                new MySqlParameter("@RealRefundFee", realRefundFee),
                new MySqlParameter("@RefundTime", refundTime),
                new MySqlParameter("@RState", rState),
                new MySqlParameter("@RefundState", refundState),
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }


        public bool Update(string outRefundNo, string refundNo, int realRefundFee, int rState, string refundState)
        {
            string sql = "UPDATE `refund` SET RefundNo = @RefundNo, RealRefundFee = @RealRefundFee, RState = @RState, RefundState = @RefundState WHERE OutRefundNo = @OutRefundNo;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@RefundNo", refundNo),
                new MySqlParameter("@RealRefundFee", realRefundFee),
                new MySqlParameter("@RState", rState),
                new MySqlParameter("@RefundState", refundState),
                new MySqlParameter("@OutRefundNo", outRefundNo)
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }
    }
}
