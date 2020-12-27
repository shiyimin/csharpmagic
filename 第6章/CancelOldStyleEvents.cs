using System;
using System.Threading;
using System.Threading.Tasks;

class CancelOldStyleEvents
{
   // .NET 4.0之前的ManualResetEvent不支持统一的取消操作
   static ManualResetEvent mre = new ManualResetEvent(false);

   static void Main()
   {
      var cts = new CancellationTokenSource();

      // 将取消任务用的token传递给线程
      ThreadPool.QueueUserWorkItem(state => DoWork(cts.Token));
      Console.WriteLine("操作按键：s - 启动/重启；p - 暂停；c - 取消。");
      Console.WriteLine("按其他按键退出程序……");

      // 主线程处理用户的按键输入
      bool goAgain = true;
      while (goAgain) {
         char ch = Console.ReadKey(true).KeyChar;

         switch (ch) {
            case 'c':
               cts.Cancel();
               break;
            case 'p':
               mre.Reset();
               break;
            case 's':
               mre.Set();
               break;
            default:
               goAgain = false;
               break;
         }

         Thread.Sleep(100);
      }
      cts.Dispose();
   }

   static void DoWork(CancellationToken token)
   {
      while (true)
      {
         // 等待ManualResetEvent对象或者CancellationToken的取消操作
         int eventThatSignaledIndex =
                WaitHandle.WaitAny(new WaitHandle[] { mre, token.WaitHandle },
                                   new TimeSpan(0, 0, 5));

         Console.WriteLine($"触发信号的对象：{eventThatSignaledIndex}");
         // 判断触发信号的是哪个对象，如果是第二个对象，说明用户执行的取消操作
         if (eventThatSignaledIndex == 1) {
            Console.WriteLine("等待操作被取消了！");
            break; 
         }
         // 是否超时了！
         else if (eventThatSignaledIndex == WaitHandle.WaitTimeout) {
            Console.WriteLine("等待用户操作超时");
            break;
         }
         else {
            Console.Write("Working... ");
            // 模拟长时间的工作！
            Thread.SpinWait(5000000);
         }
      }
   }
}