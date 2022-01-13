using System;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class MemoryTests
{
    [Fact]
    public void Operators()
    {
        var str = "string";
        ROM rom = str;
        rom.IsEmpty.ShouldBeFalse();
        rom.Length.ShouldBe(6);
        rom.GetHashCode().ShouldNotBe(0);

        (rom == new ROM(rom)).ShouldBeTrue();
        (rom != new ROM(rom)).ShouldBeFalse();

        (rom == str).ShouldBeTrue();
        (str == rom).ShouldBeTrue();
        (rom != str).ShouldBeFalse();
        (str != rom).ShouldBeFalse();

        var rom2 = rom.Slice(1);
        rom2.IsEmpty.ShouldBeFalse();
        rom2.Length.ShouldBe(5);
        rom2.GetHashCode().ShouldNotBe(0);
        rom2.GetHashCode().ShouldNotBe(rom.GetHashCode());
        rom.Slice(6).GetHashCode().ShouldBe(0);

        (rom2 == str).ShouldBeFalse();
        (str == rom2).ShouldBeFalse();
        (rom2 != str).ShouldBeTrue();
        (str != rom2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Should_Work()
    {
        var str = "string";
        ROM rom = str;
        rom.Equals(rom).ShouldBeTrue();
        rom.Equals((object)rom).ShouldBeTrue();
        rom.Equals((object)str).ShouldBeFalse();
        rom.Equals("strin").ShouldBeFalse();
    }

    [Fact]
    public void ImplicitCast()
    {
        ROM rom1 = "abc";
        ReadOnlyMemory<char> mem = rom1;

        mem.Span[0].ShouldBe('a');
        mem.Span[1].ShouldBe('b');
        mem.Span[2].ShouldBe('c');

        ROM rom2 = new char[] { 'd', 'e' };
        rom2.Span[0].ShouldBe('d');
        rom2.Span[1].ShouldBe('e');
    }

    [Fact]
    public void GraphQLName_Implicit_Cast()
    {
        var name = new GraphQLName { Value = "abc" };
        FuncROM(name).ShouldBe(name);
    }

    private ROM FuncROM(ROM r) => r;
}
