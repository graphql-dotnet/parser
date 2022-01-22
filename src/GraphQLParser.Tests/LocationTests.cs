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

    [Theory]
    [InlineData(0, 1, 1, 'q')]
    [InlineData(1, 1, 2, 'u')]
    [InlineData(8, 1, 9, '{')]
    [InlineData(9, 1, 10, '\r')] // special case, does not occur in practice
    [InlineData(10, 1, 11, '\n')] // special case, does not occur in practice
    [InlineData(11, 2, 1, ' ')]
    [InlineData(13, 2, 3, 'u')]
    [InlineData(17, 2, 7, ' ')]
    [InlineData(28, 3, 8, 'r')]
    [InlineData(74, 7, 5, 'n')]
    public void ToLocationConvertRN(int start, int line, int column, char c)
    {
        var query = @"
01234567890123
query q {
  user {
    address {
      street
      house
    }
    name
  }
}";
        query = query.Replace("\r\n", "\n").Substring(16).Replace("\n", "\r\n");
        query[start].ShouldBe(c);
        var location = new Location(query, start);
        location.Line.ShouldBe(line);
        location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(0, 1, 1, 'q')]
    [InlineData(1, 1, 2, 'u')]
    [InlineData(8, 1, 9, '{')]
    [InlineData(9, 1, 10, '\n')] // special case, does not occur in practice
    [InlineData(10, 2, 1, ' ')]
    [InlineData(12, 2, 3, 'u')]
    [InlineData(16, 2, 7, ' ')]
    [InlineData(26, 3, 8, 'r')]
    [InlineData(68, 7, 5, 'n')]
    public void ToLocationConvertN(int start, int line, int column, char c)
    {
        var query = @"
01234567890123
query q {
  user {
    address {
      street
      house
    }
    name
  }
}";
        query = query.Replace("\r\n", "\n").Substring(16);
        query[start].ShouldBe(c);
        var location = new Location(query, start);
        location.Line.ShouldBe(line);
        location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(0, 1, 1, 'q')]
    [InlineData(1, 1, 2, 'u')]
    [InlineData(8, 1, 9, '{')]
    [InlineData(9, 1, 10, '\r')] // special case, does not occur in practice
    [InlineData(10, 2, 1, ' ')]
    [InlineData(12, 2, 3, 'u')]
    [InlineData(16, 2, 7, ' ')]
    [InlineData(26, 3, 8, 'r')]
    [InlineData(68, 7, 5, 'n')]
    public void ToLocationConvertR(int start, int line, int column, char c)
    {
        var query = @"
01234567890123
query q {
  user {
    address {
      street
      house
    }
    name
  }
}";
        query = query.Replace("\r\n", "\n").Substring(16).Replace("\n", "\r");
        query[start].ShouldBe(c);
        var location = new Location(query, start);
        location.Line.ShouldBe(line);
        location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData("\r", 0, 1, 1)]
    [InlineData("\r", 1, 1, 2)]

    [InlineData("\n", 0, 1, 1)]
    [InlineData("\n", 1, 1, 2)]

    [InlineData("\n\n", 0, 1, 1)]
    [InlineData("\n\n", 1, 2, 1)]
    [InlineData("\n\n", 2, 2, 2)]
    [InlineData("\n\n ", 2, 3, 1)]

    [InlineData("\r\r", 0, 1, 1)]
    [InlineData("\r\r", 1, 2, 1)]
    [InlineData("\r\r", 2, 2, 2)]
    [InlineData("\r\r ", 2, 3, 1)]

    [InlineData("\r\n", 0, 1, 1)]
    [InlineData("\r\n", 1, 1, 2)]
    [InlineData("\r\n", 2, 1, 3)]
    [InlineData("\r\n ", 2, 2, 1)]

    [InlineData("a", 100, 1, 101)]
    public void SpecialCases(string query, int start, int line, int column)
    {
        var location = new Location(query, start);
        location.Line.ShouldBe(line);
        location.Column.ShouldBe(column);
    }
}
