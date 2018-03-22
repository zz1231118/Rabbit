using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public struct SourceLocation : IEquatable<SourceLocation>
    {
        public static readonly SourceLocation None = new SourceLocation(0, 16707566, 0, true);
        public static readonly SourceLocation Invalid = new SourceLocation(0, 0, 0, true);
        public static readonly SourceLocation MinValue = new SourceLocation(0, 1, 1);

        private int _index;
        private int _line;
        private int _column;

        public SourceLocation(int index, int line, int column)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (line < 1)
                throw new ArgumentOutOfRangeException(nameof(line));
            if (column < 1)
                throw new ArgumentOutOfRangeException(nameof(column));

            _index = index;
            _line = line;
            _column = column;
        }
        private SourceLocation(int index, int line, int column, bool noChecks)
        {
            _index = index;
            _line = line;
            _column = column;
        }

        public int Index => _index;
        public int Line => _line;
        public int Column => _column;
        public bool IsValid => _line != 0 && _column != 0;

        public static bool operator ==(SourceLocation left, SourceLocation right)
        {
            return left._index == right._index && left._line == right._line && left._column == right._column;
        }
        public static bool operator !=(SourceLocation left, SourceLocation right)
        {
            return left._index != right._index || left._line != right._line || left._column != right._column;
        }
        public static bool operator <(SourceLocation left, SourceLocation right)
        {
            return left._index < right._index;
        }
        public static bool operator >(SourceLocation left, SourceLocation right)
        {
            return left._index > right._index;
        }
        public static bool operator <=(SourceLocation left, SourceLocation right)
        {
            return left._index <= right._index;
        }
        public static bool operator >=(SourceLocation left, SourceLocation right)
        {
            return left._index >= right._index;
        }

        public static int Compare(SourceLocation left, SourceLocation right)
        {
            if (left < right)
            {
                return -1;
            }
            if (right > left)
            {
                return 1;
            }
            return 0;
        }
        public override bool Equals(object obj)
        {
            return obj is SourceLocation other && Equals(other);
        }
        public bool Equals(SourceLocation other)
        {
            return other._index == _index && other._line == _line && other._column == _column;
        }
        public override int GetHashCode()
        {
            return _line << 16 ^ _column;
        }
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", _index, _line, _column);
        }
    }
}
