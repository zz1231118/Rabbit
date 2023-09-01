using System.Buffers;

namespace IronRabbit.Compiler
{
    sealed class SourceReader
    {
        public const char InvalidCharacter = char.MaxValue;

        private readonly List<int> newLineLocations = new List<int>();
        private readonly string text;
        private int position;

        private SourceReader(string text)
        {
            this.text = text;
        }

        public bool EndOfStream => position >= text.Length;

        public SourceLocation Location
        {
            get
            {
                var line = newLineLocations.Count + 1;
                var column = position + 1;
                if (newLineLocations.Count > 0)
                {
                    var index = newLineLocations.Count - 1;
                    var num = newLineLocations[index];
                    column = index - num;
                }

                return new SourceLocation(position, line, column);
            }
        }

        public int Position => position;

        public static SourceReader From(string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new SourceReader(source);
        }

        public static SourceReader From(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var text = reader.ReadToEnd();
            return new SourceReader(text);
        }

        public char PeekChar()
        {
            return position >= text.Length ? InvalidCharacter : text[position];
        }

        public char PeekChar(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

            var index = position + n;
            return index >= text.Length ? InvalidCharacter : text[index];
        }

        public char ReadChar()
        {
            if (position >= text.Length) return InvalidCharacter;

            var value = text[position];
            if (value == '\n') newLineLocations.Add(position);
            position++;
            return value;
        }

        public string ReadLine()
        {
            if (EndOfStream) throw new EndOfStreamException();

            var start = position;
            while (!EndOfStream)
            {
                var ch = ReadChar();
                if (ch == '\r')
                {
                    if (PeekChar() == '\n')
                    {
                        Advance();
                        break;
                    }
                }
                else if (ch == '\n')
                {
                    break;
                }
            }

            var length = position - start;
            return text.Substring(start, length);
        }

        public void Advance()
        {
            var ch = text[position];
            if (ch == '\n') newLineLocations.Add(position);

            position++;
        }

        public void Advance(int n)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));

            for (int i = 0; i < n; i++)
            {
                var ch = text[position];
                if (ch == '\n') newLineLocations.Add(position);
            }

            position += n;
        }

        public bool Many(char value)
        {
            if (!EndOfStream && PeekChar() == value)
            {
                Advance();
                return true;
            }

            return false;
        }

        public string GetSubText(int start)
        {
            if (start < 0 || start > position) throw new ArgumentOutOfRangeException(nameof(start));
            
            var length = position - start;
            return text.Substring(start, length);
        }

        public string GetSubText(int start, int length)
        {
            if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0 || start + length > text.Length) throw new ArgumentOutOfRangeException(nameof(length));

            return text.Substring(start, length);
        }
    }
}
