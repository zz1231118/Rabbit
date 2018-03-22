namespace IronRabbit.Compiler
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

        Comma,
        Assign,
        Dot,

        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
    }
}
