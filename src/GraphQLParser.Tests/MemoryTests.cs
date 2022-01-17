using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class MemoryTests
{
    [Fact]
    public void GraphQLName_Cache_StringValue()
    {
        var name = new GraphQLName();
        name.StringValue.ShouldBe(string.Empty);
        name.ToString().ShouldBe(string.Empty);

        name.Value = "abc";
        name.StringValue.ShouldBe("abc");
        name.ToString().ShouldBe("abc");

        name.Value = "def";
        name.StringValue.ShouldBe("def");
        name.ToString().ShouldBe("def");
    }

    [Fact(Skip = "ReadOnlyMemory<T>.GetHashCode demonstration")]
    public void GetHashCode_Issue()
    {
        // ReadOnlyMemory<T> implementation
        // public override int GetHashCode()
        // {
        //     if (_object == null)
        //     {
        //         return 0;
        //     }
        //
        //     return CombineHashCodes(_object.GetHashCode(), _index.GetHashCode(), _length.GetHashCode());
        // }

        string s = "abcabc";
        var m = s.AsMemory();
        var s1 = m.Slice(0, 3);
        var s2 = m.Slice(3, 3);

        s1.ToString().ShouldBe("abc");
        s2.ToString().ShouldBe("abc");

        var h1 = s1.GetHashCode();
        var h2 = s2.GetHashCode();
        h1.ShouldBe(h2); // failed
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
        rom.Slice(6).GetHashCode().ShouldBe(0);

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
    public void GraphQLName_Equality()
    {
        ((GraphQLName)null == (GraphQLName)null).ShouldBeTrue();
        ((GraphQLName)null != (GraphQLName)null).ShouldBeFalse();

        var name = new GraphQLName("abc");
        (name == null).ShouldBeFalse();
        (null == name).ShouldBeFalse();
        (name != null).ShouldBeTrue();
        (null != name).ShouldBeTrue();

        name = new GraphQLName();
        (name == null).ShouldBeTrue();
        (null == name).ShouldBeTrue();
        (name != null).ShouldBeFalse();
        (null != name).ShouldBeFalse();

        name = new GraphQLName("");
        (name == null).ShouldBeTrue();
        (null == name).ShouldBeTrue();
        (name != null).ShouldBeFalse();
        (null != name).ShouldBeFalse();
    }

    [Fact]
    public void GraphQLName_Implicit_Cast()
    {
        var name = new GraphQLName("abc");
        FuncROM(name).ShouldBe(name);

        GraphQLName nameNull = null;
        FuncROM(nameNull).Length.ShouldBe(0);
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
