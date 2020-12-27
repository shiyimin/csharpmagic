// 源码位置：第二章\AnonymousTypeDemo.cs
// 编译命令：csc AnonymousTypeDemo.cs
using System;

class AnonymousTypeDemo
{
    static void Main()
    {
        var person = new { Name = "张三", Age = 18 };
        Console.WriteLine($"姓名：{person.Name}。");
    }
}