//#define DIAGNOSTICS

using Roslyn.Lua.Core.Extensions;
using Roslyn.Lua.Core.LuaSyntaxTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TokenSource = System.Collections.Generic.IEnumerable<Roslyn.Lua.Core.Token>;

namespace Roslyn.Lua.Core
{
    public static class TokenSourceExtensions
    {
        private static readonly List<Type> types = typeof(TokenSourceExtensions).Assembly.GetTypes().ToList();

        public static TokenSource TokenRange(this TokenSource source, TokenType start, TokenType end)
        {
            if (!source.Any() || source.First().Type != start)
                return null;

            List<Token> result = new List<Token> { source.First() };            
            source = source.Skip(1);

            int stack = 1;
            while (stack > 0)
            {
                if (!source.Any())
                    return null;
                if (source.First().Type == start)
                    stack++;
                if (source.First().Type == end)
                    stack--;

                result.Add(source.First());

                source = source.Skip(1);
            }

            return result;
        }

        #if DIAGNOSTICS
        public static int CreateCallCount = 0;
        public static int StackHeight = 0;
        #endif
        [DebuggerStepThrough]
        public static T Create<T>(this TokenSource source) where T : LuaSyntaxNode
        {
            #if DIAGNOSTICS
            CreateCallCount++;
            int localCall = CreateCallCount;
            StackHeight++;
            int dynamicInvokes = 0;
            

            Console.WriteLine();
            Console.WriteLine($"{StackHeight.Of(" ")}Create Called {CreateCallCount} With {typeof(T)} Stacked {StackHeight}");
            Console.WriteLine($"{StackHeight.Of(" ")}{string.Join(" ", source.Select(t => t.Source))}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            #endif
            T ret = null;
            source = source.Where(t => t.Type != TokenType.Whitespace);

            if (!source.Any())                
                goto END;            

            var tokens = source.ToList();

            var methods = types.Where(t => t.BaseTypes().Contains(typeof(T)))
                            .Select(t => t.GetMethod("Create", BindingFlags.Public | BindingFlags.Static))
                            .Where(m => m != null);
            
            ret = methods
                        .Select(m => {
                            #if DIAGNOSTICS
                            dynamicInvokes++;
                            #endif
                            var result = m.Invoke(null, new[] { source });
                            return result;
                        })
                        .Where(r => r != null)
                        .Select(t => (LuaSyntaxNode)t)
                        .OrderByDescending(t => t.Source.Count())
                        .FirstOrDefault() as T;

            END:
            #if DIAGNOSTICS
            stopwatch.Stop();
            Console.WriteLine($"{StackHeight.Of(" ")}End of call {localCall} invoked {dynamicInvokes} factory methods");
            Console.WriteLine($"{StackHeight.Of(" ")}{stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine();
            StackHeight--;
            #endif
            return ret;
        }
    }
}
