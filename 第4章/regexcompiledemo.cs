using System;
using System.Text.RegularExpressions;

public class RegexCompileDemo
{
    public static void Main(string[] args)
    {
        var pattern = @"\p{Sc}*(\s?\d+[.,]?\d*)";
        var replacement = "$1";
        var input = "$16.32 12.19 £16.29 €18.29 €18,29 ¥123.34 $123,456.00";
        
        var begin = DateTime.Now;
        for (var i = 0; i < int.Parse(args[0]); ++i)
            Regex.Replace(input, pattern, replacement);
        var elapsed = DateTime.Now - begin;
        Console.WriteLine("Regex.Replace: {0}", elapsed.TotalMilliseconds);
        
        var regex = new Regex(pattern, RegexOptions.Compiled);
        begin = DateTime.Now;
        for (var i = 0; i < int.Parse(args[0]); ++i)
            regex.Replace(input, replacement);
        elapsed = DateTime.Now - begin;
        Console.WriteLine("Compiled: {0}", elapsed.TotalMilliseconds);
    }
}