using System;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLIntValueTests
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
        var value = new GraphQLIntValue((int)1234567);
        value.Value.Length.ShouldBe(7);
        value.ClrValue.ShouldBe(1234567);
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

    [Fact]
    public void Int_SbyteValue()
    {
        var value = new GraphQLIntValue((sbyte)-10);
        value.Value.Length.ShouldBe(3);
        value.ClrValue.ShouldBe(-10);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void Int_ShortValue()
    {
        var value = new GraphQLIntValue((short)-300);
        value.Value.Length.ShouldBe(4);
        value.ClrValue.ShouldBe(-300);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void Int_UshortValue()
    {
        var value = new GraphQLIntValue((ushort)60000);
        value.Value.Length.ShouldBe(5);
        value.ClrValue.ShouldBe(60000);
        value.ClrValue.ShouldBeOfType<int>();
    }


    [Fact]
    public void Int_UintValue_ToInt()
    {
        var value = new GraphQLIntValue((uint)17);
        value.Value.Length.ShouldBe(2);
        value.ClrValue.ShouldBe(17);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void Int_UintValue_ToLong()
    {
        var value = new GraphQLIntValue((uint)2247483647);
        value.Value.Length.ShouldBe(10);
        value.ClrValue.ShouldBe(2247483647);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void Int_LongValue()
    {
        var value = new GraphQLIntValue((long)-60001);
        value.Value.Length.ShouldBe(6);
        value.ClrValue.ShouldBe(-60001);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void Int_UlongValue_ToInt()
    {
        var value = new GraphQLIntValue((ulong)123);
        value.Value.Length.ShouldBe(3);
        value.ClrValue.ShouldBe(123);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void Int_UlongValue_ToLong()
    {
        var value = new GraphQLIntValue((ulong)2247483647);
        value.Value.Length.ShouldBe(10);
        value.ClrValue.ShouldBe(2247483647);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void Int_UlongValue_ToDecimal()
    {
        var value = new GraphQLIntValue((ulong)9223372036854775808);
        value.Value.Length.ShouldBe(19);
        value.ClrValue.ShouldBe(9223372036854775808);
        value.ClrValue.ShouldBeOfType<decimal>();
    }
}
