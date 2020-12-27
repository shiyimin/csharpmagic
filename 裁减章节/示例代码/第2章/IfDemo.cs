// 源码位置：第二章\IfDemo.cs
using System;

public class IfDemo
{
    public static void Main()
    {
        int input = Console.Read();
        int value = input - '0';

        if (value % 2 == 0) Console.WriteLine("偶数！");
        else if (value % 3 == 0) Console.WriteLine("3的倍数！");
        else if (value == 9) Console.WriteLine("9！");
        else if (value == 0) Console.WriteLine("输入了0！");
        else Console.WriteLine("奇数");

        // 缩进不匹配是非常危险的
        // 下面的else其实是匹配第二个if
        if (value >= 0 && value <= 10)
            if (value > 5)
                Console.WriteLine("及格");
        else 
            Console.WriteLine("超出成绩范围！");
    }
}
