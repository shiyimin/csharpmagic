using System;
using NetMQ;
using NetMQ.Sockets;

namespace collector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== Collector进程 ======");

            using (var receiver = new PullSocket("@tcp://localhost:5558"))
            {
                var startOfBatchTrigger = receiver.ReceiveFrameString();
                Console.WriteLine("接收到任务分配方发送的工作开始通知！");

                var sum = 0;
                for (int taskNumber = 0; taskNumber < 100; taskNumber++)
                {
                    var msg = receiver.ReceiveFrameString();
                    var value = int.Parse(msg);
                    sum += value;
                }
                
                Console.WriteLine("结果：{0}", sum);
                Console.ReadLine();
            }
        }
    }
}
