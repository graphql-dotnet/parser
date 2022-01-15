using System.Collections.Generic;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLObjectValueTests
{
    [Fact]
    public void EmptyValue_NullFields()
    {
        var value = new GraphQLObjectValue { };
        value.ClrValue.ShouldBeAssignableTo<IDictionary<string, object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void EmptyValue_EmptyFields()
    {
        var value = new GraphQLObjectValue { Fields = new List<GraphQLObjectField>() };
        value.ClrValue.ShouldBeAssignableTo<IDictionary<string, object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void ObjectValue()
    {
        var value = new GraphQLObjectValue
        {
            Fields = new List<GraphQLObjectField>
            {
                new GraphQLObjectField { Name = new GraphQLName("age"), Value = new GraphQLIntValue(42) }
            }
        };
        var obj = value.ClrValue.ShouldBeAssignableTo<IDictionary<string, object>>();
        obj.Count.ShouldBe(1);
        obj["age"].ShouldBe(42);

        var obj2 = value.ClrValue;
        ReferenceEquals(obj, obj2).ShouldBeTrue();
    }
}
