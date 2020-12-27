// 源码位置：第二章\InterfaceVsAbstractClassDemo.cs
// 编译命令：csc InterfaceVsAbstractClassDemo.cs
using System;

public interface IAccount
{
    string Name { get; }
    decimal Amount { get; }
    void WithDraw();
}

public sealed class SavingAccount : IAccount
{
    public static decimal InterestRate {get; private set; } = 0.03m;
    public string Name { get; private set; }
    public decimal InitialAmount { get; private set; }
    public DateTime Begin {get; private set;}

    public SavingAccount(string name, decimal initial)
    {
        Name = name;
        InitialAmount = initial;
        Begin = DateTime.Now.AddDays(-100);
    }

    public decimal Amount
    {
        get
        {
            var days = (decimal)(DateTime.Now - Begin).TotalDays;
            return InitialAmount *
                (1 + InterestRate / 365 * days);
        }
    }

    public void WithDraw()
    {
        Console.WriteLine("SavingAccount.WithDraw!");
        InitialAmount = 0;
    }
}

public abstract class InvestAccount : IAccount
{
    public string Name { get; private set; }
    public decimal InitialFund { get; protected set; }

    public InvestAccount(string name, decimal initial)
    {
        Name = name;
        Amount = InitialFund = initial;
    }

    public void Buy(decimal price, decimal amount)
    {
         var dealed = BuyInternal(price, amount);
         Amount += dealed;
         InitialFund -= price * dealed;
    }

    public void Sell(decimal price, decimal amount)
    {
        if (price < 0)
        {
            Amount -= amount;
            InitialFund += MarketPrice * amount;
        }
        else
        {
            var dealed = SellInternal(price, amount);
            Amount -= dealed;
            InitialFund += price * dealed;
        }
    }
    
    protected abstract decimal BuyInternal(decimal price, decimal amount);
    protected abstract decimal SellInternal(decimal price, decimal amount);
    public decimal Amount { get; protected set;}
    public abstract decimal MarketPrice { get; }

    public virtual void WithDraw()
    {
        Sell(-1, Amount);
        InitialFund = 0;
        Console.WriteLine("InvestAccount.WithDraw，T + 0提现");
    }
}

public class StockAccount : InvestAccount
{
    Random _rnd = new Random();
    public override decimal MarketPrice
    {
        get { return (decimal)_rnd.NextDouble(); }
    }
    public StockAccount(string name, decimal initial)
        : base(name, initial) {}

    protected override decimal BuyInternal(decimal price, decimal amount)
    {   
        Console.WriteLine("StockAccount.BuyInternal");
        return amount * (decimal)_rnd.NextDouble();
    }

    protected override decimal SellInternal(decimal price, decimal amount) 
    {
        Console.WriteLine("StockAccount.SellInternal");
        return amount * (decimal)_rnd.NextDouble();
    }
}

public class FundAccount : InvestAccount
{
    public override decimal MarketPrice { get { return 1m; } }

    public FundAccount(string name, decimal initial)
        : base(name, initial) {}

    protected override decimal BuyInternal(decimal price, decimal amount)
    {
        Console.WriteLine("FundAccount.BuyInternal");
        return amount;
    }

    protected override decimal SellInternal(decimal price, decimal amount) 
    {
        Console.WriteLine("FundAccount.SellInternal");
        return amount;
    }

    public override void WithDraw()
    {
        Console.WriteLine("FundAccount.WithDraw，需要 T + X");
        InitialFund = 0;
    }
}

public class InterfaceVsAbstractClassDemo
{
    static void Main()
    {
        var name = "张三";
        var amount = 10000m;
        var accounts = new IAccount[] {
            new SavingAccount(name, amount),
            new StockAccount(name, amount),
            new FundAccount(name, amount)
        };

        foreach (var account in accounts)
            account.WithDraw();

        Console.WriteLine("-------------------------------");
        var fund = (FundAccount)accounts[2];
        fund.Buy(1, 100);

        for (var i = 0; i < accounts.Length; ++i)
        {
            if (accounts[i] is InvestAccount)
            {
                var invest = accounts[i] as InvestAccount;
                invest.Buy(1, 200);
                invest.WithDraw();
            }
        }
    }
}