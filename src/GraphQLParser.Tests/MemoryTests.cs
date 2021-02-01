using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void Operators()
        {
            var str = "string";
            ROM rom = str;
            (rom == str).ShouldBeTrue();
            (str == rom).ShouldBeTrue();
            (rom != str).ShouldBeFalse();
            (str != rom).ShouldBeFalse();

            var rom2 = rom.Slice(1);

            (rom2 == str).ShouldBeFalse();
            (str == rom2).ShouldBeFalse();
            (rom2 != str).ShouldBeTrue();
            (str != rom2).ShouldBeTrue();
        }
    }
}
