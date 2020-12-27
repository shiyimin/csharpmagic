// 源码位置：第五章\dlr\ExpandoObjectDemo.cs
// 编译命令：csc ExpandoObjectDemo.cs
using System;
using System.Dynamic;

public class ExpandoObjectDemo
{
    static void Main()
    {
        // 使用下面一行会导致RuntimeBinderException
        // dynamic contact = new object();
        dynamic contact = new ExpandoObject();
        contact.Name = "施懿民";
        contact.Phone = "18621519910";
        contact.Address = new ExpandoObject();
        contact.Address.City = "上海";
        contact.Address.Address = "某个不知名的小区";

        Console.WriteLine(contact.Address.Address);

        foreach (var property in contact)
            Console.WriteLine("{0}: {1}", property.Key, property.Value);

        contact.Validate = (Func<bool>)(() => !string.IsNullOrEmpty(contact.Name));
        Console.WriteLine("Validation: " + contact.Validate());
    }
}