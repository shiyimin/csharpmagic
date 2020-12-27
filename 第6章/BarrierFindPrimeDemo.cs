using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class BarrierFindPrimeDemo
{
    const int STEP = 100000;
    static void Main(string[] args)
    {
        var end = int.Parse(args[0]);
        var threadCount = int.Parse(args[1]);
        BarrierVersion(end, threadCount);
    }
    
    static Barrier s_barrier;
    static int BarrierVersion(int end, int threadCount)
    {
        var start = DateTime.Now;
        var i = 0;
        var step = end / threadCount;
        var ranges = new Range[threadCount];
        
        for (i = 0 ; i < threadCount - 1; ++i)
            ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
        ranges[i] = new Range() { Begin = i * step + 1, End = end};

        var cde = new CountdownEvent(threadCount);
        s_barrier = new Barrier(threadCount);
        for (i = 0 ; i < threadCount; ++i)
        {
            ThreadPool.QueueUserWorkItem(state => {
                FindPrime(state);
                cde.Signal();
            }, ranges[i]);
        }
        cde.Wait();

        var count = ranges.Sum(r => r.Primes.Count);
        var elapse = DateTime.Now - start;
        Console.WriteLine($"Barrier版本耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}");
        return count;
    }
    
    static void FindPrime(object state)
    {
        var start = DateTime.Now;
        TimeSpan elapse;
        var range = state as Range;
        for (var number = range.Begin; number <= range.End; ++number)
        {
            if (number < 2) continue;

            if (number % STEP == 0)
            {
                s_barrier.SignalAndWait();
                elapse = DateTime.Now - start;
                Console.WriteLine($"[{range.Begin}- {range.End}]：当前处理到{number},质数个数：{range.Primes.Count}，耗时：{elapse.TotalMilliseconds / 1000}。");
            }

            var j = 2;
            var isPrime = true;
            while (j <= number / 2)
            {
                if (number % j == 0)
                {
                    isPrime = false;
                    break;
                }

                j++;
            }
            
            if (isPrime)
                range.Primes.Add(number);
        }

        s_barrier.SignalAndWait();
        elapse = DateTime.Now - start;
        Console.WriteLine($"[{range.Begin}- {range.End}]：处理完毕,质数个数：{range.Primes.Count}，耗时：{elapse.TotalMilliseconds / 1000}。");
    }
    
    class Range
    {
        public int Begin;
        public int End;
        public List<int> Primes = new List<int>();
    }
}