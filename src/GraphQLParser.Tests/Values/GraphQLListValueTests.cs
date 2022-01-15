using System.Collections.Generic;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLListValueTests
{
    [Fact]
    public void EmptyValue_NullFields()
    {
        var value = new GraphQLListValue { };
        value.ClrValue.ShouldBeAssignableTo<IList<object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void EmptyValue_EmptyFields()
    {
        var value = new GraphQLListValue { Values = new List<GraphQLValue>() };
        value.ClrValue.ShouldBeAssignableTo<IList<object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void ObjectValue()
    {
        var value = new GraphQLListValue
        {
            Values = new List<GraphQLValue>
            {
                new GraphQLIntValue(42)
            }
        };
        var obj = value.ClrValue.ShouldBeAssignableTo<IList<object>>();
        obj.Count.ShouldBe(1);
        obj[0].ShouldBe(42);

        var obj2 = value.ClrValue;
        ReferenceEquals(obj, obj2).ShouldBeTrue();
    }
}
