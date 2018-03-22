using System;
using System.Collections.ObjectModel;

namespace IronRabbit.Extern
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ExternMethodAttribute : Attribute
    {
        public ExternMethodAttribute(string entryPoint, params string[] parameters)
        {
            EntryPoint = entryPoint;
            Parameters = new ReadOnlyCollection<string>(parameters);
        }

        public string EntryPoint { get; }
        public ReadOnlyCollection<string> Parameters { get; }
    }
}
