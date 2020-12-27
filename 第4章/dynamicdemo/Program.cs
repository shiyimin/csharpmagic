// 源码位置：第五章\dynamicdemo\Program.cs
// 编译和运行命令：dotnet run
using System;
using System.Reflection;
// 采用的外部包：Newtonsoft.Json.dll
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;

public class DynamicKeywordDemo
{
    public static void Main()
    {
        dynamic o = 1;
        Console.WriteLine("o: " + o++);
        o = "test string";
        Console.WriteLine(o);
        // 下面的语句会触发RuntimeBinderException
        // Console.WriteLine(o++);

        dynamic iunknow = CreateDynamicObject();
        Console.WriteLine(iunknow.DemoStringProperty);
        Console.WriteLine(iunknow.DemoIntProperty);
        iunknow.DemoDelegate("anonymous function");

        // 下面的语句会触发RuntimeBinderException
        // iunknow.NoExtendedValueAvailable = 123.456;
        // 序列化成JSON
        Console.WriteLine(iunknow.ToString());
        DeserializeDemo();
    }

    private static void DeserializeDemo()
    {
        dynamic o = JObject.Parse(@"{
            'Stores': [
                'Lambton Quay',
                'Willis Street'
            ],
            'Manufacturers': [{
                'Name': 'Acme Co',
                'Products': [{
                    'Name': 'Anvil',
                    'Price': 50
                }]
            }, {
                'Name': 'Contoso',
                'Products': 0
                }
            ]
        }");
        Console.WriteLine($"Count: {o.Stores.Count}, [0]: {o.Stores[0]}");
        for (var i = 0; i < o.Manufacturers.Count; ++i) {
            if (o.Manufacturers[i].Products.Type == JTokenType.Integer) {
                Console.WriteLine("Products的值为0");
            } else {
                Console.WriteLine($"Products: {o.Manufacturers[i].Products[0].Name}");
            }
        }
    }

    private static dynamic CreateDynamicObject()
    {
        /*
        dynamic obj = new object();
        obj.DemoStringProperty = "A string property";
        */
        
        dynamic obj = new {
            DemoStringProperty = "A string property",
            DemoIntProperty = 123,
            DemoDelegate = (Action<string>)delegate(string s) {
                Console.WriteLine("Hello, " + s);
            }
        };

        return obj;
    }
}