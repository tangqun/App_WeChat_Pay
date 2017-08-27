using DAL_9H;
using Helper_9H;
using IBLL_9H;
using IDAL_9H;
using Model_9H;
using Model_9H.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BLL_9H
{
    public class PayBLL : IPayBLL
    {
        private IPayDAL payDAL = new PayDAL();
        private IWXConfigDAL wxConfigDAL = new WXConfigDAL();
        private ICodeMsgDAL codeMsgDAL = new CodeMsgDAL();

        public RESTfulModel UnifiedOrder(string authorizerAppID, string openID, string outTradeNo, int totalFee, string body)
        {
            try
            {
                #region 验证参数
                RESTfulModel cv_AuthorizerAppID = ClientValidateAuthorizerAppID(authorizerAppID);
                if (cv_AuthorizerAppID.Code != 0)
                {
                    return cv_AuthorizerAppID;
                }

                RESTfulModel cv_OpenID = ClientValidateOpenID(openID);
                if (cv_OpenID.Code != 0)
                {
                    return cv_OpenID;
                }

                RESTfulModel cv_OutTradeNo = ClientValidateOutTradeNo(outTradeNo);
                if (cv_OutTradeNo.Code != 0)
                {
                    return cv_OutTradeNo;
                }

                RESTfulModel cv_TotalFee = ClientValidateTotalFee(totalFee);
                if (cv_TotalFee.Code != 0)
                {
                    return cv_TotalFee;
                }

                RESTfulModel cv_Body = ClientValidateBody(body);
                if (cv_Body.Code != 0)
                {
                    return cv_Body;
                }
                #endregion

                #region 验证商户订单是否存在
                PayModel payModel = payDAL.GetByOutTradeNo(outTradeNo);
                if (payModel != null)
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.商户订单号已存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.商户订单号已存在) };
                }
                #endregion

                DateTime dt = DateTime.Now;
                #region 操作
                if (payDAL.Insert(authorizerAppID, openID, outTradeNo, totalFee, body, dt))
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
                }
                else
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.下单失败, Msg = codeMsgDAL.GetByCode((int)CodeEnum.下单失败) };
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new RESTfulModel() { Code = (int)CodeEnum.系统异常, Msg = codeMsgDAL.GetByCode((int)CodeEnum.系统异常) };
            }
        }

        public RESTfulModel WXUnifiedOrder(string outTradeNo, string ip, int period, string tradeType, string openid)
        {
            try
            {
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

                #region 验证参数
                RESTfulModel cv_OutTradeNo = ClientValidateOutTradeNo(outTradeNo);
                if (cv_OutTradeNo.Code != 0)
                {
                    return cv_OutTradeNo;
                }

                RESTfulModel cv_Period = ClientValidatePeriod(period);
                if (cv_Period.Code != 0)
                {
                    return cv_Period;
                }
                #endregion

                #region 验证商户订单是否存在
                PayModel payModel = payDAL.GetByOutTradeNo(outTradeNo);
                if (payModel == null)
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.商户订单号不存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.商户订单号不存在) };
                }
                #endregion

                #region 获取微信签约信息
                WXConfigModel wxConfigModel = wxConfigDAL.GetByAuthorizerAppID(payModel.AuthorizerAppID);
                if (wxConfigModel == null)
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.平台号不存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.平台号不存在) };
                }
                #endregion

                DateTime dt = DateTime.Now;
                SortedDictionary<string, object> sortedDictionary_to = new SortedDictionary<string, object>();

                #region sortedDictionary装填参数
                sortedDictionary_to["appid"] = wxConfigModel.AuthorizerAppID;
                sortedDictionary_to["mch_id"] = wxConfigModel.MCHID;
                sortedDictionary_to["nonce_str"] = NonceStrHelper.GenerateNonceStr();
                sortedDictionary_to["body"] = payModel.Body;
                sortedDictionary_to["out_trade_no"] = outTradeNo;
                sortedDictionary_to["total_fee"] = payModel.TotalFee;
                sortedDictionary_to["spbill_create_ip"] = ip;
                sortedDictionary_to["time_start"] = dt.ToString("yyyyMMddHHmmss");
                sortedDictionary_to["time_expire"] = dt.AddMinutes(period).ToString("yyyyMMddHHmmss");
                sortedDictionary_to["notify_url"] = wxConfigModel.NOTIFY_URL;
                sortedDictionary_to["trade_type"] = tradeType;
                if (tradeType == "JSAPI")
                {
                    RESTfulModel cv_OpenID = ClientValidateOpenID(openid);
                    if (cv_OpenID.Code != 0)
                    {
                        sortedDictionary_to["openid"] = openid;
                    }
                    else
                    {
                        return new RESTfulModel() { Code = (int)CodeEnum.OpenID不能为空, Msg = codeMsgDAL.GetByCode((int)CodeEnum.OpenID不能为空) };
                    }
                }
                #endregion

                #region &参数，得到sign
                string para_to = "";
                if (sortedDictionary_to.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in sortedDictionary_to)
                    {
                        if (pair.Key != "sign" && pair.Value.ToString() != "")
                        {
                            para_to += pair.Key + "=" + pair.Value + "&";
                        }
                    }
                }
                para_to += "key=" + wxConfigModel.KEY;

                var bs_to = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_to));
                var sb_to = new StringBuilder();
                foreach (byte b in bs_to)
                {
                    sb_to.Append(b.ToString("x2"));
                }
                sortedDictionary_to["sign"] = sb_to.ToString().ToUpper();
                #endregion

                #region 序列化xml
                string xml_to = "<xml>";
                if (sortedDictionary_to.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in sortedDictionary_to)
                    {
                        if (pair.Value.GetType() == typeof(int))
                        {
                            xml_to += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                        }
                        else if (pair.Value.GetType() == typeof(string))
                        {
                            xml_to += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                        }
                    }
                }
                xml_to += "</xml>";
                #endregion

                LogHelper.Info_wx("unifiedorder request: ", xml_to);

                #region 发请求
                string xml_from = string.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });
                }
                request.Method = "POST";
                request.ContentType = "text/xml";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml_to);
                Stream reqest_stream = request.GetRequestStream();
                reqest_stream.Write(bytes, 0, bytes.Length);
                reqest_stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                xml_from = streamReader.ReadToEnd().Trim();
                #endregion

                LogHelper.Info_wx("unifiedorder response: ", xml_from);

                SortedDictionary<string, object> sortedDictionary_from = new SortedDictionary<string, object>();

                #region 解析xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml_from);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    sortedDictionary_from[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
                }
                #endregion

                #region 操作
                if ("SUCCESS" == sortedDictionary_from["return_code"].ToString())
                {
                    #region &参数，得到sign
                    string para_from = "";
                    if (sortedDictionary_from.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> pair in sortedDictionary_from)
                        {
                            if (pair.Key != "sign" && pair.Value.ToString() != "")
                            {
                                para_from += pair.Key + "=" + pair.Value + "&";
                            }
                        }
                    }
                    para_from += "key=" + wxConfigModel.KEY;

                    var bs_from = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_from));
                    var sb_from = new StringBuilder();
                    foreach (byte b in bs_from)
                    {
                        sb_from.Append(b.ToString("x2"));
                    }
                    string sign_from = sb_from.ToString().ToUpper();
                    #endregion

                    if (sign_from == sortedDictionary_from["sign"].ToString())
                    {
                        if ("SUCCESS" == sortedDictionary_from["result_code"].ToString())
                        {
                            #region sortedDictionary装填参数
                            SortedDictionary<string, object> sortedDictionary_res = new SortedDictionary<string, object>();
                            sortedDictionary_res["appid"] = sortedDictionary_from["appid"];
                            sortedDictionary_res["partnerid"] = sortedDictionary_from["mch_id"];
                            sortedDictionary_res["prepayid"] = sortedDictionary_from["prepay_id"];
                            sortedDictionary_res["package"] = "Sign=WXPay";
                            sortedDictionary_res["noncestr"] = NonceStrHelper.GenerateNonceStr();
                            sortedDictionary_res["timestamp"] = TimestampHelper.GenerateTimeStamp();
                            #endregion

                            #region &参数，得到sign
                            string para_res = "";
                            if (sortedDictionary_res.Count > 0)
                            {
                                foreach (KeyValuePair<string, object> pair in sortedDictionary_res)
                                {
                                    if (pair.Key != "sign" && pair.Value.ToString() != "")
                                    {
                                        para_res += pair.Key + "=" + pair.Value + "&";
                                    }
                                }
                            }
                            para_res += "key=" + wxConfigModel.KEY;

                            var bs_res = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_res));
                            var sb_res = new StringBuilder();
                            foreach (byte b in bs_res)
                            {
                                sb_res.Append(b.ToString("x2"));
                            }
                            sortedDictionary_res["sign"] = sb_res.ToString().ToUpper();
                            #endregion
                            
                            return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功), Data = sortedDictionary_res };
                        }
                        return new RESTfulModel() { Code = (int)CodeEnum.下单失败, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.下单失败), "err_code: " + sortedDictionary_from["err_code"].ToString() + ", err_code_des: " + sortedDictionary_from["err_code_des"].ToString()) };
                    }
                    return new RESTfulModel() { Code = (int)CodeEnum.Sign验证失败, Msg = codeMsgDAL.GetByCode((int)CodeEnum.Sign验证失败) };
                }
                return new RESTfulModel() { Code = (int)CodeEnum.通信异常, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.通信异常), "return_msg: " + sortedDictionary_from["return_msg"].ToString()) };
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new RESTfulModel() { Code = (int)CodeEnum.系统异常, Msg = codeMsgDAL.GetByCode((int)CodeEnum.系统异常) };
            }
        }

        public string WXReceiveNotify(string authorizerAppID, string xml_from)
        {
            try
            {
                LogHelper.Info_wx("receive data from wechat: ", xml_from);

                SortedDictionary<string, object> sortedDictionary_from = new SortedDictionary<string, object>();

                #region 解析xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml_from);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    sortedDictionary_from[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
                }
                #endregion

                #region SUCCESS/FAIL(return_code)
                if ("SUCCESS" == sortedDictionary_from["return_code"].ToString())
                {
                    #region &参数
                    string para_from = "";
                    if (sortedDictionary_from.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> pair in sortedDictionary_from)
                        {
                            if (pair.Key != "sign" && pair.Value.ToString() != "")
                            {
                                para_from += pair.Key + "=" + pair.Value + "&";
                            }
                        }
                    }
                    #endregion

                    #region 获取支付宝签约信息
                    WXConfigModel wxConfigModel = wxConfigDAL.GetByAuthorizerAppID(authorizerAppID);
                    if (wxConfigModel == null)
                    {
                        return "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[回调地址被串改]]></return_msg></xml>";
                    }
                    #endregion

                    #region 得到sign
                    para_from += "key=" + wxConfigModel.KEY;

                    var bs_from = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_from));
                    var sb_from = new StringBuilder();
                    foreach (byte b in bs_from)
                    {
                        sb_from.Append(b.ToString("x2"));
                    }
                    string sign_from = sb_from.ToString().ToUpper();
                    #endregion

                    DateTime payTime = DateTime.Now;
                    // 通知时间转化
                    DateTime.TryParseExact(sortedDictionary_from["time_end"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out payTime);

                    #region 操作
                    if (sign_from == sortedDictionary_from["sign"].ToString())
                    {
                        #region SUCCESS/FAIL(result_code)
                        if ("SUCCESS" == sortedDictionary_from["result_code"].ToString())
                        {
                            payDAL.Update(sortedDictionary_from["out_trade_no"].ToString(), (int)PayTypeEnum.微信, sortedDictionary_from["transaction_id"].ToString(), sortedDictionary_from["cash_fee"].ToInt(), sortedDictionary_from["result_code"].ToString(), (int)PayStateEnum.支付成功, payTime);

                            return "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";
                        }
                        else
                        {
                            payDAL.Update(sortedDictionary_from["out_trade_no"].ToString(), (int)PayTypeEnum.微信, sortedDictionary_from["transaction_id"].ToString(), sortedDictionary_from["cash_fee"].ToInt(), sortedDictionary_from["result_code"].ToString(), (int)PayStateEnum.支付失败, payTime);

                            return "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[result_code: " + sortedDictionary_from["out_trade_no"].ToString() + "]]></return_msg></xml>";
                        }
                        #endregion
                    }
                    else
                    {
                        LogHelper.Info_wx("Sign校验失败, out_trade_no: ", sortedDictionary_from["out_trade_no"].ToString());
                        return "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[Sign校验失败]]></return_msg></xml>";
                    }
                    #endregion
                }
                else
                {
                    // 通信失败，记录失败原因，以异常处理，不会进行回调
                    LogHelper.Info_wx("通信失败, return_msg: ", sortedDictionary_from["return_msg"].ToString());
                    return "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[return_msg: " + sortedDictionary_from["return_msg"].ToString() + "]]></return_msg></xml>";
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[服务器异常]]></return_msg></xml>";
            }
        }

        public RESTfulModel WXOrderQuery(string outTradeNo)
        {
            try
            {
                string url = "https://api.mch.weixin.qq.com/pay/orderquery";

                #region 验证参数
                RESTfulModel cv_OutTradeNo = ClientValidateOutTradeNo(outTradeNo);
                if (cv_OutTradeNo.Code != 0)
                {
                    return cv_OutTradeNo;
                }
                #endregion

                #region 验证商户订单是否存在
                PayModel payModel = payDAL.GetByOutTradeNo(outTradeNo);
                if (payModel == null)
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.商户订单号不存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.商户订单号不存在) };
                }
                #endregion

                #region 获取微信签约信息
                WXConfigModel wxConfigModel = wxConfigDAL.GetByAuthorizerAppID(payModel.AuthorizerAppID);
                if (wxConfigModel == null)
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.平台号不存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.平台号不存在) };
                }
                #endregion

                #region 优先判断本地支付状态
                //if (payModel.PayState != (int)PayStateEnum.未支付)
                //{
                //    Dictionary<string, object> dic = new Dictionary<string, object>();
                //    dic.Add("code", (int)CodeEnum.成功);
                //    dic.Add("msg", codeMsgDAL.GetByCode((int)CodeEnum.成功));
                //    dic.Add("outtradeno", payModel.OutTradeNo);
                //    dic.Add("totalfee", payModel.TotalFee);
                //    dic.Add("tradeno", payModel.TradeNo);
                //    dic.Add("realfee", payModel.RealFee);
                //    dic.Add("paytime", payModel.PayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                //    dic.Add("tradestate", payModel.TradeState);

                //    JsonResult jsonResult = Json(dic;

                //    LogHelper.Info_wx("Send Data To Client（本地）: " + new JavaScriptSerializer().Serialize(jsonResult.Data));

                //    return jsonResult;
                //}
                #endregion

                SortedDictionary<string, object> sortedDictionary_to = new SortedDictionary<string, object>();

                #region sortedDictionary_to装填参数
                sortedDictionary_to["appid"] = wxConfigModel.AuthorizerAppID;
                sortedDictionary_to["mch_id"] = wxConfigModel.MCHID;
                sortedDictionary_to["out_trade_no"] = outTradeNo;
                sortedDictionary_to["nonce_str"] = NonceStrHelper.GenerateNonceStr();
                #endregion

                #region &参数，得到sign
                string para_to = "";
                if (sortedDictionary_to.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in sortedDictionary_to)
                    {
                        if (pair.Key != "sign" && pair.Value.ToString() != "")
                        {
                            para_to += pair.Key + "=" + pair.Value + "&";
                        }
                    }
                }
                para_to += "key=" + wxConfigModel.KEY;

                var bs_to = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_to));
                var sb_to = new StringBuilder();
                foreach (byte b in bs_to)
                {
                    sb_to.Append(b.ToString("x2"));
                }
                sortedDictionary_to["sign"] = sb_to.ToString().ToUpper();
                #endregion

                #region 序列化xml
                string xml_to = "<xml>";
                if (sortedDictionary_to.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in sortedDictionary_to)
                    {
                        if (pair.Value.GetType() == typeof(int))
                        {
                            xml_to += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                        }
                        else if (pair.Value.GetType() == typeof(string))
                        {
                            xml_to += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                        }
                    }
                }
                xml_to += "</xml>";
                #endregion

                LogHelper.Info_wx("orderquery request: ", xml_to);

                #region 发请求
                string xml_from = string.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });
                }
                request.Method = "POST";
                request.ContentType = "text/xml";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml_to);
                Stream reqest_stream = request.GetRequestStream();
                reqest_stream.Write(bytes, 0, bytes.Length);
                reqest_stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                xml_from = streamReader.ReadToEnd().Trim();
                #endregion

                LogHelper.Info_wx("orderquery response: ", xml_from);

                SortedDictionary<string, object> sortedDictionary_from = new SortedDictionary<string, object>();

                #region 解析xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml_from);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    sortedDictionary_from[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
                }
                #endregion

                #region SUCCESS/FAIL(return_code)
                if ("SUCCESS" == sortedDictionary_from["return_code"].ToString())
                {
                    #region &参数，得到sign
                    string para_from = "";
                    if (sortedDictionary_from.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> pair in sortedDictionary_from)
                        {
                            if (pair.Key != "sign" && pair.Value.ToString() != "")
                            {
                                para_from += pair.Key + "=" + pair.Value + "&";
                            }
                        }
                    }
                    para_from += "key=" + wxConfigModel.KEY;

                    var bs_from = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(para_from));
                    var sb_from = new StringBuilder();
                    foreach (byte b in bs_from)
                    {
                        sb_from.Append(b.ToString("x2"));
                    }
                    string sign_from = sb_from.ToString().ToUpper();
                    #endregion

                    #region 校验sign
                    if (sign_from == sortedDictionary_from["sign"].ToString())
                    {
                        #region SUCCESS/FAIL(result_code)
                        if ("SUCCESS" == sortedDictionary_from["result_code"].ToString())
                        {
                            if ("SUCCESS" == sortedDictionary_from["trade_state"].ToString())
                            {
                                #region result_code == SUCCESS
                                DateTime payTime;
                                DateTime.TryParseExact(sortedDictionary_from["time_end"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out payTime);

                                //payBLL.Update(sortedDictionary_from["out_trade_no"].ToString(), (int)PayTypeEnum.微信, sortedDictionary_from["transaction_id"].ToString(), sortedDictionary_from["cash_fee"].ToInt(), payTime, (int)PayStateEnum.支付成功, sortedDictionary_from["result_code"].ToString());

                                Dictionary<string, object> sortedDictionary_res = new Dictionary<string, object>();
                                sortedDictionary_res.Add("outtradeno", sortedDictionary_from["out_trade_no"].ToString());
                                sortedDictionary_res.Add("totalfee", sortedDictionary_from["total_fee"].ToInt());
                                sortedDictionary_res.Add("tradeno", sortedDictionary_from["transaction_id"].ToString());
                                sortedDictionary_res.Add("realfee", sortedDictionary_from["cash_fee"].ToInt());
                                sortedDictionary_res.Add("paytime", payTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                sortedDictionary_res.Add("tradestate", sortedDictionary_from["trade_state"].ToString());

                                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功), Data = sortedDictionary_res };
                                #endregion
                            }
                            else if ("NOTPAY" == sortedDictionary_from["trade_state"].ToString() || "USERPAYING" == sortedDictionary_from["trade_state"].ToString())
                            {
                                return new RESTfulModel() { Code = (int)CodeEnum.微信服务器返回订单未支付, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.微信服务器返回订单未支付), "trade_state : " + sortedDictionary_from["trade_state"].ToString() + ", trade_state_desc : " + sortedDictionary_from["trade_state_desc"].ToString()) };
                            }
                            else if ("REFUND" == sortedDictionary_from["trade_state"].ToString())
                            {
                                return new RESTfulModel() { Code = (int)CodeEnum.微信服务器返回订单转入退款, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.微信服务器返回订单转入退款), sortedDictionary_from["trade_state"].ToString()) };
                            }
                            else
                            {
                                return new RESTfulModel() { Code = (int)CodeEnum.查单失败, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.查单失败), sortedDictionary_from["trade_state"].ToString() + ", trade_state_desc : " + sortedDictionary_from["trade_state_desc"].ToString()) };
                            }
                        }
                        else
                        {
                            // result_code == FAIL
                            return new RESTfulModel() { Code = (int)CodeEnum.查单失败, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.查单失败), sortedDictionary_from["err_code_des"].ToString()) };
                        }
                        #endregion
                    }
                    #endregion

                    return new RESTfulModel() { Code = (int)CodeEnum.Sign验证失败, Msg = codeMsgDAL.GetByCode((int)CodeEnum.Sign验证失败) };
                }
                else if ("ORDERNOTEXIST" == sortedDictionary_from["return_code"].ToString())
                {
                    return new RESTfulModel() { Code = (int)CodeEnum.微信服务器返回订单号不存在, Msg = codeMsgDAL.GetByCode((int)CodeEnum.微信服务器返回订单号不存在) };
                }
                #endregion

                return new RESTfulModel() { Code = (int)CodeEnum.通信异常, Msg = string.Format(codeMsgDAL.GetByCode((int)CodeEnum.通信异常), "return_msg: " + sortedDictionary_from["return_msg"].ToString()) };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new RESTfulModel() { Code = (int)CodeEnum.系统异常, Msg = codeMsgDAL.GetByCode((int)CodeEnum.系统异常) };
            }
        }

        private RESTfulModel ClientValidateAuthorizerAppID(string authorizerAppID)
        {
            return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
        }

        private RESTfulModel ClientValidateOpenID(string openID)
        {
            if (ValidateHelper.IsNullOrEmpty(openID))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.OpenID不能为空, Msg = codeMsgDAL.GetByCode((int)CodeEnum.OpenID不能为空) };
            }

            if (ValidateHelper.OpenID(openID))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.OpenID格式错误, Msg = codeMsgDAL.GetByCode((int)CodeEnum.OpenID格式错误) };
            }
        }

        private RESTfulModel ClientValidateOutTradeNo(string outTradeNo)
        {
            if (ValidateHelper.IsNullOrEmpty(outTradeNo))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.商户订单号不能为空, Msg = codeMsgDAL.GetByCode((int)CodeEnum.商户订单号不能为空) };
            }

            if (outTradeNo.Length == 23)
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.商户订单号格式错误, Msg = codeMsgDAL.GetByCode((int)CodeEnum.商户订单号格式错误) };
            }
        }

        private RESTfulModel ClientValidateTotalFee(int totalFee)
        {
            if (totalFee > 0)
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.订单金额不合法, Msg = codeMsgDAL.GetByCode((int)CodeEnum.订单金额不合法) };
            }
        }

        private RESTfulModel ClientValidateBody(string body)
        {
            if (ValidateHelper.IsNullOrEmpty(body))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.订单描述不能为空, Msg = codeMsgDAL.GetByCode((int)CodeEnum.订单描述不能为空) };
            }

            if (ValidateHelper.StringLength(body, 1, 128))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.订单描述格式错误, Msg = codeMsgDAL.GetByCode((int)CodeEnum.订单描述格式错误) };
            }
        }

        private RESTfulModel ClientValidateIP(string ip)
        {
            if (ValidateHelper.IsNullOrEmpty(ip))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.客户端IP不能为空, Msg = codeMsgDAL.GetByCode((int)CodeEnum.客户端IP不能为空) };
            }

            if (ValidateHelper.IP(ip))
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.客户端IP格式错误, Msg = codeMsgDAL.GetByCode((int)CodeEnum.客户端IP格式错误) };
            }
        }

        private RESTfulModel ClientValidatePeriod(int period)
        {
            if (period > 5)
            {
                return new RESTfulModel() { Code = (int)CodeEnum.成功, Msg = codeMsgDAL.GetByCode((int)CodeEnum.成功) };
            }
            else
            {
                return new RESTfulModel() { Code = (int)CodeEnum.订单过期时间值不合法, Msg = codeMsgDAL.GetByCode((int)CodeEnum.订单过期时间值不合法) };
            }
        }
    }
}
