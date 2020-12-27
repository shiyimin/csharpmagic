// 源码位置：第二章\DeconstructDemo.cs
// 编译命令：csc DeconstructDemo.cs
using System;

public class DeconstructDemo
{
    static void Main()
    {
        (string name1, int age1) = ReturnTuple();
        Console.WriteLine($"{name1}，{age1}");

        var (name2, age2) = ReturnTuple();
        Console.WriteLine($"{name2}，{age2}");

        (string name3, var age3) = ReturnTuple();
        Console.WriteLine($"{name3}，{age3}");

        string name4 = null;
        int age4 = 0;
        (name4, age4) = ReturnTuple();
        Console.WriteLine($"{name4}，{age4}");

        var (name5, _) = ReturnTuple();
        Console.WriteLine($"{name5}");

        Coordinate coord = new Coordinate()
        {
            Latitude = 100.0,
            Longitude = 150.0
        };
        var (lat, lon) = coord;
        Console.WriteLine($"{lat} - {lon}");

        var (lat1, _) = coord;
        Console.WriteLine($"{lat1}");

        (_, _, var latlon) = coord;
        Console.WriteLine(latlon);
    }

    static (string name, int age) ReturnTuple()
    {
        // return (name: "张三", age: 30);
        return ("张三", 30);
    }
}

public class Coordinate
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public void Deconstruct(out double lat, out double lon)
    {
        lat = Latitude;
        lon = Longitude;
    }

    public void Deconstruct(out double lat, out double lon, out string coord)
    {
        lat = Latitude;
        lon = Longitude;
        coord = string.Format($"({Latitude},{Longitude})");
    }
}
/*
 * 以下这个扩展方法与60行的Coordinate.Deconstruct的作用一样
 * 两个方法不能同时存在，使用扩展方法主要是为了给历史遗留类型添加新功能用的
 * 新的类型不建议使用这个方法
public static class Extensions
{
    public static void Deconstruct(this Coordinate coord, 
        out double lat, out double lon, out string str)
    {
        lat = coord.Latitude;
        lon = coord.Longitude;
        str = string.Format($"({coord.Latitude},{coord.Longitude})");
    }
}
*/