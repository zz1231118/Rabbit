using System;
using System.Collections.Generic;

namespace IronRabbit.Runtime
{
    public class RuntimeContext
    {
        private RuntimeContext _parent;
        private Dictionary<string, double> _variables = new Dictionary<string, double>();

        public RuntimeContext()
        { }
        internal RuntimeContext(RuntimeContext parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            _parent = parent;
            Domain = parent.Domain;
        }

        public double this[string name]
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

        public void Variable(string name, double value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _variables[name] = value;
        }
        public double? Assign(string name, double value)
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
        public double? Access(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null)
                return null;
            if (!context._variables.TryGetValue(name, out double value))
                return null;

            return value;
        }
    }
}
