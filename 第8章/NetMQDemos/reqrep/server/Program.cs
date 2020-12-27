using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ResponseSocket())
            {
                server.Bind("tcp://*:5555");
                while (true)
                {
                    var message = server.ReceiveFrameString();
                    Console.WriteLine("Received {0}", message);
                    // processing the request
                    Thread.Sleep(100);
                    Console.WriteLine("Sending World");
                    server.SendFrame("World");
                }
            }
        }
    }
}
