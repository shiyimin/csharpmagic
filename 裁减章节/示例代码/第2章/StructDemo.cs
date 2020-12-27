// 源码位置：第二章\StructDemo.cs
// 编译命令：csc StructDemo.cs
using System;

public class StructDemo
{
    static void Main()
    {
        Point point1 = new Point { X = 1, Y = 2},
              point2 = new Point { X = 3, Y = 4};
        point1.Add(point2);
        Console.WriteLine($"结果：{point1.X} - {point1.Y}");

        Point[] points = new Point[10000000];
        DateTime begin = DateTime.Now;
        for (var i = 0; i < points.Length; ++i)
        {
            points[i].X = i;
            points[i].Y = i;
        }
        var peroid = DateTime.Now - begin;
        Console.WriteLine($"耗时：{peroid.TotalMilliseconds}");
        
        begin = DateTime.Now;
        for (var i = 0; i < points.Length; ++i)
        {
            points[i] = new Point(i, i);
        }
        peroid = DateTime.Now - begin;
        Console.WriteLine($"耗时：{peroid.TotalMilliseconds}");
    }
}

public struct Point
{
    // public Point() {}

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;

    public int Y;

    public void Add(Point other)
    {
        X += other.X;
        Y += other.Y;
    }
}