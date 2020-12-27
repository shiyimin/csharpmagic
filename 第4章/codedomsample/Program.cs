// 源码位置：第五章\codedomsample\Program.cs
// 编译命令：
//    dotnet build
// 运行命令：
//    dotnet run

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.IO;

public class CodeDomSample
{
    CodeCompileUnit GenerateCSharpCode()
    {
        CodeCompileUnit compileUnit = new CodeCompileUnit();
       
        // 创建命名空间 - "namespace CodeDomSampleNS"
        //
        CodeNamespace codedomsamplenamespace = new CodeNamespace("CodeDomSampleNS");
 
        // 创建using子句 - "using System;"
        //
        CodeNamespaceImport firstimport = new CodeNamespaceImport("System");
 
        // 在命名空间里加上这个using子句 -
        // namespace CodeDomSampleNS {
        //      using System;
        //
        codedomsamplenamespace.Imports.Add(firstimport);
 
        // 在命名空间里定义一个类型 - public class CodeDomSample
        //
        CodeTypeDeclaration newType = new CodeTypeDeclaration("CodeDomSample");
        newType.Attributes = MemberAttributes.Public;
 
        // 创建程序入口方法
        // public static void Main
        //
        CodeEntryPointMethod mainmethod = new CodeEntryPointMethod();
 
        // 在入口方法里添加一行语句 -
        //  Console.WriteLine("Inside Main ...");
        CodeMethodInvokeExpression mainexp1 = new CodeMethodInvokeExpression(
            new CodeTypeReferenceExpression("System.Console"),
            "WriteLine", new CodePrimitiveExpression("Inside Main ..."));
        mainmethod.Statements.Add(mainexp1);
 
        // 在入口方法中再添加一行语句，构造一个CodeDomSample实例
        //  CodeDomSample cs = new CodeDomSample()
        //
        CodeStatement cs = new CodeVariableDeclarationStatement(
            typeof(CodeDomSample), "cs", 
            new CodeObjectCreateExpression(new CodeTypeReference(typeof(CodeDomSample))));
        mainmethod.Statements.Add(cs);
       
        // 最终入口方法的C#版本的代码应该是这样的
        // public static void Main() {
        //      Console.WriteLine("Inside Main ...");
        //      CodeDomSample cs = new CodeDomSample();
        // }
 
        // 创建CodeDomSample类的一个无参数构造方法
        // public CodeDomSample() { }
        //
        CodeConstructor constructor = new CodeConstructor();
        constructor.Attributes = MemberAttributes.Public;
 
        // 在构造方法里添加一行语句
        // public CodeDomSample() { Comsole.WriteLine("Inside CodeDomSample Constructor ...");
        //
        CodeMethodInvokeExpression constructorexp = new CodeMethodInvokeExpression(
            new CodeTypeReferenceExpression("System.Console"), "WriteLine", 
            new CodePrimitiveExpression("Inside CodeDomSample Constructor ..."));
        constructor.Statements.Add(constructorexp);
 
        // 将构造方法和入口方法添加到CodeDomSample类里
        //
        newType.Members.Add(constructor);
        newType.Members.Add(mainmethod);
 
        // 将类型添加到命名空间中
        //
        codedomsamplenamespace.Types.Add(newType);
 
        // 将命名空间添加到代码编译单元（CodeCompileUnit）里
        //
        compileUnit.Namespaces.Add(codedomsamplenamespace);
 
        // 返回编译单元 - 通常也就是一个源码文件
        //
        return compileUnit;
    }
 
    void GenerateCode(CodeCompileUnit ccu, String codeprovider)
    {
        CompilerParameters cp = new CompilerParameters();
        String sourceFile;
 
        switch (codeprovider)
        {
            case "CSHARP":
                CSharpCodeProvider csharpcodeprovider = new CSharpCodeProvider();
 
                if (csharpcodeprovider.FileExtension[0] == '.')
                {
                    sourceFile = "CSharpSample" + csharpcodeprovider.FileExtension;
                }
                else
                {
                    sourceFile = "CSharpSample." + csharpcodeprovider.FileExtension;
                }
                IndentedTextWriter tw1 = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
                csharpcodeprovider.GenerateCodeFromCompileUnit(ccu, tw1, new CodeGeneratorOptions());
                tw1.Close();
                
                break;
            case "VBASIC":
                VBCodeProvider vbcodeprovider = new VBCodeProvider();
                if (vbcodeprovider.FileExtension[0] == '.')
                {
                    sourceFile = "VBSample" + vbcodeprovider.FileExtension;
                }
                else
                {
                    sourceFile = "VBSample." + vbcodeprovider.FileExtension;
                }
                IndentedTextWriter tw2 = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
                vbcodeprovider.GenerateCodeFromCompileUnit(ccu, tw2, new CodeGeneratorOptions());
                tw2.Close();
                
                break;
        }

        return;
    }

    static private CompilerResults CompileCode(CodeDomProvider provider, CodeCompileUnit ccu, string fileName)
    {
        var cp = new CompilerParameters();
        // 是生成.exe可执行文件，还是一个.dll类库文件
        cp.GenerateExecutable = true;
        // 输出的文件名
        cp.OutputAssembly = fileName;
        // 输出文件保存到硬盘之中
        cp.GenerateInMemory = false;
        // 是否包含调试信息
        cp.IncludeDebugInformation = true;
        // 允许设置其他编译器选项
        cp.CompilerOptions = "/optimize";
        return provider.CompileAssemblyFromDom(cp, ccu);
    }
 
    static public void Main()
    {
        CodeDomSample cds = new CodeDomSample();
        
        CodeCompileUnit ccu = cds.GenerateCSharpCode();
        cds.GenerateCode(ccu, "CSHARP");
        cds.GenerateCode(ccu, "VBASIC");

        CompilerResults cr = CompileCode(
            new CSharpCodeProvider(), ccu, "csharpdemo.exe");
        Console.WriteLine("编译结果：" + (cr.Errors.Count > 0 ? "失败" : "成功"));
        cr = CompileCode(
            new VBCodeProvider(), ccu, "vbdemo.exe");
        Console.WriteLine("编译结果：" + (cr.Errors.Count > 0 ? "失败" : "成功"));
    }
}