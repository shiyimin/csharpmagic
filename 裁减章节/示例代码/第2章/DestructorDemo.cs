// 源码位置：第二章\DesctructorDemo.cs
// 编译命令：csc DesctructorDemo.cs
using System;

public class DesctructorDemo
{
    static void Main()
    {
        UseDestructorObjects();

        GC.Collect();
        GC.WaitForPendingFinalizers(); 

        Console.WriteLine("------- 分割线 ---------");
        UseDispose1();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("------- 分割线 ---------");
        UseDispose2();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    static void UseDestructorObjects()
    {
        var dbc = new DestructorBaseClass();
        var dc = new DestructorClass();
        dbc.DummyMethod();
        dc.DummyMethod();
    }

    static void UseDispose1()
    {
        using (var dc = new DestructorClass())
        {
            dc.DummyMethod();
        }
    }

    static void UseDispose2()
    {
        DestructorClass dc = null;
        try
        {
            dc = new DestructorClass();
            dc.DummyMethod();
        }
        finally
        {
            dc.Dispose();
        }
    }
}

public class DestructorBaseClass
{
    public void DummyMethod() {}

    ~DestructorBaseClass()
    {
        Console.WriteLine("DestructorBaseClass destructor");
    }
}

public class DestructorClass : DestructorBaseClass, IDisposable
{
    ~DestructorClass()
    {
        Console.WriteLine("DestructorClass destructor");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Console.WriteLine("DestructorClass Dispose");
    }
}