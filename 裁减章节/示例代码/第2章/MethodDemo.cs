// 源码位置：第二章\MethodDemo.cs
// 编译命令：csc MethodDemo.cs
using System;

public class MethodDemo
{
    static void Main(string[] args)
    {
        int i = 2;
        string s = "StringInMain";
        TestClass tc = new TestClass();
        tc.Value = 2;
        PassByValue(i, s, tc);
        Console.WriteLine($"ByValue: {i}, {s}, {tc.Value}");

        i = 2;
        s = "StringInMain";
        tc = new TestClass();
        tc.Value = 2;
        PassByRef(ref i, ref s, ref tc);
        Console.WriteLine($"ByRef: {i}, {s}, {tc.Value}");

        int j;
        PassByOut(out j);
        PassByOut(out int k);
        Console.WriteLine($"ByOut: {j}, {k}");

        int ret = Sum(1, 2, 3, 4, 5, 6, 7, 8, 9, 0);
        Console.WriteLine($"变参：{ret}");

        包含可选参数方法(1);
        包含可选参数方法(1, 2, "传入可变参数");
        包含可选参数方法(1, 2, "传入可变参数", 1, 2, 3);
        包含可选参数方法(1, s: "传入可变参数", 
            values: new int[] { 1, 2, 3 });

        MethodDemo md = new MethodDemo();
        md.InstanceMethod();

        Console.WriteLine(Add(1, 2));
        Console.WriteLine(Add(1, 2, 3));
        Console.WriteLine(Add("1", "2"));
        Console.WriteLine(Add(1, 2d));
    }

    private static void PassByValue(
        int i, string s, TestClass tc)
    {
        i = i * 2;
        s = "PassByValue";
        tc.Value = tc.Value * 2;
    }

    static void PassByRef(
        ref int i, ref string s, ref TestClass tc)
    {
        i = i * 2;
        s = "PassByRef";
        tc.Value = tc.Value * 2;
    }

    static void PassByOut(out int i)
    {
        i = 100;
    }

    static int Sum(params int[] values)
    {
        int result = 0;
        for (var i = 0; i < values.Length; ++i)
            result += values[i];
        return result;
    }

    static void 包含可选参数方法(
        int required, int optional = 100, 
        string s = "可选参数", params int[] values)
    {
        Console.WriteLine(
            $"{required}, {optional}, {s}, {Sum(values)}"); 
    }

    void InstanceMethod()
    {
        Console.WriteLine("调用了实例方法！");
    }

    static int Add(int x, int y) { return x + y; }

    static int Add(int x, int y, int z) { return x + y + z; }

    static double Add(string x, string y) 
    {
         return double.Parse(x) + double.Parse(y);
    }

    static double Add(double x, double y) { return x + y; }
}

class TestClass
{
    public int Value;
}