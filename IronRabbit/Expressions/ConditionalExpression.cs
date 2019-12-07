using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConditionalExpression : Expression
    {
        internal ConditionalExpression(Expression test, Expression trueExpression, Expression falseExpression)
            : base(ExpressionType.MethodCall)
        {
            Test = test;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public override ExpressionType NodeType => ExpressionType.Conditional;

        public Expression Test { get; }

        public Expression TrueExpression { get; }

        public Expression FalseExpression { get; }

        public override object Eval(RuntimeContext context)
        {
            return (bool)Test.Eval(context) ? TrueExpression.Eval(context) : FalseExpression.Eval(context);
        }

        public override string ToString()
        {
            return string.Format("if({0}, {1}, {2})", Test, TrueExpression, FalseExpression);
        }
    }

}