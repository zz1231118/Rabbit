namespace IronRabbit.Runtime
{
    public class RuntimeContext
    {
        private readonly Dictionary<string, RuntimeVariable> variables = new Dictionary<string, RuntimeVariable>();
        private readonly RuntimeContext? parent;

        public RuntimeContext()
        { }

        internal RuntimeContext(RuntimeContext parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            this.parent = parent;
            this.Domain = parent.Domain;
        }

        public object this[string name]
        {
            set
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                variables[name] = new RuntimeVariable(value);
            }
        }

        internal RabbitDomain? Domain { get; set; }

        public static RuntimeContext Variable(string name, object value)
        {
            var context = new RuntimeContext();
            context.Define(name, value);
            return context;
        }

        public static RuntimeContext Variable(params (string name, object value)[] variables)
        {
            var context = new RuntimeContext();
            foreach (var variable in variables)
            {
                context.Define(variable.name, variable.value);
            }

            return context;
        }

        private RuntimeContext? FindCurrentOrParent(string name)
        {
            return variables.ContainsKey(name) ? this : parent?.FindCurrentOrParent(name);
        }

        public RuntimeVariable Define(string name, object value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var variable = new RuntimeVariable(value);
            variables[name] = variable;
            return variable;
        }

        public RuntimeVariable? Assign(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null) return null;
            if (!context.variables.TryGetValue(name, out var rv)) return null;

            rv.SetValue(value);
            return rv;
        }

        public RuntimeVariable? Access(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var context = FindCurrentOrParent(name);
            if (context == null) return null;
            if (!context.variables.TryGetValue(name, out var rv)) return null;
            return rv;
        }

        public class RuntimeVariable
        {
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
