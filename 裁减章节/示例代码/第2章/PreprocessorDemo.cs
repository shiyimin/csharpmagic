// 源码位置：第二章\PreprocessorDemo.cs
// 编译命令：csc /d:DEBUG PreprocessorDemo.cs
using System;

public class PreprocessorDemo
{
    static void Main(string[] args)
    {
#pragma warning disable 219
        int i = 0;
#pragma warning restore CS0219

#if DEBUG
        Console.WriteLine("定义了DEBUG宏");
        // int j = 1;
#else
        Console.WriteLine("没有宏");
#endif
        OnlyDebugDefined();
        // Console.WriteLine(j + j);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    static void OnlyDebugDefined()
    {
        Console.WriteLine("只有在DEBUG定义了的情况下才有");
    }
}