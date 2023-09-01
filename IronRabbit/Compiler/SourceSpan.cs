namespace IronRabbit.Compiler
{
    public readonly struct SourceSpan
    {
        private readonly SourceLocation start;
        private readonly int length;

        public SourceSpan(SourceLocation start, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            this.start = start;
            this.length = length;
        }

        public SourceLocation Start => start;

        public int Length => Length;

        public override string ToString()
        {
            return $"{{start:{start},length:{length}";
        }
    }
}
