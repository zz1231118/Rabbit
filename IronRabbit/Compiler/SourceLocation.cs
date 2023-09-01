namespace IronRabbit.Compiler
{
    public readonly struct SourceLocation
    {
        private readonly int index;
        private readonly int line;
        private readonly int column;

        public SourceLocation(int index, int line, int column)
        {
            this.index = index;
            this.line = line;
            this.column = column;
        }

        public int Index => index;

        public int Line => line;

        public int Column => column;

        public override string ToString()
        {
            return $"{{line:{line},column:{column}}}";
        }
    }
}
