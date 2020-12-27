using System;

public class DbPlacedOrder
{
    public DbPlacedOrder() {}
    /*
    public DbPlacedOrder(string coin, string broker, string bidOrderId, string bidQuote)
    {
        Coin = coin;
        BrokerSite = broker;
        BidOrderId = bidOrderId;
        BidQuote = bidQuote;
    }
    */
    public DbPlacedOrder(string coin, string brokerSite, string bidOrderId, string bidQuote)
    {
        Coin = coin;
        BrokerSite = brokerSite;
        BidOrderId = bidOrderId;
        BidQuote = bidQuote;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public string Coin { get; set; } = null!;

    public string BrokerSite { get; set; } = null!;

    public string? AskOrderId { get; set; }

    public string? AskQuote { get; set; }

    public decimal? AskPrice { get; set; }

    public int? AskFillInfo { get; set; }

    public string BidOrderId { get; set; } = null!;

    public string BidQuote { get; set; } = null!;

    public decimal BidPrice { get; set; }

    public decimal TradingVolume { get; set; }

    public DateTime BidTimestamp { get; set; }

    public DateTime? AskTimestamp { get; set; }

    public int? BidFillInfo { get; set; }
}