namespace GraphQLParser.Tests;

public class GraphQLBooleanValueTests
{
    [Fact]
    public void BoolValue_False()
    {
        var value = new GraphQLFalseBooleanValue();
        value.Value.Length.ShouldBe(5);
        value.Value.ShouldBe("false");
        value.BoolValue.ShouldBeFalse();
    }

    [Fact]
    public void BoolValue_True()
    {
        var value = new GraphQLTrueBooleanValue();
        value.Value.Length.ShouldBe(4);
        value.Value.ShouldBe("true");
        value.BoolValue.ShouldBeTrue();
    }
}
