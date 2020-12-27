// 编译方法：csc /debug EventHandleDemo.cs
// 运行方法：
//    * 单进程：`dotnet EventHandleDemo.exe -s -p -l`
//    * 多进程（Windows系统）：
//      * 服务器端：`dotnet EventHandleDemo.exe -s -b -l`
//      * 客户端：`dotnet EventHandleDemo.exe -c -p`
using System;
using System.Threading;

public class EventHandleDemo
{
    private static EventWaitHandle s_notifyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\DemoEvents");
    // private static AutoResetEvent s_notifyEvent = new AutoResetEvent(false);
    // private static ManualResetEvent s_notifyEvent = new ManualResetEvent(false);
    // private static ManualResetEventSlim s_notifyEvent = new ManualResetEventSlim(false);

    public static void Main(string[] args)
    {
        if (args.Length > 0 && string.Compare(args[0], "-s") == 0)
            new Thread(DoNetworkThread).Start();
        
        if (args.Length > 1 && string.Compare(args[1], "-p") == 0)
            new Thread(ProcessDataThread).Start();

        if (args.Length > 2 && string.Compare(args[2], "-l") == 0)
            new Thread(LoadDataThread).Start();
    }

    private static void DoNetworkThread()
    {
        Console.WriteLine("[DoNetworkThread] 执行网络握手协议！");
        Thread.Sleep(new Random().Next(500, 2000));
        s_notifyEvent.WaitOne();
        // ManualResetEventSlim的等待方法是Wait
        // s_notifyEvent.Wait();
        Console.WriteLine("[DoNetworkThread] 通过网络传输数据！");
    }

    private static void ProcessDataThread()
    {
        s_notifyEvent.WaitOne();
        // s_notifyEvent.Wait();
        Console.WriteLine("[ProcessDataThread] 处理数据！");
    }

    private static void LoadDataThread()
    {
        Console.WriteLine("[LoadDataThread]加载数据！");
        Thread.Sleep(new Random().Next(600, 3000));
        // Set返回bool值，操作失败才会返回false，一般不检查
        s_notifyEvent.Set();
        // 如果是ManualResetEvent或ManualResetEventSlim，可以使用一个Set
        s_notifyEvent.Set();
    }
}