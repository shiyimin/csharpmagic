using System;
using System.Threading;
using System.Threading.Tasks;

public class CancelParallelDemo
{
    public static void Main()
    {
        var cts = new CancellationTokenSource();
        var task = Task.Run(() => {
            var options = new ParallelOptions { CancellationToken = cts.Token };
            try
            {
                Parallel.For(0, 50, options, i => {
                    Thread.Sleep(1000);
                    Console.WriteLine($"第{i}次迭代！");
                }); 
            }
            catch(OperationCanceledException oce)
            {
                Console.WriteLine($"任务已取消：{oce.Message}");
            }
        });
        Console.WriteLine("按任意键取消任务!");
        Console.ReadLine();
        cts.Cancel();

        Task.WaitAll(task);
    }
}