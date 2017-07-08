using Roslyn.Lua.Core.LuaSyntaxTree;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Roslyn.Lua.Core.Interpreter
{
    public class LuaState : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, dynamic> _locals = new Dictionary<string, dynamic>();

        private readonly LuaState _parentState;
        public LuaState Parent => _parentState;
        public LuaState Child { get; private set; }

        public LuaState()
        {
            _parentState = null;
        }

        protected LuaState(LuaState parent)
        {
            _parentState = parent;
            parent.Child = this;
        }
        public LuaState Push() => new LuaState(this);

        public LuaState Pop()
        {
            _parentState.Child = null;
            return _parentState;
        }

        public dynamic this[string identifier]
        {
            get
            {
                if (_locals.ContainsKey(identifier))
                    return _locals[identifier];
                else if (_parentState != null)
                    return _parentState[identifier];
                else
                    return null;
            }
        }
        
        public void Set(string identifier, dynamic value)
        {
            if (_locals.ContainsKey(identifier))
                _locals[identifier] = value;
            else if (_parentState == null)
                _locals[identifier] = value;
            else
                _parentState.Set(identifier, value);
        }

        public void SetLocal(string identifier, dynamic value)
        {
            _locals[identifier] = value;
        }

        public IEnumerable<dynamic> Execute(LuaSyntaxNode lua)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator() => 
            _locals
                .And(_parentState ?? Enumerable.Empty<KeyValuePair<string, dynamic>>())
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
