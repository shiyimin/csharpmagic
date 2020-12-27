using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

class Program
{
    public static IList<string> allowableCommandLineArgs
        = new [] { "TopicA", "TopicB", "All" };
    static void Main(string[] args)
    {
        if (args.Length != 1 || !allowableCommandLineArgs.Contains(args[0]))
        {
            Console.WriteLine("参数可以是其中一个：" +
                                "'TopicA', 'TopicB' or 'All'");
            Environment.Exit(-1);
        }
        string topic = args[0] == "All" ? "" : args[0];
        Console.WriteLine("订阅者进程订阅主题 : {0}", topic);
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost:12345");
            subSocket.Subscribe(topic);
            Console.WriteLine("订阅者进程链接到消息队列...");
            while (true)
            {
                string messageTopicReceived = subSocket.ReceiveFrameString();
                string messageReceived = subSocket.ReceiveFrameString();
                Console.WriteLine(messageReceived);
            }
        }
    }
}