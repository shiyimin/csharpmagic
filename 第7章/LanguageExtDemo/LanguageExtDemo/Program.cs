using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using LanguageExt;
using static LanguageExt.Prelude;

namespace LanguageExtDemo
{
    public class Balance
    {
        public int Available { get; set; }

        public int Frozen { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var optional = Some(123);
            //var x = match(optional,
            //    Some: v => v * 2,
            //    None: () => 0);
            //Console.WriteLine($"x: {x}");

            //optional = None;
            //x = match(optional,
            //    Some: v => v * 2,
            //    None: () => 0);

            //Console.WriteLine($"x: {x}");

            //Buy("User1", 10);
            //Console.WriteLine("user1: {0}", s_UserBalances["User1"].Available);
            //Buy("User3", 10);
            //Console.WriteLine("user3: {0}", s_UserBalances.ContainsKey("User3"));

            //BuyAsync("User1", 10).Wait();
            //Console.WriteLine("user1: {0}", s_UserBalances["User1"].Available);
            //BuyAsync("User3", 10).Wait();

            // EitherDemo();

            // PatternMatchDemo();

            PositionPatternMatch();
        }

        static Dictionary<string, Balance> s_UserBalances = new Dictionary<string, Balance>()
        {
            { "User1", new Balance() { Available = 100, Frozen = 0 } },
            { "User2", new Balance() { Available = 20, Frozen = 30 } }
        };

        static void Buy(string user, int amount)
        {
            var result = FindBalance(user);
            match(FindBalance(user),
                Some: b =>
                {
                    b.Available -= amount;
                    b.Frozen += amount;
                },
                None: () => Console.WriteLine("未找到！"));

            //FindBalance(user)
            //    .Some(b =>
            //    {
            //        b.Available -= amount;
            //        b.Frozen += amount;
            //    })
            //    .None(() => Console.WriteLine("未找到！"));
        }

        static Option<Balance> FindBalance(string user)
        {
            string key = user ?? string.Empty;

            //if (s_UserBalances.ContainsKey(key))
            //    return s_UserBalances[key];

            //// return Option<Balance>.None;
            //return None;

            return s_UserBalances.ContainsKey(key) ?
                Some(s_UserBalances[key]) : None;
        }

        static async Task BuyAsync(string user, int amount)
        {
            var result = await QueryBalance(user).MatchAsync(
                 Some: async b =>
                 {
                     await LockBalance(b, amount);
                     return true;
                 },
                 None: () =>
                 {
                     Console.WriteLine($"未找到，当前线程Id:{Thread.CurrentThread.ManagedThreadId}");
                     return false;
                 });

            if (!result)
            {
                Console.WriteLine("无法完成下单！");
            }
        }

        static async Task<Option<Balance>> QueryBalance(string user)
        {
            Console.WriteLine($"【QueryBalance】当前线程Id:{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100);
            Console.WriteLine($"【QueryBalance】暂停100毫秒后，当前线程Id:{Thread.CurrentThread.ManagedThreadId}");
            return FindBalance(user);
        }

        static async Task LockBalance(Balance balance, int amount)
        {
            Console.WriteLine($"【LockBalance】当前线程Id:{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100);
            Console.WriteLine($"【LockBalance】暂停100毫秒后，当前线程Id:{Thread.CurrentThread.ManagedThreadId}");
            balance.Available -= amount;
            balance.Frozen += amount;
        }

        static void EitherDemo()
        {
            Divide(6, 3).Match(
                Right: value => Console.WriteLine($"结果：{value}"),
                Left: error => Console.WriteLine($"发生错误：{error}")
            );

            Divide(6, 0).Match(
                Right: value => Console.WriteLine($"结果：{value}"),
                Left: error => Console.WriteLine($"发生错误：{error}")
            );
        }

        static Either<string, int> Divide(int x, int y)
        {
            if (y == 0)
                return Left("不能除0！");
            else
                return Right(x / y);
        }

        static void PatternMatchDemo()
        {
            var tuple = ("程序员", 35);
            if (tuple is (string _, int _))
                Console.WriteLine(tuple.Item1);

            ToDateTime(null).Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime("").Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime("  ").Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime("2020-07-30").Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime(0L).Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime(-1L).Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
            ToDateTime(new int[] { 2020, 07, 30 }).Match(
                Right: dt => Console.WriteLine($"解析结果：{dt}"),
                Left: err => Console.WriteLine($"解析错误：{err}")
            );
        }

        static Either<string, DateTime> ToDateTime(object value)
        {
            switch (value)
            {
                case null: // null模式
                    return Left($"{nameof(value)}为空!");
                case DateTime dt:  // 类型模式
                    return dt;
                case long ticks when ticks >= 0: // case guard
                    return new DateTime(ticks);
                case long ticks when ticks < 0:
                    return Left($"{value}不能小于0");
                case string @string when DateTime.TryParse(@string, out DateTime dt):
                    return dt;
                case int[] date when date.Length == 3 && date[0] > 0 && date[1] > 0 && date[2] > 0:
                    return new DateTime(date[0], date[1], date[2]);
                // case IConvertible convertible:
                //     try
                //     {
                //         return convertible.ToDateTime(null);
                //     } catch (Exception ex)
                //     {
                //         return Left(ex.Message);
                //     }
                case var _: // var模式
                    return Left($"不支持的参数类型：{value.GetType()}");
            }
        }

        class Point 
        { 
            public int X { get; set; }

            public int Y { get; set; }    
        }

        abstract class Shape
        {
            public Point Point { get; set; }
        }

        class Rectangle : Shape
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public void Deconstruct(out int width, out int height, out Point point)
            {
                width = Width;
                height = Height;
                point = Point;
            }
        }

        enum OrderState
        {
            Canceled,
            Ordered,
            Delivered,
            Returned
        }

        enum ActionState
        {
            Order,
            CancelOrder,
            CancelOrderCancellation,
            Return
        }

        static void PositionPatternMatch()
        {
            // 通过解构方法来拆分对象并进行匹配
            Shape shape = new Rectangle
            {
                Width = 100,
                Height = 100,
                Point = new Point { X = 0, Y = 100 }
            };
            var result = shape switch
            {
                Rectangle (100, 100, null) => "Found 100x100 rectangle without a point",
                Rectangle (100, 100, _) => "Found 100x100 rectangle",
                _ => "Different, or null shape"
            };
            Console.WriteLine($"解构方法匹配结果：{result}");

            var 订单状态 = (OrderState.Ordered, ActionState.CancelOrder, 3);
            var finalState = 订单状态 switch
            {
                (OrderState.Ordered, ActionState.CancelOrder, _) => OrderState.Canceled,
                (OrderState.Canceled, ActionState.CancelOrderCancellation, _) => OrderState.Ordered,
                (OrderState.Delivered, ActionState.Return, int days) when days < 30 => OrderState.Returned,
                (_, ActionState.Order, _) => OrderState.Ordered,
                (var state, _, _) => state
            };
            Console.WriteLine($"元组匹配结果：{finalState}");
        }
    }
}
