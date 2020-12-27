// 源码位置：第三章\UnicodeDemo.cs
// 编译命令：csc UnicodeDemo.cs
using System;
using System.Text;

class UnicodeDemo
{
    static void Main()
    {
        var emoji = "\uD83D\uDE42";
        Console.WriteLine(emoji);
        Console.WriteLine(GetUnicodeString(emoji));
        var x = "🙂";
        Console.WriteLine(GetUnicodeString(x));
        Console.WriteLine("Unicode - UTF16");
        var bytes = Encoding.Unicode.GetBytes(x);
        foreach (var b in bytes) Console.Write("{0:x2} ", b);

        Console.WriteLine("\nUTF8");
        bytes = Encoding.UTF8.GetBytes(x);
        foreach (var b in bytes) Console.Write("{0:x2} ", b);

        Console.WriteLine("\nUTF32");
        bytes = Encoding.UTF32.GetBytes(x);
        foreach (var b in bytes) Console.Write("{0:x2} ", b);
        Console.WriteLine();
    }

    static string GetUnicodeString(string s)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in s)
        {
            sb.Append("\\u");
            sb.Append(String.Format("{0:x4}", (int)c));
        }
        return sb.ToString();
    }
}