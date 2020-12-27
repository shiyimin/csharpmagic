// 源码位置：第五章\CustomerAttributeDemo.cs
// 编译命令：csc /define:DEBUG CustomerAttributeDemo.cs
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class | 
                AttributeTargets.Struct, 
                AllowMultiple = true)]
public class AuthorAttribute : Attribute
{
    public string Name { get; private set; }

    public string Version { get; set; }

    public AuthorAttribute(string name)
    {
        Name = name;
    }
}

[Author("施懿民", Version = "1.0.0.0")]
[Author("张三", Version = "1.1.0.0")]
public class FooClass
{
    public int Bar { get; set; }
}

public class PrintCodeAuthor
{
    public static void Main()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = assembly.GetType("FooClass", true);
        var attrs = type.GetCustomAttributes(typeof(AuthorAttribute), false);
        foreach (AuthorAttribute author in attrs)
            Console.WriteLine(
                $"类型{type.Name} v{author.Version} 的作者是：{author.Name}");
    }
}
