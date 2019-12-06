using System;
using System.Collections.Generic;
using System.IO;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal class Tokenizer
    {
        private const int DefaultBufferCapacity = 1024;

        private TextReader reader;
        private char[] buffer;
        private int start;
        private int end;
        private int position;
        private bool endOfStream;
        private bool multiEolns;
        private int tokenEnd;
        private int tokenStartIndex;
        private int tokenEndIndex;
        private List<int> newLineLocations;
        private SourceLocation initialLocation;

        public Tokenizer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.reader = reader;
            Initialize(DefaultBufferCapacity);
        }

        public bool EndOfStream => endOfStream;
        public int Index => tokenStartIndex + Math.Min(position, end) - start;
        public SourceLocation Position => IndexToLocation(Index);
        public IndexSpan TokenSpan => new IndexSpan(tokenStartIndex, tokenEndIndex - tokenStartIndex);
        private int TokenLength => tokenEnd - start;

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
            if (end == buffer.Length)
            {
                int newSize = Math.Max(Math.Max((end - start) * 2, buffer.Length), position);
                ResizeInternal(ref buffer, newSize, start, end - start);
                end -= start;
                position -= start;
                start = 0;
                //_bufferResized = true;
            }

            end  += reader.Read(buffer, end, buffer.Length - end);
        }
        private void Initialize(int bufferCapacity)
        {
            multiEolns = true;
            buffer = new char[bufferCapacity];
            newLineLocations = new List<int>();
            initialLocation = SourceLocation.MinValue;
        }
        private int Peek()
        {
            if (position >= end)
            {
                RefillBuffer();
                if (position >= end)
                {
                    endOfStream = true;
                    return -1;
                }
            }

            return buffer[position];
        }
        private bool NextChar(int ch)
        {
            if (Peek() == ch)
            {
                position++;
                return true;
            }
            return false;
        }
        private int NextChar()
        {
            int result = Peek();
            position++;
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
                newLineLocations.Add(Index);
            }
            BufferBack();
            return num;
        }
        private bool IsEoln(int current)
        {
            return current == 10 || (current == 13 && multiEolns);
        }
        private void BufferBack()
        {
            SeekRelative(-1);
        }
        private void SeekRelative(int disp)
        {
            position += disp;
        }
        private void MarkTokenEnd()
        {
            tokenEnd = Math.Min(position, end);
            int num = tokenEnd - start;
            tokenEndIndex = tokenStartIndex + num;
        }
        private void DiscardToken()
        {
            if (tokenEnd == -1)
            {
                MarkTokenEnd();
            }

            start = tokenEnd;
            tokenStartIndex = tokenEndIndex;
            tokenEnd = -1;
        }
        private string GetTokenString()
        {
            return new string(buffer, start, tokenEnd - start);
        }
        private string GetTokenSubstring(int offset)
        {
            return GetTokenSubstring(offset, tokenEnd - start - offset);
        }
        private string GetTokenSubstring(int offset, int length)
        {
            return new string(buffer, start + offset, length);
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
            int num = newLineLocations.BinarySearch(index);
            if (num < 0)
            {
                if (num == -1)
                {
                    int column = checked(index + initialLocation.Column);
                    if (TokenLength > 0) column -= TokenLength;
                    return new SourceLocation(index + initialLocation.Index, initialLocation.Line, column);
                }
                num = ~num - 1;
            }

            int fcolumn = index - newLineLocations[num] + initialLocation.Column;
            if (TokenLength > 0) fcolumn -= TokenLength;
            return new SourceLocation(index + initialLocation.Index, num + 1 + initialLocation.Line, fcolumn);
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
                        newLineLocations.Add(Index);
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
                        newLineLocations.Add(Index);
                        num = SkipWhiteSpace();
                        continue;
                    case 13://'\r'
                    case 32://' '
                        num = SkipWhiteSpace();
                        continue;
                    case 33://!
                        var notToken = NextChar(61) ? Tokens.NotEqualToken : Tokens.NotToken;
                        MarkTokenEnd();
                        return notToken;
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
                    case 60://<
                        var lessToken = NextChar(61) ? Tokens.LessThanOrEqualToken : Tokens.LessThanToken;
                        MarkTokenEnd();
                        return lessToken;
                    case 61://'='
                        var assignOrEqualToken = NextChar(61) ? Tokens.EqualToken : Tokens.AssignToken;
                        MarkTokenEnd();
                        return assignOrEqualToken;
                    case 62://>
                        var greaterToken = NextChar(61) ? Tokens.GreaterThanOrEqualToken : Tokens.GreaterThanToken;
                        MarkTokenEnd();
                        return greaterToken;
                    case 91://'['
                        MarkTokenEnd();
                        return Tokens.LeftBracketToken;
                    case 93://']'
                        MarkTokenEnd();
                        return Tokens.RightBracketToken;
                    case 94://'^'
                        MarkTokenEnd();
                        return Tokens.PowerToken;
                    case 105://i
                        if (NextChar(102))
                        {
                            MarkTokenEnd();
                            return Tokens.IFToken;
                        }

                        return ReadName();
                    default:
                        return ReadName();
                }
            }
        }
    }
}