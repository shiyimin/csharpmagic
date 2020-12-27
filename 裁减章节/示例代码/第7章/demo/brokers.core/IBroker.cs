using System;

namespace TraderBot.Core
{
    /// <summary>
    /// 代表交易所的接口封装
    /// </summary>
    public interface IBroker
    {
        /// <summary>
        /// 交易手续费，有的交易所分maker和taker，这里简单起见，取费率最高的作为手续费
        /// </summary>
        decimal Fee { get; }

        /// <summary>
        /// 获取交易所名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderid">要取消的订单id</param>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <returns>如果成功取消，返回true，否则返回false</returns>
        bool CancelOrder(object orderid, string quote, string coin);

        /// <summary>
        /// 获取k线历史数据
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <param name="begin">要获取k线历史的起始时间</param>
        /// <param name="end">要获取k线历史的结束时间</param>
        /// <param name="interval">k线的间隔</param>
        /// <returns>指定耗时间范围的k线历史数据</returns>
        Ticker[] GetKline(string quote, string coin, DateTime begin, DateTime end, KlineInterval interval);

        /// <summary>
        /// 获取指定交易对的当前深度
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <param name="limit">返回的深度长度</param>
        /// <returns>当前最靠前的<paramref name="limit"/>长度的深度信息</returns>
        OrderBook GetOrderBook(string quote, string coin, int limit = OrderBook.ORDER_CACHING_COUNT);

        /// <summary>
        /// 下达买单
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <param name="quantity">数量</param>
        /// <param name="price">价格</param>
        /// <returns>下单成功返回交易所的订单id，否则返回null</returns>
        object BuyLimit(string quote, string coin, decimal quantity, decimal price);

        /// <summary>
        /// 下达卖单
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <param name="quantity">数量</param>
        /// <param name="price">价格</param>
        /// <returns>下单成功返回交易所的订单id，否则返回null</returns>
        object SellLimit(string quote, string coin, decimal quantity, decimal price);

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderid">交易所的订单id</param>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <returns>交易所的订单详情，如果订单不存在，则返回null</returns>
        ExchangeOrder GetOrderInfo(object orderid, string quote, string coin);

        /// <summary>
        /// 获取账户的余额
        /// </summary>
        /// <returns>当前登录账户在交易所的所有余额</returns>
        BalanceItem[] GetBalances();

        /// <summary>
        /// 获取账户所有的未成交订单
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <returns>如果成功返回所有未成交订单，没有未成交订单的话返回空数组，调用失败的话返回null或者抛出异常。</returns>
        ExchangeOrder[] GetOpenOrders(string quote, string coin);

        /// <summary>
        /// 判断交易所是否支持指定交易对
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <returns>如果支持交易对返回true，否则返回false</returns>
        bool Support(string quote, string coin);

        /// <summary>
        /// 有的交易所有下单限制，例如价格、最小交易量等限制
        /// </summary>
        /// <param name="quote">计价币</param>
        /// <param name="coin">币，与<paramref name="quote"/>组合成交易对</param>
        /// <returns>返回交易量限制，没有限制的话返回空数组，调用失败的话返回null或者抛出异常。</returns>
        TradingLimitInfo[] GetTradeLimit(string quote, string coin);

        /// <summary>
        /// 延迟初始化交易所对象，有的交易所接口在使用前需要获取一些配置信息。
        /// 这个方法一般是静态方法，设置为接口以便调用者将配置信息传入
        /// </summary>
        /// <param name="confdir">配置文件保存的位置</param>
        void Initialize(string confdir);

        /// <summary>
        /// 有些交易所需要做一些扫尾操作，比如有些交易所要求每次接口调用客户端都要传入一个自增的唯一id，
        /// 使用该方法以便支持上述的场景 
        /// </summary>
        void Cleanup();

        /// <summary>
        /// 使用用户的授权信息创建实例
        /// </summary>
        /// <param name="apiKey">下单等授权api的api key信息</param>
        /// <param name="apiSecret">下单等授权api的api secret信息</param>
        /// <returns>授权的实例</returns>
        IBroker CreateAuthenticatedInstance(string apiKey, string apiSecret);
    }
}
