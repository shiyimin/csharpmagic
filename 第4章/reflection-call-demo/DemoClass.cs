// 源码位置：第五章\reflection-call-demo\DemoClass.cs
// 编译命令：csc /t:library DemoClass.cs
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
        Value += value;
    }
}