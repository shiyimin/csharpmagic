using System;
using System.IO;
using System.Net;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace ResizeImage
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.CreateHttp(
                "https://cn.bing.com//az/hprichbg/rb/GoldenEagle_EN-CN5621882775_1920x1080.jpg");
            var response = request.GetResponse();
            using (var inputStream = response.GetResponseStream())
            // using (FileStream inputStream = File.OpenRead("BingFeedImage_1920x1080.jpg"))
            {
                // using (FileStream outputStream = File.OpenWrite("Resized.jpg"))
                using (FileStream outputStream = File.Open("Resized.jpg", FileMode.OpenOrCreate))
                // using (MemoryStream outputStream = new MemoryStream()) 
                {
                    var img = Image.Load(inputStream, out IImageFormat format);
                    img.Mutate(data =>
                               data.Resize(img.Width / 2, img.Height / 2)
                               .Grayscale());
                    img.Save(outputStream, format);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    Console.WriteLine(outputStream.ReadByte());
                }
            }
        }
    }
}
