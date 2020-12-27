// 源码位置：第三章\GlobalizationDemo.cs
// 编译命令：csc GlobalizationDemo.cs
using System;
using System.Globalization;

public class GlobalizationDemo
{
    static void Main()
    {
        var culture = CultureInfo.CreateSpecificCulture("en-GB");
        var date = DateTime.Parse("1/9/2019", culture);
        // 输出: 2019年9月1日 星期日
        Console.WriteLine(date.ToLongDateString());
        culture = CultureInfo.CreateSpecificCulture("en-US");
        date = DateTime.Parse("1/9/2019", culture);
        // 输出: 2019年1月9日 星期三
        Console.WriteLine(date.ToLongDateString());
        // 输出：1/9/19 12:00:00 AM
        Console.WriteLine(date.ToString(culture));
        // 输出：1/9/19 12:00:00 AM
        Console.WriteLine(date.ToString(culture.DateTimeFormat));
        // 模式: yyyy'-'MM'-'dd'T'HH':'mm':'ss，输出：2019-01-09T00:00:00
        Console.WriteLine(date.ToString(culture.DateTimeFormat.SortableDateTimePattern));
        // 模式: dddd, MMMM d, yyyy，输出: 星期三, 一月 9, 2019
        Console.WriteLine(date.ToString(culture.DateTimeFormat.LongDatePattern));
        Console.WriteLine($"进程的区域设置: {CultureInfo.CurrentCulture.Name}，" + 
            $"UI界面的区域设置：{CultureInfo.CurrentUICulture.Name}");

        // 获取.NET支持的所有区域以及名称
        var cinfo = CultureInfo.GetCultures(
            CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
        foreach (var c in cinfo)
            Console.WriteLine($"Name: {c.DisplayName}, Code: {c.Name}, LCID: {c.LCID}");
            
        date = DateTime.Now;
        var str = date.ToUniversalTime().ToString(
            CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
        Console.WriteLine(str);
        var parsed = DateTime.Parse(str, new CultureInfo("en-US"));
        Console.WriteLine($"1. {date} == {parsed}");
        parsed = DateTime.Parse(str, CultureInfo.CreateSpecificCulture("en-GB"));
        Console.WriteLine($"2. {date} == {parsed}");
        parsed = DateTime.Parse(str, CultureInfo.CreateSpecificCulture("zh-CN"));
        Console.WriteLine($"3. {date} == {parsed}");

        str = date.ToString(CultureInfo.InvariantCulture);
        parsed = DateTime.Parse(str, CultureInfo.InvariantCulture);
        Console.WriteLine($"4. {date} == {parsed}");
        // 下面的代码可以解析，是因为zh-CN刚好跟InvariantCulture兼容
        parsed = DateTime.Parse(str, CultureInfo.CreateSpecificCulture("zh-CN"));
        Console.WriteLine($"5. {date} == {parsed}");
        // 下面的代码会报告解析异常，是因为en-GB刚好跟InvariantCulture不一致
        // parsed = DateTime.Parse(str, CultureInfo.CreateSpecificCulture("en-GB"));

        // 演示IFormatProvider的实现
        str = date.ToString(new DemoCultureInfo());
        Console.WriteLine(str);

        // 使用二进制保存时间和日期
        var ticks = date.Ticks;
        parsed = new DateTime(ticks);
        Console.WriteLine($"6. {date} == {parsed}");
    }

    class DemoCultureInfo : IFormatProvider
    {
        public Object GetFormat(Type formatType)
        {
            Console.WriteLine($"Type: {formatType}");
            if (formatType == typeof(NumberFormatInfo))
                return NumberFormatInfo.CurrentInfo;
            else if (formatType == typeof(DateTimeFormatInfo))
                return DateTimeFormatInfo.CurrentInfo;
            else
                return null;
        }
    }
}