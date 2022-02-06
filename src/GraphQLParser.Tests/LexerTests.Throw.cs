using GraphQLParser.Exceptions;

namespace GraphQLParser.Tests;

public class LexerTestsThrow
{
    [Fact]
    public void NameWithHyphen()
    {
        var token = "foo-name".Lex();
        token.Kind.ShouldBe(TokenKind.NAME);
        token.Start.ShouldBe(0);
        token.End.ShouldBe(3);
        token.Value.ShouldBe("foo");

        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "foo-name".Lex(token.End));
        ex.Description.ShouldBe("Invalid number, expected digit but got: \"n\"");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(5);
    }

    [Fact]
    public void Spread_With_Two_Dots()
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "..".Lex());
        ex.Description.ShouldBe("Unexpected character \".\"");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(1);
    }

    [Fact]
    public void Spread_With_Single_Dot()
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => ".".Lex());
        ex.Description.ShouldBe("Unexpected character \".\"");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(1);
    }
}
