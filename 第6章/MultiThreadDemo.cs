using System;
using System.Threading;

public class MultiThreadDemo
{
    public static void Main()
    {
        var thread1 = new Thread(ThreadFunc1);
        var thread2 = new Thread(ThreadFunc2);

        thread1.IsBackground = true;
        thread1.Start();
        thread2.Start(123);

        Console.WriteLine("结束主线程！");

        thread1.Join();
        thread2.Join();
        
        ThreadPool.QueueUserWorkItem(data => ThreadFunc1());
        ThreadPool.QueueUserWorkItem(_ => ThreadFunc1());
        ThreadPool.QueueUserWorkItem(ThreadFunc2, 123);
        Console.WriteLine("按任意键结束主线程！");
        Console.ReadLine();

        var _ = "discards";
        Console.WriteLine(_);

        // _ = 123;
    }

    static void ThreadFunc1()
    {
        Thread.Sleep(1000);
        Console.WriteLine("在无参线程中！");
    }

    static void ThreadFunc2(object state)
    {
        Console.WriteLine($"在有参线程中，参数是{state}！");
    }
}