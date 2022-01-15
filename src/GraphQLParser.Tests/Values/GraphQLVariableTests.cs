using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLVariableTests
{
    [Fact]
    public void VariableName()
    {
        var variable = new GraphQLVariable { Name = new GraphQLName("id") };
        var value = variable.ClrValue;
        value.ShouldBe("id");
        ReferenceEquals(value, variable.ClrValue).ShouldBeTrue();

        variable.Reset();
        //WOW! Even after GraphQLName.Reset ROM->string cast returns the same string instance!
        //ReferenceEquals(value, variable.ClrValue).ShouldBeFalse();
    }
}
