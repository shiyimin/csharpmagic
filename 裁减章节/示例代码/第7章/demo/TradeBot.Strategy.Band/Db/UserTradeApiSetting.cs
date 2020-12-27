using TraderBot.Core;

namespace TradeBot.Strategy.Band.Db
{
    public class UserTradeApiSetting : IApiOption
    {
        public UserTradeApiSetting(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public int Id { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }
    }
}
