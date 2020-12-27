using System;
using System.Collections.Generic;
using System.Threading;

public class CounterDownEventDemo
{
    static void Main(string[] args)
    {
        var taskCount = int.Parse(args[0]);

        using (CountdownEvent signal = new CountdownEvent(taskCount))
        {
            var i = 1;
            var thread = new Thread(() => {
                for (; i <= taskCount; ++i)
                    CreateNewDownloadTask(i, signal);

                signal.Wait();
            });
            thread.Start();

            var command = Console.ReadLine().Trim();
            while (!string.IsNullOrWhiteSpace(command) && !signal.IsSet)
            {
                switch (command)
                {
                    case "+":
                    {
                        CreateNewDownloadTask(i++, signal);
                        signal.AddCount();
                        break;
                    }
                    case "-":
                    {
                        CancelOneRandomThread();
                        break;
                    }
                    default:
                        break;
                }

                command = Console.ReadLine().Trim();
            }

            thread.Join();
        }
    }

    static List<WorkTaskData> s_workTaskDataList = new List<WorkTaskData>();
    static void CancelOneRandomThread()
    {
        var random = new Random();
        WorkTaskData data;
        lock (s_workTaskDataList)
        {
            var idx = random.Next(0, s_workTaskDataList.Count);
            data = s_workTaskDataList[idx];
            s_workTaskDataList.RemoveAt(idx);
        }
        data.Cancellation.Cancel();
        Console.WriteLine($"取消任务：{data.TaskName}！");
    }
    static void CreateNewDownloadTask(int i, CountdownEvent signal)
    {
        var wtd = new WorkTaskData()
        {
            Cancellation = new CancellationTokenSource(),
            TaskName = $"Task{i}",
            Countdown = signal
        };
        
        lock (s_workTaskDataList)
        {
            s_workTaskDataList.Add(wtd);
        }

        Console.WriteLine($"创建新任务：{wtd.TaskName}！");
        ThreadPool.QueueUserWorkItem(DownloadWorkTask, wtd);
    }

    static void DownloadWorkTask(object data)
    {
        var wtd = (WorkTaskData)data;
        var random = new Random();
        var cancelToken = wtd.Cancellation.Token;
        for (var i = 0; i < 10; ++i)
        {
            if (cancelToken.IsCancellationRequested) break;

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]{wtd.TaskName}在工作！");
            Thread.Sleep(random.Next(3000, 10000));
        }

        wtd.Countdown.Signal();
    }
}

public struct WorkTaskData
{
    public CancellationTokenSource Cancellation;

    public string TaskName;

    public CountdownEvent Countdown;
}