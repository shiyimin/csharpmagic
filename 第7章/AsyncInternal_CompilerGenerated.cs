public class AsyncInternal
{
    // Fields
    internal decimal? _rate;

    // Methods
    public AsyncInternal() {}

    // Nested Types
    [CompilerGenerated]
    private sealed class <GetPriceAsync>d__1 : IAsyncStateMachine
    {
        // Fields
        public int <>1__state;
        public AsyncTaskMethodBuilder<decimal> <>t__builder;
        public string stock;
        public AsyncInternal <>4__this;
        private decimal <usdPrice>5__1;
        private decimal <cnyPrice>5__2;
        private decimal <>s__3;
        private decimal <>s__4;
        private TaskAwaiter<decimal> <>u__1;

        // Methods
        public <GetPriceAsync>d__1() { }
        
        private void MoveNext()
        {
            int num = this.<>1__state;
            try
            {
                TaskAwaiter<decimal> awaiter;
                AsyncInternal.<GetPriceAsync>d__1 d__;
                TaskAwaiter<decimal> awaiter2;
                if (num == 0)
                {
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<decimal>();
                    this.<>1__state = num = -1;
                }
                else if (num == 1)
                {
                    awaiter2 = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<decimal>();
                    this.<>1__state = num = -1;
                    goto TR_0006;
                }
                else
                {
                    awaiter = this.<>4__this.GetUsdPrice(this.stock).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        this.<>1__state = num = 0;
                        this.<>u__1 = awaiter;
                        d__ = this;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<decimal>, AsyncInternal.<GetPriceAsync>d__1>(ref awaiter, ref d__);
                        return;
                    }
                }
                this.<>s__3 = awaiter.GetResult();
                this.<usdPrice>5__1 = this.<>s__3;
                if (this.<>4__this._rate != null)
                {
                    goto TR_0005;
                }
                else
                {
                    awaiter2 = this.<>4__this.GetRate("USD").GetAwaiter();
                    if (awaiter2.IsCompleted)
                    {
                        goto TR_0006;
                    }
                    else
                    {
                        this.<>1__state = num = 1;
                        this.<>u__1 = awaiter2;
                        d__ = this;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<decimal>, AsyncInternal.<GetPriceAsync>d__1>(ref awaiter2, ref d__);
                    }
                }
                return;
            TR_0005:
                this.<cnyPrice>5__2 = this.<usdPrice>5__1 * this.<>4__this._rate.Value;
                decimal result = this.<cnyPrice>5__2 * 100M;
                this.<>1__state = -2;
                this.<>t__builder.SetResult(result);
                return;
            TR_0006:
                this.<>s__4 = awaiter2.GetResult();
                this.<>4__this._rate = new decimal?(this.<>s__4);
                goto TR_0005;
            }
            catch (Exception exception)
            {
                this.<>1__state = -2;
                this.<>t__builder.SetException(exception);
            }
        }

        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine) { }
    }

    [CompilerGenerated]
    private sealed class <GetRate>d__3 : IAsyncStateMachine
    {
        // Fields
        public int <>1__state;
        public AsyncTaskMethodBuilder<decimal> <>t__builder;
        public string currency;
        public AsyncInternal <>4__this;

        // Methods
        public <GetRate>d__3() { }
        
        private void MoveNext()
        {
            decimal num2;
            int num = this.<>1__state;
            try
            {
                num2 = 7M;
            }
            catch (Exception exception)
            {
                this.<>1__state = -2;
                this.<>t__builder.SetException(exception);
                return;
            }
            this.<>1__state = -2;
            this.<>t__builder.SetResult(num2);
        }

        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine) { }
    }

    [CompilerGenerated]
    private sealed class <GetUsdPrice>d__2 : IAsyncStateMachine
    {
        // Fields
        public int <>1__state;
        public AsyncTaskMethodBuilder<decimal> <>t__builder;
        public string stock;
        public AsyncInternal <>4__this;

        // Methods
        public <GetUsdPrice>d__2() { }

        private void MoveNext()
        {
            decimal num2;
            int num = this.<>1__state;
            try
            {
                num2 = 123M;
            }
            catch (Exception exception)
            {
                this.<>1__state = -2;
                this.<>t__builder.SetException(exception);
                return;
            }
            this.<>1__state = -2;
            this.<>t__builder.SetResult(num2);
        }

        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine) { }
    }

    [AsyncStateMachine(typeof(<GetPriceAsync>d__1)), DebuggerStepThrough]
    public Task<decimal> GetPriceAsync(string stock)
    {
        <GetPriceAsync>d__1 stateMachine = new <GetPriceAsync>d__1 {
            <>4__this = this,
            stock = stock,
            <>t__builder = AsyncTaskMethodBuilder<decimal>.Create(),
            <>1__state = -1
        };
        stateMachine.<>t__builder.Start<<GetPriceAsync>d__1>(ref stateMachine);
        return stateMachine.<>t__builder.Task;
    }
    
    [AsyncStateMachine(typeof(<GetRate>d__3)), DebuggerStepThrough]
    public Task<decimal> GetRate(string currency) { /* 省略相似代码 */ }
    [AsyncStateMachine(typeof(<GetUsdPrice>d__2)), DebuggerStepThrough]
    public Task<decimal> GetUsdPrice(string stock) { /* 省略相似代码 */ }
}