using System;
using utility;

namespace attributedemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person()
            {
                Name = "张三",
                BirthDay = DateTime.Now,
                Age = 1
            };
            var person1 = new PersonNoAttribute()
            {
                Name = "张三",
                BirthDay = DateTime.Now
            };
            Console.WriteLine($"{person.Name} 生日：{person.BirthDay}");
            person.DebugOut();
        }
    }
}
