using System;

namespace TradeBot.Strategy.Band.Db
{
    public class BandStrategy
    {
        public BandStrategy(int userId, string quote, string coin, decimal bidPrice, decimal tradeVolume, decimal? askProfit, decimal? askPrice)
        {
            if (!askProfit.HasValue && !askPrice.HasValue)
                throw new ArgumentException("期望利润和期望卖价必须指定一个值！");
            if (askProfit.HasValue && askPrice.HasValue)
                throw new ArgumentException("期望利润和期望卖价只能指定一个值！");

            UserId = userId;
            Quote = quote;
            Coin = coin;
            BidPrice = bidPrice;
            TradeVolume = tradeVolume;
            AskProfit = askProfit;
            AskPrice = askPrice;
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public decimal BidPrice { get; set; }

        public decimal TradeVolume { get; set; }

        public decimal? AskProfit { get; set; }

        public decimal? AskPrice { get; set; }
    }
}
