// 源码位置：第五章\attributedemo\utility\ConditionalAttrDemo.cs
// 编译命令：csc /define:DEBUG SerializationDemo.cs
// #define DEBUG
using System;
using System.Diagnostics;

public class ConditionalAttrDemo
{
    static void Main()
    {
        Console.WriteLine("证明确实有文本输出");
        OnlyDebug();
    }

    [Conditional("DEBUG")]
    private static void OnlyDebug()
    {
        Console.WriteLine("OnlyDebug Called");
    }
}