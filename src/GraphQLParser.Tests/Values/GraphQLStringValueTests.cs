using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLStringValueTests
{
    [Fact]
    public void EmptyValue()
    {
        var value = new GraphQLStringValue();
        value.Value.Length.ShouldBe(0);
        value.ClrValue.ShouldBe("");
    }

    [Fact]
    public void StringValue_Ctor()
    {
        const string s = "abc";
        var value = new GraphQLStringValue(s);
        value.Value.Length.ShouldBe(3);
        var val = value.ClrValue;
        val.ShouldBe(s);
        ReferenceEquals(value.ClrValue, s).ShouldBeTrue();

        value.Reset();
        //WOW! Even after GraphQLStringValue.Reset ROM->string cast returns the same string instance!
        //ReferenceEquals(val, value.ClrValue).ShouldBeFalse();
    }

    [Fact]
    public void StringValue_Cached()
    {
        var value = new GraphQLStringValue { Value = "abc" };
        value.Value.Length.ShouldBe(3);
        var val = value.ClrValue;
        val.ShouldBe("abc");
        ReferenceEquals(val, value.ClrValue).ShouldBeTrue();

        value.Reset();
        //WOW! Even after GraphQLStringValue.Reset ROM->string cast returns the same string instance!
        //ReferenceEquals(val, value.ClrValue).ShouldBeFalse();
    }
}
