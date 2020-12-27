using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TraderBot.Core;

namespace TraderBot.Web.Models
{
    public delegate IBroker BrokerFactoryDelegate(string site);

    public class TradeOrder
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "交易所不能为空")]
        public string Site { get; set; }

        public string Quote { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "数字币不能为空")]
        public string Coin { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "单价不能为空")]
        public decimal? Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "数量不能为空")]
        public decimal? Volume { get; set; }

        public string Side { get; set; }
    }
}
