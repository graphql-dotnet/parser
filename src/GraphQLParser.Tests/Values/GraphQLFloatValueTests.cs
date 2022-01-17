using System;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLFloatValueTests
{
    [Fact]
    public void NoValue()
    {
        var value = new GraphQLFloatValue();
        value.Value.Length.ShouldBe(0);
    }

    [Fact]
    public void NanValue()
    {
        var ex = Should.Throw<ArgumentOutOfRangeException>(() => new GraphQLFloatValue(double.NaN));
        ex.Message.ShouldStartWith("Value cannot be NaN.");
    }

    [Fact]
    public void FloatValue()
    {
        var value = new GraphQLFloatValue(1.1f);
        value.Value.Length.ShouldBe(16);
        value.Value.ShouldBe("1.10000002384186");
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
