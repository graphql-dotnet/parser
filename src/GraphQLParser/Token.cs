using System;

namespace GraphQLParser
{
    public readonly struct Token
    {
        public Token(TokenKind kind, ReadOnlyMemory<char> value, int start, int end)
        {
            Kind = kind;
            Value = value;
            Start = start;
            End = end;

        }

        public TokenKind Kind { get; }

        public int Start { get; }

        public int End { get; }

        public ReadOnlyMemory<char> Value { get; }

        internal static string GetTokenKindDescription(TokenKind kind) => kind switch
        {
            TokenKind.EOF => "EOF",
            TokenKind.BANG => "!",
            TokenKind.DOLLAR => "$",
            TokenKind.PAREN_L => "(",
            TokenKind.PAREN_R => ")",
            TokenKind.SPREAD => "...",
            TokenKind.COLON => ":",
            TokenKind.EQUALS => "=",
            TokenKind.AT => "@",
            TokenKind.BRACKET_L => "[",
            TokenKind.BRACKET_R => "]",
            TokenKind.BRACE_L => "{",
            TokenKind.PIPE => "|",
            TokenKind.BRACE_R => "}",
            TokenKind.NAME => "Name",
            TokenKind.INT => "Int",
            TokenKind.FLOAT => "Float",
            TokenKind.STRING => "String",
            TokenKind.COMMENT => "#",
            _ => string.Empty
        };

        private bool HasUniqueValue =>
            Kind == TokenKind.NAME ||
            Kind == TokenKind.INT ||
            Kind == TokenKind.FLOAT ||
            Kind == TokenKind.STRING ||
            Kind == TokenKind.COMMENT ||
            Kind == TokenKind.UNKNOWN;

        /// <inheritdoc/>
        public override string ToString() => HasUniqueValue
            ? $"{GetTokenKindDescription(Kind)} \"{Value}\""
            : GetTokenKindDescription(Kind);
    }
}
