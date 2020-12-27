// 源码位置：第五章\reflection-call-demo\B.cs
// 编译命令：csc /t:library /r:C.dll B.cs
using System;
using System.Reflection;

[assembly: AssemblyVersion("1.0")]
public class B
{
    public static void HelloWorld()
    {
        string str = "HelloWorld B " + typeof(B).Assembly.GetName().Version;
        C.Output(str);
    }
}