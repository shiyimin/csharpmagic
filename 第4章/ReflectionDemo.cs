// 源码位置：第五章\ReflectionDemo.cs
// 编译命令：csc ReflectionDemo.cs
using System;
using System.Reflection;

public class ReflectionDemo
{
    static void Main()
    {
        int i = 42;
        Type type = i.GetType();
        // 打印：变量i的类型: System.Int32
        Console.WriteLine($"变量i的类型: {type}");
        // 打印：变量i的类型: System.Int32，属于：System.Private.CoreLib ……
        type = typeof(int);
        Console.WriteLine($"变量i的类型: {type}，属于：{type.Assembly}");
        // 打印：本类: ReflectionDemo，属于：ReflectionDemo ……
        type = typeof(ReflectionDemo);
        Console.WriteLine($"本类: {type}，属于：{type.Assembly}");
        // 打印：当前组装件：ReflectionDemo, ……
        Assembly assembly = Assembly.GetExecutingAssembly();
        Console.WriteLine($"当前组装件：{assembly}");

        type = typeof(DateTime);
        ConstructorInfo[] ctors = type.GetConstructors(
            BindingFlags.Public | BindingFlags.Instance);
        PrintMembers(ctors);
        MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public | BindingFlags.Instance);
        PrintMembers(methods);
        FieldInfo[] fields = type.GetFields(
            BindingFlags.Public | BindingFlags.Instance);
        PrintMembers(fields);
        PropertyInfo[] properties = type.GetProperties(
            BindingFlags.Public | BindingFlags.Instance);
        PrintMembers(properties);
    }

    static void PrintMembers(MemberInfo[] members)
    {
        foreach (var member in members)
            Console.WriteLine($"{member.MemberType} {member.Name}");

        Console.WriteLine("------------");
    }
}