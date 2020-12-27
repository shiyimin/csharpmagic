// 源码位置：第二章\ExceptionDemo.cs
using System;

public class UserDefinedException : Exception
{
    public UserDefinedException() : base() { }

    public UserDefinedException(string message) : base(message) { }

    public UserDefinedException(string message, Exception inner) 
        : base(message, inner) { }
}

public class ExceptionDemo
{
    static string ReadLine()
    {
        string str = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentNullException("至少需要输入一个数字！");
        }
        return str;
    }

	public static void Main()
	{
        try
        {
            Console.WriteLine("输入除数");
            int i = int.Parse(ReadLine());
            Console.WriteLine("输入被除数");
            int j = int.Parse(ReadLine());
            int k = i / j;
            Console.WriteLine($"结果是：{k}");
        }
        catch (ArgumentException e)
        {
            // 再次抛出异常一般将原始异常保存在InnerException字段
            throw new UserDefinedException(
                "系统错误，请参照InnerException获取具体的异常信息！", e);
        }
        catch (OverflowException)
        {
            Console.WriteLine("输入的数字太大！");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("除零错误！");
        }
        catch (Exception e)
        {
            Console.WriteLine($"未知错误：{e.Message}，堆栈：\n{e.StackTrace}");
        }
        /* 捕捉通用异常的语句也可以省略掉类型，如下面的catch块这样
        catch
        {
            Console.WriteLine("未知错误！");
        }
        */
        finally
        {
            Console.WriteLine("执行finally语句块中的代码");
        }
    }
}
