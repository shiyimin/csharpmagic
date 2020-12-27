using System;
using System.Text;
using NetMQ;
using NetMQ.Sockets;

namespace dealer
{
    class Program
    {
        static void Main(string[] args)
        {            
            var quote = args[0];
            using (var worker = new DealerSocket())
            {
                worker.Options.Identity = Encoding.Unicode.GetBytes(quote);
                worker.Connect("tcp://localhost:5555");
                Console.WriteLine($"已经链接到服务器，接收参数：{quote}！");

                int total = 0;
                bool end = false;

                while (!end)
                {
                    string request = worker.ReceiveFrameString();
                    Console.WriteLine($"接收到买卖单：{quote}-{request}！");

                    if (request == "END")
                        end = true;
                    else
                        total++;

                    var replyMsg = new NetMQMessage();
                    replyMsg.AppendEmptyFrame();
                    replyMsg.Append(Encoding.Unicode.GetBytes($"买卖单：{quote}-{request}处理完成！"));
                    worker.SendMultipartMessage(replyMsg);
                }

                Console.WriteLine($"DEALER {quote} 收到任务: {total}");
            }
        }
    }
}
