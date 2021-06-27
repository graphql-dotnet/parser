using System;

namespace GraphQLParser
{
    /// <summary>
    /// Represents a lexical token within GraphQL document.
    /// </summary>
    public readonly struct Token
    {
        /// <summary>
        /// Initializes a new instance with the specified properties.
        /// </summary>
        public Token(TokenKind kind, ROM value, int start, int end)
        {
            Kind = kind;
            Value = value;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Kind of token.
        /// </summary>
        public TokenKind Kind { get; }

        /// <summary>
        /// The index for the start of the token in the source (i.e. it's inclusive).
        /// <br/>
        /// For example:
        /// <code>
        /// { field { subfield } }
        /// <br/>
        /// --^ Start = 2
        /// </code>
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The index for the character immediately after the token in the source (i.e. it's exclusive).
        /// <br/>
        /// For example:
        /// <code>
        /// { field { subfield } }
        /// <br/>
        /// --------------------^ End = 20
        /// </code>
        /// </summary>
        public int End { get; }

        /// <summary>
        /// Token value represented as a contiguous region of memory.
        /// </summary>
        public ROM Value { get; }

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
            TokenKind.AMPERSAND => "&",
            TokenKind.BRACKET_L => "[",
            TokenKind.BRACKET_R => "]",
            TokenKind.BRACE_L => "{",
            TokenKind.PIPE => "|",
            TokenKind.BRACE_R => "}",
            TokenKind.NAME => "Name",
            TokenKind.INT => "Int",
            TokenKind.FLOAT => "Float",
            TokenKind.STRING => "String",
            TokenKind.BLOCKSTRING => "BlockString",
            TokenKind.COMMENT => "#",
            TokenKind.UNKNOWN => "Unknown",
            _ => throw new NotSupportedException(kind.ToString())
        };

        private bool HasUniqueValue =>
            Kind == TokenKind.NAME ||
            Kind == TokenKind.INT ||
            Kind == TokenKind.FLOAT ||
            Kind == TokenKind.STRING ||
            Kind == TokenKind.BLOCKSTRING ||
            Kind == TokenKind.COMMENT ||
            Kind == TokenKind.UNKNOWN;

        /// <inheritdoc/>
        public override string ToString() => HasUniqueValue
            ? $"{GetTokenKindDescription(Kind)} \"{Value}\""
            : GetTokenKindDescription(Kind);
    }
}
