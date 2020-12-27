// 源码位置：第二章\MainDemo.cs
// 编译命令：csc /main:MainDemo1 MainDemo.cs
using System;

public class MainDemo1
{
    public const int ARGUMENT_ERROR = 1;
    public const int I_HATE_ODD = 2;

    public static int Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine(
                "Usage: MainDemo <arg1> <arg2>");
            return ARGUMENT_ERROR;
        }

        Console.WriteLine(
            $"收到参数：【{args[0]}】与【{args[1]}】");
        var left = int.Parse(args[0]);
        var right = int.Parse(args[1]);
        var result = left + right;
        if (result % 2 != 0)
            return I_HATE_ODD;
        else
            return 0;
    }
}

public class MainDemo2
{
    public static void Main()
    {
        var rnd = new Random();
        var value = rnd.Next();
        if (value % 2 != 0)
            throw new Exception("奇数触发异常！");
        else
            Console.WriteLine("偶数正常执行！");
    }
}