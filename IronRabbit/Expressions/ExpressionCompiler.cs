using System;
using System.Collections.Generic;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    internal class ExpressionCompiler
    {
        private readonly Dictionary<string, LambdaCompiler> lambdas = new Dictionary<string, LambdaCompiler>();

        private LambdaCompiler GetLambdaCompiler(LambdaExpression expression)
        {
            if (!lambdas.TryGetValue(expression.Name, out var lambda))
            {
                lambda = new LambdaCompiler(this, expression);
                lambdas.Add(expression.Name, lambda);
            }
            return lambda;
        }

        public Delegate Compile(LambdaExpression expression, Type delegateType = null)
        {
            var lambda = new LambdaCompiler(this, expression);
            lambdas.Add(lambda.Name, lambda);
            return lambda.Compile(delegateType);
        }

        sealed class LambdaCompiler
        {
            private readonly ExpressionCompiler compiler;
            private readonly LambdaExpression expression;
            private readonly List<System.Linq.Expressions.ParameterExpression> parameters;

            public LambdaCompiler(ExpressionCompiler compiler, LambdaExpression expression)
            {
                this.compiler = compiler;
                this.expression = expression;
                this.parameters = new List<System.Linq.Expressions.ParameterExpression>();
                foreach (var parameter in expression.Parameters)
                {
                    parameters.Add(System.Linq.Expressions.Expression.Parameter(parameter.Type, parameter.Name));
                }
            }

            public string Name => expression.Name;

            private System.Linq.Expressions.Expression EmitExpression(Expression expression)
            {
                switch (expression)
                {
                    case ConditionalExpression conditionExpression:
                        {
                            var test = EmitExpression(conditionExpression.Test);
                            var trueExpression = EmitExpression(conditionExpression.TrueExpression);
                            var falseExpression = EmitExpression(conditionExpression.FalseExpression);
                            return System.Linq.Expressions.Expression.Condition(test, trueExpression, falseExpression);
                        }
                    case BinaryExpression binaryExpression:
                        {
                            var left = EmitExpression(binaryExpression.Left);
                            var right = EmitExpression(binaryExpression.Right);
                            return expression.NodeType switch
                            {
                                ExpressionType.Add => System.Linq.Expressions.Expression.Add(left, right),
                                ExpressionType.Subtract => System.Linq.Expressions.Expression.Subtract(left, right),
                                ExpressionType.Multiply => System.Linq.Expressions.Expression.Multiply(left, right),
                                ExpressionType.Divide => System.Linq.Expressions.Expression.Divide(left, right),
                                ExpressionType.Modulo => System.Linq.Expressions.Expression.Modulo(left, right),
                                ExpressionType.Power => System.Linq.Expressions.Expression.Power(left, right),
                                ExpressionType.LessThan => System.Linq.Expressions.Expression.LessThan(left, right),
                                ExpressionType.LessThanOrEqual => System.Linq.Expressions.Expression.LessThanOrEqual(left, right),
                                ExpressionType.Equal => System.Linq.Expressions.Expression.Equal(left, right),
                                ExpressionType.GreaterThanOrEqual => System.Linq.Expressions.Expression.GreaterThanOrEqual(left, right),
                                ExpressionType.GreaterThan => System.Linq.Expressions.Expression.GreaterThan(left, right),
                                ExpressionType.NotEqual => System.Linq.Expressions.Expression.NotEqual(left, right),
                                _ => throw new LambdaCompilerException("unknown operator char:" + expression.NodeType.ToString()),
                            };
                        }
                    case ConstantExpression constantExpression:
                        return System.Linq.Expressions.Expression.Constant(constantExpression.Value);
                    case MethodCallExpression methodCallExpression:
                        {
                            var lambda = methodCallExpression.GetLambda(this.expression.Domain);
                            if (lambda == null) throw new LambdaCompilerException(string.Format("missing method:{0}", methodCallExpression.MethodName));
                            if (lambda is SystemLambdaExpression systemLambda)
                            {
                                var arguments = new List<System.Linq.Expressions.Expression>();
                                foreach (var argument in methodCallExpression.Arguments)
                                {
                                    arguments.Add(EmitExpression(argument));
                                }
                                return System.Linq.Expressions.Expression.Call(null, systemLambda.Method, arguments);
                            }
                            else
                            {
                                var elementType = typeof(object);
                                var customLambda = compiler.GetLambdaCompiler(lambda);
                                var arguments = new System.Linq.Expressions.Expression[customLambda.parameters.Count];
                                for (var i = 0; i < arguments.Length; i++)
                                {
                                    var parameter = customLambda.parameters[i];
                                    var argument = EmitExpression(methodCallExpression.Arguments[i]);
                                    arguments[i] = parameter.Type != elementType
                                        ? System.Linq.Expressions.Expression.Convert(argument, elementType)
                                        : argument;
                                }

                                var custom = customLambda.Compile();
                                return System.Linq.Expressions.Expression.Convert(
                                    System.Linq.Expressions.Expression.Call(
                                        System.Linq.Expressions.Expression.Constant(custom),
                                        typeof(Delegate).GetMethod(nameof(Delegate.DynamicInvoke)),
                                        System.Linq.Expressions.Expression.NewArrayInit(elementType, arguments)),
                                    lambda.Type);
                            }
                        }
                    case ParameterExpression parameterExpression:
                        {
                            var parameter = parameters.Find(p => p.Name == parameterExpression.Name);
                            if (parameter == null) throw new RuntimeException("Missing member:" + parameterExpression.Name);
                            return parameter;
                        }
                    case UnaryExpression unaryExpression:
                        {
                            var operand = EmitExpression(unaryExpression.Operand);
                            return unaryExpression.NodeType switch
                            {
                                ExpressionType.Negate => System.Linq.Expressions.Expression.Negate(operand),
                                ExpressionType.Not => System.Linq.Expressions.Expression.Not(operand),
                                _ => throw new RuntimeException("unknown NodeType:" + unaryExpression.NodeType.ToString()),
                            };
                        }
                    default:
                        return null;
                }
            }

            public Delegate Compile(Type delegateType = null)
            {
                var body = EmitExpression(this.expression.Body);
                var lambda = delegateType == null
                    ? System.Linq.Expressions.Expression.Lambda(body, this.expression.Name, parameters)
                    : System.Linq.Expressions.Expression.Lambda(delegateType, body, this.expression.Name, parameters);
                return lambda.Compile();
            }
        }
    }
}