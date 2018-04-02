using System;
using System.Collections.Generic;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    internal class LambdaCompiler
    {
        private LambdaExpression _lambda;
        private List<System.Linq.Expressions.ParameterExpression> _parameters;

        public LambdaCompiler(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            _lambda = lambda;
        }

        public Delegate Compile(Type delegateType = null)
        {
            _parameters = new List<System.Linq.Expressions.ParameterExpression>();
            foreach (var parameter in _lambda.Parameters)
            {
                _parameters.Add(System.Linq.Expressions.Expression.Parameter(parameter.Type, parameter.Name));
            }

            var body = EmitExpression(_lambda.Body);
            var lambda = delegateType == null
                ? System.Linq.Expressions.Expression.Lambda(body, _lambda.Name, _parameters)
                : System.Linq.Expressions.Expression.Lambda(delegateType, body, _lambda.Name, _parameters);
            return lambda.Compile();
        }

        private System.Linq.Expressions.Expression EmitExpression(Expression expre)
        {
            if (expre is BinaryExpression be)
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
                var lambda = mce.GetLambda(_lambda.Domain);
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
                var parameter = _parameters.Find(p => p.Name == pe.Name);
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
                    default:
                        throw new RuntimeException("unknown NodeType:" + ue.NodeType.ToString());
                }
            }

            return null;
        }
    }
}