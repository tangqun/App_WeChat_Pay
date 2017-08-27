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
            new CodeMsgModel(){ Code = (int)CodeEnum.系统异常, Msg = "系统异常" },
            // 成功
            new CodeMsgModel(){ Code = (int)CodeEnum.成功, Msg = "成功" },

            //OpenID不能为空 = 500001,
            //OpenID格式错误 = 500002,
            //商户订单号不能为空 = 500003,
            //商户订单号格式错误 = 500004,
            //订单金额不合法 = 500005,
            //订单描述不能为空 = 500006,
            //订单描述格式错误 = 500007,
            //客户端IP不能为空 = 500008,
            //客户端IP格式错误 = 500009,
            //订单过期时间值不合法 = 500010,

            //商户订单号已存在 = 550001,
            //下单失败 = 550002,
            //商户订单号不存在 = 550003,
            //平台号不存在 = 550004,
            //Sign验证失败 = 550005,
            //通信异常 = 550006,
            //查单失败 = 550007,
            //微信服务器返回订单号不存在 = 550008,
            //微信服务器返回订单未支付 = 550009,
            //微信服务器返回订单转入退款 = 550010,
            //公众平台统一下单OpenID不能为空 = 550011,
            //公众平台统一下单OpenID格式错误 = 550012,

            new CodeMsgModel(){ Code = (int)CodeEnum.OpenID不能为空, Msg = "OpenID不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.OpenID格式错误, Msg = "OpenID格式错误" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号不能为空, Msg = "商户订单号不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号格式错误, Msg = "商户订单号长度必须为23个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单金额不合法, Msg = "订单金额不合法" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单描述不能为空, Msg = "订单描述不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单描述格式错误, Msg = "订单描述长度必须小于128个字符" },
            new CodeMsgModel(){ Code = (int)CodeEnum.客户端IP不能为空, Msg = "客户端IP不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.客户端IP格式错误, Msg = "客户端IP格式错误" },
            new CodeMsgModel(){ Code = (int)CodeEnum.订单过期时间值不合法, Msg = "订单过期时间值至少为5分钟" },


            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号已存在, Msg = "商户订单号已存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.下单失败, Msg = "下单失败, {0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.商户订单号不存在, Msg = "商户订单号不存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.平台号不存在, Msg = "平台号不存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.Sign验证失败, Msg = "Sign验证失败" },
            new CodeMsgModel(){ Code = (int)CodeEnum.通信异常, Msg = "通信异常, {0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.查单失败, Msg = "查单失败, {0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.微信服务器返回订单号不存在, Msg = "微信服务器返回订单号不存在" },
            new CodeMsgModel(){ Code = (int)CodeEnum.微信服务器返回订单未支付, Msg = "微信服务器返回订单未支付, {0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.微信服务器返回订单转入退款, Msg = "微信服务器返回订单转入退款, {0}" },
            new CodeMsgModel(){ Code = (int)CodeEnum.公众平台统一下单OpenID不能为空, Msg = "公众平台统一下单OpenID不能为空" },
            new CodeMsgModel(){ Code = (int)CodeEnum.公众平台统一下单OpenID格式错误, Msg = "公众平台统一下单OpenID格式错误" },





            //new CodeMsgModel(){ Code = (int)CodeEnum.提现金额不合法, Msg = "提现金额必须大于0" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包发放失败, Msg = "红包发放失败：{0}" },

            //new CodeMsgModel(){ Code = (int)CodeEnum.商户名称格式错误, Msg = "商户名称长度必须小于32个字符" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包祝福语格式错误, Msg = "红包备注长度必须小于128个字符" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.活动名称格式错误, Msg = "活动名称长度必须小于32个字符" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包备注格式错误, Msg = "红包备注长度必须小于256个字符" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.OpenID不合法, Msg = "OpenID不合法" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包订单号格式错误, Msg = "红包订单号长度必须小于32个字符" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包查询失败, Msg = "红包查询失败：{0}" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包存单失败, Msg = "红包存单失败" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包单号已存在, Msg = "红包单号已存在" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.红包单号不存在, Msg = "红包单号不存在" },

            //new CodeMsgModel(){ Code = (int)CodeEnum.退款金额不合法, Msg = "退款金额不合法" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.退款失败, Msg = "退款失败：{0}" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.订单状态无法申请退款, Msg = "订单状态无法申请退款：{0}" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.退款单号不合法, Msg = "退款单号不合法" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.退款单号已存在, Msg = "退款单号已存在" },

            //new CodeMsgModel(){ Code = (int)CodeEnum.退款查询失败, Msg = "退款查询失败：{0}" },
            //new CodeMsgModel(){ Code = (int)CodeEnum.退款单号不存在, Msg = "退款单号不存在" },
        };

        private Singleton_CodeMsg() { }

        public static List<CodeMsgModel> GetInstance()
        {
            return instance;
        }
    }
}
