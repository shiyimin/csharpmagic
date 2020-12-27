using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class FalseSharingDemo
{
    private static DataStructureBad[] s_data_bad;
    private static DataStructureGood[] s_data_good;
    private const long ITERATIONS = 1000 * 1000 * 1000;
    public static void Main()
    {
        s_data_bad = new DataStructureBad[2];
        s_data_good = new DataStructureGood[2];

        var watch = Stopwatch.StartNew();
        var tasks = new Task[2];
        tasks[0] = Task.Run(() => WorkForBad(0));
        tasks[1] = Task.Run(() => WorkForBad(1));
        Task.WaitAll(tasks);
        Console.WriteLine($"DataStructureBad数据结构的处理时间：{watch.Elapsed}, 数据：[0] = {s_data_bad[0].value}, [1] = {s_data_bad[1].value}。");

        watch = Stopwatch.StartNew();
        tasks = new Task[2];
        tasks[0] = Task.Run(() => WorkForGood(0));
        tasks[1] = Task.Run(() => WorkForGood(1));
        Task.WaitAll(tasks);
        Console.WriteLine($"DataStructureGood数据结构的处理时间：{watch.Elapsed}, 数据：[0] = {s_data_good[0].value}, [1] = {s_data_good[1].value}。");

        watch = Stopwatch.StartNew();
        FalseSharingRandom();
        Console.WriteLine($"存在伪共享问题的随机赋值方法运行时间：{watch.Elapsed}。");

        watch = Stopwatch.StartNew();
        WithoutFalseSharingRandom();
        Console.WriteLine($"并行随机赋值方法运行时间：{watch.Elapsed}。");
    }

    private static void FalseSharingRandom()
    {
        Random rnd1 = new Random(), rnd2 = new Random();
        int[] result1 = new int[20000000], result2 = new int[20000000];
        Parallel.Invoke(
            () => {
                for (int i = 0; i < result1.Length * 10; ++i)
                    result1[i % 10] = rnd1.Next();
            },
            () => {
                for (int i = 0; i < result2.Length * 10; ++i)
                    result2[i % 10] = rnd2.Next();
            }
        );
    }

    private static void WithoutFalseSharingRandom()
    {
        int[] result1, result2;
        Parallel.Invoke(
            () => {
                Random rnd1 = new Random();
                result1 = new int[20000000];
                for (int i = 0; i < result1.Length * 10; ++i)
                    result1[i % 10] = rnd1.Next();
            },
            () => {
                Random rnd2 = new Random();
                result2 = new int[20000000];
                for (int i = 0; i < result2.Length * 10; ++i)
                    result2[i % 10] = rnd2.Next();
            }
        );
    }

    private static void WorkForBad(int index)
    {
        for (var i = 0; i < ITERATIONS; ++i) {
            s_data_bad[index].value++;
        }
    }

    private static void WorkForGood(int index)
    {
        for (var i = 0; i < ITERATIONS; ++i) {
            s_data_good[index].value++;
        }
    }

    public struct DataStructureBad
    {
        public long value;
    }

    public struct DataStructureGood
    {
        public long value;

        public long p1, p2, p3, p4, p5, p6, p7; 
    }
}