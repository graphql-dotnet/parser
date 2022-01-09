using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class LocationTests
{
    [Fact]
    public void Equality()
    {
        var loc1 = new GraphQLLocation(10, 100);
        var loc2 = new GraphQLLocation(10, 100);

        (loc1 == loc2).ShouldBeTrue();
        (loc1 != loc2).ShouldBeFalse();

        loc1.Equals(loc2).ShouldBeTrue();
        loc1.Equals((object)loc2).ShouldBeTrue();
        loc1.Equals(42).ShouldBeFalse();

        loc1.GetHashCode().ShouldBe(loc2.GetHashCode());

        loc1.Equals(new GraphQLLocation(10, 99)).ShouldBeFalse();
        loc1.Equals(new GraphQLLocation(11, 100)).ShouldBeFalse();
    }

    [Fact]
    public void To_String()
    {
        var loc = new GraphQLLocation(10, 100);
        loc.ToString().ShouldBe("(10,100)");
    }
}
