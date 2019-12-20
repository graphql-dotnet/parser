namespace GraphQLParser
{
    public enum TokenKind
    {
        EOF = 1,
        BANG = 2,
        DOLLAR = 3,
        PAREN_L = 4,
        PAREN_R = 5,
        SPREAD = 6,
        COLON = 7,
        EQUALS = 8,
        AT = 9,
        BRACKET_L = 10,
        BRACKET_R = 11,
        BRACE_L = 12,
        PIPE = 13,
        BRACE_R = 14,
        NAME = 15,
        INT = 16,
        FLOAT = 17,
        STRING = 18,
        COMMENT = 19,
        UNKNOWN = 20
    }

    public readonly struct Token
    {
        public Token(TokenKind kind, string value, int start, int end)
        {
            Kind = kind;
            Value = value;
            Start = start;
            End = end;
        }

        public int End { get; }

        public TokenKind Kind { get; }

        public int Start { get; }

        public string Value { get; }

        public static string GetTokenKindDescription(TokenKind kind) => kind switch
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

        public override string ToString()
        {
            return Value != null
                ? $"{GetTokenKindDescription(Kind)} \"{Value}\""
                : GetTokenKindDescription(Kind);
        }
    }
}