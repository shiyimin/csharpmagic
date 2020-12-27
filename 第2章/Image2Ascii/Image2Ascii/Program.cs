// 源码位置：\第三章\Image2Ascii\Image2Ascii\Program.cs
// 使用Visual Studio IDE编译
using System;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Image2Ascii
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: Image2Ascii image width height");
                return;
            }

            Console.WriteLine(
                Convert(args[0], int.Parse(args[1]), int.Parse(args[2])));
        }

        static string _ASCIICharacters = "##@%=+*:-. ";

        static string Convert(string file, int width, int height)
        {
            var img = Image.Load(Path.GetFullPath(file));
            width = Math.Min(width, img.Width);
            height = Math.Min(height * img.Height / img.Width, img.Height);

            img.Mutate(data =>
                       data.Resize(width, height).Grayscale());
            var sb = new StringBuilder();
            for (var h = 0; h < height; ++h)
            {
                for (var w = 0; w < width; ++w)
                {
                    var pixel = img[w, h];
                    var idx = pixel.R * _ASCIICharacters.Length / 255;
                    idx = Math.Max(0, Math.Min(_ASCIICharacters.Length - 1, idx));
                    var c = _ASCIICharacters[idx];
                    sb.Append(c);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
