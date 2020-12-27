using System;

namespace TraderBot.Core
{
    /// <summary>
    /// 表示最新的行情
    /// </summary>
    public class Ticker
    {
        public string Site { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        /// <summary>
        /// 表示以币为基准的交易量，也就是所有成交订单的【订单成交量】，很多交易所的返回值没有这个字段，因此是一个可空字段
        /// </summary>
        public decimal? CoinVolume { get; set; }

        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 表示使用计价币为标准衡量的交易量，也就是所有成交订单的【成交价格】*【订单成交量】
        /// </summary>
        public decimal QuoteVolume { get; set; }
    }
}