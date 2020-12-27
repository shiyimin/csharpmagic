// Windows请使用ildasm反编译获取IL代码
// Mac和Linux请使用命令：
//     monodis EmitDemo1.dll 
using System;

public class EmitDemo
{
    public void DemoMethod(string arg)
    {
        Console.WriteLine("Hello" + arg);
    }
}