using IronRabbit.Compiler;

namespace IronRabbit.Syntax
{
    internal readonly struct SyntaxToken
    {
        private readonly SyntaxTokenKind kind;
        private readonly SourceSpan span;
        private readonly string text;
        private readonly object value;

        public SyntaxToken(SyntaxTokenKind kind, string text, object value, SourceSpan span)
        {
            this.text = text;
            this.kind = kind;
            this.span = span;
            this.value = value;
        }

        public SyntaxTokenKind Kind => kind;

        public SourceSpan Span => span;

        public string Text => text;

        public object Value => value;

        public override string ToString()
        {
            return $"{{kind:{kind},text:{text},value:{value},span:{span}}}";
        }
    }
}
