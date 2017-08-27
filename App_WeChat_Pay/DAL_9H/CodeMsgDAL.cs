using IDAL_9H;
using Model_9H;
using Model_9H.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_9H
{
    public class CodeMsgDAL : ICodeMsgDAL
    {
        #region 前台
        public string GetByCode(int code)
        {
            CodeMsgModel codeMsgModel = Singleton_CodeMsg.GetInstance().Where(o => o.Code == code).FirstOrDefault();
            return codeMsgModel == null ? "" : codeMsgModel.Msg;
        }
        #endregion
    }

    public sealed class Singleton_CodeMsg
    {
        // 懒汉式单例模式
        // 存在内存中效率更高
        // 每次添加或修改返回码需要改代码，所以可以用此种方式加载返回码
        private static List<CodeMsgModel> instance = new List<CodeMsgModel>() { 
            // 成功
            new CodeMsgModel(){ Code = (int)CodeEnum.成功, Msg = "成功" },
            new CodeMsgModel(){ Code = (int)CodeEnum.服务器异常, Msg = "服务器异常" },


            //new RetModel(){ Code = (int)CodeEnum.商户订单号不能为空, Msg = "商户订单号不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号格式错误, Msg = "商户订单号长度必须小于32个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单总金额不合法, Msg = "订单总金额必须大于0" },
            //new RetModel(){ Code = (int)CodeEnum.订单描述不能为空, Msg = "订单描述不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单描述格式错误, Msg = "订单描述长度必须小于128个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单过期时间值不合法, Msg = "订单过期时间值必须大于0分钟" },

            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单不存在, Msg = "商户订单不存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单信息与存单信息不符, Msg = "商户订单信息与存单信息不符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.平台号不存在, Msg = "平台号不存在" },

            new CodeMsgModel(){ Code = (int)CodeEnum.下单失败, Msg = "下单失败：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单未支付, Msg = "订单未支付：{0}" },
            //new RetModel(){ Code = (int)CodeEnum.支付异常, Msg = "支付异常：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.查单失败, Msg = "查单失败：{0}" },

            new CodeMsgModel(){ Code = (int)CodeEnum.订单转入退款, Msg = "订单转入退款：{0}" },



            new CodeMsgModel(){ Code = (int)CodeEnum.UnionUserId格式错误, Msg = "UnionUserId格式错误" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号已存在, Msg = "商户订单号已存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.存单失败, Msg = "存单失败" },



            new CodeMsgModel(){ Code = (int)CodeEnum.Sign验证失败, Msg = "Sign验证失败" },
            new CodeMsgModel(){ Code = (int)CodeEnum.通信异常, Msg = "通信异常：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户平台不存在此单号, Msg = "商户平台不存在此单号" },

            new CodeMsgModel(){ Code = (int)CodeEnum.OpenId不能为空, Msg = "公众平台统一下单OpenId不能为空" },

            new CodeMsgModel(){ Code = (int)CodeEnum.提现金额不合法, Msg = "提现金额必须大于0" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包发放失败, Msg = "红包发放失败：{0}" },

            new CodeMsgModel(){ Code = (int)CodeEnum.商户名称格式错误, Msg = "商户名称长度必须小于32个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包祝福语格式错误, Msg = "红包备注长度必须小于128个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.活动名称格式错误, Msg = "活动名称长度必须小于32个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包备注格式错误, Msg = "红包备注长度必须小于256个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.OpenId不合法, Msg = "OpenId不合法" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包订单号格式错误, Msg = "红包订单号长度必须小于32个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包查询失败, Msg = "红包查询失败：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包存单失败, Msg = "红包存单失败" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包单号已存在, Msg = "红包单号已存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.红包单号不存在, Msg = "红包单号不存在" },

            new CodeMsgModel(){ Code = (int)CodeEnum.退款金额不合法, Msg = "退款金额不合法" },
            new CodeMsgModel(){ Code = (int)CodeEnum.退款失败, Msg = "退款失败：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单状态无法申请退款, Msg = "订单状态无法申请退款：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.退款单号不合法, Msg = "退款单号不合法" },
            new CodeMsgModel(){ Code = (int)CodeEnum.退款单号已存在, Msg = "退款单号已存在" },

            new CodeMsgModel(){ Code = (int)CodeEnum.退款查询失败, Msg = "退款查询失败：{0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.退款单号不存在, Msg = "退款单号不存在" },
        };

        private Singleton_CodeMsg() { }

        public static List<CodeMsgModel> GetInstance()
        {
            return instance;
        }
    }
}
