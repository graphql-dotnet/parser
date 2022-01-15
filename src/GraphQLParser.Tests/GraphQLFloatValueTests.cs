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
        Should.Throw<InvalidOperationException>(() => value.ClrValue);
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
        value.ClrValue.ShouldBe(1.1f);
        value.ClrValue.ShouldBeOfType<double>();

        value.Value = "1.1";
        value.ClrValue.ShouldBe(1.1);
        value.ClrValue.ShouldBeOfType<double>();
    }

    [Fact]
    public void DoubleValue()
    {
        var value = new GraphQLFloatValue(1.1);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("1.1");
        value.ClrValue.ShouldBe(1.1);
        value.ClrValue.ShouldBeOfType<double>();

        value.Value = "1.1";
        value.ClrValue.ShouldBe(1.1);
        value.ClrValue.ShouldBeOfType<double>();
    }

    [Fact]
    public void DecimalValue()
    {
        var value = new GraphQLFloatValue(15.10m);
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("15.10");
        value.ClrValue.ShouldBe(15.10m);
        value.ClrValue.ShouldBeOfType<decimal>();

        value.Value = "15.10";
        value.ClrValue.ShouldBe(15.10m);
        value.ClrValue.ShouldBeOfType<decimal>();
    }
}
