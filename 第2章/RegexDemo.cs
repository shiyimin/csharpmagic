// 源码位置：第三章\RegexDemo.cs
// 编译命令：csc RegexDemo.cs
using System;
using System.Text.RegularExpressions;

class RegexDemo
{
    static void Main()
    {
        // SimpleMatchDemo();
        // GroupMatchDemo();
        // GroupCaptureDemo();
        // BalanceGroupMatchDemo();
        // NegativeLookaheadDemo();
        // SubstitutionDemo();
        BalanceDivGroupMatchDemo();
    }

    static void SimpleMatchDemo()
    {
        var regex = new Regex("^\\d{3,4}-\\d{7,8}$");
        Console.WriteLine(regex.IsMatch("021-66106610")); // True
        Console.WriteLine(regex.IsMatch("0731-6610661")); // True
        Console.WriteLine(regex.IsMatch("02166106610")); // False
        Console.WriteLine(regex.IsMatch("21-66106610")); // False
        Console.WriteLine(regex.IsMatch(" 021-66106610")); // False
    }

    static void GroupMatchDemo()
    {
        foreach (Match match in Regex.Matches("2018-12-31", @"(\d+)-"))
        {
            Console.WriteLine(match.Groups[0].Value);
            Console.WriteLine(match.Groups[1].Value);
        }

        foreach (Match match in Regex.Matches(
            // "He said that that was the the correct answer.", @"(\w+)\s(\1)"))
            "He said that that was the the correct answer.", @"(?<dup>\w+)\s(\k<dup>)"))
        {
            Console.WriteLine("重复单词：{0}，位置：{1} - {2}",
                match.Groups[1].Value, match.Groups[1].Index, match.Groups[2].Index);
        }

        var m1 = Regex.Match("2018-12-31", @"(?<year>\d+)-(?'month'\d+)-(?<day>\d+)");
        Console.WriteLine($"{m1.Groups["year"].Value}年{m1.Groups["month"].Value}月{m1.Groups["day"].Value}日");
    }

    static void GroupCaptureDemo()
    {
        var match = Regex.Match("abcd", "(.)+");
        for (var i = 0; i < match.Groups[1].Captures.Count; ++i)
            Console.WriteLine($"{i}: '{match.Groups[1].Captures[i].Value}'");

        var pattern = @"^(?:[^()]|(?<open>\()|(?<-open>\)))+$";
        match = Regex.Match("(3 * (1 + 3))", pattern);
        Console.WriteLine($"Success: {match.Success}, open: {match.Groups["open"].Value}。");
        match = Regex.Match("(1 + 3)", pattern);
        Console.WriteLine($"Success: {match.Success}, open: {match.Groups["open"].Value}。");
        match = Regex.Match("(3 * (1 + 3)", pattern);
        Console.WriteLine($"Success: {match.Success}, open: {match.Groups["open"].Value}。");
        match = Regex.Match("(3 * (1 + 3)))", pattern);
        Console.WriteLine($"Success: {match.Success}, open: {match.Groups["open"].Value}。");
        pattern = @"^(?:[^()]|(?<open>\()|(?<-open>\)))+(?(open)(?!))$";
        match = Regex.Match("(3 * (1 + 3)", pattern);
        Console.WriteLine($"Success: {match.Success}, open: {match.Groups["open"].Value}。");
        pattern = @"^(?:[^()]|(?<open>\()|(?<content-open>\)))+(?(open)(?!))$";
        match = Regex.Match("(3 * (1 + 3))", pattern);
        Console.WriteLine($"0: {match.Groups["content"].Captures[0].Value}。");
        Console.WriteLine($"1: {match.Groups["content"].Captures[1].Value}。");
    }

    static void NegativeLookaheadDemo()
    {
        var pattern = @"a(?!b(?!c))";
        Console.WriteLine(Regex.Match("a", pattern).Success);  //匹配 (?!b)
        Console.WriteLine(Regex.Match("ac", pattern).Success); //匹配 (?!b)
        Console.WriteLine(Regex.Match("abc", pattern).Success);//匹配 (?!b(?!c))
        Console.WriteLine(Regex.Match("adc", pattern).Success);//匹配 (?!b(?!c))
        Console.WriteLine(Regex.Match("ab", pattern).Success); //不匹配 (?!b(?!c))
        Console.WriteLine(Regex.Match("abe", pattern).Success);//不匹配 (?!b(?!c))
    }

    static void BalanceGroupMatchDemo()
    {
        var pattern = "^[^<>]*(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*(?(Open)(?!))$";
        var input = "<abc><mno<xyz>>";
        var match = Regex.Match(input, pattern);
        Console.WriteLine(match.Length);
    }

    static void BalanceDivGroupMatchDemo()
    {
        var pattern = "<div>(?'content'[^<>]*)</div>";
        var match = Regex.Match("(<div>test<div>aaa</div></div>", pattern);
        Console.WriteLine(match.Success);
        if (match.Success) {
            Console.WriteLine($"0: {match.Groups.Count}。");
            Console.WriteLine($"0: {match.Groups[0].Value}。");
            Console.WriteLine($"0: {match.Groups["content"].Captures[0].Value}。");
        }
    }

    static void SubstitutionDemo()
    { 
        var pattern = @"\p{Sc}*(\s?\d+[.,]?\d*)";
        var replacement = "$1";
        var input = "$16.32 12.19 £16.29 €18.29 €18,29 ¥123.34 $123,456.00";
        var result = Regex.Replace(input, pattern, replacement);
        Console.WriteLine(result);

        pattern = @"\p{Sc}*(?<amount>\s?\d+[.,]?\d*)";
        replacement = "${amount}";
        result = Regex.Replace(input, pattern, replacement);
        Console.WriteLine(result);
    }
}