// 源码位置：第二章\ArrayDemo.cs
// 编译命令：csc ArrayDemo.cs
using System;

public class ArrayDemo
{
    static void Main(string[] args)
    {
        int[] row = new int[9] {
            1, 2, 3, 4, 5, 6, 7, 8, 9 };
        // int[] row = {
        //     9, 8, 7, 6, 5, 4, 3, 2, 1 };
        int[,] grid = new int[,] {
            { 1, 1, 1, 1, 1 }, // 第一行
            { 2, 2, 2, 2, 2 }  // 第二行
        };
        int[][] jag = new int[][] {
            new int[] {1, 1, 1 },
            new int[] {2, 2 }
        };

        int[] column;
        if (args.Length > 0)
        {
            int len = int.Parse(args[0]);
            column = new int[len];
            column[len - 1] = len;
        }

        Console.WriteLine(
            $"row[0]:{row[0]},rank:{row.Rank},length:{row.Length}.");
        Console.WriteLine(
            $"grid[1,4]:{grid[1,4]},rank:{grid.Rank},length:{grid.Length}.");
        Console.WriteLine(
            $"jag[1][1]:{jag[1][1]},rank:{jag.Rank},length:{jag.Length}.");
    }
}