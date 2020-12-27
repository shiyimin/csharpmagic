// 编译方法
//     csc /addmodule:vbmodule.netmodule /target:module csmodule.cs
// 链接方法
//     al csmodule.netmodule vbmodule.netmodule /target:exe /main:MultiModuleDemo.Main /out:MultiModuleDemo.exe 
using System;

public class MultiModuleDemo
{
    public static void Main()
    {
        var calc = new Calc();
        Console.WriteLine("结果：" + calc.Add(1, 2));
    }
}