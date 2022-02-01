namespace GraphQLParser.Tests;

public class GraphQLNullValueTests
{
    [Fact]
    public void Null_Is_Always_Null()
    {
        var value = new GraphQLNullValue();
        value.Value.ShouldBe("null");
    }
}
