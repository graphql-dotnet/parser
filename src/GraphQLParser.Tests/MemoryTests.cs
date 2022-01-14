using System;
using System.Runtime.InteropServices;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class MemoryTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData(" a ", false)]
    [InlineData("\t\t\t\t\t", true)]
    [InlineData("\t   a", false)]
    public void IsEmptyOrWhiteSpace_Should_Work(string text, bool expected)
    {
        ROM.IsEmptyOrWhiteSpace(text).ShouldBe(expected);
    }

    [Fact]
    public void SizeOf_ROM_Should_Be_The_Same_As_ReadOnlyMemory()
    {
        if (OperatingSystem.IsWindows()) // TODO: weird errors on Linux
        {
            Marshal.SizeOf(default(ROM)).ShouldBe(16);
            Marshal.SizeOf(default(ReadOnlyMemory<char>)).ShouldBe(16);
        }
    }

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
        rom.Slice(6).GetHashCode().ShouldNotBe(0);

        default(ROM).GetHashCode().ShouldBe(0);

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

    [Theory]
    [InlineData("string")]
    [InlineData("")]
    //[InlineData(null)] // see Null_Roundtrip
    public void Casted_To_String_From_Original_String_Should_Be_Equal(string str)
    {
        ROM rom = str;

        // so no heap allocation when ROM is actually backed by whole string object
        ReferenceEquals(rom.ToString(), str).ShouldBeTrue();
        ReferenceEquals((string)rom, str).ShouldBeTrue();
    }

    [Fact(Skip = "Known issue with ROM - it cannot represent null values")]
    public void Null_Roundtrip()
    {
        string s = null;
        ROM r = s;

        var s1 = r.ToString(); // ""
        var s2 = r.ToString(); // ""

        ReferenceEquals(s1, s2).ShouldBeTrue();

        s1.ShouldBe(s); // failed!
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
        var name = new GraphQLName("abc");
        FuncROM(name).ShouldBe(name);
    }

    [Fact]
    public void GraphQLName_Explicit_Cast()
    {
        var name = new GraphQLName("abc");
        FuncString((string)name).ShouldBe("abc");

        GraphQLName nameNull = null;
        ((string)nameNull).ShouldBeNull();
    }

    private ROM FuncROM(ROM r) => r;

    private string FuncString(string s) => s;
}
