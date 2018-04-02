using System;

namespace IronRabbit.Extern
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ExternLambdaAttribute : Attribute
    { }
}
