// 源码位置：第二章\TupleDemo.cs
// 编译命令：csc /langversion:"7.1" TupleDemo.cs
using System;

public class TupleDemo
{
    static void Main()
    {
        var name = "张三";
        var personTuple1 = ( name: "张三", age: 30 );
        var personTuple2 = ( "张三", 30 );
        var personTuple3 = ( name, 30 );
        (string name, int age) copyTuple1 = personTuple1;
        (string, int) copyTuple2 = personTuple1;
        (string name, int age)? nullableTuple = personTuple1;

        Console.WriteLine($"{personTuple1.name}, {personTuple1.age}");
        Console.WriteLine($"{personTuple2.Item1}, {personTuple2.Item2}");
        Console.WriteLine($"{personTuple3.name}, {personTuple3.Item2}");
        Console.WriteLine($"{copyTuple1.name}, {copyTuple1.age}");
        Console.WriteLine($"{copyTuple2.Item1}, {copyTuple2.Item2}");
        Console.WriteLine(
            $"{nullableTuple.Value.name}, {nullableTuple.Value.age}");

        var tuple4 = ReturnTuple();
        Console.WriteLine($"{tuple4.name}, {tuple4.age}");

        var vtf = new TypeHasTupleField()
        {
            Person = (name: "张三", age: 30),
            Id = 2
        };
        Console.WriteLine($"{vtf.Person.name}, {vtf.Person.age}");
    }

    static (string name, int age) ReturnTuple()
    {
        // return (name: "张三", age: 30);
        return ("张三", 30);
    }
}

public class TypeHasTupleField
{
    public (string name, int age) Person { get;set; }

    public int Id { get; set; }
}