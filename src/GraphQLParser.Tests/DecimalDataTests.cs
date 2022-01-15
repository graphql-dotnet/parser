using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class DecimalDataTests
{
    [Fact]
    public void Should_Parse_Single_Interface()
    {
        var data1 = new DecimalData();
        data1.ShouldBe(default);

        var data2 = new DecimalData(1, 2, 3, 4);
        data2.Flags.ShouldBe(1U);
        data2.Hi.ShouldBe(2U);
        data2.Lo.ShouldBe(3U);
        data2.Mid.ShouldBe(4U);
    }
}
