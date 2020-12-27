using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeBot.Strategy.Band.Db;
using TraderBot.Web.Models;

namespace TraderBot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BandTradeContext _context;

        public HomeController(ILogger<HomeController> logger, BandTradeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int page = 0, int size = 20)
        {
            var orders = _context.DbPlacedOrders.Skip(page * size).Take(size);
            return View(orders.ToArray());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
