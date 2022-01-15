using System;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class ClrValueTests
{
    [Fact]
    public void Int_NoValue()
    {
        var value = new GraphQLIntValue();
        value.Value.Length.ShouldBe(0);
        Should.Throw<InvalidOperationException>(() => value.ClrValue);
    }

    [Fact]
    public void Int_IntValue()
    {
        var value = new GraphQLIntValue((int)42);
        value.Value.Length.ShouldBe(2);
        value.ClrValue.ShouldBe(42);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void Int_ByteValue()
    {
        var value = new GraphQLIntValue((byte)42);
        value.Value.Length.ShouldBe(2);
        value.ClrValue.ShouldBe(42);
        value.ClrValue.ShouldBeOfType<int>();
    }
}
