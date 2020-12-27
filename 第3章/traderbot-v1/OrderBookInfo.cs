using System;

public class OrderBookInfo
{
    public const int ORDER_CACHING_COUNT = 10;

    public OrderBookInfo() : this(ORDER_CACHING_COUNT) { }

    public OrderBookInfo(int orderCount)
    {
        Bids = new decimal[orderCount, 3];
        Asks = new decimal[orderCount, 3];
    }
    
    /// <summary>
    /// 交易所站点信息
    /// </summary>
    public string Site { get; set; }

    /// <summary>
    /// 交易所自身计价的最高买价
    /// </summary>
    public decimal HighestBid { get; set; }

    public decimal HighestBidVolume { get; set; }

    /// <summary>
    /// 交易所自身计价的最低卖价
    /// </summary>
    public decimal LowestAsk { get; set; }

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
    public const int QUOTE_PRICE_INDEX = 1;

    /// <summary>
    /// 交易量的索引
    /// </summary>
    public const int VOLUME_INDEX = 2;
}