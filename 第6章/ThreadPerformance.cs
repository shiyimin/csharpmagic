using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class ThreadPerformance
{
    const int LOOP_COUNT = 1000;
    static void Main(string[] args)
    {
        // MeasureThreadMemUsage();
        MeasureLockPerformance();
    }

    static void MeasureLockPerformance()
    {
        new Thread(MonitorLockThread).Start();
        new Thread(LocalMutexLockThread).Start();
        new Thread(GlobalMutexLockThread).Start();
    }

    static object s_monitorPerformanceLock = new object();
    static int s_resource = 0;
    static void MonitorLockThread()
    {
        var sw =  Stopwatch.StartNew();
        for (var i = 0; i < LOOP_COUNT; ++i)
        {
            lock(s_monitorPerformanceLock)
            {
                s_resource++;
            }
        }
        Console.WriteLine($"Monitor调用的时间：{sw.Elapsed}");
    }

    static void LocalMutexLockThread()
    {
        var mutex = new Mutex();

        var sw =  Stopwatch.StartNew();
        for (var i = 0; i < LOOP_COUNT; ++i)
        {
            mutex.WaitOne();
            s_resource++;
            mutex.ReleaseMutex();
        }
        Console.WriteLine($"进程内Mutex调用的时间：{sw.Elapsed}");
    }
    
    static void GlobalMutexLockThread()
    {
        var mutex = new Mutex(false, "Global\\MutexExample", out bool mutexWasCreated);

        var sw =  Stopwatch.StartNew();
        for (var i = 0; i < LOOP_COUNT; ++i)
        {
            mutex.WaitOne();
            s_resource++;
            mutex.ReleaseMutex();
        }
        Console.WriteLine($"进程外Mutex调用的时间：{sw.Elapsed}");
    }

    static void MeasureThreadMemUsage()
    {
        var threads = new List<Thread>();

        while (true)
        {
            var command = Console.ReadLine().Trim();
            if (string.Compare(command, "0") == 0)
            {
                foreach (var thread in threads) thread.Abort();
                break;
            }
            else if (string.Compare(command, "+") == 0)
            {
                var thread = new Thread(DummyThread);
                threads.Add(thread);
                thread.Start();
           
                var process = Process.GetCurrentProcess();
                var mem = process.PrivateMemorySize64;
                Console.WriteLine($"启动一个新线程，进程使用的内存数：{mem}");
            }
        }
    }

    static void DummyThread()
    {
        var random = new Random();
        while (true) Thread.Sleep(random.Next(200, 1000));
    }
}