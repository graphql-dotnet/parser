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
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"multi\rline\"".Lex());

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
            var token = "a-b".Lex();

            token.Kind.ShouldBe(TokenKind.NAME);
            token.Start.ShouldBe(0);
            token.End.ShouldBe(1);
            token.Value.ShouldBe("a");

            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => Lexer.Lex("a-b", token.End));

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"b\"\n" +
                "1: a-b\n" +
                "     ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"b\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_IncompleteSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "..".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \".\"\n" +
                "1: ..\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \".\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_InvalidCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\u0007".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Invalid character \"\\u0007\".\n" +
                "1: \\u0007\n" +
                "   ^\n");
            exception.Description.ShouldBe("Invalid character \"\\u0007\".");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceXCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\x esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\x.\n" +
                "1: \"bad \\x esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\x.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidEscapeSequenceZetCharacter_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\z esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\z.\n" +
                "1: \"bad \\z esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\z.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\u1 esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\u1 es.\n" +
                "1: \"bad \\u1 esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\u1 es.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\u0XX1 esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\u0XX1.\n" +
                "1: \"bad \\u0XX1 esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\u0XX1.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode3_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\uFXXX esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uFXXX.\n" +
                "1: \"bad \\uFXXX esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\uFXXX.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode4_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\uXXXX esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uXXXX.\n" +
                "1: \"bad \\uXXXX esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\uXXXX.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_InvalidUnicode5_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"bad \\uXXXF esc\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Invalid character escape sequence: \\uXXXF.\n" +
                "1: \"bad \\uXXXF esc\"\n" +
                "         ^\n");
            exception.Description.ShouldBe("Invalid character escape sequence: \\uXXXF.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_LineBreakInMiddleOfString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"multi\nline\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:7) Unterminated string.\n" +
                "1: \"multi\n" +
                "         ^\n" +
                "2: line\"\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(7);
        }

        [Fact]
        public void Lex_LonelyQuestionMark_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "?".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \"?\"\n" +
                "1: ?\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \"?\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_MissingExponentInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "1.0e".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:5) Invalid number, expected digit but got: <EOF>\n" +
                "1: 1.0e\n" +
                "       ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: <EOF>");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(5);
        }

        [Fact]
        public void Lex_NonNumericCharacterInNumberExponent_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "1.0eA".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:5) Invalid number, expected digit but got: \"A\"\n" +
                "1: 1.0eA\n" +
                "       ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(5);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "1.A".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: \"A\"\n" +
                "1: 1.A\n" +
                "     ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_NonNumericCharInNumber2_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "-A".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:2) Invalid number, expected digit but got: \"A\"\n" +
                "1: -A\n" +
                "    ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: \"A\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_NotAllowedUnicode_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\\u203B".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \"\\u203B\"\n" +
                "1: \\u203B\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \"\\u203B\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NotAllowedUnicode1_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\\u200b".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \"\\u200b\"\n" +
                "1: \\u200b\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \"\\u200b\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NullByteInString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"null-byte is not \u0000 end of file".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:19) Invalid character within String: \\u0000.\n" +
                "1: \"null-byte is not \\u0000 end of file\n" +
                "                     ^\n");
            exception.Description.ShouldBe("Invalid character within String: \\u0000.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(19);
        }

        [Fact]
        public void Lex_NumberDoubleZeros_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "00".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:2) Invalid number, unexpected digit after 0: \"0\"\n" +
                "1: 00\n" +
                "    ^\n");
            exception.Description.ShouldBe("Invalid number, unexpected digit after 0: " + "\"0\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_NumberNoDecimalPartEOFInstead_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "1.".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:3) Invalid number, expected digit but got: <EOF>\n" +
                "1: 1.\n" +
                "     ^\n");
            exception.Description.ShouldBe("Invalid number, expected digit but got: <EOF>");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(3);
        }

        [Fact]
        public void Lex_NumberPlusOne_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "+1".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \"+\"\n" +
                "1: +1\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \"+\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_NumberStartingWithDot_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => ".123".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected character \".\"\n" +
                "1: .123\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected character \".\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Lex_UnescapedControlChar_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"contains unescaped \u0007 control char".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:21) Invalid character within String: \\u0007.\n" +
                "1: \"contains unescaped \\u0007 control char\n" +
                "                       ^\n");
            exception.Description.ShouldBe("Invalid character within String: \\u0007.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(21);
        }

        [Fact]
        public void Lex_UnescapedControlChar_Blockstring_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"\"\"contains unescaped \u0007 control char".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:23) Invalid character within BlockString: \\u0007.\n" +
                "1: \"\"\"contains unescaped \\u0007 control char\n" +
                "                         ^\n");
            exception.Description.ShouldBe("Invalid character within BlockString: \\u0007.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(23);
        }

        [Fact]
        public void Lex_UnterminatedString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:2) Unterminated string.\n" +
                "1: \"\n" +
                "    ^\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Lex_UnterminatedStringWithText_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"no end quote".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:14) Unterminated string.\n" +
                "1: \"no end quote\n" +
                "                ^\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(14);
        }

        [Fact]
        public void Lex_UnterminatedBlockString_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"\"\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:4) Unterminated string.\n" +
                "1: \"\"\"\n" +
                "      ^\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(4);
        }

        [Fact]
        public void Lex_UnterminatedBlockStringWithText_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "\"\"\"no end triple-quote\"\"".Lex());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:25) Unterminated string.\n" +
                "1: \"\"\"no end triple-quote\"\"\n" +
                "                           ^\n");
            exception.Description.ShouldBe("Unterminated string.");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(25);
        }
    }
}
