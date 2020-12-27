#nullable enable

using System;

namespace TradeBot.Strategy.Band.Db
{
    public class DbPlacedOrder
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Coin { get; set; } = null!;

        public string BrokerSite { get; set; } = null!;

        public int StrategyId { get; set; }

        public string? AskOrderId { get; set; }

        public string? AskQuote { get; set; }

        public decimal? AskPrice { get; set; }

        public bool Closed { get; set; }

        public string BidOrderId { get; set; } = null!;

        public string BidQuote { get; set; } = null!;

        public decimal BidPrice { get; set; }

        public decimal TradingVolume { get; set; }

        public DateTime BidTimestamp { get; set; }

        public DateTime? AskTimestamp { get; set; }
    }
}
