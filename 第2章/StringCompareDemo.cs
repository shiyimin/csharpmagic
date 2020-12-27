// 源码位置：第三章\StringCompareDemo.cs
// 编译命令：csc StringCompareDemo.cs
using System;
using System.Globalization;

class StringCompareDemo
{
    static void Main()
    {
        object a = 1, b = 1;
        // Console.WriteLine(a == 1);
        Console.WriteLine(a == (object)1);
        Console.WriteLine(a == b);

        string c = "1", d = "1";
        Console.WriteLine(c == d);
        Console.WriteLine(c == "1");

        Console.WriteLine(string.Compare("1", "2"));
        Console.WriteLine(string.Compare("a", "A"));
        Console.WriteLine(string.Compare("a", "A", true));

        Console.WriteLine(string.Compare(
            "财经传讯公司", "房地产及按揭"));
        Console.WriteLine(string.Compare(
            "财经传讯公司", "房地产及按揭", false, new CultureInfo("zh-CN")));
        Console.WriteLine(string.Compare(
            "财经传讯公司", "房地产及按揭", false, new CultureInfo("en-US")));
    }
}