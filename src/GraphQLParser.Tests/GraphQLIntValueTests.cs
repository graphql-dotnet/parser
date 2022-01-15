using System;
using System.Numerics;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLIntValueTests
{
    [Fact]
    public void NoValue()
    {
        var value = new GraphQLIntValue();
        value.Value.Length.ShouldBe(0);
        Should.Throw<InvalidOperationException>(() => value.ClrValue);
    }

    [Fact]
    public void IntValue()
    {
        var value = new GraphQLIntValue((int)1234567);
        value.Value.Length.ShouldBe(7);
        value.Value.ShouldBe("1234567");
        value.ClrValue.ShouldBe(1234567);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void ByteValue()
    {
        var value = new GraphQLIntValue((byte)42);
        value.Value.Length.ShouldBe(2);
        value.Value.ShouldBe("42");
        value.ClrValue.ShouldBe(42);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void SbyteValue()
    {
        var value = new GraphQLIntValue((sbyte)-10);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("-10");
        value.ClrValue.ShouldBe(-10);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void ShortValue()
    {
        var value = new GraphQLIntValue((short)-300);
        value.Value.Length.ShouldBe(4);
        value.Value.ShouldBe("-300");
        value.ClrValue.ShouldBe(-300);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void UshortValue()
    {
        var value = new GraphQLIntValue((ushort)60000);
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("60000");
        value.ClrValue.ShouldBe(60000);
        value.ClrValue.ShouldBeOfType<int>();
    }


    [Fact]
    public void UintValue_ToInt()
    {
        var value = new GraphQLIntValue((uint)17);
        value.Value.Length.ShouldBe(2);
        value.Value.ShouldBe("17");
        value.ClrValue.ShouldBe(17);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void UintValue_ToLong()
    {
        var value = new GraphQLIntValue((uint)2247483647);
        value.Value.Length.ShouldBe(10);
        value.Value.ShouldBe("2247483647");
        value.ClrValue.ShouldBe(2247483647);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void LongValue()
    {
        var value = new GraphQLIntValue((long)-60001);
        value.Value.Length.ShouldBe(6);
        value.Value.ShouldBe("-60001");
        value.ClrValue.ShouldBe(-60001);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void UlongValue_ToInt()
    {
        var value = new GraphQLIntValue((ulong)123);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("123");
        value.ClrValue.ShouldBe(123);
        value.ClrValue.ShouldBeOfType<int>();
    }

    [Fact]
    public void UlongValue_ToLong()
    {
        var value = new GraphQLIntValue((ulong)2247483647);
        value.Value.Length.ShouldBe(10);
        value.Value.ShouldBe("2247483647");
        value.ClrValue.ShouldBe(2247483647);
        value.ClrValue.ShouldBeOfType<long>();
    }

    [Fact]
    public void UlongValue_ToDecimal()
    {
        var value = new GraphQLIntValue((ulong)9223372036854775808);
        value.Value.Length.ShouldBe(19);
        value.Value.ShouldBe("9223372036854775808");
        value.ClrValue.ShouldBe(9223372036854775808);
        value.ClrValue.ShouldBeOfType<decimal>();
    }

    [Fact]
    public void BigIntegerValue()
    {
        var value = new GraphQLIntValue(new BigInteger(1234554321));
        value.Value.Length.ShouldBe(10);
        value.Value.ShouldBe("1234554321");
        value.ClrValue.ShouldBe(new BigInteger(1234554321));
        value.ClrValue.ShouldBeOfType<BigInteger>();
    }

    [Fact]
    public void DecimalValue()
    {
        var value = new GraphQLIntValue((decimal)1234554321);
        value.Value.Length.ShouldBe(10);
        value.Value.ShouldBe("1234554321");
        value.ClrValue.ShouldBe(1234554321);
        value.ClrValue.ShouldBeOfType<decimal>();
    }
}
