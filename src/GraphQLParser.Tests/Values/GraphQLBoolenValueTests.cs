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
        value.ClrValue.ShouldBe(false);
    }

    [Fact]
    public void BoolValue_False()
    {
        var value = new GraphQLBooleanValue(false);
        value.Value.Length.ShouldBe(5);
        value.ClrValue.ShouldBe(false);
        ReferenceEquals(value.ClrValue, value.ClrValue).ShouldBeTrue();
        value.Reset(); // does nothing
        ReferenceEquals(value.ClrValue, value.ClrValue).ShouldBeTrue();
    }

    [Fact]
    public void BoolValue_True()
    {
        var value = new GraphQLBooleanValue(true);
        value.Value.Length.ShouldBe(4);
        value.ClrValue.ShouldBe(true);
        ReferenceEquals(value.ClrValue, value.ClrValue).ShouldBeTrue();
        value.Reset(); // does nothing
        ReferenceEquals(value.ClrValue, value.ClrValue).ShouldBeTrue();
    }
}
