using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class Huobi
{
    private static string API_BASE = "https://api.huobi.pro";

    private class Response<T>
    {
        public string status { get; set; }

        public string ch { get; set; }

        public long ts { get; set; }

        public T data { get; set; }
    }

    public class MarketHistoryKlineResponse
    {
        public long id { get; set; }

        public decimal amount { get; set; }

        public int count { get; set; }

        public decimal open { get; set; }

        public decimal close { get; set; }

        public decimal low { get; set; }

        public decimal high { get; set; }

        public decimal vol { get; set;}
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

    public static T FromJson<T>(string json)
    {
        if (!string.IsNullOrEmpty(json))
        {
            var item = JsonConvert.DeserializeObject<T>(json);
            return item;
        }
        else
        {
            return default(T);
        }
    }

    public class DepthResponse
    {
        public string status { get; set; }

        public string ch { get; set; }

        public long ts { get; set; }

        public long version { get; set; }

        public TickClass tick { get; set; }

        public class TickClass
        {
            public decimal[][] asks { get; set; }

            public decimal[][] bids { get; set; }
        }
    }

    public static DepthResponse Depth(string quote, string coin, int limit = OrderBookInfo.ORDER_CACHING_COUNT)
    {
        var symbol = BuildSymbol(quote, coin);
        var queries = new List<string>();
        queries.Add(string.Format("symbol={0}", symbol));
        queries.Add(string.Format("depth={0}", limit));
        queries.Add("type=step0");

        var json = CallPublicApi("/market/depth", queries.ToArray());
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                var ret = FromJson<DepthResponse>(json);
                return ret;
            }
            catch (Exception)
            {
            }
        }
        return null;
    }

    private static string BuildSymbol(string quote, string coin)
    {
        return string.Format($"{coin}{quote}").ToLower();
    }

    public static string ToJson<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    
    private static string CallPublicApi(string command, params string[] queries)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

            using (HttpResponseMessage response = client.GetAsync(uri).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}