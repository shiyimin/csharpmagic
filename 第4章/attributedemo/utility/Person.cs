using System;
using System.Diagnostics;

namespace utility
{
    [DebuggerDisplay("姓名：{Name,nq}")]
    public class Person
    {
        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        [Obsolete("Age字段表示年龄很繁琐，请新的代码采用BirthDay字段")]
        public int Age { get; set; }

        [Conditional("DEBUG")]
        public void DebugOut()
        {
            Console.WriteLine("调试用输出！");
        }
    }

    public class PersonNoAttribute
    {
        public string Name { get; set; }

        public DateTime BirthDay { get; set; }
    }
}
