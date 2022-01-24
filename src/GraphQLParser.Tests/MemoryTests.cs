using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class MemoryTests
{
    [Fact]
    public void GetHashCode_ShouldBe_Equal()
    {
        ROM r = "abcabc";
        var r1 = r.Slice(0, 3);
        var r2 = r.Slice(3, 3);

        r1.ToString().ShouldBe("abc");
        r2.ToString().ShouldBe("abc");

        var h1 = r1.GetHashCode();
        var h2 = r2.GetHashCode();
        h1.ShouldBe(h2);
    }

    [Fact]
    public void HashSet()
    {
        ROM r = "abc";
        HashSet<ROM> hashSet = new();
        hashSet.Add(r);
        hashSet.Contains(r).ShouldBeTrue();

        ROM r2 = "abcdef";
        r2 = r2.Slice(0, 3);

        hashSet.Contains(r2).ShouldBeTrue();
    }

    [Fact]
    public void Dictionary()
    {
        ROM r = "abc";
        Dictionary<ROM, int> dictionary = new();
        dictionary[r] = 42;
        dictionary.ContainsKey(r).ShouldBeTrue();

        ROM r2 = "abcdef";
        r2 = r2.Slice(0, 3);

        dictionary.ContainsKey(r2).ShouldBeTrue();
        dictionary["abc"].ShouldBe(42);
    }

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

#if NET6_0_OR_GREATER
    [Fact]
    public void SizeOf_ROM_Should_Be_The_Same_As_ReadOnlyMemory()
    {
        if (OperatingSystem.IsWindows()) // TODO: weird errors on Linux
        {
            System.Runtime.InteropServices.Marshal.SizeOf(default(ROM)).ShouldBe(16);
            System.Runtime.InteropServices.Marshal.SizeOf(default(ReadOnlyMemory<char>)).ShouldBe(16);
        }
    }
#endif

    [Fact]
    public void Implicit_Operator_From_Null_String()
    {
        string s = null;
        ROM r = s;
        r.Length.ShouldBe(0);
    }

    [Fact]
    public void Operators()
    {
        var str = "string";
        ROM rom = str;

        var romImplicitMem = str.AsMemory(); // ReadOnlyMemory
        (rom == romImplicitMem).ShouldBeTrue();

        romImplicitMem = new Memory<char>(new char[] { 's', 't', 'r', 'i', 'n', 'g' }); // Memory
        (rom == romImplicitMem).ShouldBeTrue();

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
#if NET6_0_OR_GREATER
        rom.Slice(6).GetHashCode().ShouldNotBe(0);
        default(ROM).GetHashCode().ShouldNotBe(0);
#else
        rom.Slice(6).GetHashCode().ShouldBe(0);
        default(ROM).GetHashCode().ShouldBe(0);
#endif
        (rom2 == str).ShouldBeFalse();
        (str == rom2).ShouldBeFalse();
        (rom2 != str).ShouldBeTrue();
        (str != rom2).ShouldBeTrue();

        ROM.Empty.Length.ShouldBe(0);
        ROM.Empty.ShouldBe(string.Empty);
    }

    [Fact]
    public void Default_ROM_ShouldBe_Always_StringEmpty_Instance()
    {
        ReferenceEquals((string)default(ROM), (string)default(ROM)).ShouldBeTrue();
        ReferenceEquals((string)default(ROM), string.Empty).ShouldBeTrue();
        ReferenceEquals(default(ROM).ToString(), default(ROM).ToString()).ShouldBeTrue();
        ReferenceEquals(default(ROM).ToString(), string.Empty).ShouldBeTrue();
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
}
