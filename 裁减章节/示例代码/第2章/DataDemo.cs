// 源码路径：第二章\DataDemo.cs
// 编译命令：csc DataDemo.cs
using System;

public class DataDemo
{
    const double PI = 3.1415926; // 常量
    // readonly常量，其值不能在编译器确定
    static readonly long ticks = DateTime.Now.Ticks; 

	public static void Main()
	{
        Console.WriteLine($"PI:\t{PI}");
        Console.WriteLine($"ticks:\t{ticks}");

        const int nine = 9;

        // 取消下面的注释，编译器会报告下列编译错误
        // error CS0220: 在 checked 模式下，运算在编译时溢出
        //
        // int i1 = 2147483647 + nine;
        // Console.WriteLine($"i1:\t{i1}");

        int ten = 10;
        int i2 = 2147483647 + ten;
        Console.WriteLine($"i2:\t{i2}");

        int i3 = checked(2147483647 + ten);
        Console.WriteLine($"i3:\t{i3}");

        checked
        {
            int i4 = 2147483647 + ten;
            Console.WriteLine($"i4:\t{i4}");
        }

        // 取消下面的注释，编译器会报告下列编译错误
        // error CS0220: 在 checked 模式下，运算在编译时溢出
        //
        // long l1 = 2147483647 + nine;
        // Console.WriteLine($"l1:\t{l1}");

        long l2 = unchecked(2147483647 + nine);
        Console.WriteLine($"l2:\t{l2}");
    }
}
