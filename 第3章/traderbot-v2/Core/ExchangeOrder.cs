using System;

namespace TraderBot.Core
{
    /// <summary>
    /// 代表各个站点的订单情况
    /// </summary>
    public class ExchangeOrder
    {
        /// <summary>
        ///  订单Id
        /// </summary>
        public object Id { get; set; }

        public string Site { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public decimal Quantity { get; set; }

        public decimal QuantityRemaining { get; set; }

        public decimal Price { get; set; }

        public DateTime? PlacedTimestamp { get; set; }

        public DateTime? ClosedTimestamp { get; set; }

        public TradeSide Side { get; set; }

        public decimal? Commission { get; set; }

        public bool IsCancelled { get; set; }
    }
}