// 源码位置：第四章\Logging\TraceSourceDemo.cs
// 编译命令：csc /d:TRACE TraceSourceDemo.cs
using System;
using System.Diagnostics;

public class TraceSourceDemo
{
    static TraceSource s_mts = new TraceSource("MainTrace");
    static TraceSource s_tmts = new TraceSource("TestMethodTrace");
    public static void Main()
    {       
        s_mts.TraceEvent(TraceEventType.Error, 1, "进入Main方法!");
        TestMethod();
        s_mts.TraceEvent(TraceEventType.Information, 2, "离开Main方法!");
    }

    private static void TestMethod()
    {
        s_tmts.TraceEvent(TraceEventType.Error, 1, "进入TestMethod方法!");
        s_tmts.TraceEvent(TraceEventType.Information, 2, "离开TestMethod方法!");
    }
}