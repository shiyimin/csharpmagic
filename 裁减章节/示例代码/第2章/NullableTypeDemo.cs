// 源码位置：第二章\NullableTypeDemo.cs
// 编译命令：csc NullableTypeDemo.cs
using System;

public class NullableTypeDemo
{
    static void Main()
    {
        int? x1 = null;
        int?[] arr = new int?[10];
        Nullable<int> x2 = null;
        Console.WriteLine($"{x1.HasValue} - {x2.HasValue}");
        Console.WriteLine($"{x1 == null} - {x2 == null}");
        // Console.WriteLine($"{x1.GetType()}");
        // Console.WriteLine($"{x2.GetType()}");
        int i = x1 ?? default(int);

        x1 = x2 = arr[0] = 10;
        var x3 = x1 + x2;
        Console.WriteLine($"{typeof(int?)} - {typeof(Nullable<int>)}");
        Console.WriteLine(
            $"{x1.GetType()} - {x2.GetType()} - {x3.GetType()}");
        i = (int)x1;
        Console.WriteLine($"{i}");
    }
}