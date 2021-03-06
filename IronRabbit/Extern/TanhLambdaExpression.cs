﻿using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class TanhLambdaExpression : SystemLambdaExpression
    {
        public TanhLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Tanh), new Type[] { typeof(double) }), "tanh", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Tanh(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
