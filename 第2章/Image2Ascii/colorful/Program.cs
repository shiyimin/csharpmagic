// 源码位置：第三章\Image2Ascii\colorful\Program.cs
// 使用Visual Studio IDE编译
using System;

namespace colorful
{
    class Program
    {
        static string _ASCIICharacters = "#@%=+*:-. ";
        static void Main(string[] args)
        {
            var originColor = Console.ForegroundColor;
            var colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            char c = (char)Console.Read();
            while (c > 0 && c < 255)
            {
                var idx = _ASCIICharacters.IndexOf(c);
                if (idx >= 0)
                    Console.ForegroundColor = (ConsoleColor)(colors[colors.Length - idx - 1]);
                else
                    Console.ForegroundColor = originColor;
                Console.Write(c);

                c = (char)Console.Read();
            }
        }
    }
}
