using System.Text;
using GraphQLParser.Exceptions;

namespace GraphQLParser.Tests;

public class ParserTestsDepth
{
    [Fact]
    public void Should_Throw_With_Deep_Query()
    {
        var count = 63;
        var sb = new StringBuilder(count * 3);
        for (int i = 0; i < count; i++)
            sb.Append("{a");
        sb.Append(new string('}', count));
        var query = sb.ToString();
        Should.Throw<GraphQLMaxDepthExceededException>(() => query.Parse());
    }

    [Fact]
    public void Should_Throw_With_Deep_Literal()
    {
        var count = 61;
        var sb = new StringBuilder(count * 4 + 10);
        sb.Append("{a(b:");
        for (int i = 0; i < count; i++)
            sb.Append("{c:");
        sb.Append("{}");
        sb.Append(new string('}', count));
        sb.Append(")}");
        var query = sb.ToString();
        Should.Throw<GraphQLMaxDepthExceededException>(() => query.Parse());
    }

    [Fact]
    public void Should_Parse_With_Almost_Deep_Query()
    {
        var count = 62;
        var sb = new StringBuilder(count * 3);
        for (int i = 0; i < count; i++)
            sb.Append("{a");
        sb.Append(new string('}', count));
        var query = sb.ToString();
        _ = query.Parse();
    }

    [Fact]
    public void Should_Parse_With_Almost_Deep_Literal()
    {
        var count = 60;
        var sb = new StringBuilder(count * 4 + 10);
        sb.Append("{a(b:");
        for (int i = 0; i < count; i++)
            sb.Append("{c:");
        sb.Append("{}");
        sb.Append(new string('}', count));
        sb.Append(")}");
        var query = sb.ToString();
        _ = query.Parse();
    }

    [Fact]
    public void Should_Parse_With_Shallow_Long_Query()
    {
        var count = 200;
        var sb = new StringBuilder(count * 5);
        sb.Append('{');
        for (int i = 0; i < count; i++)
            sb.Append(" a" + i);
        sb.Append('}');
        var query = sb.ToString();
        _ = query.Parse();
    }

    [Fact]
    public void Should_Throw_With_MaxDepth_0_On_SimpleQuery()
    {
        var query = "{a}";
        Should.Throw<GraphQLMaxDepthExceededException>(() => Parser.Parse(query, new ParserOptions { MaxDepth = 0 }));
    }

    [Fact]
    public void Should_Throw_With_MaxDepth_2_On_TypeDefinition()
    {
        var query = "scalar Test";
        Should.Throw<GraphQLMaxDepthExceededException>(() => Parser.Parse(query, new ParserOptions { MaxDepth = 2 }));
    }

    [Fact]
    public void Should_Throw_With_MaxDepth_4_On_SimpleQuery()
    {
        var query = "{a}";
        Should.Throw<GraphQLMaxDepthExceededException>(() => Parser.Parse(query, new ParserOptions { MaxDepth = 4 }));
    }

    [Fact]
    public void Should_Parse_With_MaxDepth_3_On_TypeDefinition()
    {
        var query = "scalar Test";
        _ = Parser.Parse(query, new ParserOptions { MaxDepth = 3 });
    }

    [Fact]
    public void Should_Parse_With_MaxDepth_5_On_SimpleQuery()
    {
        var query = "{a}";
        _ = Parser.Parse(query, new ParserOptions { MaxDepth = 5 });
    }
}
