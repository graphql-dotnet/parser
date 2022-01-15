using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class GraphQLEnumValueTests
{
    [Fact]
    public void EnumValueName()
    {
        var enumValue = new GraphQLEnumValue { Name = new GraphQLName("GREEN") };
        var value = enumValue.ClrValue;
        value.ShouldBe("GREEN");
        ReferenceEquals(value, enumValue.ClrValue).ShouldBeTrue();

        enumValue.Reset();
        //WOW! Even after GraphQLName.Reset ROM->string cast returns the same string instance!
        //ReferenceEquals(value, enumValue.ClrValue).ShouldBeFalse();
    }
}
