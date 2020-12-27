// 代码位置：\第五章\reflection-emit\Program.cs
// 编译和运行方法：
//  dotnet build
//  dotnet run
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace reflection_emit
{
    class Program
    {
        static void Main(string[] args)
        {
            var an = new AssemblyName("EmitDemo");
            // AppDomain ad = AppDomain.CurrentDomain;
            // AssemblyBuilder builder = ad.DefineDynamicAssembly(
            //    an, AssemblyBuilderAccess.RunAndSave);
            AssemblyBuilder builder = AssemblyBuilder.DefineDynamicAssembly(
                an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule("Main");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "EmitDemo", TypeAttributes.Public | TypeAttributes.Class);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("DemoMethod", 
                MethodAttributes.Public, 
                typeof(void), 
                new Type[] { typeof(string) });
            ILGenerator generator = methodBuilder.GetILGenerator();
            // ldstr "Hello"
            generator.Emit(OpCodes.Ldstr, "Hello ");
            // ldarg.1
            generator.Emit(OpCodes.Ldarg_1);
            // call string string::Concat(string, string)
            generator.Emit(OpCodes.Call, 
                typeof(string).GetMethod(
                    "Concat", new Type[] { 
                        typeof(string), typeof(string)
                    }
                )
            );
            // call void class [mscorlib]System.Console::WriteLine(string)
            generator.Emit(OpCodes.Call, 
                typeof(Console).GetMethod(
                    "WriteLine", new Type[] { 
                        typeof(string)
                    }
                )
            );
            // ret
            generator.Emit(OpCodes.Ret);

            Type t = typeBuilder.CreateType();
            object instance = Activator.CreateInstance(t);
            t.InvokeMember("DemoMethod", 
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, 
                null, instance, new object[] { "Emitted C# code" });

            dynamic di = instance;
            di.DemoMethod("Dynamic");
            // builder.Save("EmitDemo.dll");
        }
    }
}
