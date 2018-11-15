namespace IronRabbit.Syntax
{
    internal enum TokenKind : byte
    {
        EndOfFile,
        NewLine,
        Error,
        Identifier,
        Constant,
        Comment,

        Add,
        Subtract,
        Multiply,
        Divide,
        Mod,
        Power,

        /// <summary>
        /// ,
        /// </summary>
        Comma,
        /// <summary>
        /// =
        /// </summary>
        Assign,
        /// <summary>
        /// .
        /// </summary>
        Dot,

        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,

        Not,
        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThanOrEqual,
        GreaterThan,
        NotEqual,

        IF,
    }
}
