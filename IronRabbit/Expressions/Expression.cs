﻿using System;
using System.Collections.Generic;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public abstract class Expression
    {
        protected Expression()
        { }
        protected Expression(ExpressionType nodeType)
        {
            NodeType = nodeType;
        }

        public virtual ExpressionType NodeType { get; }

        public static ParameterExpression Parameter(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new ParameterExpression(ExpressionType.Parameter, name);
        }
        public static ParameterExpression Variable(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new ParameterExpression(ExpressionType.Variable, name);
        }
        public static MemberExpression Member(Expression instance, string name)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }
        public static MemberExpression Field(Expression instance, string name)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }
        public static MemberExpression Property(Expression instance, string name)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }
        public static BinaryExpression Assign(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Assign, left, right);
        }
        public static ConstantExpression Constant(decimal value)
        {
            return new ConstantExpression(value);
        }
        public static UnaryExpression Convert(Expression expression, Type type)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return new UnaryExpression(ExpressionType.Constant, expression);
        }
        public static BinaryExpression Add(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Add, left, right);
        }
        public static BinaryExpression Subtract(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Subtract, left, right);
        }
        public static BinaryExpression Multiply(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Multiply, left, right);
        }
        public static BinaryExpression Divide(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Divide, left, right);
        }
        public static BinaryExpression Modulo(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Modulo, left, right);
        }
        public static BinaryExpression Power(Expression left, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Power, left, right);
        }
        public static UnaryExpression Negate(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new UnaryExpression(ExpressionType.Negate, expression);
        }
        public static BinaryExpression ArrayIndex(Expression array, Expression index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index == null)
                throw new ArgumentNullException(nameof(index));

            return new BinaryExpression(ExpressionType.ArrayIndex, array, index);
        }
        public static UnaryExpression ArrayLength(Expression array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            return new UnaryExpression(ExpressionType.ArrayLength, array);
        }
        public static MethodCallExpression Call(Expression instance, string methodName, params Expression[] arguments)
        {
            if (methodName == null)
                throw new ArgumentNullException(nameof(methodName));

            return new MethodCallExpression(instance, methodName, arguments);
        }
        public static MethodCallExpression Call(Expression instance, string methodName, IList<Expression> arguments)
        {
            if (methodName == null)
                throw new ArgumentNullException(nameof(methodName));
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            return new MethodCallExpression(instance, methodName, arguments);
        }
        public static LambdaExpression Lambda(string name, Expression body, params ParameterExpression[] parameters)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            return new LambdaExpression(name, parameters, body);
        }
        public static LambdaExpression Lambda(string name, Expression body, IList<ParameterExpression> parameters)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            return new LambdaExpression(name, parameters, body);
        }

        public virtual decimal Eval(RuntimeContext context)
        {
            return default(decimal);
        }
    }
}