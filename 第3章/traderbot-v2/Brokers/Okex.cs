using System;
using System.Collections.Generic;
using System.Text;
using TraderBot.Core;

namespace TraderBot.Brokers
{
    public class Okex : IBroker
    {
        private const string SITE_NAME = "Okex";
        private string _apiKey;
        private string _apiSecret;

        public Okex(ApiOption option)
        {
            _apiKey = option.ApiKey;
            _apiSecret = option.ApiSecret;
        }

        public string Name => SITE_NAME;

        public object BuyLimit(string quote, string coin, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public bool CancelOrder(object orderid, string quote, string coin)
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        public BalanceItem[] GetBalances()
        {
            throw new NotImplementedException();
        }

        public Ticker[] GetKline(string quote, string coin, DateTime begin, DateTime end, KlineInterval interval)
        {
            throw new NotImplementedException();
        }

        public ExchangeOrder[] GetOpenOrders(string quote, string coin)
        {
            throw new NotImplementedException();
        }

        public OrderBook GetOrderBook(string quote, string coin, int limit = 10)
        {
            throw new NotImplementedException();
        }

        public ExchangeOrder GetOrderInfo(object orderid, string quote, string coin)
        {
            throw new NotImplementedException();
        }

        public TradingLimitInfo[] GetTradeLimit(string quote, string coin)
        {
            throw new NotImplementedException();
        }

        public void Initialize(string confdir)
        {
            throw new NotImplementedException();
        }

        public object SellLimit(string quote, string coin, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public bool Support(string quote, string coin)
        {
            throw new NotImplementedException();
        }
    }
}
