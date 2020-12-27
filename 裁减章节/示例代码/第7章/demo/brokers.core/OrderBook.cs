using System;
using System.Diagnostics;

namespace TraderBot.Core
{
    /// <summary>
    /// 行情深度
    /// </summary>
    public class OrderBook
    {
        /// <summary>
        /// 默认只取前十个深度
        /// </summary>
        public const int ORDER_CACHING_COUNT = 10;

        public OrderBook() : this(ORDER_CACHING_COUNT) { }

        public OrderBook(int orderCount)
        {
            Bids = new decimal[orderCount, 2];
            Asks = new decimal[orderCount, 2];
        }

        public string Site { get; set; }

        /// <summary>
        /// 交易所自身计价的最高买价
        /// </summary>
        public decimal HighestBid { get; set; }

        /// <summary>
        /// 交易所自身计价的最高买单的交易量
        /// </summary>
        public decimal HighestBidVolume { get; set; }

        /// <summary>
        /// 交易所自身计价的最低卖价
        /// </summary>
        public decimal LowestAsk { get; set; }

        /// <summary>
        /// 交易所自身计价的最低卖单的交易量
        /// </summary>
        public decimal LowestAskVolume { get; set; }

        /// <summary>
        /// 报价的货币，例如用usd买btc，那么Quote就是usd，而btc是Coin
        /// </summary>
        public string Quote { get; set; }

        /// <summary>
        /// 购买的货币，例如用usd买btc，那么Quote就是usd，而btc是Coin
        /// </summary>
        public string Coin { get; set; }

        /// <summary>
        /// 获取订单的时间
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 第一个元素是btc价格，第二个元素是交易所计价价格，第三个元素是交易量
        /// </summary>
        public decimal[,] Bids { get; private set; }

        /// <summary>
        /// 第一个元素是btc价格，第二个元素是交易所计价价格，第三个元素是交易量
        /// </summary>
        public decimal[,] Asks { get; private set; }

        /// <summary>
        /// 交易所市场里的计价，比如是USDT-DASH市场，那就是USDT的价格，如果是BTC-DASH市场，就是BTC价格
        /// </summary>
        public const int QUOTE_PRICE_INDEX = 0;

        /// <summary>
        /// 交易量的索引
        /// </summary>
        public const int VOLUME_INDEX = 1;

        [Conditional("DEBUG")]
        public void SetBids(decimal[,] value) { Bids = value; }

        [Conditional("DEBUG")]
        public void SetAsks(decimal[,] value) { Asks = value; }
    }
}
