// 源码位置：第五章\assemblyversion\AssemblyVersionDemo.cs
// 编译命令：
//    sn -k key.snk
//    csc /t:library /keyfile:key.snk AssemblyVersionDemo.cs
// 打印公钥token
//    sn -T AssemblyVersionDemo.dll
using System;
using System.Reflection;

[assembly: AssemblyVersion("1.0.0.0")]
public class AssemblyVersionDemo
{
    public static void Main()
    {
        Console.WriteLine("Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
    }
}