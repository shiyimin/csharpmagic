using System;
using System.Threading;

public class LockBasic
{
    private static int _count;
    private static object _countLock = new object();
    private static int _loopCount;

    public static void Main(string[] args)
    {
        _loopCount = int.Parse(args[0]);

        var thread1 = new Thread(IncrementNoLock);
        var thread2 = new Thread(IncrementNoLock);

        var start = DateTime.Now;
        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
        var elapse = DateTime.Now - start;
        Console.WriteLine($"两个线程不同步累加的结果：{_count}，耗时：{elapse.TotalMilliseconds / 1000}s。");

        _count = 0;
        thread1 = new Thread(IncrementLocked);
        thread2 = new Thread(IncrementLocked);

        start = DateTime.Now;
        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
        elapse = DateTime.Now - start;
        Console.WriteLine($"两个线程使用lock子句累加的结果：{_count}，耗时：{elapse.TotalMilliseconds / 1000}s。");


        _count = 0;
        thread1 = new Thread(IncrementMonitor);
        thread2 = new Thread(IncrementMonitor);

        start = DateTime.Now;
        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
        elapse = DateTime.Now - start;
        Console.WriteLine($"两个线程使用Monitor累加的结果：{_count}，耗时：{elapse.TotalMilliseconds / 1000}s。");


        _count = 0;
        thread1 = new Thread(InterlockedIncrement);
        thread2 = new Thread(InterlockedIncrement);

        start = DateTime.Now;
        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
        elapse = DateTime.Now - start;
        Console.WriteLine($"两个线程使用Interlocked累加的结果：{_count}，耗时：{elapse.TotalMilliseconds / 1000}s。");
        
        _count = 0;
        start = DateTime.Now;
        for (var i = 0; i < 2; ++i) IncrementNoLock();
        elapse = DateTime.Now - start;
        Console.WriteLine($"单线程累加的结果：{_count}，耗时：{elapse.TotalMilliseconds / 1000}s。");
    }

    static void IncrementNoLock()
    {
        for (var i = 0; i < _loopCount; ++i)
            _count++;
    }

    static void IncrementLocked()
    {
        for (var i = 0; i < _loopCount; ++i)
        {
            lock (_countLock)
            {
                _count++;
            }
        }
    }

    static void IncrementMonitor()
    {
        for (var i = 0; i < _loopCount; ++i)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(_countLock, ref lockTaken);
                _count++;
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_countLock);
            }
        }
    }

    static void InterlockedIncrement()
    {
        for (var i = 0; i < _loopCount; ++i)
        {
            var ret = Interlocked.Increment(ref _count);
            Console.WriteLine($"ret: {ret}, _count: {_count}");
        }
    }
}