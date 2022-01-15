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
        value.ClrValue.ShouldBe(s);
        ReferenceEquals(value.ClrValue, s).ShouldBeTrue();
    }

    [Fact]
    public void StringValue_Cached()
    {
        var value = new GraphQLStringValue { Value = "abc" };
        value.Value.Length.ShouldBe(3);
        value.ClrValue.ShouldBe("abc");
        ReferenceEquals(value.ClrValue, value.ClrValue).ShouldBeTrue();
    }
}
