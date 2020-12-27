using System;

namespace nullableref
{
#nullable enable
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person("施", "懿民");
            Console.WriteLine(person.FirstName.Length);
            // warning CS8625: 无法将 null 文本转换为不可为 null 的引用类型。 
            person.FirstName = null;
            // warning CS8602: 取消引用可能出现的空引用。
            Console.WriteLine(person.MiddleName.Length);
            if (person.MiddleName != null)
                Console.WriteLine(person.MiddleName.Length);

            // 不会造成编译警告
            Console.WriteLine(person.MiddleName!.Length);
            
            // 不会造成编译警告
            person.MiddleName = "作者";
            Console.WriteLine(person.MiddleName.Length);

            var people = new Person[10];
             // 不会造成编译警告
            Console.WriteLine(people[0].FirstName);
        }
    }

    class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName;    // 不可为空
        public string? MiddleName;  // 可空引用
        public string LastName;     // 不可为空
    }
#nullable disable
}
