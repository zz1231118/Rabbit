using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public readonly struct IndexSpan : IEquatable<IndexSpan>
    {
        private readonly int start;
        private readonly int length;

        public IndexSpan(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            this.start = start;
            this.length = length;
        }

        public int Start => start;

        public int End => start + length;

        public int Length => length;

        public bool IsEmpty => length == 0;

        public static bool operator ==(IndexSpan left, IndexSpan right)
        {
            return left.start == right.start && left.length == right.length;
        }

        public static bool operator !=(IndexSpan left, IndexSpan right)
        {
            return left.start != right.start || left.length != right.length;
        }

        public bool Equals(IndexSpan other)
        {
            return other.start == start && other.length == length;
        }

        public override bool Equals(object obj)
        {
            return obj is IndexSpan other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (start.GetHashCode() << 16) ^ length.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{{start:{0} length:{1}}}", start, length);
        }
    }
}
