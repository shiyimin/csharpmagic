// 编译命令：csc /r:assemblyversion.dll Program.cs
public static class Program
{
    public static void Main()
    {
        AssemblyVersionDemo.Main();
        Console.WriteLine(typeof(AssemblyVersionDemo).Assembly.FullName);
    }
}