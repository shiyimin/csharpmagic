using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class CounterDownFindPrimeDemo
{
    static void Main(string[] args)
    {
        var end = int.Parse(args[0]);
        var threadCount = int.Parse(args[1]);
        CountDownVersion(end, threadCount);
    }
    
    static int CountDownVersion(int end, int threadCount)
    {
        var start = DateTime.Now;
        var i = 0;
        var step = end / threadCount;
        var ranges = new Range[threadCount];
        
        for (i = 0 ; i < threadCount - 1; ++i)
            ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
        ranges[i] = new Range() { Begin = i * step + 1, End = end};

        var cde = new CountdownEvent(threadCount);
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
        Console.WriteLine($"Countdown版本耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}");
        return count;
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
    
    class Range
    {
        public int Begin;
        public int End;
        public List<int> Primes = new List<int>();
    }
}