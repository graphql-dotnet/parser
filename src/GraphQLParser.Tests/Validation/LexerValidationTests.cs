using GraphQLParser.Exceptions;
using GraphQLParser;
using System;
using Xunit;

namespace GraphQLParser.Tests.Validation
{
    public class LexerValidationTests
    {
        [Fact]
        public void Lex_CarriageReturnInMiddleOfString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"multi\rline\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Unterminated string.\n" +
                "1: \"multi\rline\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_DashesInName_ThrowsExceptionWithCorrectMessage()
        {
            var token = new Lexer().Lex(new Source("a-b"));

            Assert.Equal(TokenKind.NAME, token.Kind);
            Assert.Equal(0, token.Start);
            Assert.Equal(1, token.End);
            Assert.Equal("a", token.Value);

            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("a-b"), token.End));

            Assert.Equal(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"b\"\n" +
                "1: a-b\n" +
                "     ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_IncompleteSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("..")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \".\"\n" +
                "1: ..\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\u0007")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Invalid character \"\\u0007\".\n" +
                "1: \\u0007\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceXCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\x esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\x.\n" +
                "1: \"bad \\x esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceZetCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\z esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\z.\n" +
                "1: \"bad \\z esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\u1 esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\u1 es.\n" +
                "1: \"bad \\u1 esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidUnicode2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\u0XX1 esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\u0XX1.\n" +
                "1: \"bad \\u0XX1 esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidUnicode3_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uFXXX esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uFXXX.\n" +
                "1: \"bad \\uFXXX esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidUnicode4_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uXXXX esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uXXXX.\n" +
                "1: \"bad \\uXXXX esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_InvalidUnicode5_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"bad \\uXXXF esc\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uXXXF.\n" +
                "1: \"bad \\uXXXF esc\"\n" +
                "         ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_LineBreakInMiddleOfString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"multi\nline\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:7) Unterminated string.\n" +
                "1: \"multi\n" +
                "         ^\n" +
                "2: line\"\n",
                exception.Message);
        }

        [Fact]
        public void Lex_LonelyQuestionMark_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("?")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \"?\"\n" +
                "1: ?\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_MissingExponentInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.0e")));

            Assert.Equal(
                "Syntax Error GraphQL (1:5) Invalid number, expected digit but got: <EOF>\n" +
                "1: 1.0e\n" +
                "       ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NonNumericCharacterInNumberExponent_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.0eA")));

            Assert.Equal(
                "Syntax Error GraphQL (1:5) Invalid number, expected digit but got: \"A\"\n" +
                "1: 1.0eA\n" +
                "       ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.A")));

            Assert.Equal(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"A\"\n" +
                "1: 1.A\n" +
                "     ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("-A")));

            Assert.Equal(
                "Syntax Error GraphQL (1:2) Invalid number, expected digit but got: \"A\"\n" +
                "1: -A\n" +
                "    ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NotAllowedUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\\u203B")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \"\\u203B\"\n" +
                "1: \\u203B\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NotAllowedUnicode1_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\\u200b")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \"\\u200b\"\n" +
                "1: \\u200b\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NullByteInString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"null-byte is not \u0000 end of file")));

            Assert.Equal(
                "Syntax Error GraphQL (1:19) Invalid character within String: \\u0000.\n" +
                "1: \"null-byte is not \\u0000 end of file\n" +
                "                     ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NumberDoubleZeros_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("00")));

            Assert.Equal(
                "Syntax Error GraphQL (1:2) Invalid number, unexpected digit after 0: \"0\"\n" +
                "1: 00\n" +
                "    ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NumberNoDecimalPartEOFInstead_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("1.")));

            Assert.Equal(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: <EOF>\n" +
                "1: 1.\n" +
                "     ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NumberPlusOne_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("+1")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \"+\"\n" +
                "1: +1\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_NumberStartingWithDot_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source(".123")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected character \".\"\n" +
                "1: .123\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_UnescapedControlChar_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"contains unescaped \u0007 control char")));

            Assert.Equal(
                "Syntax Error GraphQL (1:21) Invalid character within String: \\u0007.\n" +
                "1: \"contains unescaped \\u0007 control char\n" +
                "                       ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_UnterminatedString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"")));

            Assert.Equal(
                "Syntax Error GraphQL (1:2) Unterminated string.\n" +
                "1: \"\n" +
                "    ^\n",
                exception.Message);
        }

        [Fact]
        public void Lex_UnterminatedStringWithText_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Lexer().Lex(new Source("\"no end quote")));

            Assert.Equal(
                "Syntax Error GraphQL (1:14) Unterminated string.\n" +
                "1: \"no end quote\n" +
                "                ^\n",
                exception.Message);
        }
    }
}
