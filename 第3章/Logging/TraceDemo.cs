// 源码位置：第四章\Logging\TraceDemo.cs
// 编译命令：csc /d:TRACE TraceDemo.cs
using System;
using System.Diagnostics;

public class TraceDemo
{
    public static void Main()
    {       
        // Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        Trace.Indent();
        Trace.TraceError("Entering Main");
        Console.WriteLine("Hello World.");
        Trace.Unindent();
        Trace.TraceInformation("Exiting Main"); 
    }
}