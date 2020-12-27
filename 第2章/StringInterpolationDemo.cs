// 源码位置：第三章\StringInterpolationDemo.cs
// 编译命令：csc StringInterpolationDemo.cs
using System;

class StringInterpolationDemo
{
    static void Main()
    {
        var degree = 24.5d;
        var str = $"当前时间：{DateTime.Now}, 温度是：{degree}°C。";
        Console.WriteLine(str);

        var lang = "C#";
        str = $@"c:\china-pub\{lang}\sample-code";
        Console.WriteLine(str);

        Console.WriteLine($"使用表达式：{degree * 2 / 3}");
    }
}