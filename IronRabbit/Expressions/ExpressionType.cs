namespace IronRabbit.Expressions
{
    public enum ExpressionType : byte
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Power,

        Constant,
        ArrayIndex,
        ArrayLength,
        Negate,
        Convert,
        Assign,

        MemberAccess,
        Variable,
        Parameter,
        Lambda,
        MethodCall,

        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThanOrEqual,
        GreaterThan,
        NotEqual,

        Not,
        Conditional,
    }
}
