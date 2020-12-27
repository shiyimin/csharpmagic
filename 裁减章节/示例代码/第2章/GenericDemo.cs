// 源码位置：第二章\GenericDemo.cs
// 编译命令：csc /unsafe /langversion:"7.3" GenericDemo.cs
using System;
using System.Collections.Generic;

public class GenericDemo
{
    static void Main()
    {
        var intList = new List<int>();
        for (var i = 0; i < 10; ++i) {
            intList.Add(i);
        }

        var strList = new List<string>();
        strList.Add("A string message");

        var vto1 = new ValueTypeOnly<int>();
        var vto2 = new ValueTypeOnly<TestStruct>();
        // TestBaseClass是引用类型，不能具象化ValueTypeOnly
        // var vto3 = new ValueTypeOnly<TestBaseClass>();

        var rto1 = new RefTypeOnly<Object>();
        var rto2 = new RefTypeOnly<TestBaseClass>();
        // 值类型不能具象化ValueTypeOnly
        // var rto3 = new RefTypeOnly<int>();
        // var rto4 = new RefTypeOnly<TestStruct>();

        var nbo1 = new NewableOnly<TestDeriveClass>();
        // 值类型是可以用在new() 限制条件中的
        var nbo2 = new NewableOnly<TestStruct>();
        var nbo3 = new NewableOnly<int>();
        // TestBaseClass的构造函数是protected，不能使用
        // var nbo4 = new NewableOnly<TestBaseClass>();

        /*
        var map = EnumNamedValues<Rainbow>();

        foreach (var pair in map)
            Console.WriteLine($"{pair.Key}:\t{pair.Value}");

        UnmanagedBuffer ub;
        for (var i = 0;i < 64; ++i)
            ub.Buffer[i] = i;
        var bytes = ToByteArray(ub);
        Console.WriteLine($"{bytes.Length}");

        bytes = ToByteArray(123);
        Console.WriteLine($"{bytes.Length}");
        
        var value = 123;

        unsafe
        {
            fixed (char *ptr = &value)
            {
                bytes = ToByteArray(ptr);
                Console.WriteLine($"{bytes.Length}");
            }
        }
        */
    }

    /*
    // 7.3以后的版本才支持
    unsafe public static byte[] ToByteArray<T>(T argument) where T : unmanaged
    {
        var size = sizeof(T);
        var result = new Byte[size];
        Byte* p = (byte*)&argument;
        for (var i = 0; i < size; i++)
            result[i] = *p++;
        return result;
    }

    // 7.3以后的版本才支持
    public static T TypeSafeCombine<T>(
        T source, T target) where T : System.Delegate
    => Delegate.Combine(source, target) as T;

    // 7.3以后的版本才支持
    public static Dictionary<int, string> EnumNamedValues<T>() where T : System.Enum
    {
        var result = new Dictionary<int, string>();
        var values = Enum.GetValues(typeof(T));

        foreach (int item in values)
            result.Add(item, Enum.GetName(typeof(T), item));
        return result;
    }
    */
}

public class ValueTypeOnly<T> where T : struct {}

public struct TestStruct {}

public class RefTypeOnly<T> where T : class {}

public class NewableOnly<T> where T : new() 
{
    private T _item = default(T);
    public NewableOnly()
    {
        Console.WriteLine($"在new之前：{_item}");
        _item = new T();
        Console.WriteLine($"在new之后：{_item}");
    }
}

public class TestBaseClass
{
    protected TestBaseClass() {} 
}

public class TestDeriveClass : TestBaseClass
{
    public TestDeriveClass() : base() 
    {
        Console.WriteLine("TestDeriveClass.ctor被调用了！");
    }
}

/*
enum Rainbow
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

public unsafe struct UnmanagedBuffer
{
    public fixed char Buffer[64];
}
*/