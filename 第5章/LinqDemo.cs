using System;
using System.Linq;
using System.Collections.Generic;

public class LinqDemo
{
    static void Main()
    {
        var baseline = 30000;
        foreach (dynamic chapter in Chapters())
        {
            if (chapter.Words > baseline)
                Console.WriteLine($"{chapter.Name}: {chapter.Words}");
        }
        
        var chapters = from c in Chapters()
                       where c.Words > baseline
                       select c;
        foreach (dynamic chapter in chapters)
            Console.WriteLine($"{chapter.Name}: {chapter.Words}");

        chapters = Chapters().Where(c => c.Words > baseline);
        foreach (dynamic chapter in chapters)
            Console.WriteLine($"{chapter.Name}: {chapter.Words}");
            
        chapters = from c in Chapters()
                   where c.Words > baseline
                   orderby c.Chars
                   select c;
        foreach (dynamic chapter in chapters)
            Console.WriteLine($"{chapter.Name}: {chapter.Words}");

        chapters = Chapters().Where(c => c.Words > baseline).OrderBy(c => c.Chars);
        foreach (dynamic chapter in chapters)
            Console.WriteLine($"{chapter.Name}: {chapter.Words}");

        var statistics = from c in Chapters()
                         group c by c.Part into p
                         // orderby p.Key
                         select new { Name = p.Key, Pages = p.Sum(c => c.Pages) };
        foreach (dynamic s in statistics)
            Console.WriteLine($"{s.Name}: {s.Pages}");

        statistics = Chapters().GroupBy(c => c.Part)
        //                       .OrderBy(p => p.Key)
                               .Select(p => new { Name = p.Key, Pages = p.Sum(c => c.Pages)});
        foreach (dynamic s in statistics)
            Console.WriteLine($"{s.Name}: {s.Pages}");
            
        foreach (dynamic s in from c in Chapters()
                              group c by c.Part into p
                              select new { Name = p.Key, MaxPages = p.Max(c => c.Pages) })
            Console.WriteLine($"{s.Name}: {s.MaxPages}");

        foreach (dynamic s in Chapters().GroupBy(c => c.Part)
                                        .Select(p => new { Name = p.Key, MaxPages = p.Max(c => c.Pages)}))
            Console.WriteLine($"{s.Name}: {s.MaxPages}");
            
        foreach (dynamic s in from c in Chapters()
                              group c by c.Part into p
                              select new { Name = p.Key, Pages = p.Sum(c => c.Pages), MaxPages = p.Max(c => c.Pages) })
            Console.WriteLine($"{s.Name}: {s.Pages}: {s.MaxPages}");

        var query = Chapters().Where(c => {
            Console.WriteLine("判断 {c.Name} 是否符合过滤条件！");
            return c.Chars > 50000;
        });
        Console.WriteLine("------- 验证延迟处理 ------------");
        foreach (dynamic chapter in query)
            Console.WriteLine($"{chapter.Name}: {chapter.Chars}: {chapter.Pages}");

        query = from c in Chapters()
                     where c.Chars > 50000
                     select c;
        query = from c in query
                 where c.Pages > 50
                 select c;
        foreach (dynamic chapter in query)
            Console.WriteLine($"{chapter.Name}: {chapter.Chars}: {chapter.Pages}");
            
        query = Chapters().Where(c => c.Chars > 50000);
        query = query.Where(c => c.Pages > 50);
        foreach (dynamic chapter in query)
            Console.WriteLine($"{chapter.Name}: {chapter.Chars}: {chapter.Pages}");
    }

    static void QuerySyntax()
    {
        var items = new int[] { 1, 2, 3, 4, 5 };
        var query = from c in items select c;
        foreach (var q in query)
            Console.WriteLine($"{q}");
    }

    static void MethodSyntax()
    {
        var items = new int[] { 1, 2, 3, 4, 5 };
        var query = items.Select(c => c);
        foreach (var q in query)
            Console.WriteLine($"{q}");
    }

    static IEnumerable<dynamic> Chapters()
    {
        yield return new { Part = "基本知识", Name = "第一章 导言", Words = 11986, Chars = 18397, Pages = 25 };
        yield return new { Part = "基本知识", Name = "第二章 C#基本语法", Words = 39648, Chars = 81866, Pages = 69 };
        yield return new { Part = "基本知识", Name = "第三章 C#基础编程", Words = 26387, Chars = 50169, Pages = 45 };
        yield return new { Part = "进阶知识", Name = "第四章 C#面向对象编程", Words = 32987, Chars = 68445, Pages = 54 };
        yield return new { Part = "进阶知识", Name = "第五章 反射与动态编程", Words = 27173, Chars = 56150, Pages = 51 };
        yield return new { Part = "案例实操", Name = "第六章 数据处理编程", Words = 0, Chars = 0, Pages = 0 };
    }
}