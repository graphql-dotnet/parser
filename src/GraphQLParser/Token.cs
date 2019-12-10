namespace GraphQLParser
{
    public readonly struct Token
    {
        public Token(TokenKind kind, string value, int start, int end)
        {
            Kind = kind;
            Value = value;
            Start = start;
            End = end;
        }

        public int Start { get; }

        public int End { get; }

        public TokenKind Kind { get; }

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