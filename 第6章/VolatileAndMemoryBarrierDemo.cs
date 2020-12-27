// 编译命令：csc -o+ VolatileAndMemoryBarrierDemo.cs
using System;
using System.Threading;

public class VolatileAndMemoryBarrierDemo
{
    static /* volatile */ bool _flag = true;
    static void Main()
    {
        var thread = new Thread(() => {
            bool toggle = false;
            while (_flag) {
                // 使用MemoryBarrier也同样可以强制刷新_flag的值
                // Thread.MemoryBarrier();
                toggle = !toggle;
            }
        });
        thread.Start();

        Console.ReadLine();
        Console.WriteLine("结束线程");
        _flag = false;
        thread.Join();
    }
}