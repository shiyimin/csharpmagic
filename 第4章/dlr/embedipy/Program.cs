// 需要添加以下几个包：
//     dotnet add package IronPython
//     dotnet add package IronPython.StdLib
// 运行：
//     dotnet run "\sample-code\第五章\dlr\embedipy\dotnetcore.py"
using System;

namespace embedipy
{
    class Program
    {
        static void Main(string[] args)
        {
            // ExecuteSource();
            ExecuteFile(args);
            // ExecutePythonFunction();
        }

        static void ExecuteFile(string[] args)
        {            
            var pythonEngine = 
                IronPython.Hosting.Python.CreateEngine();
            /*
            var searchPaths = pythonEngine.GetSearchPaths();
            searchPaths.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\");
            pythonEngine.SetSearchPaths(searchPaths);
            */
            var pythonScript = 
                pythonEngine.CreateScriptSourceFromFile(args[0], System.Text.Encoding.UTF8);
            Microsoft.Scripting.Hosting.ScriptScope scope = pythonEngine.CreateScope();
            scope.SetVariable("csvariable", "String from C# code");
            pythonScript.Execute(scope);
            Console.WriteLine("csvariable: {0}", scope.GetVariable("csvariable"));
            Console.WriteLine("m: {0}", scope.GetVariable("m"));
        }

        static void ExecuteSource()
        {
            Microsoft.Scripting.Hosting.ScriptEngine pythonEngine = 
                IronPython.Hosting.Python.CreateEngine();
            Microsoft.Scripting.Hosting.ScriptSource pythonScript = 
                pythonEngine.CreateScriptSourceFromString("print 'Hello World!'");
            pythonScript.Execute();
        }

        static void ExecutePythonFunction()
        {
            var pythonEngine = 
                IronPython.Hosting.Python.CreateEngine();
            var pyFunc = @"def isodd(n): return 1 == n % 2;";
            var source = 
                pythonEngine.CreateScriptSourceFromString(pyFunc);
            var scope = pythonEngine.CreateScope();
            source.Execute(scope);
            Func<int, bool> IsOdd = scope.GetVariable<Func<int, bool>>("isodd");

            Console.WriteLine("调用python方法的结果：{0}", IsOdd(101));
        }
    }
}
