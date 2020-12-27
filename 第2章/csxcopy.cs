// 源码位置：第三章\csxcopy.cs
// 编译命令：csc csxcopy.cs
using System;
using System.IO;

class csxcopy
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: csxcopy <srcdir> <dstdir> <searchPattern>");
            return;
        }

        try
        {
            DoXCopy(args[0], args[1], args[2]);
        }
        catch (Exception e)
        {
            Console.WriteLine($"出错啦，错误消息：\n{e.Message}");
        }
    }

    static void DoXCopy(string srcdir, string dstdir, string searchPattern)
    {
        if (string.IsNullOrEmpty(srcdir) || string.IsNullOrEmpty(dstdir) || string.IsNullOrEmpty(searchPattern))
            throw new ArgumentNullException($"{nameof(srcdir)}或{nameof(dstdir)}或{nameof(searchPattern)}为空！");
        srcdir = srcdir.Trim();
        dstdir = dstdir.Trim();
        if (!Directory.Exists(srcdir)) throw new InvalidOperationException($"{srcdir}不存在！");      
        if (Directory.Exists(dstdir)) throw new InvalidOperationException($"{dstdir}已经存在了！");     

        if (srcdir[srcdir.Length - 1] == Path.DirectorySeparatorChar) 
            srcdir = srcdir.Substring(0, srcdir.Length - 1);
        if (dstdir[dstdir.Length - 1] == Path.DirectorySeparatorChar) 
            dstdir = dstdir.Substring(0, dstdir.Length - 1);

        var files = Directory.GetFiles(srcdir, searchPattern, SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var directory = Path.GetDirectoryName(file);
            var filename = Path.GetFileName(file);
            var newdirectory = dstdir;
            if (directory.Length > srcdir.Length)
            {
                var relativePath = directory.Substring(
                    srcdir.Length + 1, directory.Length - srcdir.Length - 1);
                newdirectory = Path.Combine(dstdir, relativePath);
            }

            if (!Directory.Exists(newdirectory))
                Directory.CreateDirectory(newdirectory);
                
            File.Copy(file, Path.Combine(newdirectory, filename));
        }
    }
}