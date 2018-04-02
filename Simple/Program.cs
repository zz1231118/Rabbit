using System;
using IronRabbit;
using IronRabbit.Runtime;

namespace Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            Test4();
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
            lhdomain.CompileFromSource("vf(x)=x+1");
            var expr1 = lhdomain.CompileFromSource("exp(lv)=vf(lv)*10");

            var rhdomain = new RabbitDomain();
            rhdomain.CompileFromSource("vf(x)=x+2");
            var expr2 = rhdomain.CompileFromSource("exp(lv)=vf(lv)*10");

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
        static void Test4()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("maxhp(lv)=sqrt(lv)+100");
            var func = expression.Compile<Func<double, double>>();
            var result = func(4);
            var a = result;
        }
    }
}
