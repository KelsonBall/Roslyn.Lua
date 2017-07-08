using KelsonBall.Testing;
using Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowExpressions
{
    [TestClass]
    public class FunctionStatementTest
    {        
        [TestMethod]
        public void FunctionFactoryTest_ExpectsSingleFunction()
        {
            var func = "function add(a, b) return a + b end"
                        .Tokens()
                        .Create<FunctionStatement>();

            Assert.True(func != null);
            Assert.True(func.Name.Identifier.Source == "add");
            Assert.True((func.Statements.First() as ReturnStatement) != null);
        }

        [TestMethod]
        public void FunctionFactoryTest_NestedFunction()
        {
            var func = "function outer() function inner() x = 3 end end"
                        .Tokens()
                        .Create<FunctionStatement>();

            Assert.True(func.Name.Identifier.Source == "outer");
            Assert.True(((FunctionStatement)func.Statements.Single()).Statements.Count() == 1);
        }

        [TestMethod]
        public void FunctionFactoryTest_NoFunction()
        {
            var func = "lol = true"
                        .Tokens()
                        .Create<FunctionStatement>();

            Assert.True(func == null);
        }

        [TestMethod]
        public void FunctionFactoryTest_NoMatchingEndToken()
        {
            var func = "function outer() return 3"
                        .Tokens()
                        .Create<FunctionStatement>();

            Assert.True(func == null);
        }
    }
}
