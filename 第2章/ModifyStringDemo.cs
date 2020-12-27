// 源码位置：第三章\ModifyStringDemo.cs
// 编译命令：csc /main:ModifyStringDemo ModifyStringDemo.cs
using System;
using System.Text;

class ModifyStringDemo
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: ModifyStringDemo <循环次数>");
            return;
        }

        var begin = DateTime.Now;
        var loops = int.Parse(args[0]);
        var value = string.Empty;
        for (var i = 0; i < loops; ++i)
        {
            // 大于1M就删除掉
            if (value.Length > 1024 * 1024)
                value = string.Empty;
            
            value = value + i.ToString();
        }
        var end = DateTime.Now;
        // Console.WriteLine(value);
        Console.WriteLine("String 用时：\n{0}", (end - begin).TotalMilliseconds);
    }
}

class ModifyStringBuilderDemo
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: ModifyStringDemo <循环次数>");
            return;
        }

        var begin = DateTime.Now;
        var loops = int.Parse(args[0]);
        var sb = new StringBuilder();
        for (var i = 0; i < loops; ++i)
        {
            // 大于1M就删除掉
            if (sb.Length > 1024 * 1024)
                sb.Clear();
            
            sb.Append(i.ToString());
        }
        var end = DateTime.Now;
        // Console.WriteLine(sb.ToString());
        Console.WriteLine("StringBuilder 用时：\n{0}", (end - begin).TotalMilliseconds);
    }
}