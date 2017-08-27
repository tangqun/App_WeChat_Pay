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
    public class RedPackDAL : IRedPackDAL
    {
        public bool Insert(int appId, string unionUserId, string mch_BillNo, string wishing, string remark, string act_name, string openid)
        {
            string sql = "INSERT INTO `redpack`" +
                        "(`AppId`, `UnionUserId`, `Mch_BillNo`,`TxNo`, `SendState`, `TxState`, `SendType`, `Type`, `Total_Num`, `Total_Amount`, `Fail_Reason`, `Send_Time`, `Refund_Time`, `Refund_Amount`, `Wishing`, `Remark`, `Act_Name`, `OpenId`, `Single_Amount`, `Rev_Time`, `CreateTime`, `UpdateTime`)" +
                        "VALUES (@AppId, @UnionUserId, @Mch_BillNo, '', '1', '', '', '', '0', '0', '', '', '', '0', @Wishing, @Remark, @Act_Name, @OpenId, '0', '', NOW(), '1970-01-01');";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AppId",appId),
                new MySqlParameter("@UnionUserId", unionUserId),
                new MySqlParameter("@Mch_BillNo", mch_BillNo),
                new MySqlParameter("@Wishing", wishing),
                new MySqlParameter("@Remark", remark),
                new MySqlParameter("@Act_Name", act_name),
                new MySqlParameter("@OpenId", openid),
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }

        public RedPackModel GetByMchBillNo(string mch_billno)
        {
            string sql = "SELECT * FROM `redpack` WHERE `Mch_BillNo` = @Mch_BillNo";
            DataRow dr = MySqlHelper.ExecuteDataRow(ConfigHelper.ConnStr, sql, new MySqlParameter("@Mch_BillNo", mch_billno));
            if (dr != null)
            {
                RedPackModel model = new RedPackModel();
                model.Id = dr["Id"].ToInt();
                model.AppId = dr["AppId"].ToInt();
                model.UnionUserId = dr["UnionUserId"].ToString();
                model.Mch_BillNo = dr["Mch_BillNo"].ToString();
                model.TxNo = dr["TxNo"].ToString();
                model.SendState = dr["SendState"].ToInt();
                model.TxState = dr["TxState"].ToString();
                model.SendType = dr["SendType"].ToString();
                model.Type = dr["Type"].ToString();
                model.Total_Num = dr["Total_Num"].ToInt();
                model.Total_Amount = dr["Total_Amount"].ToInt();
                model.Fail_Reason = dr["Fail_Reason"].ToString();
                model.Send_Time = dr["Send_Time"].ToString();
                model.Refund_Time = dr["Refund_Time"].ToString();
                model.Refund_Amount = dr["Refund_Amount"].ToInt();
                model.Wishing = dr["Wishing"].ToString();
                model.Remark = dr["Remark"].ToString();
                model.Act_Name = dr["Act_Name"].ToString();
                model.OpenId = dr["OpenId"].ToString();
                model.Single_Amount = dr["Single_Amount"].ToString();
                model.Rev_Time = dr["Rev_Time"].ToString();
                model.CreateTime = dr["CreateTime"].ToDateTime();
                model.UpdateTime = dr["UpdateTime"].ToDateTime();
                return model;
            }
            return null;
        }

        public bool Update(string mch_billno, string txNo, int sendState, string txState, string sendType, string type, int total_num, int total_amount, string fail_reason, string send_time, string refund_time, int refund_amount, string single_amount, string rev_time)
        {
            string sql = "UPDATE `redpack` SET `TxNo` = @TxNo, `SendState` = @SendState, `TxState` = @TxState, `SendType` = @SendType, `Type` = @Type, `Total_Num` = @Total_Num, `Total_Amount` = @Total_Amount, `Fail_Reason` = @Fail_Reason, `Send_Time` = @Send_Time, `Refund_Time` = @Refund_Time, `Refund_Amount` = @Refund_Amount, `Single_Amount` = @Single_Amount, `Rev_Time` = @Rev_Time, `UpdateTime` = NOW() WHERE `Mch_BillNo` = @Mch_BillNo";
            MySqlParameter[] parameters = {
                new MySqlParameter("@TxNo", txNo),
                new MySqlParameter("@SendState", sendState),
                new MySqlParameter("@TxState", txState),
                new MySqlParameter("@SendType", sendType),
                new MySqlParameter("@Type", type),
                new MySqlParameter("@Total_Num", total_num),
                new MySqlParameter("@Total_Amount", total_amount),
                new MySqlParameter("@Fail_Reason", fail_reason),
                new MySqlParameter("@Send_Time", send_time),
                new MySqlParameter("@Refund_Time", refund_time),
                new MySqlParameter("@Refund_Amount", refund_amount),
                new MySqlParameter("@Single_Amount", single_amount),
                new MySqlParameter("@Rev_Time", rev_time),
                new MySqlParameter("@Mch_BillNo", mch_billno),
            };
            return MySqlHelper.ExecuteNonQuery(ConfigHelper.ConnStr, sql, parameters) > 0;
        }
    }
}
