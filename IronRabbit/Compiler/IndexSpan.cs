using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public struct IndexSpan : IEquatable<IndexSpan>
    {
        private int _start;
        private int _length;

        public IndexSpan(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            _start = start;
            _length = length;
        }

        public int Start => _start;
        public int End => _start + _length;
        public int Length => _length;
        public bool IsEmpty => _length == 0;

        public static bool operator ==(IndexSpan self, IndexSpan other)
        {
            return self._start == other._start && self._length == other._length;
        }
        public static bool operator !=(IndexSpan self, IndexSpan other)
        {
            return self._start != other._start || self._length != other._length;
        }

        public override bool Equals(object obj)
        {
            return obj is IndexSpan other && Equals(other);
        }
        public bool Equals(IndexSpan other)
        {
            return other._start == _start && other._length == _length;
        }
        public override int GetHashCode()
        {
            return _start.GetHashCode() ^ _length.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{{start:{0} length:{1}}}", _start, _length);
        }
    }
}
