// 源码位置：第五章\reflection-call-demo\C.cs
// 编译命令：csc /t:library C.cs
using System;
using System.Reflection;

[assembly: AssemblyVersion("2.0")]
public class C
{
    public static void Output(string value)
    {
        Console.WriteLine("Output C " + typeof(C).Assembly.GetName().Version);
        Console.WriteLine(value);
    }
}