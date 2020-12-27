// 源码位置：第二章\FloatingPointDemo.cs
// 编译命令：csc FloatingPointDemo.cs
using System;

public class FloatingPointDemo
{
    public static void Main()
    {  
        float a, b, c;
        a = 1.345f;  
        b = 1.123f;  
        c = a + b;  
        Console.WriteLine(c == 2.468f);
        Console.WriteLine(c == 2.468);  
        Console.WriteLine((double)c == 2.468);    

        float z = 0.1f;
        for (int i = 0; i < 10; ++i)
            z += 0.1f;
        Console.WriteLine($"0.1f * 10 == 1.0f? {z == 1.0f}"); 

        Console.WriteLine($"a:\t{ToBinaryString(a)}");
        Console.WriteLine($"b:\t{ToBinaryString(b)}");
        Console.WriteLine($"c:\t{ToBinaryString(c)}");
        Console.WriteLine($"2.468f:\t{ToBinaryString(2.468f)}");
        Console.WriteLine($"c:\t{ToBinaryString((double)c)}");
        Console.WriteLine($"2.468:\t{ToBinaryString(2.468)}");

        double d, e, f;
        d = 1.345d;
        e = 1.123d;
        f = d + e;
        Console.WriteLine(f == 2.468);  
        Console.WriteLine($"d:\t{ToBinaryString(d)}");
        Console.WriteLine($"e:\t{ToBinaryString(e)}");
        Console.WriteLine($"f:\t{ToBinaryString(f)}");
    }

    private static string ToBinaryString(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        string s = ToBinaryString(bytes);
        // 符号位 指数部分 尾数部分
        return s.Substring(0, 1) + " " + s.Substring(1, 11) + " " + s.Substring(12); 
    }

    private static string ToBinaryString(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        string s = ToBinaryString(bytes);
        // 符号位 指数部分 尾数部分
        return s.Substring(0, 1) + " " + s.Substring(1, 8) + " " + s.Substring(9); 
    }

    private static string ToBinaryString(byte[] bytes)
    {  
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (Byte b in bytes)
        {
            for (int i = 0; i < 8; i++)
            {
                builder.Insert(0, ((b >> i) & 1) == 1 ? "1" : "0");
            }
        }
        
        return builder.ToString();
    }
}