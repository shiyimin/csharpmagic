// 编译方法：csc /debug MutexDemo.cs
// 运行方法：dotnet MutexDemo.exe
using System;
using System.Threading;

public class MutexDemo
{
    /*
    public static void Main()
    {
        // Mac/Linux上，mutex名称需要加上"Global\"才能被看成系统级别的
        var mutex = new Mutex(false, "Global\\MutexExample", out bool mutexWasCreated);
        Console.WriteLine($"尝试获取锁！是否新建：{mutexWasCreated}");
        var ret = mutex.WaitOne();

        Console.WriteLine($"使用共享资源，ret = {ret}");
        Console.ReadLine();

        mutex.ReleaseMutex();
        Console.WriteLine("释放锁，按任意键退出……");
        Console.ReadLine();
    }
    */

    public static void Main()
    {
        using (var mutex = new Mutex(false, "Global\\MutexExample", out bool mutexWasCreated))
        {
            if (mutexWasCreated)
            {
                Console.WriteLine("进程在系统中是第一个实例，运行程序");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("系统中已经有实例在运行了！");
            }
        }
    }
}