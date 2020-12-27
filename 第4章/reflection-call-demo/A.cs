// 源码位置：第五章\reflection-call-demo\A.cs
// 编译命令：csc /t:library /r:B.dll /r:C.dll A.cs
public class DemoClass
{
    public int Value { get; private set; }

    public DemoClass(int v) { Value = v;}

    static int Add(int left, int right)
    {
        return left + right;
    }

    public void Add(int value)
    {
        B.HelloWorld();
        C.Output("A Add");
    }
}