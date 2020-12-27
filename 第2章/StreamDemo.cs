// 源码位置：第三章\StreamDemo.cs
// 编译命令：csc StreamDemo.cs
using System;
using System.IO;

class StreamDemo
{
    static void Main(string[] args)
    {
        FileStreamDemo();
    }

    static void FileStreamDemo()
    {
        using (FileStream fs = new FileStream("filestream.demo", FileMode.OpenOrCreate))
        // using (FileStream fs = File.Open("filestream.demo", FileMode.OpenOrCreate))
        {
            for (var i = 0; i < 26; ++i)
                fs.WriteByte((byte)(i + 'a'));

            fs.Seek(0, SeekOrigin.Begin);
            int b = 0;
            while ((b = fs.ReadByte()) > 0)
                Console.Write((char)b);
            Console.WriteLine();
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[20];
            int count = 0;
            while((count = fs.Read(bytes, 0, bytes.Length)) > 0)
            {
                Console.Write(System.Text.Encoding.ASCII.GetString(bytes));
                Array.Clear(bytes, 0, bytes.Length);
            }
            Console.WriteLine();
        }
    }
}
