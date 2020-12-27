using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class TaskContinuation
{
    public static void Main()
    {
        var signal = new AutoResetEvent(false);
        var tasks = new List<Task<int>>();
        var requirement = new Task<int>(() => {
            Console.WriteLine("需求分析结束");
            return 1;
        });
        tasks.Add(requirement);
        var design = requirement.ContinueWith(result => {
            Console.WriteLine("详细设计结束");
            return 2;
        });
        tasks.Add(design);
        var random = new Random();
        var devs = new List<Task<int>>();
        for (var i = 0; i < 5; ++i) {
            var id = i;
            devs.Add(design.ContinueWith(result => {
                Console.WriteLine($"开发{id}根据设计{result.Result}完成开发");
                return id;
            }));
        }
        tasks.AddRange(devs);
        var testers = new ConcurrentQueue<Task<int>>();
        var tester_count = 3;
        var test_work_id = 0;

        int test_work(Task<Task<int>> result) {
            var code_completed = result.Result;
            devs.Remove(code_completed);
            var devid = code_completed.Result;
            var id = test_work_id++ % tester_count;
            Console.WriteLine($"测试{id}测试完开发{devid}的工作");

            if (devs.Count > 0) {
                testers.Enqueue(Task.WhenAny(devs).ContinueWith(test_work));
            } else {
                signal.Set();
            }

            return 1;
        }

        testers.Enqueue(Task.WhenAny(devs).ContinueWith(test_work));
        requirement.Start();
        signal.WaitOne();
        tasks.AddRange(testers);
        var release = Task.WhenAll(tasks).Result;
        var sum = release.Length;
        Console.WriteLine($"发布：{sum}");
    }
}