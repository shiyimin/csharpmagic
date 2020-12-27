// 源码位置：第9章\MemoryAccessDemo.cs
// 编译命令：csc /debug MemoryAccessDemo.cs
using System;
using System.Diagnostics;

public class MemoryAccessDemo
{
    static void Main()
    {
        const int SIZE = 20000;
        int[,] matrix = new int[SIZE, SIZE];

        // 访问较快
        var watch = Stopwatch.StartNew();
        for (var row = 0; row < SIZE; ++row) {
            for (var column = 0; column < SIZE; ++column) {
                matrix[row, column] = (row * SIZE) + column;
            }
        }
        Console.WriteLine($"先行后列访问的耗时：{watch.Elapsed}");

        // 访问较慢
        watch = Stopwatch.StartNew();
        for (var column = 0; column < SIZE; ++column) {
            for (var row = 0; row < SIZE; ++row) {
                matrix[row, column] = (row * SIZE) + column;
            }
        }
        Console.WriteLine($"先列后行访问的耗时：{watch.Elapsed}");
    }
}