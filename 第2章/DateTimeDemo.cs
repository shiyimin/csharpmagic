// 源码位置：第三章\DateTimeDemo.cs
// 编译命令：csc DateTimeDemo.cs
using System;
using System.Globalization;

public class DateTimeDemo
{
    static void Main()
    {
        var date = new DateTime(2019, 1, 20, 17, 18, 20);
        Console.WriteLine($"ticks: {date.Ticks}, oadate: {date.ToOADate()}," +
            $" unix: {ToUnixTimestamp(date)}, file: {date.ToFileTime()}");

        // 获取所有的时区信息
        foreach (var z in TimeZoneInfo.GetSystemTimeZones())
            Console.WriteLine($"{z.Id}: {z.DisplayName}");

        // 北京时间
        TimeZoneInfo bjtz = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
        // 在macOS下运行需要改成下面的代码
        // TimeZoneInfo bjtz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
        // 微软总部西雅图时间
        TimeZoneInfo mstz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        // 在macOS下运行需要改成下面的代码
        // TimeZoneInfo mstz = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
        // var date = DateTime.UtcNow;
        date = new DateTime(2019, 1, 25).ToUniversalTime();
        var bjtime = TimeZoneInfo.ConvertTimeFromUtc(date, bjtz);
        Console.WriteLine($"北京时间：{bjtime}");
        var mstime = TimeZoneInfo.ConvertTimeFromUtc(date, mstz);
        Console.WriteLine($"微软时间：{mstime}");
        
        date = new DateTime(2019, 6, 25).ToUniversalTime();
        bjtime = TimeZoneInfo.ConvertTimeFromUtc(date, bjtz);
        Console.WriteLine($"北京时间：{bjtime}");
        mstime = TimeZoneInfo.ConvertTimeFromUtc(date, mstz);
        Console.WriteLine($"微软时间：{mstime}");

        date = new DateTime(2019, 2, 5);
        var 公历 = new GregorianCalendar();
        var 农历 = new ChineseLunisolarCalendar();

        var 干支 = 农历.GetSexagenaryYear(date);
        var 天干 = (CelestialStem)农历.GetCelestialStem(干支);
        var 地支 = (TerrestrialBranch)农历.GetTerrestrialBranch(干支);
        Console.WriteLine(
            $"公历：{公历.GetMonth(date)}，农历：{农历.GetMonth(date)}" +
            $"，干支：{天干}{地支}");
    }

    static double ToUnixTimestamp(DateTime value)
    {
        value = value.ToUniversalTime();
        return value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static DateTime FromUnixTimestamp(double value)
    {
        var date = new DateTime(1970, 1, 1);
        var utc = date.AddSeconds(value);
        return utc.ToLocalTime();
    }
}

// 天干
enum CelestialStem
{
    甲 = 1, 乙, 丙, 丁, 戊,
    己, 庚, 辛, 壬, 癸
}

// 地支
enum TerrestrialBranch
{
    子 = 1, 丑, 寅, 卯, 辰, 巳,
    午, 未, 申, 酉, 戌, 亥
}