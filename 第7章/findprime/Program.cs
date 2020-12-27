// 源码位置：第9章/findprime/Program.cs
// 运行方式：dotnet run 1000000 8
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace findprime
{
    class Range
    {
        public int Begin;
        public int End;
        public List<int> Primes = new List<int>();
    }

    public class FindPrimeDemo
    {
        const int MB = 1024 * 1024;

        async static Task Main(string[] args)
        // static void Main(string[] args)
        {
            var end = int.Parse(args[0]);
            var taskCount = int.Parse(args[1]);
    
            // Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            // var start = DateTime.Now;
            // var count = SingleThreadVersion(end);
            // var elapse = DateTime.Now - start;
            // Console.WriteLine($"单线程耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");

            // Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            // var start = DateTime.Now;
            // var count = ParallelVersion(end, taskCount);
            // var elapse = DateTime.Now - start;
            // Console.WriteLine($"{taskCount}任务并行耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");

            Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            var start = DateTime.Now;
            var count = await AsyncAwaitVersion(end, taskCount);
            var elapse = DateTime.Now - start;
            Console.WriteLine($"{taskCount}线程async await方式并行耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");

            Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            start = DateTime.Now;
            count = await AsyncAwaitAllVersion(end, taskCount);
            elapse = DateTime.Now - start;
            Console.WriteLine($"{taskCount}线程async await方式并行耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");

            // Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            // start = DateTime.Now;
            // count = MultiThreadJoinVersion(end, taskCount);
            // elapse = DateTime.Now - start;
            // Console.WriteLine($"{taskCount}线程Join耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            
            // Console.WriteLine($"运行计算质数之前, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            // start = DateTime.Now;
            // count = ParallelForVersion(end);
            // elapse = DateTime.Now - start;
            // Console.WriteLine($"ParallelForVersion耗时：{elapse.TotalMilliseconds / 1000}，质数个数：{count}, 使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
        }

        async static Task<int> AsyncAwaitVersion(int end, int taskCount)
        {
            var i = 0;
            var step = end / taskCount;
            var ranges = new Range[taskCount];
            
            for (i = 0 ; i < taskCount - 1; ++i)
                ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
            ranges[i] = new Range() { Begin = i * step + 1, End = end};

            for (i = 0 ; i < taskCount; ++i) {
                await Task.Run(() => {
                    FindPrime(ranges[i]);
                });
            }

            Console.WriteLine($"AsyncAwaitVersion使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            return ranges.Sum(r => r.Primes.Count);
        }
        
        async static Task<int> AsyncAwaitAllVersion(int end, int taskCount)
        {
            var i = 0;
            var step = end / taskCount;
            var ranges = new Range[taskCount];
            var tasks = new Task[taskCount];
            
            for (i = 0 ; i < taskCount - 1; ++i)
                ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
            ranges[i] = new Range() { Begin = i * step + 1, End = end};

            for (i = 0 ; i < taskCount; ++i) {
                var state = i;
                tasks[i] = Task.Run(() => {
                    FindPrime(ranges[state]);
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine($"AsyncAwaitVersion使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            return ranges.Sum(r => r.Primes.Count);
        }

        static int ParallelVersion(int end, int taskCount)
        {
            var i = 0;
            var step = end / taskCount;
            var ranges = new Range[taskCount];
            var tasks = new Task[taskCount];
            
            for (i = 0 ; i < taskCount - 1; ++i)
                ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
            ranges[i] = new Range() { Begin = i * step + 1, End = end};

            for (i = 0 ; i < taskCount; ++i) {
                var state = i;   
                tasks[i] = Task.Run(() => {
                    FindPrime(ranges[state]);
                });
            }

            Console.WriteLine($"ParallelVersion使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
            Task.WaitAll(tasks);
            return ranges.Sum(r => r.Primes.Count);
        }

        // static int SingleThreadVersion(int end)
        // {
        //     var range = new Range() { Begin = 1, End = end};
        //     FindPrime(range);
        //     Console.WriteLine($"SingleThreadVersion使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
        //     return range.Primes.Count;
        // }

        // static int MultiThreadJoinVersion(int end, int threadCount)
        // {
        //     var i = 0;
        //     var step = end / threadCount;
        //     var threads = new Thread[threadCount];
        //     var ranges = new Range[threadCount];
            
        //     for (; i < threadCount; ++i) threads[i] = new Thread(FindPrime);
        //     for (i = 0 ; i < threadCount - 1; ++i)
        //         ranges[i] = new Range() { Begin = i * step + 1, End = (i + 1) * step};
        //     ranges[i] = new Range() { Begin = i * step + 1, End = end};

        //     for (i = 0 ; i < threadCount; ++i) threads[i].Start(ranges[i]);
        //     Console.WriteLine($"MultiThreadJoinVersion使用物理内存：{Process.GetCurrentProcess().WorkingSet64 / MB} MB");
        //     for (i = 0; i < threadCount; ++i) threads[i].Join();

        //     return ranges.Sum(r => r.Primes.Count);
        // }

        // static void FindPrime(object state)
        // {
        //     var range = state as Range;
        //     for (var number = range.Begin; number <= range.End; ++number)
        //     {
        //         if (number < 2) continue;

        //         var j = 2;
        //         var isPrime = true;
        //         while (j <= number / 2)
        //         {
        //             if (number % j == 0)
        //             {
        //                 isPrime = false;
        //                 break;
        //             }

        //             j++;
        //         }
                
        //         if (isPrime)
        //             range.Primes.Add(number);
        //     }
        // }
        
        static void FindPrime(object state)
        {
            var range = state as Range;
            for (var number = range.Begin; number <= range.End; ++number)
            {
                if (FindPrimeForSingleNumber(number))
                    range.Primes.Add(number);
            }
        }

        // static int ParallelForVersion(int end)
        // {
        //     var primeCount = 0;
        //     Parallel.For(0, end + 1, number => {
        //         if (FindPrimeForSingleNumber(number)) {
        //             Interlocked.Increment(ref primeCount);
        //         }
        //     });

        //     return primeCount;
        // }

        static bool FindPrimeForSingleNumber(int number)
        {
            if (number < 2) return false;

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
            
            return isPrime;
        }
    }
}
