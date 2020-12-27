using System;
using System.Threading;
using System.Threading.Tasks;

public class TaskWait
{
    public static void Main()
    {
        // var task = new Task(async () => {
        //     Console.WriteLine($"当前线程Id {Thread.CurrentThread.ManagedThreadId}");
        //     await Task.Delay(1000);
        //     Console.WriteLine($"等待1秒后，当前线程Id {Thread.CurrentThread.ManagedThreadId}");
        // });
        var task = new Task(() => {
            Console.WriteLine($"当前线程Id {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"等待1秒后，当前线程Id {Thread.CurrentThread.ManagedThreadId}");
        });
        Console.WriteLine($"主线程Id {Thread.CurrentThread.ManagedThreadId}");
        task.Start();
        Thread.Sleep(2000);
        task.Wait();
        Console.WriteLine($"任务执行完，线程Id {Thread.CurrentThread.ManagedThreadId}");
    }
}