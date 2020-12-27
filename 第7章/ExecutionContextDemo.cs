using System;
using System.Threading;
using System.Threading.Tasks;

public class ExecutionContextDemo
{
    public static void Main()
    {
        ExecutionContextSyncDemo().Wait();
        ExecutionContextAsyncDemo().Wait();
    }

    public static Task ExecutionContextSyncDemo()
    {
        var al = new AsyncLocal<int>();
        al.Value = 42;

        return Task
            .Run(() => Console.WriteLine($"Task run: {al.Value}"))
            .ContinueWith(_ => Console.WriteLine($"ContinueWith: {al.Value}"));
    }

    public static async Task ExecutionContextAsyncDemo()
    {
        var al = new AsyncLocal<int>();
        al.Value = 42;

        await Task.Delay(100);
        Console.WriteLine($"第一次await: {al.Value}");

        var task =  Task.Yield();
        task.GetAwaiter().UnsafeOnCompleted(() => 
            Console.WriteLine($"UnsafeOnCompleted: {al.Value}")
        );

        await task;

        Console.WriteLine($"第二次await: {al.Value}");
    }
}