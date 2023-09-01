using System.Globalization;
using System.Text;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal class Lexer
    {
        private const char UnderlineChar = '_';

        private readonly SourceReader reader;
        private StringBuilder? builder;
        private SourceLocation tokenBeginIndex;

        public Lexer(SourceReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            this.reader = reader;
        }

        private SyntaxToken CreateSyntaxToken(SyntaxTokenKind kind, string text, object value)
        {
            var index = reader.Position;
            var span = new SourceSpan(tokenBeginIndex, index - tokenBeginIndex.Index);
            return new SyntaxToken(kind, text, value, span);
        }

        private SyntaxToken CreateSyntaxToken(SyntaxTokenKind kind, string text)
        {
            return CreateSyntaxToken(kind, text, text);
        }

        private SyntaxToken ReadSymbol()
        {
            var start = reader.Position;
            var ch = reader.ReadChar();
            if (ch != UnderlineChar && !char.IsLetter(ch)) throw new CompilerException(reader.Location, $"invalid symbol char:{ch}");

            while (!reader.EndOfStream)
            {
                ch = reader.PeekChar();
                if (ch != UnderlineChar && !char.IsLetterOrDigit(ch)) break;

                reader.Advance();
            }

            var text = reader.GetSubText(start);
            return CreateSyntaxToken(SyntaxTokenKind.Symbol, text);
        }

        private SyntaxToken ReadNumeric()
        {
            var start = reader.Position;
            var ch = reader.ReadChar();
            if (!char.IsDigit(ch))
            {
                throw new CompilerException(reader.Location, $"invalid numeric char:{ch}");
            }
            if (ch == '0' && IsHexSeparator(reader.PeekChar()))
            {
                //十六进制
                reader.Advance();
                start = reader.Position;
                ch = reader.ReadChar();
                if (!IsHexChar(ch))
                {
                    throw new CompilerException(reader.Location, $"invalid hex numeric char:{ch}");
                }
                while (!reader.EndOfStream)
                {
                    ch = reader.PeekChar();
                    if (!IsHexChar(ch)) break;

                    reader.Advance();
                }

                var text = reader.GetSubText(start);
                var value = long.Parse(text, NumberStyles.HexNumber);
                return CreateSyntaxToken(SyntaxTokenKind.Numeric, text, checked((decimal)value));
            }
            else
            {
                ch = reader.PeekChar();
                while (char.IsDigit(ch))
                {
                    reader.Advance();
                    ch = reader.PeekChar();
                }
                if (ch == '.')
                {
                    //小数
                    reader.Advance();
                    ch = reader.ReadChar();
                    if (!char.IsDigit(ch))
                    {
                        throw new CompilerException(reader.Location, "invalid numeric. incomplete exponent");
                    }
                    ch = reader.PeekChar();
                    while (char.IsDigit(ch))
                    {
                        reader.Advance();
                        ch = reader.PeekChar();
                    }
                }
                if (ch == 'e' || ch == 'E')
                {
                    //科学计算法
                    reader.Advance();
                    ch = reader.ReadChar();
                    if (ch == '-' || ch == '+')
                    {
                        ch = reader.ReadChar();
                    }
                    if (!char.IsDigit(ch))
                    {
                        throw new CompilerException(reader.Location, "invalid numeric. incomplete exponent");
                    }
                    ch = reader.PeekChar();
                    while (char.IsDigit(ch))
                    {
                        reader.Advance();
                        ch = reader.PeekChar();
                    }

                    var text = reader.GetSubText(start);
                    var value = decimal.Parse(text, NumberStyles.Float);
                    return CreateSyntaxToken(SyntaxTokenKind.Numeric, text, value);
                }
                else
                {
                    var text = reader.GetSubText(start);
                    var value = decimal.Parse(text, NumberStyles.Number);
                    return CreateSyntaxToken(SyntaxTokenKind.Numeric, text, value);
                }
            }

            static bool IsHexSeparator(char value)
            {
                return value == 'x' || value == 'X';
            }
            static bool IsHexChar(char value)
            {
                return char.IsDigit(value) || ('a' <= value && value <= 'f') || ('A' <= value && value <= 'F');
            }
        }

        private SyntaxToken ReadAnnotation()
        {
            var ch = reader.ReadChar();
            if (ch != '/') throw new CompilerException(reader.Location, $"invalid annotation char:{ch}");

            ch = reader.ReadChar();
            if (ch == '/')
            {
                //单行注释
                var text = reader.ReadLine().Trim();
                return CreateSyntaxToken(SyntaxTokenKind.Annotation, text);
            }
            if (ch == '*')
            {
                //多行注释
                var start = reader.Position;
                builder ??= new StringBuilder();
                while (!reader.EndOfStream)
                {
                    ch = reader.PeekChar();
                    if (ch == '\\')
                    {
                        var length = reader.Position - start;
                        if (length > 0) builder.Append(reader.GetSubText(start, length));

                        reader.Advance();
                        builder.Append(reader.ReadChar());
                        start = reader.Position;
                    }
                    else if (ch == '*')
                    {
                        if (reader.PeekChar(1) == '/')
                        {
                            var length = reader.Position - start;
                            if (length > 0) builder.Append(reader.GetSubText(start, length));

                            reader.Advance(2);
                            var text = builder.ToString();
                            builder.Clear();
                            return CreateSyntaxToken(SyntaxTokenKind.Annotation, text);
                        }
                    }

                    reader.Advance();
                }

                throw new CompilerException(reader.Location, "miscorrect multiline comment end symbol");
            }

            throw new CompilerException(reader.Location, $"unknown annotation char:{ch}");
        }

        public SyntaxToken Lex()
        {
            while (!reader.EndOfStream)
            {
                switch (reader.PeekChar())
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        reader.Advance();
                        continue;
                    default:
                        break;
                }

                break;
            }

            tokenBeginIndex = reader.Location;
            if (reader.EndOfStream)
            {
                return CreateSyntaxToken(SyntaxTokenKind.EndOfFile, "<eof>");
            }
            var ch = reader.PeekChar();
            switch (ch)
            {
                case '!':
                    reader.Advance();
                    return reader.Many('=') ? CreateSyntaxToken(SyntaxTokenKind.NotEqual, "!=") : CreateSyntaxToken(SyntaxTokenKind.Not, "!");
                case '%':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Mod, "%");
                case '(':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.LeftParen, "(");
                case ')':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.RightParen, ")");
                case '*':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Multiply, "*");
                case '+':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Add, "+");
                case ',':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Comma, ",");
                case '-':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Subtract, "-");
                case '.':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Dot, ".");
                case '/':
                    var nch = reader.PeekChar(1);
                    if (nch == '/' || nch == '*')
                    {
                        return ReadAnnotation();
                    }

                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Divide, "/");
                case ';':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.NewLine, ";");
                case '<':
                    reader.Advance();
                    return reader.Many('=') ? CreateSyntaxToken(SyntaxTokenKind.LessThanOrEqual, "<=") : CreateSyntaxToken(SyntaxTokenKind.LessThan, "<");
                case '=':
                    reader.Advance();
                    return reader.Many('=') ? CreateSyntaxToken(SyntaxTokenKind.Equal, "==") : CreateSyntaxToken(SyntaxTokenKind.Assign, "=");
                case '>':
                    reader.Advance();
                    return reader.Many('=') ? CreateSyntaxToken(SyntaxTokenKind.GreaterThanOrEqual, ">=") : CreateSyntaxToken(SyntaxTokenKind.GreaterThan, ">");
                case '[':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.LeftBracket, "[");
                case ']':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.RightBracket, "]");
                case '^':
                    reader.Advance();
                    return CreateSyntaxToken(SyntaxTokenKind.Power, "^");
                default:
                    if (char.IsDigit(ch))
                    {
                        return ReadNumeric();
                    }
                    if (ch == UnderlineChar || char.IsLetter(ch))
                    {
                        return ReadSymbol();
                    }

                    throw new CompilerException(reader.Location, $"unknown token char:{ch}");
            }
        }
    }
}