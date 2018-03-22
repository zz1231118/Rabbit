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
            Test3();
        }

        static void Test1()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("f(x)=4^x+5");
            var result = expression.Eval(new RuntimeContext()
            {
                ["x"] = 2,
            });

            var a = result;
        }
        static void Test2()
        {
            var domain = new RabbitDomain();
            domain.CompileFromSource("exp(lv)=lv*10");
            var expression = domain.CompileFromSource("maxhp(lv)=exp(lv)+100");
            var result = expression.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });

            var a = result;
        }
        static void Test3()
        {
            var lhdomain = new RabbitDomain();
            var expr1 = lhdomain.CompileFromSource("exp(lv)=lv*10");

            var rhdomain = new RabbitDomain();
            var expr2 = rhdomain.CompileFromSource("exp(lv)=lv*20");

            var result1 = expr1.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });
            var result2 = expr2.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });

            var a = result1;
        }
    }
}
