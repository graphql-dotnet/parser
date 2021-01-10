using GraphQLParser.Exceptions;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Validation
{
    public class ParserValidationTests
    {
        [Fact]
        public void Parse_FragmentInvalidOnName_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "fragment on on on { on }".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:10) Unexpected Name \"on\"\n" +
                "1: fragment on on on { on }\n" +
                "            ^\n");
            exception.Description.ShouldBe("Unexpected Name " + "\"on\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(10);
        }

        [Fact]
        public void Parse_InvalidDefaultValue_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "query Foo($x: Complex = { a: { b: [ $var ] } }) { field }".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:37) Unexpected $\n" +
                "1: query Foo($x: Complex = { a: { b: [ $var ] } }) { field }\n" +
                "                                       ^\n");
            exception.Description.ShouldBe("Unexpected $");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(37);
        }

        [Fact]
        public void Parse_InvalidFragmentNameInSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "{ ...on }".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:9) Expected Name, found }\n" +
                "1: { ...on }\n" +
                "           ^\n");
            exception.Description.ShouldBe("Expected Name, found }");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(9);
        }

        [Fact]
        public void Parse_LonelySpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "...".Parse());

            exception.Message.ShouldBe(
                    "Syntax Error GraphQL (1:1) Unexpected ...\n" +
                    "1: ...\n" +
                    "   ^\n");
            exception.Description.ShouldBe("Unexpected ...");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Parse_MissingEndingBrace_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "{".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:2) Expected Name, found EOF\n" +
                "1: {\n" +
                "    ^\n");
            exception.Description.ShouldBe("Expected Name, found EOF");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Parse_MissingFieldNameWhenAliasing_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "{ field: {} }".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:10) Expected Name, found {\n" +
                "1: { field: {} }\n" +
                "            ^\n");
            exception.Description.ShouldBe("Expected Name, found {");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(10);
        }

        [Fact]
        public void Parse_MissingFragmentType_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "{ ...MissingOn }\nfragment MissingOn Type".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (2:20) Expected \"on\", found Name \"Type\"\n" +
                "1: { ...MissingOn }\n" +
                "2: fragment MissingOn Type\n" +
                "                      ^\n");
            exception.Description.ShouldBe("Expected \"on\", found Name \"Type\"");
            exception.Line.ShouldBe(2);
            exception.Column.ShouldBe(20);
        }

        [Fact]
        public void Parse_UnknownOperation_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(() => "notanoperation Foo { field }".Parse());

            exception.Message.ShouldBe(
                "Syntax Error GraphQL (1:1) Unexpected Name " + "\"notanoperation\"\n" +
                "1: notanoperation Foo { field }\n" +
                "   ^\n");
            exception.Description.ShouldBe("Unexpected Name \"notanoperation\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }
    }
}
