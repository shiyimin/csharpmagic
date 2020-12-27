// 源码位置：第三章\StringFormatDemo.cs
// 编译命令：csc StringFormatDemo.cs
using System;

class StringFormatDemo
{
    static void Main()
    {
        var str = string.Format(
            "当前时间：{0}, 温度是：{1}°C。", DateTime.Now, 24.5);
        Console.WriteLine(str);
        Console.WriteLine(
            "当前日期：{0:d}，时间：{0:t}", DateTime.Now);

        int[] years = { 2013, 2014, 2015 };
        int[] population = { 1025632, 1105967, 1148203 };
        Console.WriteLine(
            "{0,6}{1,15}", "Year", "Population");
        for (int i = 0; i < years.Length; i++)
            Console.WriteLine(
                "{0,6}{1,15:N0}", 
                years[i], population[i]);

        Console.WriteLine("");
        Console.WriteLine("{0, -12}{1,8:yyyy}{2,12:N2}{3,14:P1}", 
            "BeiJing", DateTime.Now, 123456, 0.32d);

        // 标准数字格式
        Console.WriteLine(123.456m.ToString("N2"));
        // 自定义数字格式
        Console.WriteLine(123.4.ToString("00000.000"));

        char c1 = '{', c2 = '}';
        Console.WriteLine(
            "打印大括号用法相同：\n {{ 和 }}\n {0} 和 {1}", c1, c2);
        var path1 = @"c:\china-pub\C#\sample-code";
        var path2 = "c:\\china-pub\\C#\\sample-code";
        Console.WriteLine(
            "两个路径是相同的：\n{0}\n{1}", path1, path2);
    }
}