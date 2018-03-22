using System;
using System.Collections.Generic;

namespace IronRabbit.Runtime
{
    public class RuntimeContext
    {
        private RuntimeContext _parent;
        private Dictionary<string, decimal> _variables = new Dictionary<string, decimal>();

        public RuntimeContext()
        { }
        internal RuntimeContext(RuntimeContext parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            _parent = parent;
            Domain = parent.Domain;
        }

        public decimal this[string name]
        {
            set
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                _variables[name] = value;
            }
        }
        internal RabbitDomain Domain { get; set; }

        private RuntimeContext FindCurrentOrParent(string name)
        {
            return _variables.ContainsKey(name) ? this : _parent?.FindCurrentOrParent(name);
        }

        public void Variable(string name, decimal value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _variables[name] = value;
        }
        public decimal? Assign(string name, decimal value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null)
                return null;
            if (!context._variables.ContainsKey(name))
                return null;

            context._variables[name] = value;
            return value;
        }
        public decimal? Access(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null)
                return null;
            if (!context._variables.TryGetValue(name, out decimal value))
                return null;

            return value;
        }
    }
}
