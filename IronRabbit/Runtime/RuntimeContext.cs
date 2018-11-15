using System;
using System.Collections.Generic;

namespace IronRabbit.Runtime
{
    public class RuntimeContext
    {
        private RuntimeContext _parent;
        private Dictionary<string, RuntimeVariable> _variables = new Dictionary<string, RuntimeVariable>();

        public RuntimeContext()
        { }
        internal RuntimeContext(RuntimeContext parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            _parent = parent;
            Domain = parent.Domain;
        }

        public object this[string name]
        {
            set
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                _variables[name] = new RuntimeVariable(value);
            }
        }
        internal RabbitDomain Domain { get; set; }

        private RuntimeContext FindCurrentOrParent(string name)
        {
            return _variables.ContainsKey(name) ? this : _parent?.FindCurrentOrParent(name);
        }

        public void Variable(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _variables[name] = new RuntimeVariable(value);
        }
        public RuntimeVariable Assign(string name, double value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null)
                return null;
            if (!context._variables.TryGetValue(name, out RuntimeVariable rv))
                return null;

            rv.SetValue(value);
            return rv;
        }
        public RuntimeVariable Access(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null)
                return null;
            if (!context._variables.TryGetValue(name, out RuntimeVariable rv))
                return null;

            return rv;
        }

        public class RuntimeVariable
        {
            public RuntimeVariable()
            { }
            public RuntimeVariable(object value)
            {
                Value = value;
                IsAssigned = true;
            }

            public object Value { get; set; }
            public bool IsAssigned { get; set; }

            public void SetValue(object value)
            {
                Value = value;
                IsAssigned = true;
            }
        }
    }
}
