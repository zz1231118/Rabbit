using System;
using System.Collections.Generic;
using System.IO;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal class Tokenizer
    {
        private const int DefaultBufferCapacity = 1024;

        private TextReader _reader;
        private char[] _buffer;
        private int _start;
        private int _end;
        private int _position;
        private bool _endOfStream;
        private bool _multiEolns;
        private int _tokenEnd;
        private int _tokenStartIndex;
        private int _tokenEndIndex;
        private List<int> _newLineLocations;
        private SourceLocation _initialLocation;

        public Tokenizer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _reader = reader;
            Initialize(DefaultBufferCapacity);
        }

        public bool EndOfStream => _endOfStream;
        public int Index => _tokenStartIndex + Math.Min(_position, _end) - _start;
        public SourceLocation Position => IndexToLocation(Index);
        public IndexSpan TokenSpan => new IndexSpan(_tokenStartIndex, _tokenEndIndex - _tokenStartIndex);
        private int TokenLength => _tokenEnd - _start;

        private static void ResizeInternal(ref char[] array, int newSize, int start, int count)
        {
            char[] array2 = (newSize != array.Length) ? new char[newSize] : array;
            Buffer.BlockCopy(array, start * 2, array2, 0, count * 2);
            array = array2;
        }
        private static bool IsNameStart(int ch)
        {
            return char.IsLetter((char)ch) || ch == 95;
        }
        private static bool IsNamePart(int ch)
        {
            return char.IsLetterOrDigit((char)ch) || ch == 95;
        }
        private void RefillBuffer()
        {
            if (_end == _buffer.Length)
            {
                int newSize = Math.Max(Math.Max((_end - _start) * 2, _buffer.Length), _position);
                ResizeInternal(ref _buffer, newSize, _start, _end - _start);
                _end -= _start;
                _position -= _start;
                _start = 0;
                //_bufferResized = true;
            }

            _end  += _reader.Read(_buffer, _end, _buffer.Length - _end);
        }
        private void Initialize(int bufferCapacity)
        {
            _multiEolns = true;
            _buffer = new char[bufferCapacity];
            _newLineLocations = new List<int>();
            _initialLocation = SourceLocation.MinValue;
        }
        private int Peek()
        {
            if (_position >= _end)
            {
                RefillBuffer();
                if (_position >= _end)
                {
                    _endOfStream = true;
                    return -1;
                }
            }

            return _buffer[_position];
        }
        private bool NextChar(int ch)
        {
            if (Peek() == ch)
            {
                _position++;
                return true;
            }
            return false;
        }
        private int NextChar()
        {
            int result = Peek();
            _position++;
            return result;
        }
        private int ReadLine()
        {
            int num;
            do
            {
                num = NextChar();
            } while (num != -1 && !IsEoln(num));
            if (num == 10)
            {
                _newLineLocations.Add(Index);
            }
            BufferBack();
            return num;
        }
        private bool IsEoln(int current)
        {
            return current == 10 || (current == 13 && _multiEolns);
        }
        private void BufferBack()
        {
            SeekRelative(-1);
        }
        private void SeekRelative(int disp)
        {
            _position += disp;
        }
        private void MarkTokenEnd()
        {
            _tokenEnd = Math.Min(_position, _end);
            int num = _tokenEnd - _start;
            _tokenEndIndex = _tokenStartIndex + num;
        }
        private void DiscardToken()
        {
            if (_tokenEnd == -1)
            {
                MarkTokenEnd();
            }

            _start = _tokenEnd;
            _tokenStartIndex = _tokenEndIndex;
            _tokenEnd = -1;
        }
        internal string GetTokenString()
        {
            return new string(_buffer, _start, _tokenEnd - _start);
        }
        private string GetTokenSubstring(int offset)
        {
            return GetTokenSubstring(offset, _tokenEnd - _start - offset);
        }
        private string GetTokenSubstring(int offset, int length)
        {
            return new string(_buffer, _start + offset, length);
        }
        private int SkipWhiteSpace()
        {
            int num;
            do
            {
                num = NextChar();
            } while (num == 32 || num == 9);

            BufferBack();
            DiscardToken();
            SeekRelative(1);
            return num;
        }
        private SourceLocation IndexToLocation(int index)
        {
            int num = _newLineLocations.BinarySearch(index);
            if (num < 0)
            {
                if (num == -1)
                {
                    int column = checked(index + _initialLocation.Column);
                    if (TokenLength > 0) column -= TokenLength;
                    return new SourceLocation(index + _initialLocation.Index, _initialLocation.Line, column);
                }
                num = ~num - 1;
            }

            int fcolumn = index - _newLineLocations[num] + _initialLocation.Column;
            if (TokenLength > 0) fcolumn -= TokenLength;
            return new SourceLocation(index + _initialLocation.Index, num + 1 + _initialLocation.Line, fcolumn);
        }
        
        private static Token BadChar(int ch)
        {
            return Token.Error(((char)ch).ToString());
        }
        private Token ReadName()
        {
            BufferBack();
            int num = NextChar();
            if (!IsNameStart(num))
                return BadChar(num);

            while (IsNamePart(num))
            {
                num = this.NextChar();
            }

            BufferBack();
            MarkTokenEnd();
            return Token.Identifier(GetTokenString());
        }
        private Token ReadNumber()
        {
            int num;
            do
            {
                num = NextChar();
            } while (48 <= num && num <= 57);
            if (num == 46)
            {
                do
                {
                    num = NextChar();
                } while (48 <= num && num <= 57);
            }

            BufferBack();
            MarkTokenEnd();
            return Token.Constant(double.Parse(GetTokenString()));
        }
        private Token ReadComment()
        {
            BufferBack();
            int num = NextChar();
            if (num == 42)
            {
                do
                {
                    num = NextChar();
                    if (num == 10)
                    {
                        _newLineLocations.Add(Index);
                    }
                } while (!(num == 42 && NextChar(47)));

                SeekRelative(-2);
                MarkTokenEnd();
                SeekRelative(2);
                return Token.Comment(GetTokenSubstring(2));
            }
            else
            {
                ReadLine();
                MarkTokenEnd();
                return Token.Comment(GetTokenSubstring(2));
            }
        }

        public Token NextToken()
        {
            DiscardToken();
            int num = NextChar();
            while (true)
            {
                switch (num)
                {
                    case -1:
                        return Tokens.EndOfFileToken;
                    case 10://'\n'
                        _newLineLocations.Add(Index);
                        num = SkipWhiteSpace();
                        continue;
                    case 13://'\r'
                    case 32://' '
                        num = SkipWhiteSpace();
                        continue;
                    case 37://'%'
                        MarkTokenEnd();
                        return Tokens.ModToken;
                    case 40://'('
                        MarkTokenEnd();
                        return Tokens.LeftParenToken;
                    case 41://')'
                        MarkTokenEnd();
                        return Tokens.RightParenToken;
                    case 42://'*'
                        MarkTokenEnd();
                        return Tokens.MultiplyToken;
                    case 43://'+'
                        MarkTokenEnd();
                        return Tokens.AddToken;
                    case 44://','
                        MarkTokenEnd();
                        return Tokens.CommaToken;
                    case 45://'-'
                        MarkTokenEnd();
                        return Tokens.SubtractToken;
                    case 46://'.'
                        MarkTokenEnd();
                        return Tokens.DotToken;
                    case 47://'/'
                        if (NextChar(42) || NextChar(47))
                        {
                            return ReadComment();
                        }

                        MarkTokenEnd();
                        return Tokens.DivideToken;
                    case 48://'0'
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                    case 56:
                    case 57://'9'
                        return ReadNumber();
                    case 59://';'
                        MarkTokenEnd();
                        return Tokens.NewLineToken;
                    case 61://'='
                        MarkTokenEnd();
                        return Tokens.AssignToken;
                    case 91://'['
                        MarkTokenEnd();
                        return Tokens.LeftBracketToken;
                    case 93://']'
                        MarkTokenEnd();
                        return Tokens.RightBracketToken;
                    case 94://'^'
                        MarkTokenEnd();
                        return Tokens.PowerToken;
                    default:
                        return ReadName();
                }
            }
        }
    }
}