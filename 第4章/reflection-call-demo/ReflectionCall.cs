// 源码位置：第五章\reflection-call-demo\ReflectionCall.cs
// 编译命令：csc ReflectionCall.cs
using System;
using System.Reflection;

public class ReflectionCall
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: ReflectionCall DemoClass.dll");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type type = assembly.GetType("DemoClass");
        MethodInfo sAdd = type.GetMethod("Add", BindingFlags.Static | BindingFlags.NonPublic);
        int result = (int)sAdd.Invoke(null, new object[] { 1, 2 });
        Console.WriteLine($"Static Add: {result}");
        ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(int) });
        object dcInst = ctor.Invoke(new object[] { 10 });
        PropertyInfo property = type.GetProperty("Value");
        result = (int)property.GetValue(dcInst);
        Console.WriteLine($"Value: {result}");
        MethodInfo add = type.GetMethod("Add", new Type[] { typeof(int) });
        add.Invoke(dcInst, new object[] { 3 });
        property = type.GetProperty("Value");
        result = (int)property.GetValue(dcInst);
        Console.WriteLine($"Instance Add: {result}");
    }
}