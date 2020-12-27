using System;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace router
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new RouterSocket())
            {
                server.Bind("tcp://*:5555");
                Task.Factory.StartNew(() => {
                    while (true) {
                        var msg = server.ReceiveMultipartMessage();
                        var id = msg[0].ConvertToString(Encoding.Unicode);
                        var response = msg[2].ConvertToString(Encoding.Unicode);
                        Console.WriteLine($"ROUTER端接收到{id}的响应消息：{response}");
                    }
                });
                Console.WriteLine("ROUTER服务器已启动，当DEALER启动后按任意键开始！");
                Console.ReadLine();
                var random = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 10; ++i)
                {
                    var quote = random.Next(3) > 0 ? "BTC" : "ETH";
                    server.SendMoreFrame(Encoding.Unicode.GetBytes(quote));
                    server.SendFrame($"下达[{quote}-XRP]的买卖单！");
                }

                server.SendMoreFrame(Encoding.Unicode.GetBytes("BTC"));
                server.SendFrame("END");
                server.SendMoreFrame(Encoding.Unicode.GetBytes("ETH"));
                server.SendFrame("END");
            }

            Console.WriteLine("任务分配完毕，按任意键退出！");
            Console.ReadLine();
        }
    }
}
