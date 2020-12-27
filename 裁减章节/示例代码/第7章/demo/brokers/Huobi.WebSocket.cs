using System;
using System.Collections.Generic;
using System.Security.Authentication;
using TraderBot.Brokers.Internal;
using WebSocket4Net;

namespace TraderBot.Brokers
{
    public partial class Huobi
    {
        private static bool s_WebSocketInitialized = false;

        private class WebSocketClient
        {
            private static WebSocket s_websocket;
            private static Dictionary<string, SubscribeInfo> s_topicDic = new Dictionary<string, SubscribeInfo>();
            public static bool s_isOpened { get; private set; }
            private const string HUOBI_WEBSOCKET_API = "wss://api.huobi.pro/ws";
            private static Dictionary<string, Queue<MarketHistoryKlineResponse>> s_klines =
                new Dictionary<string, Queue<MarketHistoryKlineResponse>>();
            private static Dictionary<string, MarketDepthResponse> s_latestDepth = new Dictionary<string, MarketDepthResponse>();
            private static int s_subscribeTopicId = 1;

            #region 市场信息常量
            public const string MARKET_KLINE = "market.{0}.kline.{1}";
            public const string MARKET_DEPTH = "market.{0}.depth.{1}";
            #endregion

            public class SubscribeInfo
            {
                public int Id { get; set; }

                public string Topic { get; set; }

                public Func<string, string, bool> Callback { get; set; }

                public string SubscribeJson { get; set; }
            }

            public class SubscribeResponse
            {
                public string id { get; set; }

                public string status { get; set; }

                public string subbed { get; set; }

                public long ts { get; set; }
            }

            public class MarketHistoryKlineWsResponse
            {
                public string id { get; set; }

                public string status { get; set; }

                public string subbed { get; set; }

                public string ch { get; set; }

                public long ts { get; set; }

                public MarketHistoryKlineResponse tick { get; set; }
            }

            public static bool Init()
            {
                s_websocket = new WebSocket(HUOBI_WEBSOCKET_API);
                s_websocket.Security.EnabledSslProtocols = SslProtocols.Tls12;
                s_websocket.Opened += OnOpened;
                s_websocket.DataReceived += ReceivedMsg;
                s_websocket.Error += OnError;
                s_websocket.Open();
                return true;
            }

            public static MarketHistoryKlineResponse[] MarketHistoryKline(string symbol, string period, int size)
            {
                if (!s_klines.ContainsKey(symbol))
                {
                    SubscribeKline(symbol, period, size);
                    return null;
                }
                else
                {
                    var kline = s_klines[symbol];
                    return (kline.Count > 0) ? kline.ToArray() : null;
                }
            }

            public static MarketDepthResponse MarketDepth(string symbol, string type)
            {
                var topic = string.Format(MARKET_DEPTH, symbol, type);
                if (!s_latestDepth.ContainsKey(topic))
                {
                    SubscribeOrderBook(symbol, type);
                    return null;
                }
                else
                {
                    return s_latestDepth[topic];
                }
            }

            public static string SubscribeKline(string symbol, string period, int size)
            {
                var topic = string.Format(MARKET_KLINE, symbol, period);
                s_klines.Add(symbol, new Queue<MarketHistoryKlineResponse>(size));
                Subscribe(topic, KlineMessageHandler);
                return topic;
            }

            public static string SubscribeOrderBook(string symbol, string type)
            {
                var topic = string.Format(MARKET_DEPTH, symbol, type);

                Subscribe(topic, OrderBookMessageHandler);
                s_latestDepth[topic] = new MarketDepthResponse();
                return topic;
            }

            public static void UnSubscribe(string topic, string id)
            {
                if (!s_topicDic.ContainsKey(topic) || !s_isOpened)
                    return;
                var msg = $"{{\"unsub\":\"{topic}\",\"id\":\"{id}\"}}";
                s_topicDic.Remove(topic);
                SendSubscribeTopic(msg);
            }

            private static bool KlineMessageHandler(string topic, string json)
            {
                if (string.IsNullOrEmpty(json)) return false;
                foreach (var item in s_klines)
                {
                    if (json.IndexOf(item.Key) >= 0)
                    {
                        var response = FromJson<MarketHistoryKlineWsResponse>(json);
                        if (response == null) break;
                        if (response.tick == null) break;

                        item.Value.TrimExcess();
                        item.Value.Enqueue(response.tick);
                    }
                }

                return true;
            }

            private static bool OrderBookMessageHandler(string topic, string json)
            {
                if (string.IsNullOrEmpty(json)) return false;
                var idx = json.IndexOf(topic, StringComparison.Ordinal);
                if (idx < 0) return false;
                idx = json.LastIndexOf("\"ch\":", idx, StringComparison.Ordinal);
                if (idx < 0) return false;

                var depth = FromJson<MarketDepthResponse>(json);
                if (depth == null) return false;

                s_latestDepth[topic] = depth;
                return true;
            }

            private static void Subscribe(string topic, Func<string, string, bool> callback)
            {
                if (s_topicDic.ContainsKey(topic))
                    return;

                s_subscribeTopicId++;
                var info = new SubscribeInfo()
                {
                    Topic = topic,
                    Callback = callback,
                    Id = s_subscribeTopicId
                };

                var msg = ToJson(new { sub = topic, id = s_subscribeTopicId });
                s_topicDic.Add(topic, info);
                if (s_isOpened)
                    SendSubscribeTopic(msg);
            }

            private static void OnError(object sender, EventArgs e)
            {
                Console.WriteLine("Error:" + e.ToString());
            }

            private static void OnOpened(object sender, EventArgs e)
            {
                s_isOpened = true;
                foreach (var item in s_topicDic)
                    SendSubscribeTopic(item.Value.SubscribeJson);
            }

            private static void DefaultMessageHandler(string message)
            {
                if (string.IsNullOrEmpty(message)) return;

                foreach (var handler in s_topicDic)
                {
                    if (handler.Value.Callback(handler.Key, message))
                        break;
                }
            }

            private static void ReceivedMsg(object sender, DataReceivedEventArgs args)
            {
                var msg = GZipHelper.GZipDecompressString(args.Data);
                if (msg.IndexOf("ping") != -1) //响应心跳包
                {
                    var reponseData = msg.Replace("ping", "pong");
                    s_websocket.Send(reponseData);
                }
                else//接收消息
                {
                    DefaultMessageHandler(msg);
                }
            }

            private static void SendSubscribeTopic(string msg)
            {
                s_websocket.Send(msg);
            }
        }
    }
}
