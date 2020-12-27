using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MMapDemo
{
    class SetPixelProgram
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: MMapDemo <image> <result> <thread count>");
                return;
            }

            var begin = DateTime.Now;
            MemMapDemoMultiThread(args[0], args[1], int.Parse(args[2]));
            var elapse = DateTime.Now - begin;
            Console.WriteLine($"耗时：{elapse.TotalMilliseconds / 1000}");
        }

        static void MemMapDemoMultiThread(string source, string destination, int threadCount)
        {
            File.Copy(source, destination, true);
            var (offset, width, height) = ReadHeaders(source);
            var threads = new Thread[threadCount];
            using (var mm = MemoryMappedFile.CreateFromFile(destination))
            {
                var whiteRow = new byte[width];
                for (var i = 0; i < width; ++i) whiteRow[i] = 255;
                using (var writer = mm.CreateViewAccessor(offset, width * height))
                {
                    var step = height / threadCount;
                    for (var i = 0; i < threadCount; ++i )
                    {
                        threads[i] = new Thread(state => {
                            // var begin = i * step;
                            var index = (int)state;
                            var begin = index * step;
                            var end = index == threadCount - 1 ? height : (index + 1) * step;
                            for (var j = begin; j < end; j += 50)
                            {
                                writer.WriteArray(j * width, whiteRow, 0, whiteRow.Length);
                            }
                        });
                    }

                    for (var i = 0; i < threadCount; ++i) threads[i].Start(i);
                    foreach (var thread in threads) thread.Join();
                }
            }
        }

        static (int offset, int width, int height) ReadHeaders(string filename)
        {
            var bmpHeader = new BmpHeader();
            var dibHeader = new DibHeader();
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var br = new BinaryReader(fs))
                {
                    bmpHeader.MagicNumber = br.ReadInt16();
                    bmpHeader.Filesize = br.ReadInt32();
                    bmpHeader.Reserved1 = br.ReadInt16();
                    bmpHeader.Reserved2 = br.ReadInt16();
                    bmpHeader.DataOffset = br.ReadInt32();

                    dibHeader.HeaderSize = br.ReadInt32();
                    if (dibHeader.HeaderSize != 40)
                    {
                        throw new ApplicationException("Only Windows V3 format supported.");
                    }
                    dibHeader.Width = br.ReadInt32();
                    dibHeader.Height = br.ReadInt32();
                    dibHeader.ColorPlanes = br.ReadInt16();
                    dibHeader.Bpp = br.ReadInt16();
                    dibHeader.CompressionMethod = br.ReadInt32();
                    dibHeader.ImageDataSize = br.ReadInt32();
                    dibHeader.HorizontalResolution = br.ReadInt32();
                    dibHeader.VerticalResolution = br.ReadInt32();
                    dibHeader.NumberOfColors = br.ReadInt32();
                    dibHeader.NumberImportantColors = br.ReadInt32();
                }
            }

            return (bmpHeader.DataOffset, dibHeader.RowSize, dibHeader.Height);
        }

        public class BmpHeader
        {
            public short MagicNumber { get; set; }
            public int Filesize { get; set; }
            public short Reserved1 { get; set; }
            public short Reserved2 { get; set; }
            public int DataOffset { get; set; }
        }

        public class DibHeader
        {
            public int HeaderSize { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public short ColorPlanes { get; set; }
            public short Bpp { get; set; }
            public int CompressionMethod { get; set; }
            public int ImageDataSize { get; set; }
            public int HorizontalResolution { get; set; }
            public int VerticalResolution { get; set; }
            public int NumberOfColors { get; set; }
            public int NumberImportantColors { get; set; }
            public int RowSize
            {
                get
                {
                    return 4 * ((Bpp * Width) / 32);
                }
            }
        }
    }
}
