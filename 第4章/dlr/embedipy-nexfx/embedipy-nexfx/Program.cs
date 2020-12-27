using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace embedipy_netfx
{
    class Program
    {
        static void Main(string[] args)
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
            pythonScript.Execute();
        }
    }
}

