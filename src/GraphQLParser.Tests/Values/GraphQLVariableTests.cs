using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphVariableTests
{
    [Fact]
    public void VariableName()
    {
        var variable = new GraphQLVariable { Name = new GraphQLName("id") };
        variable.ClrValue.ShouldBe("id");
    }
}
