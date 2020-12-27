using System;

namespace TraderBot.Core
{
    /// <summary>
    /// ��ʾ���µ�����
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
        /// ��ʾ�Ա�Ϊ��׼�Ľ�������Ҳ�������гɽ������ġ������ɽ��������ཻܶ�����ķ���ֵû������ֶΣ������һ���ɿ��ֶ�
        /// </summary>
        public decimal? CoinVolume { get; set; }

        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ��ʾʹ�üƼ۱�Ϊ��׼�����Ľ�������Ҳ�������гɽ������ġ��ɽ��۸�*�������ɽ�����
        /// </summary>
        public decimal QuoteVolume { get; set; }
    }
}