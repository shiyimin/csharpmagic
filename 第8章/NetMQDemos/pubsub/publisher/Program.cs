using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

class Program
{
    static void Main(string[] args)
    {
        Random rand = new Random(50);
        using (var pubSocket = new PublisherSocket())
        {
            Console.WriteLine("启动发布者进程...");
            pubSocket.Options.SendHighWatermark = 1000;
            pubSocket.Bind("tcp://*:12345");
            for (var i = 0; i < 100; i++)
            {
                var randomizedTopic = rand.NextDouble();
                if (randomizedTopic > 0.5)
                {
                    var msg = "TopicA 消息-" + i;
                    Console.WriteLine("发送消息主题: {0}", msg);
                    pubSocket.SendMoreFrame("TopicA").SendFrame(msg);
                }
                else
                {
                    var msg = "TopicB 消息-" + i;
                    Console.WriteLine("发送消息主题: {0}", msg);
                    pubSocket.SendMoreFrame("TopicB").SendFrame(msg);
                }
                Thread.Sleep(500);
            }
        }
    }
}