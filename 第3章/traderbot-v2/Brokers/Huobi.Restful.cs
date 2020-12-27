using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TraderBot.Core;

namespace TraderBot.Brokers
{
    public partial class Huobi
    {
        public class RestfulClient
        {
            private static string API_BASE = "https://api.huobi.pro";

            private class Response<T>
            {
                public string status { get; set; }

                public string ch { get; set; }

                public long ts { get; set; }

                public T data { get; set; }
            }

            public static MarketDepthResponse MarketDepth(string symbol, int depth = OrderBook.ORDER_CACHING_COUNT, string type = "step0")
            {
                var queries = new List<string>();
                queries.Add($"symbol={symbol}");
                queries.Add($"depth={depth}");
                queries.Add($"type={type}");

                var json = CallPublicApi("/market/depth", queries.ToArray());
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<MarketDepthResponse>>(json);
                    return response?.data;
                }
                else
                    return null;
            }
            
            public static MarketTickersResponse[] MarketTickers(string symbol, string period = "1day", int size = 150)
            {
                var queries = new List<string>();
                queries.Add($"symbol={symbol}");
                queries.Add($"period={period}");
                queries.Add($"size={size}");

                var json = CallPublicApi("/market/history/kline", queries.ToArray());
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<MarketTickersResponse[]>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static OpenOrdersResponse[] OpenOrders(string accountid, string symbol, string side, string apiKey, string apiSecret, int size = 10)
            {
                var command = $"/v1/order/openOrders";
                var req = new List<string>();
                req.Add($"account-id={accountid}");
                if (!string.IsNullOrEmpty(symbol)) req.Add($"symbol={symbol}");
                if (!string.IsNullOrEmpty(side)) req.Add($"side={side}");

                var json = GetAuthorizedRequest(command, apiKey, apiSecret, req.ToArray());
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<OpenOrdersResponse[]>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static OrderDetailResponse OrderDetails(string orderid, string apiKey, string apiSecret)
            {
                var command = $"/v1/order/orders/{orderid}";

                var json = GetAuthorizedRequest(command, apiKey, apiSecret);
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<OrderDetailResponse>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static AccountsResponse[] GetAllAccount(string apiKey, string apiSecret)
            {
                var json = GetAuthorizedRequest("/v1/account/accounts", apiKey, apiSecret);
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<AccountsResponse[]>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static AccountsBalanceResponse AccountsBalance(string account_id, string apiKey, string apiSecret)
            {
                var command = $"/v1/account/accounts/{account_id}/balance";
                var json = GetAuthorizedRequest(command, apiKey, apiSecret);
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<AccountsBalanceResponse>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static long? OrderPlace(string symbol, string type, string amount, string price, string accountid, string apiKey, string apiSecret, string source)
            {
                var req = new Dictionary<string, string>();
                req.Add("account-id", accountid);
                req.Add("symbol", symbol);
                req.Add("type", type);
                req.Add("amount", amount);
                req.Add("price", price);
                req.Add("source", source);

                var json = PostAuthorizedRequest("/v1/order/orders/place", req, apiKey, apiSecret);
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<long>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static string OrdersSubmitCancel(string order_id, string apiKey, string apiSecret)
            {
                var command = $"/v1/order/orders/{order_id}/submitcancel";
                var json = PostAuthorizedRequest(command, new Dictionary<string, string>(), apiKey, apiSecret);
                if (!string.IsNullOrEmpty(json))
                {
                    var response = FromJson<Response<string>>(json);
                    return response?.data;
                }
                else
                    return null;
            }

            public static MarketHistoryKlineResponse[] MarketHistoryKline(string symbol, string period, int size)
            {
                using (var client = new WebClient())
                {
                    var url = $"{API_BASE}/market/history/kline?" +
                            $"symbol={symbol}&period={period}&size={size}";
                    var response = FromJson<Response<MarketHistoryKlineResponse[]>>(client.DownloadString(url));
                    if (string.CompareOrdinal("ok", response.status) == 0)
                        return response.data;
                    else
                        return null;
                }
            }

            private static string CallPublicApi(string command, params string[] queries)
            {
                using (var client = new WebClient())
                {
                    var uri = string.Format("{0}{1}", API_BASE, command);

                    if (queries != null && queries.Length > 0)
                    {
                        var sb = new StringBuilder();
                        sb.Append(uri);

                        sb.Append(string.Format("?{0}", queries[0]));
                        for (int i = 1; i < queries.Length; ++i)
                        {
                            var query = queries[i];
                            sb.Append(string.Format("&{0}", query));
                        }
                        uri = sb.ToString();
                    }

                    return client.DownloadString(uri);
                }
            }

            private static string PostAuthorizedRequest(string resourcePath, Dictionary<string, string> paras, string apiKey, string apiSecret)
            {
                var parameters = UriEncodeParameterValue(GetCommonParameters(apiKey));//请求参数
                var sign = GetSignatureStr("POST", API_BASE.Replace("https://", string.Empty).ToLower(), resourcePath, parameters, apiSecret);//签名
                parameters += $"&Signature={sign}";
                var url = $"{API_BASE}{resourcePath}?{parameters}";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type","application/json");
                    var json = client.UploadString(url, ToJson(paras));
                    return json;
                }
            }

            /// <summary>
            /// 发起Http请求
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="resourcePath"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
            private static string GetAuthorizedRequest(string resourcePath, string apiKey, string apiSecret, string[] queries = null)
            {
                var sb = new StringBuilder();
                if (queries != null)
                {
                    foreach (var q in queries)
                        sb.Append($"&{q}");
                }
                var parameters = UriEncodeParameterValue(GetCommonParameters(apiKey) + (sb.Length > 0 ? sb.ToString() : ""));//请求参数
                var sign = GetSignatureStr("GET", API_BASE.Replace("https://", string.Empty).ToLower(), resourcePath, parameters, apiSecret);//签名
                parameters += $"&Signature={sign}";

                var url = $"{API_BASE}{resourcePath}?{parameters}";

                using (var client = new WebClient())
                {
                    var json = client.DownloadString(url);
                    return json;
                }
            }

            /// <summary>
            /// 加密方法
            /// </summary>
            private const string HUOBI_SIGNATURE_METHOD = "HmacSHA256";
            /// <summary>
            /// API版本
            /// </summary>
            private const int HUOBI_SIGNATURE_VERSION = 2;

            /// <summary>
            /// 获取通用签名参数
            /// </summary>
            /// <returns></returns>
            private static string GetCommonParameters(string apiKey)
            {
                return $"AccessKeyId={apiKey}&SignatureMethod={HUOBI_SIGNATURE_METHOD}&SignatureVersion={HUOBI_SIGNATURE_VERSION}&Timestamp={DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")}";
            }

            /// <summary>
            /// Uri参数值进行转义
            /// </summary>
            /// <param name="parameters">参数字符串</param>
            /// <returns></returns>
            private static string UriEncodeParameterValue(string parameters)
            {
                var sb = new StringBuilder();
                var paraArray = parameters.Split('&');
                var sortDic = new SortedDictionary<string, string>();
                foreach (var item in paraArray)
                {
                    var para = item.Split('=');
                    sortDic.Add(para[0], UrlEncode(para[1]));
                }
                foreach (var item in sortDic)
                {
                    sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
                }
                return sb.ToString().TrimEnd('&');
            }

            /// <summary>
            /// 转义字符串
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            private static string UrlEncode(string str)
            {
                StringBuilder builder = new StringBuilder();
                foreach (char c in str)
                {
                    if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
                    {
                        builder.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                return builder.ToString();
            }

            /// <summary>
            /// Hmacsha256加密
            /// </summary>
            /// <param name="text"></param>
            /// <param name="secretKey"></param>
            /// <returns></returns>
            private static string CalculateSignature256(string text, string secretKey)
            {
                using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                    return Convert.ToBase64String(hashmessage);
                }
            }
            /// <summary>
            /// 请求参数签名
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="host">API域名</param>
            /// <param name="resourcePath">资源地址</param>
            /// <param name="parameters">请求参数</param>
            /// <returns></returns>
            private static string GetSignatureStr(
                string method, string host, string resourcePath, string parameters, string apiSecret, bool urlencodeResult = true)
            {
                var sign = string.Empty;
                StringBuilder sb = new StringBuilder();
                sb.Append(method.ToUpper()).Append("\n")
                    .Append(host).Append("\n")
                    .Append(resourcePath).Append("\n");
                //参数排序
                var paraArray = parameters.Split('&');
                List<string> parametersList = new List<string>();
                foreach (var item in paraArray)
                {
                    parametersList.Add(item);
                }
                parametersList.Sort(delegate (string s1, string s2) { return string.CompareOrdinal(s1, s2); });
                foreach (var item in parametersList)
                {
                    sb.Append(item).Append("&");
                }
                sign = sb.ToString().TrimEnd('&');
                //计算签名，将以下两个参数传入加密哈希函数
                sign = CalculateSignature256(sign, apiSecret);
                return urlencodeResult ? UrlEncode(sign) : sign;
            }
        }
    }
}
