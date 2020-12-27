using System;
using NetMQ;
using NetMQ.Sockets;

namespace producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== 任务分配方 ======");
            using (var sender = new PushSocket("@tcp://*:5557"))
            using (var sink = new PushSocket(">tcp://localhost:5558"))
            {
                Console.WriteLine("当所有工作进程启动后，敲任意键开始");
                Console.ReadLine();
                //the first message it "0" and signals start of batch
                //see the Sink.csproj Program.cs file for where this is used
                Console.WriteLine("告诉collector进程开始工作");
                sink.SendFrame("0");
                Console.WriteLine("发送消息给工作进程");
                Random rand = new Random(0);
                int totalMs = 0;
                for (int taskNumber = 0; taskNumber < 100; taskNumber++)
                {
                    int workload = rand.Next(0, 100);
                    totalMs += workload * 2;
                    sender.SendFrame(workload.ToString());
                }
                Console.WriteLine("期望结果: {0}", totalMs);
                Console.WriteLine("等工作进程完成后，按任意键退出");
                Console.ReadLine();
            }
        }
    }
}
