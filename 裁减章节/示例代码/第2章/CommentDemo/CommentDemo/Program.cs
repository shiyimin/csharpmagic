using System;

namespace CommentDemo
{
    /// <summary>
    /// 文档注释，这里可以写上对Program类型的注释说明
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main方法的注释说明，如：
        /// Main方法是C#程序的入口方法
        /// </summary>
        /// <param name="args">这是参数args的注释说明</param>
        static void Main(string[] args)
        {
            // 以双斜杠开头是单行注释
            Console.WriteLine("Hello World!");
            /* 
             * 这是多行注释
             * 下面的语句的作用是等待用户输入任意一行文本
             */
            Console.ReadLine();
        }
    }
}
