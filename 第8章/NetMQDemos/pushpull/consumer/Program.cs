using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== 工作进程 ======");
            using (var receiver = new PullSocket(">tcp://localhost:5557"))
            using (var sender = new PushSocket(">tcp://localhost:5558"))
            {
                while (true)
                {
                    string workload = receiver.ReceiveFrameString();
                    int value = int.Parse(workload);
                    Thread.Sleep(value);
                    Console.WriteLine("将结果发送给collector");
                    sender.SendFrame((value * 2).ToString());
                }
            }
        }
    }
}
