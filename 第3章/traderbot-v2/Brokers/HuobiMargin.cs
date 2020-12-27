using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TraderBot.Core;
using Microsoft.Extensions.Logging;

namespace TraderBot.Brokers
{
    public class HuobiMargin : Huobi
    {
        private ILogger<HuobiMargin> _logger;

        public HuobiMargin(ApiOption option, ILogger<HuobiMargin> logger) : base(option, logger)
        {
            _logger = logger;
        }

        public override object BuyLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return PlaceOrder(quote, coin, quantity, price, "buy-limit", "margin-api");
        }

        public override object SellLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return PlaceOrder(quote, coin, quantity, price, "sell-limit", "margin-api");
        }

        public override BalanceItem[] GetBalances()
        {
            var ret = new List<BalanceItem>();
            if (AllAccounts != null && AllAccounts.Length > 0)
            {
                foreach (var account in AllAccounts)
                {
                    if (string.Compare(account.type, "margin", true) == 0)
                    {
                        var result = GetBalancesImpl(account.id.ToString());
                        if (result != null)
                        {
                            ret.AddRange(result);
                        }
                    }
                }
            }

            return ret.ToArray();
        }

        protected override string 获取交易账号(string quote, string coin)
        {
            var symbol = BuildSymbol(quote, coin);
            if (AllAccounts != null && AllAccounts.Length > 0)
            {
                foreach (var account in AllAccounts)
                {
                    if (string.Compare(account.type, "margin", true) == 0 && 
                        string.Compare(account.subtype, symbol, true) == 0)
                    {
                        return account.id.ToString();
                    }
                }
            }
            
            return null;
        }
    }
}