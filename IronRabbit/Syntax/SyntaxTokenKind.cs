namespace IronRabbit.Syntax
{
    internal enum SyntaxTokenKind : byte
    {
        EndOfFile,
        NewLine,
        Symbol,
        Numeric,
        Annotation,

        /// <summary>
        /// +
        /// </summary>
        Add,
        /// <summary>
        /// -
        /// </summary>
        Subtract,
        /// <summary>
        /// *
        /// </summary>
        Multiply,
        /// <summary>
        /// /
        /// </summary>
        Divide,
        /// <summary>
        /// %
        /// </summary>
        Mod,
        /// <summary>
        /// ^
        /// </summary>
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

        /// <summary>
        /// (
        /// </summary>
        LeftParen,
        /// <summary>
        /// )
        /// </summary>
        RightParen,
        /// <summary>
        /// [
        /// </summary>
        LeftBracket,
        /// <summary>
        /// ]
        /// </summary>
        RightBracket,

        /// <summary>
        /// !
        /// </summary>
        Not,
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
    }
}
