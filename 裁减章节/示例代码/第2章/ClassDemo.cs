// 源码位置：第二章\ClassDemo.cs
// 编译命令：csc ClassDemo.cs
using System;

public class SavingAccount
{
    public static decimal InterestRate { get; private set; } = 0.03m;

    public string AccountName { get; private set; }

    public decimal Amount 
    {
         get
         {
             var now = DateTime.Now;
             int days = (int)(now - BeginDate).TotalDays;
             return InitialAmount * (1 + InterestRate / 365 * days);
         }
    }

    private decimal _initialAmount;
    public decimal InitialAmount 
    {
         get { return _initialAmount; }
         private set 
         {
             if (value <= 0)
                throw new InvalidOperationException("只接受正数！");
              _initialAmount = value; 
        }
    }

    public DateTime BeginDate { get; private set; }

    public SavingAccount(string name, decimal amount, DateTime? begin = null)
    {
        AccountName = name;
        InitialAmount = amount;
        BeginDate = begin ?? DateTime.Now;
    }

    public void WithDraw(decimal amount)
    {
        if (amount > this.Amount) 
            throw new ArgumentException("余额不足");
        InitialAmount = Amount - amount;
        BeginDate = DateTime.Now;
    }
}

class ClassDemo
{
    static void Main()
    {
        var account1 = new SavingAccount("张三", 10000);
        var account2 = 
            new SavingAccount("李四", 10000, new DateTime(2018, 1, 1));

        Console.WriteLine($"利率是：{SavingAccount.InterestRate * 100}%");
        Console.WriteLine($"{account1.AccountName}的初始余额是" +
            $"{account1.InitialAmount}，当前计息余额是{account1.Amount}");
        Console.WriteLine($"{account2.AccountName}的初始余额是" +
            $"{account2.InitialAmount}，当前计息余额是{account2.Amount}");

        account1.WithDraw(9000);
        account2.WithDraw(9000);

        Console.WriteLine("取款后！");
        Console.WriteLine($"{account1.AccountName}的初始余额是" +
            $"{account1.InitialAmount}，当前计息余额是{account1.Amount}");
        Console.WriteLine($"{account2.AccountName}的初始余额是" +
            $"{account2.InitialAmount}，当前计息余额是{account2.Amount}");
    }
}