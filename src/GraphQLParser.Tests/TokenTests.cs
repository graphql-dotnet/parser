using System;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class TokenTests
{
    [Theory]
    [InlineData(TokenKind.EOF, null, "EOF")]
    [InlineData(TokenKind.BANG, null, "!")]
    [InlineData(TokenKind.DOLLAR, null, "$")]
    [InlineData(TokenKind.PAREN_L, null, "(")]
    [InlineData(TokenKind.PAREN_R, null, ")")]
    [InlineData(TokenKind.SPREAD, null, "...")]
    [InlineData(TokenKind.COLON, null, ":")]
    [InlineData(TokenKind.EQUALS, null, "=")]
    [InlineData(TokenKind.AT, null, "@")]
    [InlineData(TokenKind.BRACKET_L, null, "[")]
    [InlineData(TokenKind.BRACKET_R, null, "]")]
    [InlineData(TokenKind.BRACE_L, null, "{")]
    [InlineData(TokenKind.PIPE, null, "|")]
    [InlineData(TokenKind.BRACE_R, null, "}")]
    [InlineData(TokenKind.AMPERSAND, null, "&")]

    [InlineData(TokenKind.NAME, "abc", "Name \"abc\"")]
    [InlineData(TokenKind.INT, "42", "Int \"42\"")]
    [InlineData(TokenKind.FLOAT, "4.2", "Float \"4.2\"")]
    [InlineData(TokenKind.STRING, "def", "String \"def\"")]
    [InlineData(TokenKind.COMMENT, "xyz", "# \"xyz\"")]
    [InlineData(TokenKind.UNKNOWN, "???", "Unknown \"???\"")]
    public void ToStringTest(TokenKind kind, string value, string expectedDescription)
    {
        var token = new Token(kind, value, 0, 0);
        token.ToString().ShouldBe(expectedDescription);
    }

    [Fact]
    public void NotSupported_Token_Should_Throw()
    {
        Should.Throw<NotSupportedException>(() => new Token((TokenKind)999, "", 0, 0).ToString());
    }
}
