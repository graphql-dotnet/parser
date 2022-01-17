using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLBooleanValueTests
{
    [Fact]
    public void EmptyValue()
    {
        var value = new GraphQLBooleanValue();
        value.Value.Length.ShouldBe(0);
    }

    [Fact]
    public void BoolValue_False()
    {
        var value = new GraphQLBooleanValue(false);
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("false");
    }

    [Fact]
    public void BoolValue_True()
    {
        var value = new GraphQLBooleanValue(true);
        value.Value.Length.ShouldBe(4);
        value.Value.ShouldBe("true");
    }
}
