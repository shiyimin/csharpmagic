// 源码位置：第五章\SerializationDemo.cs
// 编译命令：csc SerializationDemo.cs
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
// 仅支持.NET Framework，.NET Core需要其它包
// using System.Runtime.Serialization.Formatters.Soap;

public class SerializationDemo
{
    static void Main()
    {
        var order = new Order { 
            Id = Guid.NewGuid(), UserId = 888, Market = "BTC/USDT", Volume = 1.23m,
            PlacedDate = DateTime.Now, ClientIdentity = Guid.NewGuid().ToByteArray()
        };

        IFormatter formatter = new BinaryFormatter();
        var filename = "serialization.bin";
        // IFormatter formatter = new SoapFormatter();
        // var filename = "serialization.xml";
        using (var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            formatter.Serialize(stream, order);
            
        Order deserialized = null;
        using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            deserialized = (Order)formatter.Deserialize(stream);
        
        Console.WriteLine($"order.Id: {order.Id}, deserialized.Id: {deserialized.Id}");
        Console.WriteLine($"order.ClientIdentity: {order.ClientIdentity.Length}, deserialized.ClientIdentity: {deserialized.ClientIdentity}");
    }
}

[Serializable]
public class Order // : ISerializable
{
    public Guid Id { get; set; }

    public uint UserId { get; set; }

    public string Market { get; set; }

    public decimal? Price { get; set; }

    public decimal Volume { get; set; }

    public DateTime PlacedDate { get; set; }

    public bool IsCancelled { get; set; }

    public byte[] ClientIdentity { get; set; }

    #region ISerializable成员
    public Order() { }

    public Order(SerializationInfo info, StreamingContext context)
    {
        IsCancelled = info.GetBoolean("c");
        PlacedDate = new DateTime(info.GetInt64("d"));
        Id = (Guid)info.GetValue("i", typeof(Guid));
        Market = (string)info.GetValue("m", typeof(string));
        Price = (decimal)info.GetValue("p", typeof(decimal));
        UserId = (uint)info.GetValue("u", typeof(uint));
        Volume = (decimal)info.GetValue("v", typeof(decimal));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("c", IsCancelled);
        info.AddValue("d", PlacedDate.ToUniversalTime().Ticks);
        info.AddValue("i", Id);
        info.AddValue("m", Market);
        info.AddValue("p", Price.HasValue ? Price.Value : 0);
        info.AddValue("u", UserId);
        info.AddValue("v", Volume);
    }
    #endregion
}
