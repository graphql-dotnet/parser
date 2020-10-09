using System;
using GraphQLParser.Exceptions;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Validation
{
    public class LexerValidationTests
    {
        [Fact]
        public void Lex_CarriageReturnInMiddleOfString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"multi\rline\"")));

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Unterminated string.\n" +
                "1: \"multi\rline\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_DashesInName_ThrowsExceptionWithCorrectMessage()
        {
            var token = new Lexer().Lex(new Source("a-b"));

            token.Kind.ShouldBe(TokenKind.NAME);
            token.Start.ShouldBe(0);
            token.End.ShouldBe(1);
            token.Value.ShouldBe("a");

            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("a-b"), token.End));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"b\"" + @"
1: a-b
     ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"b\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_IncompleteSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("..")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \".\"" + @"
1: ..
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \".\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_InvalidCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\u0007")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:1) Invalid character " + "\"\\u0007\"" + @".
1: \u0007
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character \"\\u0007\".");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceXCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\x esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \x.
1: " + "\"bad \\x esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\x.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceZetCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\z esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \z.
1: " + "\"bad \\z esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\z.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\u1 esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \u1 es.
1: " + "\"bad \\u1 esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\u1 es.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\u0XX1 esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \u0XX1.
1: " + "\"bad \\u0XX1 esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\u0XX1.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode3_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uFXXX esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \uFXXX.
1: " + "\"bad \\uFXXX esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\uFXXX.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode4_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uXXXX esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \uXXXX.
1: " + "\"bad \\uXXXX esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\uXXXX.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode5_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uXXXF esc\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Invalid character escape sequence: \uXXXF.
1: " + "\"bad \\uXXXF esc\"" + @"
         ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character escape sequence: \\uXXXF.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_LineBreakInMiddleOfString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"multi\nline\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:7) Unterminated string.
1: " + "\"multi" + @"
         ^
2: line" + "\"" + @"
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_LonelyQuestionMark_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("?")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \"?\"" + @"
1: ?
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \"?\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_MissingExponentInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.0e")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:5) Invalid number, expected digit but got: <EOF>" + @"
1: 1.0e
       ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: <EOF>");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(5);
        }

        [Fact]
        public void Lex_NonNumericCharacterInNumberExponent_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.0eA")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:5) Invalid number, expected digit but got: \"A\"" + @"
1: 1.0eA
       ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(5);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.A")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"A\"" + @"
1: 1.A
     ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("-A")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:2) Invalid number, expected digit but got: \"A\"" + @"
1: -A
    ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_NotAllowedUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\\u203B")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \"\\u203B\"" + @"
1: \u203B
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \"\\u203B\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NotAllowedUnicode1_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\\u200b")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \"\\u200b\"" + @"
1: \u200b
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \"\\u200b\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NullByteInString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"null-byte is not \u0000 end of file")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:19) Invalid character within String: \u0000.
1: " + "\"null-byte is not \\u0000 end of file" + @"
                     ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character within String: \\u0000.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(19);
        }

        [Fact]
        public void Lex_NumberDoubleZeros_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("00")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:2) Invalid number, unexpected digit after 0: " + "\"0\"" + @"
1: 00
    ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, unexpected digit after 0: " + "\"0\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_NumberNoDecimalPartEOFInstead_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:3) Invalid number, expected digit but got: <EOF>" + @"
1: 1.
     ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid number, expected digit but got: <EOF>");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_NumberPlusOne_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("+1")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \"+\"" + @"
1: +1
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \"+\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NumberStartingWithDot_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source(".123")));

            exception.Message.ShouldBe(("Syntax Error GraphQL (1:1) Unexpected character \".\"" + @"
1: .123
   ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected character \".\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_UnescapedControlChar_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"contains unescaped \u0007 control char")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:21) Invalid character within String: \u0007.
1: " + "\"contains unescaped \\u0007 control char" + @"
                       ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Invalid character within String: \\u0007.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(21);
        }

        [Fact]
        public void Lex_UnterminatedString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:2) Unterminated string.
1: " + "\"" + @"
    ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_UnterminatedStringWithText_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"no end quote")));

            exception.Message.ShouldBe((@"Syntax Error GraphQL (1:14) Unterminated string.
1: " + "\"no end quote" + @"
                ^
").Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(14);
        }
    }
}
