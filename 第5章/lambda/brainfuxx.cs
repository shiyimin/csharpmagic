using System;

public class Program
{
    public static void Main(string[] args)
    {
        Func<dynamic, dynamic> I = x => x;
        Console.WriteLine(I(5));
        Console.WriteLine(I(I)(5));
        Console.WriteLine(I(I));

        /*
        var NO = new Func<dynamic, Func<dynamic, dynamic>>(f => x => x);
        var succ = new Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>>(
            n => f => x => f(n(f)(x)));

        Func<dynamic, uint> toInt = x => x(new Func<dynamic, dynamic>(i => i + 1))(0u);
        Func<uint, dynamic> fromInt = null;

        fromInt = n => (n == 0 ? NO : succ(fromInt(n - 1)));

        var plus = new Func<dynamic, Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>>>(
            m => n => f => x => m(f)(n(f)(x)));

        var prev = NO;
        var curr = succ(NO);

        while (true) 
        {
            Console.WriteLine(toInt(curr));
            var t = curr;
            curr = plus(curr)(prev);
            prev = t;
        }
        */
    }
}