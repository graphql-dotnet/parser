using System.Collections.Generic;
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
    public void GraphQLName_GetHashCode()
    {
        var dictionary = new Dictionary<GraphQLName, int>
        {
            [new GraphQLName("abc")] = 42
        };

        dictionary.ContainsKey(new GraphQLName("abc")).ShouldBeTrue();
        dictionary[new GraphQLName("abc")].ShouldBe(42);

        dictionary.ContainsKey(new GraphQLName("def")).ShouldBeFalse();
        dictionary.ContainsKey(new GraphQLName("")).ShouldBeFalse();
        dictionary.ContainsKey(new GraphQLName("def")).ShouldBeFalse();
    }

    [Fact]
    public void GraphQLName_Equality()
    {
        var name = new GraphQLName("abc");
        var nameNull = (GraphQLName)null;
        name.Equals(nameNull).ShouldBeFalse();
        name.Equals((object)nameNull).ShouldBeFalse();
        name.Equals((object)name).ShouldBeTrue();

        var nameother = new GraphQLName("def");
        name.Equals(nameother).ShouldBeFalse();
        name.Equals((object)nameother).ShouldBeFalse();

        var nameothersamevalue = new GraphQLName("abc");
        name.Equals(nameothersamevalue).ShouldBeTrue();
        name.Equals((object)nameothersamevalue).ShouldBeTrue();
    }

    [Fact]
    public void GraphQLName_Equality_Operators()
    {
        GraphQLName empty = new("");

        (empty == (GraphQLName)null).ShouldBeTrue();
        (empty == (string)null).ShouldBeTrue();
        ((GraphQLName)null != empty).ShouldBeFalse();
        ((string)null != empty).ShouldBeFalse();

        ((GraphQLName)null == (GraphQLName)null).ShouldBeTrue();
        ((GraphQLName)null != (GraphQLName)null).ShouldBeFalse();

        var name = new GraphQLName("abc");
        (name == (GraphQLName)null).ShouldBeFalse();
        (name == (string)null).ShouldBeFalse();
        ((GraphQLName)null == name).ShouldBeFalse();
        ((string)null == name).ShouldBeFalse();
        (name != (GraphQLName)null).ShouldBeTrue();
        (name != (string)null).ShouldBeTrue();
        ((GraphQLName)null != name).ShouldBeTrue();
        ((string)null != name).ShouldBeTrue();

        (name == empty).ShouldBeFalse();
        (empty == name).ShouldBeFalse();
        (name != empty).ShouldBeTrue();
        (empty != name).ShouldBeTrue();

        name = new GraphQLName("");
        (name == (GraphQLName)null).ShouldBeTrue();
        (name == (string)null).ShouldBeTrue();
        ((GraphQLName)null == name).ShouldBeTrue();
        ((string)null == name).ShouldBeTrue();
        (name != (GraphQLName)null).ShouldBeFalse();
        (name != (string)null).ShouldBeFalse();
        ((GraphQLName)null != name).ShouldBeFalse();
        ((string)null != name).ShouldBeFalse();

        var nameNull = (GraphQLName)null;
        (nameNull == (string)null).ShouldBeTrue();
        (nameNull != (string)null).ShouldBeFalse();

        (name == "def").ShouldBeFalse();
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
