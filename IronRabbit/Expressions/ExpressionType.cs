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

        /// <summary>
        /// &lt;
        /// </summary>
        LessThan,
        /// <summary>
        /// &lt;=
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// ==
        /// </summary>
        Equal,
        /// <summary>
        /// &gt;=
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// &gt;
        /// </summary>
        GreaterThan,
        /// <summary>
        /// !=
        /// </summary>
        NotEqual,

        /// <summary>
        /// !
        /// </summary>
        Not,
        /// <summary>
        /// IF
        /// </summary>
        Conditional,
    }
}
