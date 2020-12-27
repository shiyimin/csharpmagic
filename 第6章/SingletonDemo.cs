// 源码位置：第8章\SingletonDemo.cs
// 编译命令：csc SingletonDemo.cs
using System;
using System.Threading;

public sealed class Singleton
{
    private static int s_counter = 0;
    public static Singleton s_Instance;
    public static Singleton Instance
    {
        get
        {
            if (s_Instance == null)
                s_Instance = new Singleton();

            return s_Instance;
        }
    }

    private Singleton()
    {
        s_counter++;
    }

    public static void Print() 
    {
        Console.WriteLine($"Singleton Counter value: {s_counter}");
    }
}

public sealed class MultiThreadSingleton
{
    private static int s_counter = 0;
    private static readonly object s_lock = new object();
    public static MultiThreadSingleton s_Instance;
    public static MultiThreadSingleton Instance
    {
        get
        {
            if (s_Instance == null)
            {
                lock(s_lock)
                {
                    if (s_Instance == null)
                        s_Instance = new MultiThreadSingleton();
                }
            }

            return s_Instance;
        }
    }

    private MultiThreadSingleton()
    {
        s_counter++;
    }

    public static void Print() 
    {
        Console.WriteLine($"MultiThreadSingleton Counter value: {s_counter}");
    }
}

public class LazySingleton
{
    private static int s_counter = 0;
    private static Lazy<LazySingleton> s_Instance = new Lazy<LazySingleton>(() => new LazySingleton());
    public static LazySingleton Instance
    {
        get
        {
            return s_Instance.Value;
        }
    }

    private LazySingleton()
    {
        s_counter++;
    }

    public static void Print() 
    {
        Console.WriteLine($"LazySingleton Counter value: {s_counter}");
    }
}

public class SingletonDemo
{
    static void Main(string[] args)
    {
        var thread1 = new Thread(ThreadFunc);
        var thread2 = new Thread(ThreadFunc);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Singleton.Print();
        MultiThreadSingleton.Print();
        LazySingleton.Print();
    }

    static void ThreadFunc()
    {
        var instance1 = Singleton.Instance;
        var instance2 = MultiThreadSingleton.Instance;
        var instance3 = LazySingleton.Instance;
    }
}