﻿using System;
using System.Collections.Generic;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    internal class LambdaCompiler
    {
        private LambdaExpression lambda;
        private List<System.Linq.Expressions.ParameterExpression> parameters;

        public LambdaCompiler(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            this.lambda = lambda;
        }

        private System.Linq.Expressions.Expression EmitExpression(Expression expre)
        {
            if (expre is ConditionalExpression conditionExpre)
            {
                var test = EmitExpression(conditionExpre.Test);
                var trueExpre = EmitExpression(conditionExpre.TrueExpression);
                var falseExpre = EmitExpression(conditionExpre.FalseExpression);
                return System.Linq.Expressions.Expression.Condition(test, trueExpre, falseExpre);
            }
            else if (expre is BinaryExpression be)
            {
                var left = EmitExpression(be.Left);
                var right = EmitExpression(be.Right);
                switch (expre.NodeType)
                {
                    case ExpressionType.Add:
                        return System.Linq.Expressions.Expression.Add(left, right);
                    case ExpressionType.Subtract:
                        return System.Linq.Expressions.Expression.Subtract(left, right);
                    case ExpressionType.Multiply:
                        return System.Linq.Expressions.Expression.Multiply(left, right);
                    case ExpressionType.Divide:
                        return System.Linq.Expressions.Expression.Divide(left, right);
                    case ExpressionType.Modulo:
                        return System.Linq.Expressions.Expression.Modulo(left, right);
                    case ExpressionType.Power:
                        return System.Linq.Expressions.Expression.Power(left, right);

                    case ExpressionType.LessThan:
                        return System.Linq.Expressions.Expression.LessThan(left, right);
                    case ExpressionType.LessThanOrEqual:
                        return System.Linq.Expressions.Expression.LessThanOrEqual(left, right);
                    case ExpressionType.Equal:
                        return System.Linq.Expressions.Expression.Equal(left, right);
                    case ExpressionType.GreaterThanOrEqual:
                        return System.Linq.Expressions.Expression.GreaterThanOrEqual(left, right);
                    case ExpressionType.GreaterThan:
                        return System.Linq.Expressions.Expression.GreaterThan(left, right);
                    case ExpressionType.NotEqual:
                        return System.Linq.Expressions.Expression.NotEqual(left, right);
                    default:
                        throw new LambdaCompilerException("unknown operator char:" + expre.NodeType.ToString());
                }
            }
            else if (expre is ConstantExpression ce)
            {
                return System.Linq.Expressions.Expression.Constant(ce.Value);
            }
            else if (expre is MethodCallExpression mce)
            {
                var lambda = mce.GetLambda(this.lambda.Domain);
                if (lambda == null)
                    throw new LambdaCompilerException(string.Format("missing method:{0}", mce.MethodName));
                if (!(lambda is SystemLambdaExpression sle))
                    throw new LambdaCompilerException("method call not supported!");

                var arguments = new List<System.Linq.Expressions.Expression>();
                foreach (var argument in mce.Arguments)
                {
                    arguments.Add(EmitExpression(argument));
                }
                return System.Linq.Expressions.Expression.Call(null, sle.Method, arguments);
            }
            else if (expre is ParameterExpression pe)
            {
                var parameter = parameters.Find(p => p.Name == pe.Name);
                if (parameter == null)
                    throw new RuntimeException("Missing member:" + pe.Name);

                return parameter;
            }
            else if (expre is UnaryExpression ue)
            {
                var operand = EmitExpression(ue.Operand);
                switch (ue.NodeType)
                {
                    case ExpressionType.Negate:
                        return System.Linq.Expressions.Expression.Negate(operand);
                    case ExpressionType.Not:
                        return System.Linq.Expressions.Expression.Not(operand);
                    default:
                        throw new RuntimeException("unknown NodeType:" + ue.NodeType.ToString());
                }
            }

            return null;
        }

        public Delegate Compile(Type delegateType = null)
        {
            parameters = new List<System.Linq.Expressions.ParameterExpression>();
            foreach (var parameter in this.lambda.Parameters)
            {
                parameters.Add(System.Linq.Expressions.Expression.Parameter(parameter.Type, parameter.Name));
            }

            var body = EmitExpression(this.lambda.Body);
            var lambda = delegateType == null
                ? System.Linq.Expressions.Expression.Lambda(body, this.lambda.Name, parameters)
                : System.Linq.Expressions.Expression.Lambda(delegateType, body, this.lambda.Name, parameters);
            return lambda.Compile();
        }
    }
}