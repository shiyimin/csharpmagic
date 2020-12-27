// 源码位置：第二章\ForDemo.cs
using System;

public class ForDemo
{
    public static void Main()
    {
        int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        for (int i = 0; i < numbers.Length; i++)
        {
            int number = numbers[i];
            Console.WriteLine(number);
        }

        foreach (int number in numbers)
        {
            // 不能在foreach语句中更新循环用的变量number
            // number++;
            Console.WriteLine(numbers);
        }
    }
}
