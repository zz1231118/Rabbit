using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public abstract class Expression
    {
        protected Expression()
        { }

        public abstract ExpressionType NodeType { get; }

        public abstract Type Type { get; }

        public static ParameterExpression Parameter(Type type, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new ParameterExpression(ExpressionType.Parameter, type, name);
        }

        public static ParameterExpression Variable(Type type, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new ParameterExpression(ExpressionType.Variable, type, name);
        }

        public static MemberExpression Member(Expression instance, string name)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }

        public static MemberExpression Field(Expression instance, string name)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }

        public static MemberExpression Property(Expression instance, string name)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return new MemberExpression(instance, name);
        }

        public static BinaryExpression Assign(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Assign, left, right, right.Type);
        }

        public static ConstantExpression Constant(object value)
        {
            return new ConstantExpression(value);
        }

        public static UnaryExpression Convert(Expression expression, Type type)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new UnaryExpression(ExpressionType.Constant, expression);
        }

        public static BinaryExpression Add(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Add, left, right, left.Type);
        }

        public static BinaryExpression Subtract(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Subtract, left, right, left.Type);
        }

        public static BinaryExpression Multiply(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Multiply, left, right, left.Type);
        }

        public static BinaryExpression Divide(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Divide, left, right, left.Type);
        }

        public static BinaryExpression Modulo(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Modulo, left, right, left.Type);
        }

        public static BinaryExpression Power(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new BinaryExpression(ExpressionType.Power, left, right, left.Type);
        }

        public static UnaryExpression Negate(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new UnaryExpression(ExpressionType.Negate, expression);
        }

        public static BinaryExpression ArrayIndex(Expression array, Expression index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (index == null) throw new ArgumentNullException(nameof(index));

            return new BinaryExpression(ExpressionType.ArrayIndex, array, index, array.Type);
        }

        public static UnaryExpression ArrayLength(Expression array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return new UnaryExpression(ExpressionType.ArrayLength, array);
        }

        public static MethodCallExpression Call(Expression instance, string methodName, params Expression[] arguments)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            return new MethodCallExpression(instance, methodName, arguments);
        }

        public static MethodCallExpression Call(Expression? instance, string methodName, IList<Expression> arguments)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));

            return new MethodCallExpression(instance, methodName, arguments);
        }

        public static LambdaExpression Lambda(string name, Expression body, params ParameterExpression[] parameters)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return new LambdaExpression(name, body, parameters);
        }

        public static LambdaExpression Lambda(string name, Expression body, IList<ParameterExpression> parameters)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return new LambdaExpression(name, body, parameters.ToArray());
        }

        public static UnaryExpression Not(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new UnaryExpression(ExpressionType.Not, expression);
        }

        public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
        {
            if (test == null) throw new ArgumentNullException(nameof(test));
            if (ifTrue == null) throw new ArgumentNullException(nameof(ifTrue));
            if (ifFalse == null) throw new ArgumentNullException(nameof(ifFalse));

            return new ConditionalExpression(test, ifTrue, ifFalse);
        }

        public virtual object Eval(RuntimeContext context)
        {
            throw new NotImplementedException();
        }
    }
}