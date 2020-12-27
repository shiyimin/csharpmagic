// 源码位置：第二章\MultiCastDelegateDemo.cs
// 编译命令：csc MultiCastDelegateDemo.cs
using System;

public delegate void GreetingDelegate(string value);

public class  MultiCastDelegateDemo
{
    static void SayHello(string value)
    {
        Console.WriteLine("Hello {0}", value);
    }

    static void SayGoodbye(string value)
    {
        Console.WriteLine("Bye {0}", value);
    }

    static void Main()
    {
        GreetingDelegate singleDel1 = SayHello;
        singleDel1("singleDel1");

        GreetingDelegate multiDel1 = (GreetingDelegate)Delegate.Combine(
            singleDel1, new GreetingDelegate(SayGoodbye));
        multiDel1("multiDel1");

        GreetingDelegate singleDel2 = (GreetingDelegate)Delegate.Remove(
            multiDel1, singleDel1);
        singleDel2("singleDel2");
        
        GreetingDelegate multiDel2 = singleDel1 + SayGoodbye;
        multiDel2("multiDel2");

        GreetingDelegate singleDel3 = multiDel2 - SayGoodbye;
        singleDel3("singleDel3");
        
        GreetingDelegate multiDel3 = SayHello;
        multiDel3("multiDel3");

        multiDel3 += SayGoodbye;
        multiDel3("multiDel3");
    }
}