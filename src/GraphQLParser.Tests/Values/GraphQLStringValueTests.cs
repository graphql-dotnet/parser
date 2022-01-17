using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLStringValueTests
{
    [Fact]
    public void EmptyValue()
    {
        var value = new GraphQLStringValue("");
        value.Value.Length.ShouldBe(0);
        value.Value.ShouldBe("");
    }

    [Fact]
    public void NullValue()
    {
        var value = new GraphQLStringValue((string)null);
        value.Value.Length.ShouldBe(0);
        value.Value.ShouldBe("");
    }

    [Fact]
    public void StringValue()
    {
        const string s = "abc";
        var value = new GraphQLStringValue(s);
        value.Value.Length.ShouldBe(3);
    }
}
