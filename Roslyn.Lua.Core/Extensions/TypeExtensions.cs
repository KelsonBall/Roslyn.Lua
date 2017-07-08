using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> BaseTypes(this Type type)
        {
            do
                yield return type;
            while ((type = type.BaseType) != null) ;                
        }
    }
}
