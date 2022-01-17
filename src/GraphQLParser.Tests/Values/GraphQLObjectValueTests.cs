using System.Collections.Generic;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLObjectValueTests
{
    [Fact]
    public void Field_From_NullFields()
    {
        var obj = new GraphQLObjectValue();
        obj.Field("abc").ShouldBeNull();
    }

    [Fact]
    public void Field_From_EmptyFields()
    {
        var obj = new GraphQLObjectValue { Fields = new List<GraphQLObjectField>() };
        obj.Field("abc").ShouldBeNull();
    }

    [Fact]
    public void Field_From_FilledFields_FirstFound()
    {
        var obj = new GraphQLObjectValue
        {
            Fields = new List<GraphQLObjectField>
            {
                new GraphQLObjectField { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(42) },
                new GraphQLObjectField { Name = new GraphQLName("def"), Value = new GraphQLIntValue(420) },
                new GraphQLObjectField { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(4200) },
            }
        };
        var field = obj.Field("abc");
        field.Name.ShouldBe(new GraphQLName("abc"));
        field.Value.ShouldBeOfType<GraphQLIntValue>().Value.ShouldBe("42");
    }

    [Fact]
    public void Field_From_FilledFields_SecondFound()
    {
        var obj = new GraphQLObjectValue
        {
            Fields = new List<GraphQLObjectField>
            {
                new GraphQLObjectField { Name = new GraphQLName("def"), Value = new GraphQLIntValue(420) },
                new GraphQLObjectField { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(42) },
                new GraphQLObjectField { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(4200) },
            }
        };
        var field = obj.Field("abc");
        field.Name.ShouldBe(new GraphQLName("abc"));
        field.Value.ShouldBeOfType<GraphQLIntValue>().Value.ShouldBe("42");
    }
}
