// 源码位置：第六章\lambda\lambdademo.cs
// 编译命令：csc /debug lambdademo.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class LambdaDemo
{
    static void UseDelegate(Action<int> action, int value)
    {
        action(value);
    }

    static void Main(string[] args)
    {
        Func<string, int> parser = Int32.Parse;
        Console.WriteLine(parser("12345"));

        // 匿名委托
        Func<int, int, int> calculator = delegate (int left, int right) { return left + right; };
        Console.WriteLine(calculator(12, 34));

        calculator = (left, right) => left * right;
        Console.WriteLine(calculator(12, 34));

        calculator = (int left, int right) => { return left - right; };
        Console.WriteLine(calculator(34, 12));

        var result = new Func<int, int, int>((left, right) => left + right)(12, 34);
        Console.WriteLine($"result: {result}");

        /* 
         * 编译错误：error CS0815: Cannot assign lambda expression to an implicitly-typed variable
        var calculator1 = (left, right) => left + right;
        Console.WriteLine(calculator1(12, 34));
        var calculator2 = (int left, int right) => left + right;
        Console.WriteLine(calculator2(12, 34));
        */
        
        int total = 0;
        Action<int> sum = delegate(int n) { total = total + n; };
        sum(12);
        Console.WriteLine($"total: {total}");
        UseDelegate(sum, 34);
        Console.WriteLine($"total: {total}");

        total = 0;
        sum = n => total = total + n;
        sum(12);
        Console.WriteLine($"total: {total}");
        UseDelegate(sum, 34);
        Console.WriteLine($"total: {total}");

        Expression<Func<int, int, int>> expr = ((left, right) => left + right);
        // 打印结果：expr: (left, right) => (left + right)
        Console.WriteLine($"expr: {expr}");
        Console.WriteLine($"ret: {expr.Compile().Invoke(12, 34)}");

        LocalFuntionBasicDemo();
        LocalFuntionExceptionDemo();
    }

    static void LocalFuntionBasicDemo()
    {
        var total = 0;
        Console.WriteLine($"本地方法：{LocalFunc(12)}, total: {total}");
        Console.WriteLine($"LocalFib：{LocalFib(12)}");
        return ;

        int LocalFunc(int value)
        {
            total = total + value;
            return total;
        }

        int LocalFib(int n)
        {
            return n > 1 ? LocalFib(n - 1) + LocalFib(n - 2) : n;
        }
    }
    
    static void LocalFuntionExceptionDemo()
    {
        foreach (var i in UseDemoSequenceByYield(1, 10))
            Console.Write($"{i} ");
            
        try {
            foreach (var i in UseDemoSequenceByYield(10, 1))
                Console.Write($"{i} ");
        }
        catch (Exception e) {
            Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        }
            
        try {
            foreach (var i in UseDemoSequenceByLocalFuncYield(10, 1))
                Console.Write($"{i} ");
        }
        catch (Exception e) {
            Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        }

        UseDemoSequenceInForeach(1, 10);
        try {
            UseDemoSequenceInForeach(10, 1);
        }
        catch (Exception e) {
            Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        }

        UseDemoSequenceInWhileLoop(1, 10);
        try {
            UseDemoSequenceInWhileLoop(10, 1);
        }
        catch (Exception e) {
            Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        }
    }

    class DemoSequenceEnumerable : IEnumerable<int>
    {
        private int _start;
        private int _end;

        public DemoSequenceEnumerable(int start, int end)
        {
            _start = start;
            _end = end;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new DemoSequenceEnumerator(_start, _end);
        }
    }

    class DemoSequenceEnumerator : IEnumerator<int>
    {
        private int _start;
        private int _end;
        private int _iter;
        public DemoSequenceEnumerator(int start, int end)
        {
            if (start >= end)
                throw new ArgumentException("start必须小于end.");
            _start = start;
            _end = end;
            _iter = start;
        }

        public bool MoveNext() { return _iter++ < _end; }

        public void Reset() { _iter = _start; }

        public int Current { get { return _iter; } }

        object IEnumerator.Current { get { return Current; } }

        void IDisposable.Dispose() { }
    }

    static void UseDemoSequenceInWhileLoop(int start, int end)
    {
        if (start >= end)
            throw new ArgumentException("start必须小于end.");

        var enumerable = new DemoSequenceEnumerable(start, end);
        var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
            Console.WriteLine($"enumerable in while loop: {enumerator.Current}");
    }

    // IEnumerable的文档说明，请参考：
    // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=netframework-4.8
    static void UseDemoSequenceInForeach(int start, int end)
    {
        var enumerable = new DemoSequenceEnumerable(start, end);
        foreach (var i in enumerable)
            Console.WriteLine($"enumerable in foreach: {i}");
    }

    static IEnumerable<int> UseDemoSequenceByYield(int start, int end)
    {      
        if (start >= end)
            throw new ArgumentException("start必须小于end.");

        for (int i = start; i < end; ++i)
        {
            if (i % 2 == 0)
                yield return i;
        }
    }
    
    static IEnumerable<int> UseDemoSequenceByLocalFuncYield(int start, int end)
    {      
        if (start >= end)
            throw new ArgumentException("start必须小于end.");

        IEnumerable<int> impl()
        {
            for (int i = start; i < end; ++i)
            {
                if (i % 2 == 0)
                    yield return i;
            }
        }

        return impl();
    }
}
