using System;
using System.Threading;

public class ThreadLocalStorageDemo
{
    [ThreadStatic]
    static int s_intval_tls = 123456;
    
    static int s_intval_shared;

    static void Main(string[] args)
    {
        var thread1 = new Thread(ThreadFunc);
        var thread2 = new Thread(ThreadFunc);

        thread1.Start(1000000);
        thread2.Start(200000);
        
        thread1.Join();
        thread2.Join();

        Console.WriteLine($"[main]: s_intval_tls = {s_intval_tls}");

        using (var tls = new ThreadLocal<int>(() => 10))
        {
            thread1 = new Thread(ThreadLocalDemoFunc);
            thread2 = new Thread(ThreadLocalDemoFunc);

            thread1.Start(tls);
            thread2.Start(tls);
            
            thread1.Join();
            thread2.Join();
        }
    }

    static void ThreadLocalDemoFunc(object state)
    {
        var tls = (ThreadLocal<int>)state;
        tls.Value++;
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: tls = {tls.Value}");
    }

    static void ThreadFunc(object state)
    {
        int round = (int)state;
        for (var i = 0; i < round; ++i) {
            s_intval_tls++;
            s_intval_shared++;
        }

        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: s_intval_tls = {s_intval_tls}, s_intval_shared: {s_intval_shared}");
    }
}