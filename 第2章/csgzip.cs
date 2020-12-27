// 源码位置：第三章\csgzip.cs
// 编译命令：csc csgzip.cs
using System;
using System.IO;
using System.IO.Compression;

class csgzip
{
    static void Usage()
    {
        Console.WriteLine("csgzip -c <file>");
        Console.WriteLine("Or...");
        Console.WriteLine("csgzip -d <gzip file>");
    }

    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Usage();
            return;
        }

        var comparison = StringComparison.OrdinalIgnoreCase;
        if (string.Equals(args[0], "-c", comparison))
            Compress(Path.GetFullPath(args[1]));
        else if (string.Equals(args[0], "-d", comparison))
            Decompress(Path.GetFullPath(args[1]));
        else
            Usage();
    }

    static void Compress(string file)
    {
        using (var fileStream = File.OpenRead(file))
        {
            if ((File.GetAttributes(file) & FileAttributes.Hidden) 
                == FileAttributes.Hidden) 
                return;
            using (var outputStream = File.Create(file + ".gz"))
            {
                using (var gzstream = new GZipStream(
                    outputStream, CompressionMode.Compress))
                {
                    fileStream.CopyTo(gzstream);
                }
            }
        }
    }

    static void Decompress(string file)
    {
        using (var fileStream = File.OpenRead(file))
        {
            var outfile = file.Remove(file.Length - ".gz".Length);
            using (var outputStream = File.Create(outfile))
            {
                using (var gzstream = new GZipStream(
                    fileStream, CompressionMode.Decompress))
                {
                    gzstream.CopyTo(outputStream);
                }
            }
        }
    }
}