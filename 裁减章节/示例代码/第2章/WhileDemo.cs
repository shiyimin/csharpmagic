// 源码位置：第二章\whiledemo.cs
using System;

public class WhileDemo
{
    public static void Main()
    {
        int i = 0;
        while (i < 10)
        {
            Console.WriteLine(i);
            i++;
        }

        i = 0;
        do
        {
            Console.WriteLine(i);
            i++;
        } while (i < 10);
    }
}
