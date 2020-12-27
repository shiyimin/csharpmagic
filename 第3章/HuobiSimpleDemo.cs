// 源码位置：第四章\HuobiSimpleDemo.cs
// 编译命令：csc /debug HuobiSimpleDemo.cs
using System;
using System.Net;

public class Huobi
{
    private const string API_BASE = "https://api.huobi.pro";
    
    public static string MarketHistoryKline(string symbol, string period, int size)
    {
        using (var client = new WebClient())
        {
            var url = $"{API_BASE}/market/history/kline?" + 
                      $"symbol={symbol}&period={period}&size={size}";
            return client.DownloadString(url);
        }
    }

    static void Main(string[] args)
    {
        var json = Huobi.MarketHistoryKline("btcusdt", "1day", 20);
        Console.WriteLine(json);
    }
}