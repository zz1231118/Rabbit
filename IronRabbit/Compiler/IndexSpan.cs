using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public struct IndexSpan : IEquatable<IndexSpan>
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

        public static bool operator ==(IndexSpan lh, IndexSpan rh)
        {
            return lh.start == rh.start && lh.length == rh.length;
        }
        public static bool operator !=(IndexSpan lh, IndexSpan rh)
        {
            return lh.start != rh.start || lh.length != rh.length;
        }

        public override bool Equals(object obj)
        {
            return obj is IndexSpan other && Equals(other);
        }
        public bool Equals(IndexSpan other)
        {
            return other.start == start && other.length == length;
        }
        public override int GetHashCode()
        {
            return start.GetHashCode() ^ length.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{{start:{0} length:{1}}}", start, length);
        }
    }
}
