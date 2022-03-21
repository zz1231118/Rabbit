using System;
using IronRabbit;
using IronRabbit.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronRabbitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("f(x)=4^x+5");
            var result = expression.Eval(new RuntimeContext()
            {
                ["x"] = 2,
            });

            Assert.AreEqual(result, 21D);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var domain = new RabbitDomain();
            domain.CompileFromSource("exp(lv)=lv*10");
            var expression = domain.CompileFromSource("maxhp(lv)=exp(lv)+100");
            var str = expression.ToString();
            var result = expression.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });

            Assert.AreEqual(result, 140D);
            Assert.AreEqual(str, "maxhp(lv)=exp(lv)+100");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var domain1 = new RabbitDomain();
            domain1.CompileFromSource("vf(x)=x+1");
            var expr1 = domain1.CompileFromSource("exp(lv)=vf(lv)*10");
            var str1 = expr1.ToString();

            var domain2 = new RabbitDomain();
            domain2.CompileFromSource("vf(x)=x+2");
            var expr2 = domain2.CompileFromSource("exp(lv)=vf(lv)*10");
            var str2 = expr2.ToString();

            var result1 = expr1.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });
            var result2 = expr2.Eval(new RuntimeContext()
            {
                ["lv"] = 4,
            });

            Assert.AreEqual(result1, 50D);
            Assert.AreEqual(str1, "exp(lv)=vf(lv)*10");
            Assert.AreEqual(result2, 60D);
            Assert.AreEqual(str2, "exp(lv)=vf(lv)*10");
        }

        [TestMethod]
        public void TestMethod4()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("maxhp(lv)=sqrt(lv)+100");
            var func = expression.Compile<Func<double, double>>();
            var result = func(4);
            var str = expression.ToString();

            Assert.AreEqual(result, 102D);
            Assert.AreEqual(str, "maxhp(lv)=sqrt(lv)+100");
        }

        [TestMethod]
        public void TestMethod5()
        {
            var domain = new RabbitDomain();
            var expression = domain.CompileFromSource("f(lv)=if(lv<=0,0,5/lv)*10");
            var str = expression.ToString();
            var result = expression.Eval(new RuntimeContext()
            {
                ["lv"] = 5,
            });

            Assert.AreEqual(result, 10D);
            Assert.AreEqual(str, "f(lv)=if(lv<=0,0,5/lv)*10");
        }

        [TestMethod]
        public void TestMethod6()
        {
            var domain = new RabbitDomain();
            domain.CompileFromSource("lv(t)=t+2");
            var expr = domain.CompileFromSource("exp(t)=lv(t)*2");
            var func = expr.Compile<Func<double, double>>();
            Assert.AreEqual(func(6), 16D);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var domain = new RabbitDomain();
            var expr = domain.CompileFromSource(@"
p(lv)=lv+2
f(lv)=p(lv)*10
");
            Assert.AreEqual(expr.Eval(new RuntimeContext() { ["lv"] = 7 }), 90D);
        }
    }
}