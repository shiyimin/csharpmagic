using System;
using System.Threading;

public class SemaphoreMolecureDemo
{
    static int s_molecures = 0;
    const int MAX_WATER_MOLECURE_COUNT = 10;
    static SemaphoreSlim s_hydrogenSemaphore = new SemaphoreSlim(1);
    static SemaphoreSlim s_oxygenSemaphore = new SemaphoreSlim(2);
    static object s_lock = new object();

    static void Main()
    {
        var threadCount = 5;
        var hydrogenThreads = new Thread[threadCount * 2];
        var oxygenThreads = new Thread[threadCount];
        for (int i = 0; i < hydrogenThreads.Length; ++i)
            hydrogenThreads[i] = new Thread(HydrogenProductionThread);
        for (int i = 0; i < oxygenThreads.Length; ++i)
            oxygenThreads[i] = new Thread(OxygenProductionThread);

        for (int i = 0; i < hydrogenThreads.Length; ++i) hydrogenThreads[i].Start();
        for (int i = 0; i < oxygenThreads.Length; ++i) oxygenThreads[i].Start();

        for (int i = 0; i < oxygenThreads.Length; ++i)
            oxygenThreads[i].Join();
        
        Console.WriteLine($"总共生产{s_molecures}个水分子！");
        for (int i = 0; i < hydrogenThreads.Length; ++i) hydrogenThreads[i].Abort();
    }

    /*
    static object s_hydrolock = new object();
    static int s_hydroCount = 0;
    */

    static void HydrogenProductionThread()
    {
        while (true)
        {
            Thread.Sleep(new Random().Next(500, 1000));
            s_hydrogenSemaphore.Release();

            /*
            lock (s_hydrolock) 
            {
                s_hydroCount++;
                Console.WriteLine($"    --产生一个氢原子，总计产生{s_hydroCount}个氢原子！");
            }
            */

            s_oxygenSemaphore.Wait();
        }
    }

    /*
    static object s_oxylock = new object();
    static int s_oxyCount = 0;
    */

    static void OxygenProductionThread()
    {
        while (true)
        {
            s_hydrogenSemaphore.Wait();
            s_hydrogenSemaphore.Wait();

            lock (s_lock)
            {
                if (s_molecures >= MAX_WATER_MOLECURE_COUNT) break;
                s_molecures++;
            }

            /*
            lock (s_oxylock)
            {
                s_oxyCount++;
                Console.WriteLine($"  ++产生一个氧原子，总计产生{s_oxyCount}个氧原子！");
            }
            */

            s_oxygenSemaphore.Release();
            s_oxygenSemaphore.Release();
            
            /*
            Console.WriteLine($"[{DateTime.Now}] 产生一个水分子！");
            */
        }
    }
}