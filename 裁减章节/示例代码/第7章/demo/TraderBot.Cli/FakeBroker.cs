using System;
using System.Collections.Generic;
using System.Text;
using TraderBot.Core;

namespace TraderBot.Cli
{
    public class FakeBroker : IBroker
    {
        public decimal Fee => 0.001m;

        public string Name => "Fake";

        public object BuyLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return 1;
        }

        public bool CancelOrder(object orderid, string quote, string coin)
        {
            return true;
        }

        public void Cleanup()
        {
        }

        public IBroker CreateAuthenticatedInstance(string apiKey, string apiSecret)
        {
            return this;
        }

        public BalanceItem[] GetBalances()
        {
            return new BalanceItem[0];
        }

        public Ticker[] GetKline(string quote, string coin, DateTime begin, DateTime end, KlineInterval interval)
        {
            return new Ticker[0];
        }

        public ExchangeOrder[] GetOpenOrders(string quote, string coin)
        {
            return new ExchangeOrder[0];
        }

        public OrderBook GetOrderBook(string quote, string coin, int limit = 10)
        {
            return null;
        }

        public ExchangeOrder GetOrderInfo(object orderid, string quote, string coin)
        {
            return null;
        }

        public TradingLimitInfo[] GetTradeLimit(string quote, string coin)
        {
            return null;
        }

        public void Initialize(string confdir)
        {
        }

        public object SellLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return 1;
        }

        public bool Support(string quote, string coin)
        {
            return true;
        }
    }
}
