// 编译命令：csc /t:library AsyncInternal.cs
#pragma warning disable CS0436, CS1998

using System;
using System.Threading.Tasks;

public class AsyncInternal
{
    internal decimal? _rate;
    public async Task<decimal> GetPriceAsync(string stock)
    {
        var usdPrice = await GetUsdPrice(stock);
        if (!_rate.HasValue)
            _rate = await GetRate("USD");
        var cnyPrice = usdPrice * _rate.Value;
        return cnyPrice * 100; // 一手的价格
    }

    public async Task<int> GetUsdPrice(string stock)
    {
        await Task.Delay(1000);
        // ... 获取美股价格 ...
        return 123;
    }

    public async Task<short> GetRate(string currency)
    {
        await Task.Delay(1000);
        // ... 获取货币汇率 ...
        return 7;
    }
}

public class AsyncInternal_StateMachine
{
     enum State { Start, Step1, Step2, Step3 };
     private readonly AsyncInternal @this;
     private readonly string _stock;
     private readonly TaskCompletionSource<decimal> _tcs = 
         new TaskCompletionSource<decimal>();
     private State _state = State.Start;
     private Task<decimal> _getUsdPriceTask;
     private Task<decimal> _getRateTask;
     private decimal _usdPrice;

     public AsyncInternal_StateMachine(AsyncInternal @this, string stock)
     {
         this.@this = @this;
         _stock = stock;
     }

     public void Start()
     {
         try
         {
             if (_state == State.Start)
             {
                 _getUsdPriceTask = @this.GetUsdPrice(_stock);
                 _state = State.Step1;
                 _getUsdPriceTask.ContinueWith(_ => Start());
             }
             else if (_state == State.Step1)
             {
                 if (_getUsdPriceTask.Status == TaskStatus.Canceled)
                     _tcs.SetCanceled();
                 else if (_getUsdPriceTask.Status == TaskStatus.Faulted)
                     _tcs.SetException(_getUsdPriceTask.Exception.InnerException);
                 else 
                 {
                     _usdPrice = _getUsdPriceTask.Result;
                     if (!@this._rate.HasValue)
                     {
                         _getRateTask = @this.GetRate("USD");
                         _state = State.Step2;
                         _getRateTask.ContinueWith(_ => Start());
                     }
                     else
                     {
                         _state = State.Step3;
                         Start();
                     }
                 }
             }
             else if (_state == State.Step2)
             {
                 if (_getRateTask.Status == TaskStatus.Canceled)
                     _tcs.SetCanceled();
                 else if (_getRateTask.Status == TaskStatus.Faulted)
                     _tcs.SetException(_getRateTask.Exception.InnerException);
                 else 
                 {
                     @this._rate = _getRateTask.Result;
                     _state = State.Step3;
                     Start();
                 }
             }
             else if (_state == State.Step3)
             {
                 var cnyPrice = _usdPrice * @this._rate.Value;
                 _tcs.SetResult(cnyPrice * 100);
             }
         }
         catch (Exception e)
         {
             _tcs.SetException(e);
         }
     }

     public Task<decimal> Task => _tcs.Task;

     // AsyncInternal的GetPriceAsync被重写成下面的形式
     // public static Task<decimal> GetPriceAsync(string stock)
     // {
     //     var stateMachine = new AsyncInternal_StateMachine(this, stock);
     //     stateMachine.Start();
     //     return stateMachine.Task;
     // }
}

namespace System.Runtime.CompilerServices
{
    public class AsyncVoidMethodBuilder
    {
        public AsyncVoidMethodBuilder() => Console.WriteLine(".ctor");

        public static AsyncVoidMethodBuilder Create() => new AsyncVoidMethodBuilder();

        public void SetResult() => Console.WriteLine("SetResult");

        public void Start<T>(ref T stateMachine) where T : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("AsyncVoidMethodBuilder AwaitOnCompleted");
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        { 
            Console.WriteLine("AsyncVoidMethodBuilder AwaitUnsafeOnCompleted");
        }

        public void SetException(Exception e) { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) 
        {
            Console.WriteLine("AsyncVoidMethodBuilder SetStateMachine");
        }
    }

    public class AsyncTaskMethodBuilder<T>
    {
        public static AsyncTaskMethodBuilder<T> LastInstance { get; private set; }

        public Exception Exception { get; private set; }

        private Task<T> _task;
        public Task<T> Task
        {
            get
            {
                if (_task == null)
                    _task = Threading.Tasks.Task.FromResult(default(T));
                return _task;
            }
        }

        public AsyncTaskMethodBuilder() => Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> .ctor");

        public static AsyncTaskMethodBuilder<T> Create() => LastInstance = new AsyncTaskMethodBuilder<T>();

        public void SetResult(T result)
        {
            Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> SetResult: {result}.");
            _task = Threading.Tasks.Task.FromResult(result);
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> Start");
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> AwaitOnCompleted");
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        { 
            Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> AwaitUnsafeOnCompleted");
        }

        public void SetException(Exception e) { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) 
        {
            Console.WriteLine($"AsyncTaskMethodBuilder<{typeof(T)}> SetStateMachine");
        }
    }
}

namespace SampleProgram
{
    using System.Runtime.CompilerServices;

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("准备调用GetPriceAsync!");
            var instance = new AsyncInternal();
            var price = instance.GetPriceAsync("MSFT").Result;
            VoidAsync();
            Console.WriteLine($"价格：{price}");

            Foo().Wait();
        }

        public static async void VoidAsync() { }

        public static async Task Foo()
        {
            var lazy = new Lazy<int>(() => 42);
            var result = await lazy;
            Console.WriteLine(result);
        }
    }

    public struct LazyAwaiter<T> : INotifyCompletion
    {
        private readonly Lazy<T> _lazy;
    
        public LazyAwaiter(Lazy<T> lazy) => _lazy = lazy;
    
        public T GetResult() => _lazy.Value;
    
        public bool IsCompleted => true;
    
        public void OnCompleted(Action continuation) { }
    }
    
    public static class LazyAwaiterExtensions
    {
        public static LazyAwaiter<T> GetAwaiter<T>(this Lazy<T> lazy)
        {
            return new LazyAwaiter<T>(lazy);
        }
    }
}