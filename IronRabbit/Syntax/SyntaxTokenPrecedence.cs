namespace IronRabbit.Syntax
{
    internal enum SyntaxTokenPrecedence : byte
    {
        Expression,
        Equality,
        Relational,
        Additive,
        Mutiplicative,
    }
}
