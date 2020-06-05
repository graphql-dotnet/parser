using GraphQLParser.Exceptions;
using GraphQLParser;
using System;
using Xunit;

namespace GraphQLParser.Tests.Validation
{
    public class ParserValidationTests
    {
        [Fact]
        public void Parse_FragmentInvalidOnName_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("fragment on on on { on }")));

            Assert.Equal(
                "Syntax Error GraphQL (1:10) Unexpected Name \"on\"\n" +
                "1: fragment on on on { on }\n" +
                "            ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_InvalidDefaultValue_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("query Foo($x: Complex = { a: { b: [ $var ] } }) { field }")));

            Assert.Equal(
                "Syntax Error GraphQL (1:37) Unexpected $\n" +
                "1: query Foo($x: Complex = { a: { b: [ $var ] } }) { field }\n" +
                "                                       ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_InvalidFragmentNameInSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{ ...on }")));

            Assert.Equal(
                "Syntax Error GraphQL (1:9) Expected Name, found }\n" +
                "1: { ...on }\n" +
                "           ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_LonelySpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("...")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected ...\n" +
                "1: ...\n" +
                "   ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_MissingEndingBrace_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{")));

            Assert.Equal(
                "Syntax Error GraphQL (1:2) Expected Name, found EOF\n" +
                "1: {\n" +
                "    ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_MissingFieldNameWhenAliasing_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{ field: {} }")));

            Assert.Equal(
                "Syntax Error GraphQL (1:10) Expected Name, found {\n" +
                "1: { field: {} }\n" +
                "            ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_MissingFragmentType_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{ ...MissingOn }\nfragment MissingOn Type")));

            Assert.Equal(
                "Syntax Error GraphQL (2:20) Expected \"on\", found Name " + "\"Type\"\n" +
                "1: { ...MissingOn }\n" +
                "2: fragment MissingOn Type\n" +
                "                      ^\n",
                exception.Message);
        }

        [Fact]
        public void Parse_UnknownOperation_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Assert.Throws<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("notanoperation Foo { field }")));

            Assert.Equal(
                "Syntax Error GraphQL (1:1) Unexpected Name " + "\"notanoperation\"\n" +
                "1: notanoperation Foo { field }\n" +
                "   ^\n",
                exception.Message);
        }
    }
}
