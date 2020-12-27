// 源码位置：第二章\DecimalDemo.cs
// 编译命令：csc DecimalDemo.cs
using System;

public class DecimalDemo
{
    public static void Main(string[] args)
    {
        int times = int.Parse(args[0]);
        float f1 = 1.0f / times, f2 = 0.0f;
        double d1 = 1.0d / times, d2 = 0.0d;
        decimal c1 = 1.0m / times, c2 = 0.0m;

        DateTime begin = DateTime.Now;
        for (int i = 0; i < times; ++i)
            f2 += f1;
        TimeSpan period = DateTime.Now - begin;
        Console.WriteLine(
            $"float类型计算耗时：{period.TotalMilliseconds}" +
            $"，最终结果：{f2}");

        begin = DateTime.Now;
        for (int i = 0; i < times; ++i)
            d2 += d1;
        period = DateTime.Now - begin;
        Console.WriteLine("double类型计算耗时：" + 
            $"{period.TotalMilliseconds}，最终结果：{d2}");

        begin = DateTime.Now;
        for (int i = 0; i < times; ++i)
            c2 += c1;
        period = DateTime.Now - begin;
        Console.WriteLine($"decimal类型计算耗时：" + 
            $"{period.TotalMilliseconds}，最终结果：{c2}");

        int[] bits = decimal.GetBits(c2);
        Console.WriteLine(
            "{0,8}  {1,10:X8}{2,10:X8}{3,10:X8}{4,10:X8}", 
            "值", "Bits[3]", "Bits[2]", "Bits[1]", "Bits[0]");
        Console.WriteLine(
            "{0,8}  {1,10:X8}{2,10:X8}{3,10:X8}{4,10:X8}", 
            "---------", "--------", "--------", 
            "--------", "--------");
        Console.WriteLine(
            "{0,8}  {1,10:X8}{2,10:X8}{3,10:X8}{4,10:X8}", 
            c2, bits[3], bits[2], bits[1], bits[0]);

        Console.WriteLine(1.0m / 3.0m * 3.0m);
        Console.WriteLine(Math.Round(1.0m / 3.0m * 3.0m));
    }
}