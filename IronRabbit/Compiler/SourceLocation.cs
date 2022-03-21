using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public readonly struct SourceLocation : IEquatable<SourceLocation>
    {
        public static readonly SourceLocation None = new SourceLocation(0, 16707566, 0, true);
        public static readonly SourceLocation Invalid = new SourceLocation(0, 0, 0, true);
        public static readonly SourceLocation MinValue = new SourceLocation(0, 1, 1);

        private readonly int index;
        private readonly int line;
        private readonly int column;

        public SourceLocation(int index, int line, int column)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (line < 1)
                throw new ArgumentOutOfRangeException(nameof(line));
            if (column < 1)
                throw new ArgumentOutOfRangeException(nameof(column));

            this.index = index;
            this.line = line;
            this.column = column;
        }

        private SourceLocation(int index, int line, int column, bool _)
        {
            this.index = index;
            this.line = line;
            this.column = column;
        }

        public int Index => index;

        public int Line => line;

        public int Column => column;

        public bool IsValid => line != 0 && column != 0;

        public static bool operator ==(SourceLocation left, SourceLocation right)
        {
            return left.index == right.index && left.line == right.line && left.column == right.column;
        }

        public static bool operator !=(SourceLocation left, SourceLocation right)
        {
            return left.index != right.index || left.line != right.line || left.column != right.column;
        }

        public static bool operator <(SourceLocation left, SourceLocation right)
        {
            return left.index < right.index;
        }

        public static bool operator >(SourceLocation left, SourceLocation right)
        {
            return left.index > right.index;
        }

        public static bool operator <=(SourceLocation left, SourceLocation right)
        {
            return left.index <= right.index;
        }

        public static bool operator >=(SourceLocation left, SourceLocation right)
        {
            return left.index >= right.index;
        }

        public static int Compare(SourceLocation left, SourceLocation right)
        {
            return left > right ? 1 : left < right ? -1 : 0;
        }

        public bool Equals(SourceLocation other)
        {
            return other.index == index && other.line == line && other.column == column;
        }

        public override bool Equals(object obj)
        {
            return obj is SourceLocation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (line << 16) ^ column;
        }

        public override string ToString()
        {
            return string.Format("{{index:{0} line:{1} column:{2}}}", index, line, column);
        }
    }
}
