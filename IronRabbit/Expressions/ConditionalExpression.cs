using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConditionalExpression : Expression
    {
        internal ConditionalExpression(Expression test, Expression ifTrue, Expression ifFalse)
        {
            Test = test;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
        }

        public override ExpressionType NodeType => ExpressionType.Conditional;

        public override Type Type => IfTrue.Type;

        public Expression Test { get; }

        public Expression IfTrue { get; }

        public Expression IfFalse { get; }

        public override object Eval(RuntimeContext context)
        {
            return (bool)Test.Eval(context) ? IfTrue.Eval(context) : IfFalse.Eval(context);
        }

        public override string ToString()
        {
            return string.Format("if({0},{1},{2})", Test, IfTrue, IfFalse);
        }
    }
}