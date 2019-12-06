using System;
using System.Linq;
using IronRabbit;
using IronRabbit.Runtime;

namespace Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            var exp = IronRabbit.Rabbit.CompileFromSource("f(times,nash,depr)=nash*(1-depr)*0.6");
            Test7();
            Console.ReadKey();
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
            var str = expression.ToString();
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
            var str1 = expr1.ToString();

            var rhdomain = new RabbitDomain();
            rhdomain.CompileFromSource("vf(x)=x+2");
            var expr2 = rhdomain.CompileFromSource("exp(lv)=vf(lv)*10");
            var str2 = expr2.ToString();

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
            var str = expression.ToString();
            var a = result;
        }
        static void Test5()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("f(lv)=if(lv<=0,0,5/lv)*10");
            var str = expression.ToString();
            var result = expression.Eval(new RuntimeContext()
            {
                ["lv"] = 5,
            });

            var a = result;
        }
        static void Test6()
        {
            var formula = "f(pcv,tsv,pac,tac,sro,nsro,fro)=(if(tsv>0,pcv/tsv,0)*1.5+if(tac>0,pac/tac,0)*0.5+if(fro>0,sro/fro,0)+if(fro>0,nsro/fro,0)*0.2)*10000";
            var expression = IronRabbit.Rabbit.CompileFromSource(formula);
            var eval = expression.Compile<Func<double, double, double, double, double, double, double, double>>();
            double contribution = 112;
            double development = 22673249;
            double areaCount = 12;
            double sceneSellCount = 39919;
            double sro = 0;
            double nsro = 373784.15;
            double roFlowing = 1560100.44;

            var val = eval(contribution, development, areaCount, sceneSellCount, sro, nsro, roFlowing);
        }
        static void Test7()
        {
            //var formula = "f(days,per)=max(0.1,1-floor(days/10)*per)";
            var formula = "f(days,per)=max(0.1,1.0/(2^floor(days/10)))";
            var expression = Rabbit.CompileFromSource(formula);
            var factory = expression.Compile<Func<double, double, double>>();
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < 100; i++)
            {
                var value = factory(i, 0.2);
                //var value = expression.Eval(new RuntimeContext()
                //{
                //    ["days"] = i,
                //    ["per"] = 0.2
                //});
                sb.AppendFormat("{0}: {1}", i, value);
                sb.AppendLine();
            }

            var str = sb.ToString();
        }
    }
}
