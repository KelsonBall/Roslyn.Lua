using Roslyn.Lua.Core;
using Roslyn.Lua.Core.LuaSyntaxTree;
using System;
using System.Linq;
using System.Xml;

namespace Roslyn.Lua
{
    class Compiler
    {
        const string exampleSource =
@"function exit()
	return 3
end

x = 4


function add(a, b)
	return a + b
end
";
        static void Main(string[] args)
        {
            Console.WriteLine("Source:");
            Console.WriteLine(exampleSource);
            Console.WriteLine("Syntax Tree:");
            var tree = exampleSource.Tokens().ToList().Create<Program>();
            var builder = tree.SerializeToXml(new System.Text.StringBuilder());
            Console.WriteLine(builder.ToString());
            var doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            
        }
    }
}
