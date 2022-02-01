namespace GraphQLParser.Tests;

public class GraphQLArgumentsTests
{
    [Fact]
    public void Value_From_NullArgs()
    {
        var args = new GraphQLArguments();
        args.ValueFor("abc").ShouldBeNull();
        args.Find("abc").ShouldBeNull();
    }

    [Fact]
    public void Value_From_EmptyFields()
    {
        var args = new GraphQLArguments { Items = new List<GraphQLArgument>() };
        args.ValueFor("abc").ShouldBeNull();
        args.Find("abc").ShouldBeNull();
    }

    [Fact]
    public void Value_From_FilledArgs_FirstFound()
    {
        var args = new GraphQLArguments
        {
            Items = new List<GraphQLArgument>
            {
                new GraphQLArgument { Name = new GraphQLName("abc"),Value = new GraphQLIntValue(42) },
                new GraphQLArgument { Name = new GraphQLName("def"), Value = new GraphQLIntValue(420) },
                new GraphQLArgument { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(4200) },
            }
        };
        var val = args.ValueFor("abc");
        args.Find("abc").Value.ShouldBe(val);
        val.ShouldBeOfType<GraphQLIntValue>().Value.ShouldBe("42");
    }

    [Fact]
    public void Value_From_FilledArgs_SecondFound()
    {
        var args = new GraphQLArguments
        {
            Items = new List<GraphQLArgument>
            {
                new GraphQLArgument { Name = new GraphQLName("def"), Value = new GraphQLIntValue(420) },
                new GraphQLArgument { Name = new GraphQLName("abc"),Value = new GraphQLIntValue(42) },
                new GraphQLArgument { Name = new GraphQLName("abc"), Value = new GraphQLIntValue(4200) },
            }
        };
        var val = args.ValueFor("abc");
        args.Find("abc").Value.ShouldBe(val);
        val.ShouldBeOfType<GraphQLIntValue>().Value.ShouldBe("42");
    }
}
