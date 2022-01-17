using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLNameTests
{
    [Fact]
    public void GraphQLName_Cache_StringValue()
    {
        var name = new GraphQLName("");
        name.StringValue.ShouldBe(string.Empty);
        name.ToString().ShouldBe(string.Empty);

        name = new GraphQLName("abc");
        name.StringValue.ShouldBe("abc");
        name.ToString().ShouldBe("abc");
        ReferenceEquals(name.StringValue, name.StringValue);
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

        name = new GraphQLName("");
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
