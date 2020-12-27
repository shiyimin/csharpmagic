using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class BlockWaitingDemo
{
    class Range
    {
        public int Begin;
        public int End;
        public List<int> Primes = new List<int>();
    }

    static void Main(string[] args)
    {
        var end = int.Parse(args[0]);
        var start = DateTime.Now;
        var count = SingleThreadVersion(end);
        var elapse = DateTime.Now - start;
        var singleThreadElapse = elapse;
        Console.WriteLine($"单线程耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}");
        
        var threadCount = int.Parse(args[1]);
        start = DateTime.Now;
        count = MultiThreadJoinVersion(end, threadCount);
        elapse = DateTime.Now - start;
        Console.WriteLine($"{threadCount}线程Join耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}");

        start = DateTime.Now;
        count = MultiThreadSleepVersion(end, threadCount, (int)singleThreadElapse.TotalMilliseconds);
        elapse = DateTime.Now - start;
        Console.WriteLine($"{threadCount}线程Sleep耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}");
    }

    static int SingleThreadVersion(int end)
    {
        var range = new Range() { Begin = 1, End = end};
        FindPrime(range);
        return range.Primes.Count;
    }

    static int MultiThreadJoinVersion(int end, int threadCount)
    {
        var i = 0;
        var step = end / threadCount;
        var threads = new Thread[threadCount];
        var ranges = new Range[threadCount];
        
        for (; i < threadCount; ++i) threads[i] = new Thread(FindPrime);
        for (i = 0 ; i < threadCount - 1; ++i)
            ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
        ranges[i] = new Range() { Begin = i * step + 1, End = end};

        for (i = 0 ; i < threadCount; ++i) threads[i].Start(ranges[i]);
        for (i = 0; i < threadCount; ++i) threads[i].Join();

        return ranges.Sum(r => r.Primes.Count);
    }

    static int MultiThreadSleepVersion(int end, int threadCount, int waitTimeout)
    {
        var i = 0;
        var step = end / threadCount;
        var threads = new Thread[threadCount];
        var ranges = new Range[threadCount];
        
        for (; i < threadCount; ++i) threads[i] = new Thread(FindPrime);
        for (i = 0 ; i < threadCount - 1; ++i)
            ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
        ranges[i] = new Range() { Begin = i * step + 1, End = end};

        for (i = 0 ; i < threadCount; ++i) threads[i].Start(ranges[i]);
        Thread.Sleep(waitTimeout);

        return ranges.Sum(r => r.Primes.Count);
    }

    static void FindPrime(object state)
    {
        var range = state as Range;
        for (var number = range.Begin; number <= range.End; ++number)
        {
            if (number < 2) continue;

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
    }
}