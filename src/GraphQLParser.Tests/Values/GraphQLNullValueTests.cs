using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLNullValueTests
{
    [Fact]
    public void Null_Is_Always_Null()
    {
        var value = new GraphQLNullValue();
        value.ClrValue.ShouldBe(null);
        value.Reset();
        value.ClrValue.ShouldBe(null);
    }
}
