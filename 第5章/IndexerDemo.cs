using System;
using System.Collections.Generic;

public class IndexerDemo
{
    static void Main(string[] args)
    {
        var bag = new IndexableObject<string>();
        bag.Add("第一章", "导言");
        bag.Add("第一章", "多操作系统支持");
        bag.Add("第六章", "ADO.NET");
        bag.Add("第六章", "Linq");
        bag.Add("第六章", "EntityFramework");

        Console.WriteLine($"bag[0]: {bag[0]}");
        Console.WriteLine($"bag[\"第六章\", 1]: {bag["第六章", 1]}");
        bag["第六章", 1] = "lambda";
        Console.WriteLine($"bag[\"第六章\", 1]: {bag["第六章", 1]}");
    }
}

public class IndexableObject<T>
{
    private Dictionary<string, List<T>> _items = new Dictionary<string, List<T>>();

    public T this[int idx] {
        get {
            var i = 0;
            foreach (var list in _items)
            {
                foreach (var item in list.Value)
                {
                    if (i == idx) 
                        return item;
                    else
                        i++;
                }
            }

            throw new IndexOutOfRangeException($"{idx}不在集合里！");
        }
    }

    public T this[string key, int idx] {
        get {
            var list = _items[key];
            return list[idx];
        }
        set {
            var list = _items[key];
            list[idx] = value;
        }
    }

    public void Add(string key, T item)
    {
        var list = _items.ContainsKey(key) ? _items[key] : new List<T>();
        list.Add(item);
        _items[key] = list;
    }
}