using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TradeBot.Strategy.Band.Db;
using TraderBot.Core;
using TraderBot.Web.Models;

namespace TraderBot.Web.Controllers
{
    public class TradeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BrokerFactoryDelegate _factory;
        private IConfiguration _config;
        private BandTradeContext _context;
        public TradeController(BrokerFactoryDelegate factory, IConfiguration config, BandTradeContext context, ILogger<HomeController> logger)
        {
            _factory = factory;
            _config = config;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(string q = null, string c = null, string s = null, decimal? p = null, decimal? v = null, int? d = null)
        {
            return View(new TradeOrder()
            {
                Quote = q,
                Coin = c,
                Site = s,
                Side = d.HasValue && d.Value == (int)TradeSide.Sell ? "Sell" : "Buy",
                Price = p,
                Volume = v
            });
        }

        [HttpPost]
        public IActionResult Index(TradeOrder model)
        {
            if (string.IsNullOrEmpty(model.Side))
            {
                this.TempData["ErrorMessage"] = "请指明是要买入还是卖出！";
                return View();
            }
            if (!model.Volume.HasValue)
            {
                this.TempData["ErrorMessage"] = "请指明是要交易的数量！";
                return View();
            }
            if (string.IsNullOrEmpty(model.Quote))
            {
                this.TempData["ErrorMessage"] = "请指明需要交易的计价币！";
                return View();
            }
            if (string.IsNullOrEmpty(model.Coin))
            {
                this.TempData["ErrorMessage"] = "请指明需要交易的数字货币！";
                return View();
            }
            if (ModelState.IsValid)
            {
                var broker = _factory(model.Site);
                var cfgbase = _config["configdir"];
                broker.Initialize(cfgbase);
                decimal price = 0M;
                if (!model.Price.HasValue)
                {
                    var ob = broker.GetOrderBook(model.Quote, model.Coin);
                    if (ob == null)
                    {
                        ViewBag.ErrorMessage = "无法从交易所获取最新竞价信息，可能是网络原因，交易所返回数据太慢，请重试.....";
                        return View();
                    }
                    price = model.Side == "Buy" ? ob.LowestAsk : ob.HighestBid;
                }
                else
                {
                    price = model.Price.Value;
                }

                object orderid = null;
                string side = null;
                if (model.Side == "Buy")
                {
                    side = "买入";
                    orderid = broker.BuyLimit(model.Quote, model.Coin, model.Volume.Value, price);
                }
                else
                {
                    side = "卖出";
                    orderid = broker.SellLimit(model.Quote, model.Coin, model.Volume.Value, price);
                }

                if (orderid != null)
                {
                    var order = new DbPlacedOrder()
                    {
                        BrokerSite = model.Site,
                        Coin = model.Coin,
                        TradingVolume = model.Volume.Value
                    };

                    if (model.Side == "Buy")
                    {
                        order.BidQuote = model.Quote;
                        order.BidPrice = price;
                        order.BidOrderId = orderid.ToString();
                        order.BidTimestamp = DateTime.Now;
                        order.AskQuote = model.Quote;
                        order.AskTimestamp = DateTime.Now;
                        order.AskOrderId = "manual";
                        order.AskPrice = 0;
                    }
                    else
                    {
                        order.BidQuote = model.Quote;
                        order.BidPrice = 0;
                        order.BidOrderId = "manual";
                        order.AskQuote = model.Quote;
                        order.AskPrice = price;
                        order.AskOrderId = orderid.ToString();
                        order.AskTimestamp = DateTime.Now;
                    }

                    _context.DbPlacedOrders.Add(order);
                    _context.SaveChanges();
                    var msg = "已完成向交易所" + model.Site + side + model.Coin + "，交易量：" + model.Volume + "，价格：" + model.Price + "，订单号：" + orderid;
                    _logger.LogInformation(msg);
                    ViewBag.ErrorMessage = msg;
                    return View();
                }
                else
                {
                    var msg = "向交易所" + model.Site + side + model.Coin + "，交易量：" + model.Volume + "，价格：" + model.Price + "，执行失败，可能是网络原因，交易所返回数据太慢，请重试.....";
                    _logger.LogWarning(msg);
                    ViewBag.ErrorMessage = msg;
                    return View();
                }
            }

            ViewBag.ErrorMessage = "下单成功";
            return View();
        }
    }
}