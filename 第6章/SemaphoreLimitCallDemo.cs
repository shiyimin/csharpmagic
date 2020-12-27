using System;
using System.Threading;
using System.Collections.Generic;

public class SemaphoreLimitCallDemo
{
    const int MAX_CALL_COUNT = 9;
    static SemaphoreSlim s_limitedCallSemaphore = new SemaphoreSlim(MAX_CALL_COUNT);

    static void Main(string[] args)
    {
        var random = new Random();
        var rounds = int.Parse(args[0]);
        var threads = new List<Thread>();

        for (var i = 0; i < rounds; ++i)
        {
            var threadCount = random.Next(5, 30);
            for (var j = 0; j < threadCount; ++j)
            {
                threads.Add(new Thread(LimitedCallThread));
                threads[threads.Count - 1].Start();
            }

            Console.WriteLine($"当前启动了{threadCount}个线程，共启动{threads.Count}线程！");
            Thread.Sleep(random.Next(100, 1000));
        }

        foreach (var thread in threads) thread.Join();
    }

    static void LimitedCallThread()
    {
        s_limitedCallSemaphore.Wait();
        try {
            LimitedCall();
        } finally {
            s_limitedCallSemaphore.Release();
        }
    }

    static int s_currentCalls = 0;
    static object s_calllock = new object();
    static void LimitedCall()
    {
        var callcount = 0;
        lock (s_calllock)
        {
            s_currentCalls++;
            if (s_currentCalls > MAX_CALL_COUNT)
                throw new InvalidOperationException($"当前调用的次数【{s_currentCalls}】大于最大允许的数量【{MAX_CALL_COUNT}】!");
            callcount = s_currentCalls;
        }

        Console.WriteLine($"执行调用，当前是第{callcount}次调用！");
        Thread.Sleep(new Random().Next(500, 2000));
        lock (s_calllock) s_currentCalls--;
    }
}