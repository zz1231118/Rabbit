﻿namespace IronRabbit.Compiler
{
    internal static class Tokens
    {
        public static readonly Token EndOfFileToken = Token.Symbol(TokenKind.EndOfFile, "<eof>");
        public static readonly Token NewLineToken = Token.Symbol(TokenKind.NewLine, "<newline>");

        public static readonly Token AddToken = Token.Operator(TokenKind.Add, "+", 4);
        public static readonly Token SubtractToken = Token.Operator(TokenKind.Subtract, "-", 4);
        public static readonly Token MultiplyToken = Token.Operator(TokenKind.Multiply, "*", 5);
        public static readonly Token DivideToken = Token.Operator(TokenKind.Divide, "/", 5);
        public static readonly Token ModToken = Token.Operator(TokenKind.Mod, "%", 5);
        public static readonly Token PowerToken = Token.Operator(TokenKind.Power, "^", 6);
        public static readonly Token CommaToken = Token.Symbol(TokenKind.Comma, ",");
        public static readonly Token AssignToken = Token.Symbol(TokenKind.Assign, "=");
        public static readonly Token DotToken = Token.Symbol(TokenKind.Dot, ".");

        public static readonly Token LeftParenToken = Token.Symbol(TokenKind.LeftParen, "(");
        public static readonly Token RightParenToken = Token.Symbol(TokenKind.RightParen, ")");
        public static readonly Token LeftBracketToken = Token.Symbol(TokenKind.LeftBracket, "[");
        public static readonly Token RightBracketToken = Token.Symbol(TokenKind.RightBracket, "]");
    }
}