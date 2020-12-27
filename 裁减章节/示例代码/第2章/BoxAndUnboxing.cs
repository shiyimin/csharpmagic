// 源码：第二章\BoxAndUnboxing.cs
using System;

public class BoxAndUnboxing
{
	public static void Main()
	{
        int foo = 42; // 值类型
        // 将值类型赋值给引用类型，引发装箱
        object bar = foo;

        // 修改foo的值不会影响bar的值
        foo = 43;
        Console.WriteLine(bar + "," + foo);

        // 拆箱回值类型
        int oof = (int)bar;
        // 隐含两个装箱，一个拆箱操作
        Console.WriteLine(oof + "," + (int)bar);


        object[] arr = new object[] { foo, bar };
        // 没有装箱和拆箱操作
        Console.WriteLine(arr[0] + "," + arr[1]);
	}
}
