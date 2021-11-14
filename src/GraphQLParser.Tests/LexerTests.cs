using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "Tests")]
    public class LexerTests
    {
        [Fact]
        public void Lex_ATPunctuation_HasCorrectEnd()
        {
            var token = GetATPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_ATPunctuation_HasCorrectKind()
        {
            var token = GetATPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.AT);
        }

        [Fact]
        public void Lex_ATPunctuation_HasCorrectStart()
        {
            var token = GetATPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_ATPunctuation_HasCorrectValue()
        {
            var token = GetATPunctuationTokenLexer();
            token.Value.ShouldBe("@");
        }

        [Fact]
        public void Lex_BangPunctuation_HasCorrectEnd()
        {
            var token = GetBangPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_BangPunctuation_HasCorrectKind()
        {
            var token = GetBangPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.BANG);
        }

        [Fact]
        public void Lex_BangPunctuation_HasCorrectStart()
        {
            var token = GetBangPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_BangPunctuation_HasCorrectValue()
        {
            var token = GetBangPunctuationTokenLexer();
            token.Value.ShouldBe("!");
        }

        [Fact]
        public void Lex_ColonPunctuation_HasCorrectEnd()
        {
            var token = GetColonPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_ColonPunctuation_HasCorrectKind()
        {
            var token = GetColonPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.COLON);
        }

        [Fact]
        public void Lex_ColonPunctuation_HasCorrectStart()
        {
            var token = GetColonPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_ColonPunctuation_HasCorrectValue()
        {
            var token = GetColonPunctuationTokenLexer();
            token.Value.ShouldBe(":");
        }

        [Fact]
        public void Lex_DollarPunctuation_HasCorrectEnd()
        {
            var token = GetDollarPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_DollarPunctuation_HasCorrectKind()
        {
            var token = GetDollarPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.DOLLAR);
        }

        [Fact]
        public void Lex_DollarPunctuation_HasCorrectStart()
        {
            var token = GetDollarPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_DollarPunctuation_HasCorrectValue()
        {
            var token = GetDollarPunctuationTokenLexer();
            token.Value.ShouldBe("$");
        }

        [Fact]
        public void Lex_EmptySource_ReturnsEOF()
        {
            var token = "".Lex();

            token.Kind.ShouldBe(TokenKind.EOF);
        }

        [Fact]
        public void Lex_EqualsPunctuation_HasCorrectEnd()
        {
            var token = GetEqualsPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_EqualsPunctuation_HasCorrectKind()
        {
            var token = GetEqualsPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.EQUALS);
        }

        [Fact]
        public void Lex_EqualsPunctuation_HasCorrectStart()
        {
            var token = GetEqualsPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_EqualsPunctuation_HasCorrectValue()
        {
            var token = GetEqualsPunctuationTokenLexer();
            token.Value.ShouldBe("=");
        }

        [Fact]
        public void Lex_EscapedStringToken_HasCorrectEnd()
        {
            var token = GetEscapedStringTokenLexer();
            token.End.ShouldBe(20);
        }

        [Fact]
        public void Lex_EscapedStringToken_HasCorrectStart()
        {
            var token = GetEscapedStringTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_EscapedStringToken_HasCorrectValue()
        {
            var token = GetEscapedStringTokenLexer();
            token.Value.ShouldBe("escaped \n\r\b\t\f");
        }

        [Fact]
        public void Lex_EscapedStringToken_HasStringKind()
        {
            var token = GetEscapedStringTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Fact]
        public void Lex_LeftBracePunctuation_HasCorrectEnd()
        {
            var token = GetLeftBracePunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_LeftBracePunctuation_HasCorrectKind()
        {
            var token = GetLeftBracePunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.BRACE_L);
        }

        [Fact]
        public void Lex_LeftBracePunctuation_HasCorrectStart()
        {
            var token = GetLeftBracePunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_LeftBracePunctuation_HasCorrectValue()
        {
            var token = GetLeftBracePunctuationTokenLexer();
            token.Value.ShouldBe("{");
        }

        [Fact]
        public void Lex_LeftBracketPunctuation_HasCorrectEnd()
        {
            var token = GetLeftBracketPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_LeftBracketPunctuation_HasCorrectKind()
        {
            var token = GetLeftBracketPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.BRACKET_L);
        }

        [Fact]
        public void Lex_LeftBracketPunctuation_HasCorrectStart()
        {
            var token = GetLeftBracketPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_LeftBracketPunctuation_HasCorrectValue()
        {
            var token = GetLeftBracketPunctuationTokenLexer();
            token.Value.ShouldBe("[");
        }

        [Fact]
        public void Lex_LeftParenthesisPunctuation_HasCorrectEnd()
        {
            var token = GetLeftParenthesisPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_LeftParenthesisPunctuation_HasCorrectKind()
        {
            var token = GetLeftParenthesisPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.PAREN_L);
        }

        [Fact]
        public void Lex_LeftParenthesisPunctuation_HasCorrectStart()
        {
            var token = GetLeftParenthesisPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_LeftParenthesisPunctuation_HasCorrectValue()
        {
            var token = GetLeftParenthesisPunctuationTokenLexer();
            token.Value.ShouldBe("(");
        }

        [Fact]
        public void Lex_MultipleDecimalsIntToken_HasCorrectEnd()
        {
            var token = GetMultipleDecimalsIntTokenLexer();
            token.End.ShouldBe(3);
        }

        [Fact]
        public void Lex_MultipleDecimalsIntToken_HasCorrectStart()
        {
            var token = GetMultipleDecimalsIntTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_MultipleDecimalsIntToken_HasCorrectValue()
        {
            var token = GetMultipleDecimalsIntTokenLexer();
            token.Value.ShouldBe("123");
        }

        [Fact]
        public void Lex_MultipleDecimalsIntToken_HasIntKind()
        {
            var token = GetMultipleDecimalsIntTokenLexer();
            token.Kind.ShouldBe(TokenKind.INT);
        }

        [Fact]
        public void Lex_NameTokenWithComments_HasCorrectEnd()
        {
            var token = GetSingleNameTokenLexerWithComments();
            token.End.ShouldBe(10);
        }

        [Fact]
        public void Lex_NameTokenWithComments_HasCorrectStart()
        {
            var token = GetSingleNameTokenLexerWithComments();
            token.Start.ShouldBe(1);
        }

        [Fact]
        public void Lex_NameTokenWithComments_HasCorrectValue()
        {
            var token = GetSingleNameTokenLexerWithComments();
            token.Value.ShouldBe("comment");
        }

        [Fact]
        public void Lex_NameTokenWithComments_HasNameKind()
        {
            var token = GetSingleNameTokenLexerWithComments();
            token.Kind.ShouldBe(TokenKind.COMMENT);
        }

        [Fact]
        public void Lex_NameTokenWithWhitespaces_HasCorrectEnd()
        {
            var token = GetSingleNameTokenLexerSurroundedWithWhitespaces();
            token.End.ShouldBe(12);
        }

        [Fact]
        public void Lex_NameTokenWithWhitespaces_HasCorrectStart()
        {
            var token = GetSingleNameTokenLexerSurroundedWithWhitespaces();
            token.Start.ShouldBe(9);
        }

        [Fact]
        public void Lex_NameTokenWithWhitespaces_HasCorrectValue()
        {
            var token = GetSingleNameTokenLexerSurroundedWithWhitespaces();
            token.Value.ShouldBe("foo");
        }

        [Fact]
        public void Lex_NameTokenWithWhitespaces_HasNameKind()
        {
            var token = GetSingleNameTokenLexerSurroundedWithWhitespaces();
            token.Kind.ShouldBe(TokenKind.NAME);
        }

        [Fact]
        public void Lex_NullInput_ReturnsEOF()
        {
            var token = ((string)null).Lex();

            token.Kind.ShouldBe(TokenKind.EOF);
        }

        [Fact]
        public void Lex_PipePunctuation_HasCorrectEnd()
        {
            var token = GetPipePunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_PipePunctuation_HasCorrectKind()
        {
            var token = GetPipePunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.PIPE);
        }

        [Fact]
        public void Lex_PipePunctuation_HasCorrectStart()
        {
            var token = GetPipePunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_PipePunctuation_HasCorrectValue()
        {
            var token = GetPipePunctuationTokenLexer();
            token.Value.ShouldBe("|");
        }

        [Fact]
        public void Lex_QuoteStringToken_HasCorrectEnd()
        {
            var token = GetQuoteStringTokenLexer();
            token.End.ShouldBe(10);
        }

        [Fact]
        public void Lex_QuoteStringToken_HasCorrectStart()
        {
            var token = GetQuoteStringTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_QuoteStringToken_HasCorrectValue()
        {
            var token = GetQuoteStringTokenLexer();
            token.Value.ShouldBe("quote \"");
        }

        [Fact]
        public void Lex_QuoteStringToken_HasStringKind()
        {
            var token = GetQuoteStringTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Fact]
        public void Lex_RightBracePunctuation_HasCorrectEnd()
        {
            var token = GetRightBracePunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_RightBracePunctuation_HasCorrectKind()
        {
            var token = GetRightBracePunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.BRACE_R);
        }

        [Fact]
        public void Lex_RightBracePunctuation_HasCorrectStart()
        {
            var token = GetRightBracePunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_RightBracePunctuation_HasCorrectValue()
        {
            var token = GetRightBracePunctuationTokenLexer();
            token.Value.ShouldBe("}");
        }

        [Fact]
        public void Lex_RightBracketPunctuation_HasCorrectEnd()
        {
            var token = GetRightBracketPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_RightBracketPunctuation_HasCorrectKind()
        {
            var token = GetRightBracketPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.BRACKET_R);
        }

        [Fact]
        public void Lex_RightBracketPunctuation_HasCorrectStart()
        {
            var token = GetRightBracketPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_RightBracketPunctuation_HasCorrectValue()
        {
            var token = GetRightBracketPunctuationTokenLexer();
            token.Value.ShouldBe("]");
        }

        [Fact]
        public void Lex_RightParenthesisPunctuation_HasCorrectEnd()
        {
            var token = GetRightParenthesisPunctuationTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_RightParenthesisPunctuation_HasCorrectKind()
        {
            var token = GetRightParenthesisPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.PAREN_R);
        }

        [Fact]
        public void Lex_RightParenthesisPunctuation_HasCorrectStart()
        {
            var token = GetRightParenthesisPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_RightParenthesisPunctuation_HasCorrectValue()
        {
            var token = GetRightParenthesisPunctuationTokenLexer();
            token.Value.ShouldBe(")");
        }

        [Fact]
        public void Lex_SimpleStringToken_HasCorrectEnd()
        {
            var token = GetSimpleStringTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SimpleStringToken_HasCorrectStart()
        {
            var token = GetSimpleStringTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SimpleStringToken_HasCorrectValue()
        {
            var token = GetSimpleStringTokenLexer();
            token.Value.ShouldBe("str");
        }

        [Fact]
        public void Lex_SimpleStringToken_HasStringKind()
        {
            var token = GetSimpleStringTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Fact]
        public void Lex_SingleDecimalIntToken_HasCorrectEnd()
        {
            var token = GetSingleDecimalIntTokenLexer();
            token.End.ShouldBe(1);
        }

        [Fact]
        public void Lex_SingleDecimalIntToken_HasCorrectStart()
        {
            var token = GetSingleDecimalIntTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleDecimalIntToken_HasCorrectValue()
        {
            var token = GetSingleDecimalIntTokenLexer();
            token.Value.ShouldBe("0");
        }

        [Fact]
        public void Lex_SingleDecimalIntToken_HasIntKind()
        {
            var token = GetSingleDecimalIntTokenLexer();
            token.Kind.ShouldBe(TokenKind.INT);
        }

        [Fact]
        public void Lex_SingleFloatTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SingleFloatTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatTokenLexer();
            token.Value.ShouldBe("4.123");
        }

        [Fact]
        public void Lex_SingleFloatWithZeroTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatWithZeroTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SingleFloatWithZeroTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatWithZeroTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatWithZeroTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatWithZeroTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatWithZeroTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatWithZeroTokenLexer();
            token.Value.ShouldBe("12.10");
        }

        [Fact]
        public void Lex_SingleFloatWithExplicitlyPositiveExponentTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatWithExplicitlyPositiveExponentTokenLexer();
            token.End.ShouldBe(6);
        }

        [Fact]
        public void Lex_SingleFloatWithExplicitlyPositiveExponentTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatWithExplicitlyPositiveExponentTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatWithExplicitlyPositiveExponentTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatWithExplicitlyPositiveExponentTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatWithExplicitlyPositiveExponentTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatWithExplicitlyPositiveExponentTokenLexer();
            token.Value.ShouldBe("123e+4");
        }

        [Fact]
        public void Lex_SingleFloatWithExponentCapitalLetterTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatWithExponentCapitalLetterTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentCapitalLetterTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatWithExponentCapitalLetterTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentCapitalLetterTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatWithExponentCapitalLetterTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentCapitalLetterTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatWithExponentCapitalLetterTokenLexer();
            token.Value.ShouldBe("123E4");
        }

        [Fact]
        public void Lex_SingleFloatWithExponentTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatWithExponentTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatWithExponentTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatWithExponentTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatWithExponentTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatWithExponentTokenLexer();
            token.Value.ShouldBe("123e4");
        }

        [Fact]
        public void Lex_SingleFloatWithNegativeExponentTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleFloatWithNegativeExponentTokenLexer();
            token.End.ShouldBe(6);
        }

        [Fact]
        public void Lex_SingleFloatWithNegativeExponentTokenLexer_HasCorrectKind()
        {
            var token = GetSingleFloatWithNegativeExponentTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleFloatWithNegativeExponentTokenLexer_HasCorrectStart()
        {
            var token = GetSingleFloatWithNegativeExponentTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleFloatWithNegativeExponentTokenLexer_HasCorrectValue()
        {
            var token = GetSingleFloatWithNegativeExponentTokenLexer();
            token.Value.ShouldBe("123e-4");
        }

        [Fact]
        public void Lex_SingleNameSurroundedByCommasTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleNameSurroundedByCommasTokenLexer();
            token.End.ShouldBe(6);
        }

        [Fact]
        public void Lex_SingleNameSurroundedByCommasTokenLexer_HasCorrectKind()
        {
            var token = GetSingleNameSurroundedByCommasTokenLexer();
            token.Kind.ShouldBe(TokenKind.NAME);
        }

        [Fact]
        public void Lex_SingleNameSurroundedByCommasTokenLexer_HasCorrectStart()
        {
            var token = GetSingleNameSurroundedByCommasTokenLexer();
            token.Start.ShouldBe(3);
        }

        [Fact]
        public void Lex_SingleNameSurroundedByCommasTokenLexer_HasCorrectValue()
        {
            var token = GetSingleNameSurroundedByCommasTokenLexer();
            token.Value.ShouldBe("foo");
        }

        [Fact]
        public void Lex_SingleNameWithBOMHeaderTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleNameWithBOMHeaderTokenLexer();
            token.End.ShouldBe(5);
        }

        [Fact]
        public void Lex_SingleNameWithBOMHeaderTokenLexer_HasCorrectKind()
        {
            var token = GetSingleNameWithBOMHeaderTokenLexer();
            token.Kind.ShouldBe(TokenKind.NAME);
        }

        [Fact]
        public void Lex_SingleNameWithBOMHeaderTokenLexer_HasCorrectStart()
        {
            var token = GetSingleNameWithBOMHeaderTokenLexer();
            token.Start.ShouldBe(2);
        }

        [Fact]
        public void Lex_SingleNameWithBOMHeaderTokenLexer_HasCorrectValue()
        {
            var token = GetSingleNameWithBOMHeaderTokenLexer();
            token.Value.ShouldBe("foo");
        }

        [Fact]
        public void Lex_SingleNegativeFloatTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleNegativeFloatTokenLexer();
            token.End.ShouldBe(6);
        }

        [Fact]
        public void Lex_SingleNegativeFloatTokenLexer_HasCorrectKind()
        {
            var token = GetSingleNegativeFloatTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleNegativeFloatTokenLexer_HasCorrectStart()
        {
            var token = GetSingleNegativeFloatTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleNegativeFloatTokenLexer_HasCorrectValue()
        {
            var token = GetSingleNegativeFloatTokenLexer();
            token.Value.ShouldBe("-0.123");
        }

        [Fact]
        public void Lex_SingleNegativeFloatWithExponentTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleNegativeFloatWithExponentTokenLexer();
            token.End.ShouldBe(6);
        }

        [Fact]
        public void Lex_SingleNegativeFloatWithExponentTokenLexer_HasCorrectKind()
        {
            var token = GetSingleNegativeFloatWithExponentTokenLexer();
            token.Kind.ShouldBe(TokenKind.FLOAT);
        }

        [Fact]
        public void Lex_SingleNegativeFloatWithExponentTokenLexer_HasCorrectStart()
        {
            var token = GetSingleNegativeFloatWithExponentTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleNegativeFloatWithExponentTokenLexer_HasCorrectValue()
        {
            var token = GetSingleNegativeFloatWithExponentTokenLexer();
            token.Value.ShouldBe("-123e4");
        }

        [Fact]
        public void Lex_SingleNegativeIntTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleNegativeIntTokenLexer();
            token.End.ShouldBe(2);
        }

        [Fact]
        public void Lex_SingleNegativeIntTokenLexer_HasCorrectKind()
        {
            var token = GetSingleNegativeIntTokenLexer();
            token.Kind.ShouldBe(TokenKind.INT);
        }

        [Fact]
        public void Lex_SingleNegativeIntTokenLexer_HasCorrectStart()
        {
            var token = GetSingleNegativeIntTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleNegativeIntTokenLexer_HasCorrectValue()
        {
            var token = GetSingleNegativeIntTokenLexer();
            token.Value.ShouldBe("-3");
        }

        [Fact]
        public void Lex_SingleStringWithSlashesTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleStringWithSlashesTokenLexer();
            token.End.ShouldBe(15);
        }

        [Fact]
        public void Lex_SingleStringWithSlashesTokenLexer_HasCorrectKind()
        {
            var token = GetSingleStringWithSlashesTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Fact]
        public void Lex_SingleStringWithSlashesTokenLexer_HasCorrectStart()
        {
            var token = GetSingleStringWithSlashesTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleStringWithSlashesTokenLexer_HasCorrectValue()
        {
            var token = GetSingleStringWithSlashesTokenLexer();
            token.Value.ShouldBe("slashes \\ /");
        }

        [Fact]
        public void Lex_SingleStringWithUnicodeCharactersTokenLexer_HasCorrectEnd()
        {
            var token = GetSingleStringWithUnicodeCharactersTokenLexer();
            token.End.ShouldBe(34);
        }

        [Fact]
        public void Lex_SingleStringWithUnicodeCharactersTokenLexer_HasCorrectKind()
        {
            var token = GetSingleStringWithUnicodeCharactersTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Fact]
        public void Lex_SingleStringWithUnicodeCharactersTokenLexer_HasCorrectStart()
        {
            var token = GetSingleStringWithUnicodeCharactersTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SingleStringWithUnicodeCharactersTokenLexer_HasCorrectValue()
        {
            var token = GetSingleStringWithUnicodeCharactersTokenLexer();
            token.Value.ShouldBe("unicode \u1234\u5678\u90AB\uCDEF");
        }

        [Fact]
        public void Lex_SpreadPunctuation_HasCorrectEnd()
        {
            var token = GetSpreadPunctuationTokenLexer();
            token.End.ShouldBe(3);
        }

        [Fact]
        public void Lex_SpreadPunctuation_HasCorrectKind()
        {
            var token = GetSpreadPunctuationTokenLexer();
            token.Kind.ShouldBe(TokenKind.SPREAD);
        }

        [Fact]
        public void Lex_SpreadPunctuation_HasCorrectStart()
        {
            var token = GetSpreadPunctuationTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_SpreadPunctuation_HasCorrectValue()
        {
            var token = GetSpreadPunctuationTokenLexer();
            token.Value.ShouldBe("...");
        }

        [Fact]
        public void Lex_WhiteSpaceStringToken_HasCorrectEnd()
        {
            var token = GetWhiteSpaceStringTokenLexer();
            token.End.ShouldBe(15);
        }

        [Fact]
        public void Lex_WhiteSpaceStringToken_HasCorrectStart()
        {
            var token = GetWhiteSpaceStringTokenLexer();
            token.Start.ShouldBe(0);
        }

        [Fact]
        public void Lex_WhiteSpaceStringToken_HasCorrectValue()
        {
            var token = GetWhiteSpaceStringTokenLexer();
            token.Value.ShouldBe(" white space ");
        }

        [Fact]
        public void Lex_WhiteSpaceStringToken_HasStringKind()
        {
            var token = GetWhiteSpaceStringTokenLexer();
            token.Kind.ShouldBe(TokenKind.STRING);
        }

        [Theory]
        [InlineData(1, "test", "test")]
        [InlineData(2, "te\\\"\"\"st", "te\"\"\"st")]
        [InlineData(3, "\ntest", "test")]
        [InlineData(4,"\r\ntest", "test")]
        [InlineData(5, " \ntest", "test")]
        [InlineData(6, "\t\ntest", "test")]
        [InlineData(7, "\n\ntest", "test")]
        [InlineData(8, "test\nline2", "test\nline2")]
        [InlineData(9,"test\rline2", "test\nline2")]
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
        public void Lex_BlockString_Tests(int number, string input, string expected)
        {
            number.ShouldBeGreaterThan(0);

            input = input.Replace("___", new string('_', 9000));
            expected = expected.Replace("___", new string('_', 9000));
            input = "\"\"\"" + input + "\"\"\"";
            var actual = input.Lex();
            actual.Kind.ShouldBe(TokenKind.STRING);
            actual.Value.ToString().ShouldBe(expected);
        }

        private static Token GetATPunctuationTokenLexer()
        {
            return "@".Lex();
        }

        private static Token GetBangPunctuationTokenLexer()
        {
            return "!".Lex();
        }

        private static Token GetColonPunctuationTokenLexer()
        {
            return ":".Lex();
        }

        private static Token GetDollarPunctuationTokenLexer()
        {
            return "$".Lex();
        }

        private static Token GetEqualsPunctuationTokenLexer()
        {
            return "=".Lex();
        }

        private static Token GetEscapedStringTokenLexer()
        {
            return "\"escaped \\n\\r\\b\\t\\f\"".Lex();
        }

        private static Token GetLeftBracePunctuationTokenLexer()
        {
            return "{".Lex();
        }

        private static Token GetLeftBracketPunctuationTokenLexer()
        {
            return "[".Lex();
        }

        private static Token GetLeftParenthesisPunctuationTokenLexer()
        {
            return "(".Lex();
        }

        private static Token GetMultipleDecimalsIntTokenLexer()
        {
            return "123".Lex();
        }

        private static Token GetPipePunctuationTokenLexer()
        {
            return "|".Lex();
        }

        private static Token GetQuoteStringTokenLexer()
        {
            return "\"quote \\\"\"".Lex();
        }

        private static Token GetRightBracePunctuationTokenLexer()
        {
            return "}".Lex();
        }

        private static Token GetRightBracketPunctuationTokenLexer()
        {
            return "]".Lex();
        }

        private static Token GetRightParenthesisPunctuationTokenLexer()
        {
            return ")".Lex();
        }

        private static Token GetSimpleStringTokenLexer()
        {
            return "\"str\"".Lex();
        }

        private static Token GetSingleDecimalIntTokenLexer()
        {
            return "0".Lex();
        }

        private static Token GetSingleFloatTokenLexer()
        {
            return "4.123".Lex();
        }

        private static Token GetSingleFloatWithZeroTokenLexer()
        {
            return "12.10".Lex();
        }

        private static Token GetSingleFloatWithExplicitlyPositiveExponentTokenLexer()
        {
            return "123e+4".Lex();
        }

        private static Token GetSingleFloatWithExponentCapitalLetterTokenLexer()
        {
            return "123E4".Lex();
        }

        private static Token GetSingleFloatWithExponentTokenLexer()
        {
            return "123e4".Lex();
        }

        private static Token GetSingleFloatWithNegativeExponentTokenLexer()
        {
            return "123e-4".Lex();
        }

        private static Token GetSingleNameSurroundedByCommasTokenLexer()
        {
            return ",,,foo,,,".Lex();
        }

        private static Token GetSingleNameTokenLexerSurroundedWithWhitespaces()
        {
            return $"\n        foo\n\n    ".Lex();
        }

        private static Token GetSingleNameTokenLexerWithComments()
        {
            return $"\n#comment\nfoo#comment".Lex();
        }

        private static Token GetSingleNameWithBOMHeaderTokenLexer()
        {
            return "\uFEFF foo\\".Lex();
        }

        private static Token GetSingleNegativeFloatTokenLexer()
        {
            return "-0.123".Lex();
        }

        private static Token GetSingleNegativeFloatWithExponentTokenLexer()
        {
            return "-123e4".Lex();
        }

        private static Token GetSingleNegativeIntTokenLexer()
        {
            return "-3".Lex();
        }

        private static Token GetSingleStringWithSlashesTokenLexer()
        {
            return "\"slashes \\\\ \\/\"".Lex();
        }

        private static Token GetSingleStringWithUnicodeCharactersTokenLexer()
        {
            return "\"unicode \\u1234\\u5678\\u90AB\\uCDEF\"".Lex();
        }

        private static Token GetSpreadPunctuationTokenLexer()
        {
            return "...".Lex();
        }

        private static Token GetWhiteSpaceStringTokenLexer()
        {
            return "\" white space \"".Lex();
        }
    }
}
