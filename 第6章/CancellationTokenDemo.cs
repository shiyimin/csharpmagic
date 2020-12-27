using System;
using System.Threading;
using System.Threading.Tasks;

public static class CancellationTokenDemo
{
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task))
            {
                throw new OperationCanceledException(cancellationToken);
            }
        }

        return task.Result;
    }

    static void Main()
    {
        Console.Write("> ");
        
        using (var cts = new CancellationTokenSource())
        {
            // 十秒后取消操作
            cts.CancelAfter(1000);
            var token = cts.Token;
            token.Register(() => Console.WriteLine("十秒超时，取消操作……"));
            try
            {
                var msg = Console.In.ReadLineAsync().WithCancellation(token).Result;
                Console.WriteLine($"| {msg}");
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}