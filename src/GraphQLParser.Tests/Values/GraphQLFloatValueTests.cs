using System;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLFloatValueTests
{
    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void BadDoubleValue(double value)
    {
        var ex = Should.Throw<ArgumentOutOfRangeException>(() => new GraphQLFloatValue(value));
        ex.Message.ShouldStartWith("Value cannot be NaN or Infinity.");
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void BadFloatValue(float value)
    {
        var ex = Should.Throw<ArgumentOutOfRangeException>(() => new GraphQLFloatValue(value));
        ex.Message.ShouldStartWith("Value cannot be NaN or Infinity.");
    }

    [Fact]
    public void FloatValue()
    {
        var value = new GraphQLFloatValue(1.1f);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("1.1");
    }

    [Fact]
    public void DoubleValue()
    {
        var value = new GraphQLFloatValue(1.1);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("1.1");
    }

    [Fact]
    public void DecimalValue()
    {
        var value = new GraphQLFloatValue(15.10m);
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("15.10");
    }
}
