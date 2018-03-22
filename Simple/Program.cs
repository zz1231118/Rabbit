using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronRabbit;
using IronRabbit.Runtime;

namespace Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            var domain = new RabbitDomain();
            domain.CompileFromSource("nv(x, y) = x + 2 - y");
            var expression = domain.CompileFromSource("exp(lv) = 2^3 * lv - nv(lv, 2)");
            var context = new RuntimeContext();
            context.Variable("lv", 4);
            var result = expression.Eval(context);

            var a = result;
        }
    }
}
