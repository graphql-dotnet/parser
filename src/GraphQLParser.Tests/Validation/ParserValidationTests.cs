using GraphQLParser.Exceptions;
using GraphQLParser;
using System;
using Xunit;
using Shouldly;

namespace GraphQLParser.Tests.Validation
{
    public class ParserValidationTests
    {
        [Fact]
        public void Parse_FragmentInvalidOnName_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("fragment on on on { on }")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:10) Unexpected Name " + "\"on\"" + @"
1: fragment on on on { on }
            ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected Name " + "\"on\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(10);
        }

        [Fact]
        public void Parse_InvalidDefaultValue_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("query Foo($x: Complex = { a: { b: [ $var ] } }) { field }")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:37) Unexpected $
1: query Foo($x: Complex = { a: { b: [ $var ] } }) { field }
                                       ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected $");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(37);
        }

        [Fact]
        public void Parse_InvalidFragmentNameInSpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{ ...on }")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:9) Expected Name, found }
1: { ...on }
           ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Expected Name, found }");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(9);
        }

        [Fact]
        public void Parse_LonelySpread_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("...")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:1) Unexpected ...
1: ...
   ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected ...");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }

        [Fact]
        public void Parse_MissingEndingBrace_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:2) Expected Name, found EOF
1: {
    ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Expected Name, found EOF");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(2);
        }

        [Fact]
        public void Parse_MissingFieldNameWhenAliasing_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("{ field: {} }")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:10) Expected Name, found {
1: { field: {} }
            ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Expected Name, found {");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(10);
        }

        [Fact]
        public void Parse_MissingFragmentType_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source(@"{ ...MissingOn }
fragment MissingOn Type")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (2:20) Expected " + "\"on\"" + @", found Name " + "\"Type\"" + @"
1: { ...MissingOn }
2: fragment MissingOn Type
                      ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Expected \"on\", found Name \"Type\"");
            exception.Line.ShouldBe(2);
            exception.Column.ShouldBe(20);
        }

        [Fact]
        public void Parse_UnknownOperation_ThrowsExceptionWithCorrectMessage()
        {
            var exception = Should.Throw<GraphQLSyntaxErrorException>(
                () => new Parser(new Lexer()).Parse(new Source("notanoperation Foo { field }")));

            exception.Message.ShouldBe(@"Syntax Error GraphQL (1:1) Unexpected Name " + "\"notanoperation\"" + @"
1: notanoperation Foo { field }
   ^
".Replace(Environment.NewLine, "\n"));
            exception.Description.ShouldBe("Unexpected Name \"notanoperation\"");
            exception.Line.ShouldBe(1);
            exception.Column.ShouldBe(1);
        }
    }
}
