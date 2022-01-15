using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLEnumValueTests
{
    [Fact]
    public void EnumValueName()
    {
        var value = new GraphQLEnumValue { Name = new GraphQLName("GREEN") };
        value.ClrValue.ShouldBe("GREEN");
    }
}
