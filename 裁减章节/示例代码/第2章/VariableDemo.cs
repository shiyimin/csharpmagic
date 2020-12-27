// 源码位置：第二章\VariableDemo.cs
using System;

public class VariableDemo
{
	public static void Main()
	{
        int value;
        int itemCount, employeeNumber;
        // 因为value没有任何初始值，所以下面的语句会导致一个编译错误
        // Console.WriteLine("{0}", value);
        int employeeCount = 10000;
        var index = 0;
        double defaultDoubleValue = 0.0, doubleValue = 0.0d;
        float floatValue = 0.0f;
        decimal decimalValue1 = 0.0m, decimalValue2 = 0.0M;
        ulong ulongValue = 123ul;
        byte byteValue1 = 0x00AB, byteValue2 = 0b1100_1001;
    }
}
