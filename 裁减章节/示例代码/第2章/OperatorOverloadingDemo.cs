// 源码位置：第二章\OperatorOverloadingDemo.cs
// 编译命令：csc OperatorOverloadingDemo.cs
using System;

public class OperatorOverloadingDemo
{
    static void Main()
    {
        Number a = new Number(1), b = new Number(2);
        Number sum = a + b;
        Console.WriteLine($"{sum}");
        sum = sum + 3;
        Console.WriteLine($"{sum}");
        double dsum = a + b + 3.4d;
        Console.WriteLine($"{dsum}");
        Console.WriteLine($"{(short)sum}");
    }
}

public class Number
{
    private int Value { get; set; }

    public Number(int v)
    {
        Value = v;
    }

    public static Number operator+(Number left, Number right)
    {
        return new Number(left.Value + right.Value);
    }

    public static Number operator+(Number left, int right)
    {
        return new Number(left.Value + right);
    }

    public static implicit operator double(Number num)
    {
        return (double)num.Value;
    }

    public static explicit operator short(Number num)
    {
        return (short)num.Value;
    }

    public override string ToString() { return Value.ToString(); }
}