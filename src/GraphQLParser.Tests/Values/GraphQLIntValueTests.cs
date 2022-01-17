using System.Numerics;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLIntValueTests
{
    [Fact]
    public void Int()
    {
        var value = new GraphQLIntValue(1234567);
        value.Value.Length.ShouldBe(7);
        value.Value.ShouldBe("1234567");
    }

    [Fact]
    public void Byte()
    {
        var value = new GraphQLIntValue((byte)42);
        value.Value.Length.ShouldBe(2);
        value.Value.ShouldBe("42");
    }

    [Fact]
    public void Sbyte()
    {
        var value = new GraphQLIntValue((sbyte)-10);
        value.Value.Length.ShouldBe(3);
        value.Value.ShouldBe("-10");
    }

    [Fact]
    public void Short()
    {
        var value = new GraphQLIntValue((short)-300);
        value.Value.Length.ShouldBe(4);
        value.Value.ShouldBe("-300");
    }

    [Fact]
    public void Ushort()
    {
        var value = new GraphQLIntValue((ushort)60000);
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("60000");
    }

    [Fact]
    public void Uint()
    {
        var value = new GraphQLIntValue(2247483647U);
        value.Value.Length.ShouldBe(10);
        value.Value.ShouldBe("2247483647");
    }

    [Fact]
    public void Long()
    {
        var value = new GraphQLIntValue(-60001L);
        value.Value.Length.ShouldBe(6);
        value.Value.ShouldBe("-60001");
    }

    [Fact]
    public void Ulong()
    {
        var value = new GraphQLIntValue(9223372036854775808UL);
        value.Value.Length.ShouldBe(19);
        value.Value.ShouldBe("9223372036854775808");
    }

    [Fact]
    public void BigInt()
    {
        var value = new GraphQLIntValue(BigInteger.Parse("7922816251426433759354395033579228162514264337593543950335"));
        value.Value.Length.ShouldBe(58);
        value.Value.ShouldBe("7922816251426433759354395033579228162514264337593543950335");
    }
}
