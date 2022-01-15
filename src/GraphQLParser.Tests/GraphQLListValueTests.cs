using System;
using System.Collections.Generic;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLListValueTests
{
    [Fact]
    public void NoValue()
    {
        var value = new GraphQLListValue();
        value.Value.Length.ShouldBe(0);
        var ex = Should.Throw<InvalidOperationException>(() => value.ClrValue);
        ex.Message.ShouldStartWith("Invalid list (empty string)");
    }

    [Fact]
    public void EmptyValue_NullFields()
    {
        var value = new GraphQLListValue { Value = "_UNUSED_" };
        value.Value.Length.ShouldBe(8);
        value.ClrValue.ShouldBeAssignableTo<IList<object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void EmptyValue_EmptyFields()
    {
        var value = new GraphQLListValue { Value = "_UNUSED_", Values = new List<GraphQLValue>() };
        value.Value.Length.ShouldBe(8);
        value.ClrValue.ShouldBeAssignableTo<IList<object>>().Count.ShouldBe(0);
    }

    [Fact]
    public void ObjectValue()
    {
        var value = new GraphQLListValue
        {
            Value = "_UNUSED_",
            Values = new List<GraphQLValue>
            {
                new GraphQLIntValue(42)
            }
        };
        value.Value.Length.ShouldBe(8);
        var obj = value.ClrValue.ShouldBeAssignableTo<IList<object>>();
        obj.Count.ShouldBe(1);
        obj[0].ShouldBe(42);

        var obj2 = value.ClrValue;
        ReferenceEquals(obj, obj2).ShouldBeTrue();
    }
}
