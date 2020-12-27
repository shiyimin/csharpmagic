using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using X = System.Double;
using Y = System.Double;
using Z = System.Double;

public static class PartialFunctionExtensions
{
    public static Func<T2, R> Bind<T1, T2, R>(this Func<T1, T2, R> func, T1 t1) 
        => t2 => func(t1, t2);

    public static Func<T2, T3, R> Bind<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 t1) 
        => (t2, t3) => func(t1, t2, t3);

    public static Func<T1, T2, R> Bind3rdArg<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T3 t3) 
        => (t1, t2) => func(t1, t2, t3);

    public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
        => t1 => t2 => t3 => func(t1, t2, t3);
}

public class FunctionalProgrammingBasic
{
    static void Main()
    {
        // FuncBasic();
        // var (year, month, day) = Today();
        // WriteLine($"{year}-{month}-{day}");
        PartialFunctionDemo();
    }

    static (int year, int month, int day) Today() {
        var now = DateTime.Now;
        return (now.Year, now.Month, now.Day);
    }

    static void FuncBasic()
    {
        int add(int a, int b) => a + b;
        var array = new List<int>() { 1, 2, 3 };
        var sum = array.Aggregate(add);
        array.ForEach(WriteLine);
        // Console.WriteLine($"{sum}");
        WriteLine($"{sum}");

        Func<int, int> addOne = n => add(1, n);
        array.Select(addOne).ToList().ForEach(WriteLine);
        WriteLine(addOne(5));

        Func<int, int> addNum(int i) => n => add(i, n);
        array.Select(addNum(1)).ToList().ForEach(WriteLine);
        WriteLine(addNum(1)(5));
    }

    static double Distance(X x, Y y, Z z)
    {
        return Math.Sqrt(x * x + y * y + z * z);
    }

    static void PartialFunctionDemo()
    {
        Func<double, double, double> pow = Math.Pow;
        var binaryPow = pow.Bind(2);

        WriteLine($"{binaryPow(10)}");

        Func<X, Y, Z, double> distance3D = Distance;
        var distance2D = distance3D.Bind3rdArg(0);
        WriteLine($"{distance2D(1, 2)}");

        var curried = distance3D.Curry();
        Func<X, Y, double> distance2D_c = (x, y) => curried(x)(y)(0);
        WriteLine($"{distance2D_c(1, 2)}");
    }
}