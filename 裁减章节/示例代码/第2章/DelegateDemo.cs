// 源码位置：第二章\DelegateDemo.cs
// 编译命令：csc DelegateDemo.cs
using System;

public class DelegateDemo
{
    public delegate bool OrderBy(int left, int right);

    private static void Sort(int[] items, OrderBy order)
    {
        for (var i = 0; i < items.Length; ++i)
        {
            for (var j = i; j < items.Length; ++j)
            {
                if (order(items[i], items[j]))
                {
                    int swap = items[i];
                    items[i] = items[j];
                    items[j] = swap;
                }
            }
        }
    }

    private static bool Asc(int left, int right)
    {
        return left > right;
    }

    private static bool Desc(int left, int right)
    {
        return left < right;
    }

    static void Main(string[] args)
    {
        var items = new int[10];
        var rnd = new Random();
        for (var i = 0; i < items.Length; ++i)
            items[i] = rnd.Next(0, 100);

        // var order = new OrderBy(Asc);
        var order = new OrderBy(Desc);
        Sort(items, order);
        for (var i = 0; i < items.Length; ++i)
            Console.Write($"{items[i]} ");
        Console.WriteLine();
    }
}