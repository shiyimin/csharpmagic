using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class ConcurrentStackSimplified<T>
{
    private class Node
    {
        internal readonly T m_value;
        internal Node m_next;
        internal Node(T value)
        {
            m_value = value;
            m_next = null;
        }
    }

    private volatile Node m_head;

    public int Count 
    {
        get
        {
            int count = 0;
            for (Node curr = m_head; curr != null; curr = curr.m_next)
                count++;

            return count;
        }
    }

    public void Push(T item)
    {
        var node = new Node(item);
        Node head;
        do
        {
            head = m_head;
            node.m_next = head;
        } while (Interlocked.CompareExchange(ref m_head, node, head) != head);
    }

    public bool TryPop(out T result)
    {
        Node head = m_head;
        while (head != null)
        {
            if (Interlocked.CompareExchange(ref m_head, head.m_next, head) == head)
            {
                result = head.m_value;
                return true;
            }
 
             head = m_head;
        }
 
        result = default(T);
        return false;
    }
}

public class ConcurrentStackSimplified
{
    static void Main()
    {
        var sw =  Stopwatch.StartNew();
        for (var round = 0; round < 1000; ++round)
        {
            var stack = new ConcurrentStackSimplified<int>();
            // var stack = new ConcurrentStack<int>();
            for (var i = 0; i < 10000; ++i) {
                stack.Push(i);
            }
            
            var thread1 = new Thread(() => {
                for (var i = 10000; i < 20000; ++i) {
                    stack.Push(i);
                }
            });
            var thread2 = new Thread(() => {
                for (var i = 20000; i < 30000; ++i) {
                    stack.Push(i);
                }
            });

            var thread3 = new Thread(() => {
                for (var i = 0; i < 10000; ++i)
                    stack.TryPop(out int result);
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            if (stack.Count != 20000) {
                Console.WriteLine($"数量：{stack.Count}");
            }
        }
        Console.WriteLine($"无锁版本耗时：{sw.Elapsed}");

        sw =  Stopwatch.StartNew();
        for (var round = 0; round < 1000; ++round)
        {
            var stack = new Stack<int>();
            for (var i = 0; i < 10000; ++i) {
                stack.Push(i);
            }
            
            var thread1 = new Thread(() => {
                for (var i = 10000; i < 20000; ++i) {
                    lock (stack) {
                        stack.Push(i);
                    }
                }
            });
            var thread2 = new Thread(() => {
                for (var i = 20000; i < 30000; ++i) {
                    lock (stack) {
                        stack.Push(i);
                    }
                }
            });

            var thread3 = new Thread(() => {
                for (var i = 0; i < 10000; ++i) {
                    lock (stack) {
                        stack.TryPop(out int result);
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            if (stack.Count != 20000) {
                Console.WriteLine($"数量：{stack.Count}");
            }
        }
        Console.WriteLine($"有锁版本耗时：{sw.Elapsed}");
    }
}