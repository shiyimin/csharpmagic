using System;

namespace traderobot
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var klines = Huobi.MarketHistoryKline("btcusdt", "1day", 20);
            Console.WriteLine($"返回长度：{klines.Length}, 第一个元素：{klines[0].vol}");
            var orderbook = Huobi.Depth("USDT", "BTC");
            Console.WriteLine($"【USDT/BTC】ask: {orderbook.tick.asks[0][0]}, bid: {orderbook.tick.bids[0][0]}");
            */
            
            HuobiWs.OnMessage += (o, a) => {
                Console.WriteLine(a.Message);
            };

            if (!HuobiWs.Init())
            {
                Console.WriteLine("无法打开web socket!s");
                return;
            }

            HuobiWs.Subscribe(string.Format(HuobiWs.MARKET_DEPTH, "btcusdt", "step0"), "1");
            Console.ReadLine();
        }
    }
}
