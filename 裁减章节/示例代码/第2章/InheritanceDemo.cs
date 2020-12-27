// 源码位置：第二章\InheritanceDemo.cs
// 编译命令：csc InheritanceDemo.cs
using System;

public class Account
{
    public string Name { get; protected set; }

    public decimal InitialAmount { get; private set; }

    public DateTime Begin {get; private set;}

    public Account(string name, decimal initial)
    {
        Name = name;
        InitialAmount = initial;
        Begin = DateTime.Now.AddDays(-100);
    }

    public virtual decimal Amount
    {
        get { return InitialAmount; }
    }
}

public sealed class SavingAccount : Account
{
    public static decimal InterestRate {get; private set; } = 0.03m;

    public SavingAccount(string name, decimal initial)
        : base(name, initial) {}

    public override decimal Amount
    {
        get
        {
            var days = (decimal)(DateTime.Now - Begin).TotalDays;
            return InitialAmount *
                (1 + InterestRate / 365 * days);
        }
    }
}

public class StockAccount : Account
{
    public StockAccount(string name, decimal initial)
        : base(name, initial) 
    {
        Name = name + "的股票账户";
    }

    public override decimal Amount
    {
        get
        {
            var rnd = new Random();
            var earning = (decimal)rnd.NextDouble() - 0.5m;
            return ((Account)this).InitialAmount * (1 + earning);
        }
    }

    public new decimal InitialAmount { get; set; }
}

public class InheritanceDemo
{
    static void Main()
    {
        var name = "张三";
        var amount = 10000m;
        var accounts = new Account[] {
            new SavingAccount(name, amount),
            new StockAccount(name, amount)
        };

        foreach (var acc in accounts)
            Console.WriteLine($"截止目前：{acc.Name}的余额是{acc.Amount}");

        var stock = (StockAccount)accounts[1];
        stock.InitialAmount = 99999;
        Console.WriteLine($"股票账户初始余额：{stock.InitialAmount}，" +
            $"而对应的账户数组中余额：{accounts[1].InitialAmount}");
    }
}