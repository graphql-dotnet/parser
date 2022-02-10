namespace GraphQLParser.Tests;

public class LexerTests
{
    [Fact]
    public void ATPunctuation()
    {
        var token = "@".Lex();
        token.Kind.ShouldBe(TokenKind.AT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("@");
    }

    [Fact]
    public void BangPunctuation()
    {
        var token = "!".Lex();
        token.Kind.ShouldBe(TokenKind.BANG);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("!");
    }

    [Fact]
    public void ColonPunctuation()
    {
        var token = ":".Lex();
        token.Kind.ShouldBe(TokenKind.COLON);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe(":");
    }

    [Fact]
    public void DollarPunctuation()
    {
        var token = "$".Lex();
        token.Kind.ShouldBe(TokenKind.DOLLAR);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("$");
    }

    [Fact]
    public void EmptySource_ReturnsEOF()
    {
        var token = "".Lex();

        token.Kind.ShouldBe(TokenKind.EOF);
    }

    [Fact]
    public void EqualsPunctuation()
    {
        var token = "=".Lex();
        token.Kind.ShouldBe(TokenKind.EQUALS);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("=");
    }

    [Theory]
    [InlineData("\"escaped \\n\\r\\b\\t\\f\"", 0, 20, "escaped \n\r\b\t\f")]
    [InlineData($"\"abc\\tdef\"", 0, 10, "abc\tdef")]
    public void EscapedString(string text, int start, int end, string value)
    {
        var token = text.Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(start);
        token.End.ShouldBe(end);
        token.Value.ShouldBe(value);
    }

    [Fact]
    public void LeftBracePunctuation()
    {
        var token = "{".Lex();
        token.Kind.ShouldBe(TokenKind.BRACE_L);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("{");
    }

    [Fact]
    public void LeftBracketPunctuation()
    {
        var token = "[".Lex();
        token.Kind.ShouldBe(TokenKind.BRACKET_L);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("[");
    }

    [Fact]
    public void LeftParenthesisPunctuation()
    {
        var token = "(".Lex();
        token.Kind.ShouldBe(TokenKind.PAREN_L);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("(");
    }

    [Fact]
    public void MultipleDecimalsInt()
    {
        var token = "123".Lex();
        token.Kind.ShouldBe(TokenKind.INT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(3);
        token.Value.ShouldBe("123");
    }

    [Fact]
    public void NameTokenWithComments()
    {
        var token = $"\n#comment\nfoo#comment".Lex();
        token.Kind.ShouldBe(TokenKind.COMMENT);
        token.Start.ShouldBe(1);
        token.End.ShouldBe(10);
        token.Value.ShouldBe("comment");
    }

    [Fact]
    public void NameTokenWithEscapedComment()
    {
        var token = $"#abc\\tdef\nfoo".Lex();
        token.Kind.ShouldBe(TokenKind.COMMENT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(10);
        token.Value.Length.ShouldBe(7);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void NameTokenWithVeryLongComment(bool escape)
    {
        string comment = new('w', 4096 + 10); // causes IndexOutOfRangeException in LexerContext.ReadComment
        var token = (escape ? $"#\\t{comment}\nfoo" : $"#{comment}\nfoo").Lex();
        token.Kind.ShouldBe(TokenKind.COMMENT);
        token.Value.Length.ShouldBe(4096 + 10 + (escape ? 1 : 0));
        token.Value.Span[0].ShouldBe(escape ? '\t' : 'w');
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void VeryLongString(bool escape)
    {
        string text = new('w', 4096 + 10); // causes IndexOutOfRangeException in LexerContext.ReadString
        var token = (escape ? $"\"\\t{text}\"" : $"\"{text}\"").Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Value.Length.ShouldBe(4096 + 10 + (escape ? 1 : 0));
        token.Value.Span[0].ShouldBe(escape ? '\t' : 'w');
    }

    [Fact]
    public void NameTokenWithWhitespaces()
    {
        var token = $"\n        foo\n\n    ".Lex();
        token.Kind.ShouldBe(TokenKind.NAME);
        token.Start.ShouldBe(9);
        token.End.ShouldBe(12);
        token.Value.ShouldBe("foo");
    }

    [Fact]
    public void NullInput_ReturnsEOF()
    {
        var token = ((string)null).Lex();

        token.Kind.ShouldBe(TokenKind.EOF);
    }

    [Fact]
    public void PipePunctuation()
    {
        var token = "|".Lex();
        token.Kind.ShouldBe(TokenKind.PIPE);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("|");
    }

    [Fact]
    public void QuoteString()
    {
        var token = "\"quote \\\"\"".Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(10);
        token.Value.ShouldBe("quote \"");
    }

    [Fact]
    public void RightBracePunctuation()
    {
        var token = "}".Lex();
        token.Kind.ShouldBe(TokenKind.BRACE_R);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("}");
    }

    [Fact]
    public void RightBracketPunctuation()
    {
        var token = "]".Lex();
        token.Kind.ShouldBe(TokenKind.BRACKET_R);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("]");
    }

    [Fact]
    public void RightParenthesisPunctuation()
    {
        var token = ")".Lex();
        token.Kind.ShouldBe(TokenKind.PAREN_R);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe(")");
    }

    [Fact]
    public void SimpleString()
    {
        var token = "\"str\"".Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("str");
    }

    [Fact]
    public void SingleDecimalInt()
    {
        var token = "0".Lex();
        token.Kind.ShouldBe(TokenKind.INT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(1);
        token.Value.ShouldBe("0");
    }

    [Fact]
    public void SingleFloat()
    {
        var token = "4.123".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("4.123");
    }

    [Fact]
    public void SingleFloatWithZero()
    {
        var token = "12.10".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("12.10");
    }

    [Fact]
    public void SingleFloatWithExplicitlyPositiveExponent()
    {
        var token = "123e+4".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(6);
        token.Value.ShouldBe("123e+4");
    }

    [Fact]
    public void SingleFloatWithExponentCapitalLetter()
    {
        var token = "123E4".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("123E4");
    }

    [Fact]
    public void SingleFloatWithExponent()
    {
        var token = "123e4".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("123e4");
    }

    [Fact]
    public void SingleFloatWithNegativeExponent()
    {
        var token = "123e-4".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(6);
        token.Value.ShouldBe("123e-4");
    }

    [Fact]
    public void SingleNameSurroundedByCommas()
    {
        var token = ",,,foo,,,".Lex();
        token.Kind.ShouldBe(TokenKind.NAME);
        token.Start.ShouldBe(3);
        token.End.ShouldBe(6);
        token.Value.ShouldBe("foo");
    }

    [Fact]
    public void SingleNameWithBOMHeader()
    {
        var token = "\uFEFF foo\\".Lex();
        token.Kind.ShouldBe(TokenKind.NAME);
        token.Start.ShouldBe(2);
        token.End.ShouldBe(5);
        token.Value.ShouldBe("foo");
    }

    [Fact]
    public void SingleNegativeFloat()
    {
        var token = "-0.123".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(6);
        token.Value.ShouldBe("-0.123");
    }

    [Fact]
    public void SingleNegativeFloatWithExponent()
    {
        var token = "-123e4".Lex();
        token.Kind.ShouldBe(TokenKind.FLOAT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(6);
        token.Value.ShouldBe("-123e4");
    }

    [Fact]
    public void SingleNegativeInt()
    {
        var token = "-3".Lex();
        token.Kind.ShouldBe(TokenKind.INT);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(2);
        token.Value.ShouldBe("-3");
    }

    [Fact]
    public void SingleStringWithSlashes()
    {
        var token = "\"slashes \\\\ \\/\"".Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(15);
        token.Value.ShouldBe("slashes \\ /");
    }

    [Fact]
    public void SingleStringWithUnicodeCharacters()
    {
        var token = "\"unicode \\u1234\\u5678\\u90AB\\uCDEF\"".Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(34);
        token.Value.ShouldBe("unicode \u1234\u5678\u90AB\uCDEF");
    }

    [Fact]
    public void SpreadPunctuation()
    {
        var token = "...".Lex();
        token.Kind.ShouldBe(TokenKind.SPREAD);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(3);
        token.Value.ShouldBe("...");
    }

    [Fact]
    public void WhiteSpaceString()
    {
        var token = "\" white space \"".Lex();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(15);
        token.Value.ShouldBe(" white space ");
    }

    [Theory]
    [InlineData(1, "test", "test")]
    [InlineData(2, "te\\\"\"\"st", "te\"\"\"st")]
    [InlineData(3, "\ntest", "test")]
    [InlineData(4, "\r\ntest", "test")]
    [InlineData(5, " \ntest", "test")]
    [InlineData(6, "\t\ntest", "test")]
    [InlineData(7, "\n\ntest", "test")]
    [InlineData(8, "test\nline2", "test\nline2")]
    [InlineData(9, "test\rline2", "test\nline2")]
    [InlineData(10, "test\r\nline2", "test\nline2")]
    [InlineData(11, "test\r\r\nline2", "test\n\nline2")]
    [InlineData(12, "test\r\n\nline2", "test\n\nline2")]
    [InlineData(13, "test\n", "test")]
    [InlineData(14, "test\n ", "test")]
    [InlineData(15, "test\n\t", "test")]
    [InlineData(16, "test\n\n", "test")]
    [InlineData(17, "test\n  line2", "test\nline2")]
    [InlineData(18, "test\n\t\tline2", "test\nline2")]
    [InlineData(19, "test\n \tline2", "test\nline2")]
    [InlineData(20, "  test\nline2", "  test\nline2")]
    [InlineData(21, "  test\n  line2", "  test\nline2")]
    [InlineData(22, "\n  test\n  line2", "test\nline2")]
    [InlineData(23, "  test\n line2\n\t\tline3\n  line4", "  test\nline2\n\tline3\n line4")]
    [InlineData(24, "  test\n  Hello,\n\n    world!\n ", "  test\nHello,\n\n  world!")]
    [InlineData(25, "  \n  Hello,\r\n\n    world!\n ", "Hello,\n\n  world!")]
    [InlineData(26, "  \n  Hello,\r\n\n    wor___ld!\n ", "Hello,\n\n  wor___ld!")]
    [InlineData(27, "\r\n    Hello,\r\n      World!\r\n\r\n    Yours,\r\n      GraphQL.\r\n  ", "Hello,\n  World!\n\nYours,\n  GraphQL.")]
    [InlineData(28, "Test \\n escaping", "Test \\n escaping")]
    [InlineData(29, "Test \\u1234 escaping", "Test \\u1234 escaping")]
    [InlineData(30, "Test \\ escaping", "Test \\ escaping")]
    public void BlockString(int number, string input, string expected)
    {
        number.ShouldBeGreaterThan(0);

        input = input.Replace("___", new string('_', 9000));
        expected = expected.Replace("___", new string('_', 9000));
        input = "\"\"\"" + input + "\"\"\"";
        var actual = input.Lex();
        actual.Kind.ShouldBe(TokenKind.STRING);
        actual.Value.ToString().ShouldBe(expected);
    }

    [Fact]
    public void BlockString_LineFeeds()
    {
        var str = "\"\"\"test" + new string(' ', 4092) + "\r\ntest\rtest\ntest\"\"\"";
        var test = new LexerContext(str, 0);
        var token = test.GetToken();
        token.Kind.ShouldBe(TokenKind.STRING);
        token.Value.ToString().ShouldBe("test" + new string(' ', 4092) + "\ntest\ntest\ntest");
    }
}
