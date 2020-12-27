// 源码位置：第四章/Configuration/NetFx/ConfigurationDemo.cs
// 编译：csc /debug ConfigurationDemo.cs
// 运行：
//    Windows: 直接运行ConfigurationDemo.exe
//    Mac: 执行 mono ConfigurationDemo.exe
using System;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;

public class Program
{
    static void Main()
    {
        Console.WriteLine("配置：" + ConfigurationManager.AppSettings["Demo"]);
        Console.WriteLine("数据库链接：" + ConfigurationManager.ConnectionStrings["DemoDb"].ConnectionString);

        IDictionary sampleSection = ConfigurationManager.GetSection("sampleSection") as IDictionary;
        Console.WriteLine("自定义配置节：" + (string)sampleSection["ApplicationTitle"]);

        DemoSection demoSection = ConfigurationManager.GetSection("demoSection") as DemoSection;
        Console.WriteLine("自定义面向对象配置节：" + demoSection.StringValue);
        Console.WriteLine("TimeSpanValue: " + demoSection.TimeSpanValue);

        NameValueCollection collection = (NameValueCollection)ConfigurationManager.GetSection("CSharpMagic/utilitySection");
        string title = collection["ApplicationTitle"];
        string connectString = collection["ConnectionString"];
        Console.WriteLine("自定义配置节组，t：" + title + ", c: " + connectString);
    }
}

public class DemoSection : ConfigurationSection
{
    [ConfigurationProperty("stringValue", IsRequired=true)]
    public string StringValue
    {
        get { return (string)base["stringValue"]; }
    }

    [ConfigurationProperty("boolValue")]
    public bool BooleanValue
    {
        get { return (bool)base["boolValue"]; }
    }

    [ConfigurationProperty("timeSpanValue")]
    public TimeSpan TimeSpanValue
    {
        get { return (TimeSpan)base["timeSpanValue"]; }
    }
}