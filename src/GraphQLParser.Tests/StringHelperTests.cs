using Shouldly;
using System;
using Xunit;

namespace GraphQLParser.Tests
{
    public class StringHelperTests
    {
        [Theory]

        [InlineData("", "", 0, 0)]
        [InlineData("a", "a", 0, 1)]
        [InlineData("ab", "ab", 0, 2)]
        [InlineData("abc", "abc", 0, 3)]
        [InlineData("abcdefg____________1234567890", "abcdefg____________1234567890", 0, 29)]

        [InlineData("a", "abcdef", 0, 1)]
        [InlineData("ab", "abcdef", 0, 2)]
        [InlineData("abc", "abcdef", 0, 3)]

        [InlineData("b", "abcdef", 1, 2)]
        [InlineData("bc", "abcdef", 1, 3)]
        [InlineData("bcd", "abcdef", 1, 4)]

        [InlineData("f", "abcdef", 5, 6)]
        [InlineData("ef", "abcdef", 4, 6)]
        [InlineData("def", "abcdef", 3, 6)]
        public void GetHashCode_Should_Be_The_Same(string value, string source, int start, int end)
        {
            var code1 = value.GetHashCode();
            var code2 = StringHelper.GetHashCode(source, start, end);
            var code3 = StringHelper.GetHashCodeUnsafe(source, start, end);

            code2.ShouldBe(code3);

            //code2.ShouldBe(code1);
           // code3.ShouldBe(code1);
        }

        [Theory]

        [InlineData("", 0, 1)]
        [InlineData("", 1, 0)]
        [InlineData("", -1, 0)]
        [InlineData("", 0, -1)]

        [InlineData("abc", 0, 4)]
        [InlineData("abc", 4, 0)]
        [InlineData("abc", -1, 0)]
        [InlineData("abc", 0, -1)]
        public void GetHashCode_Should_Throw_Exception(string source, int start, int end)
        {
            Should.Throw<IndexOutOfRangeException>(() => StringHelper.GetHashCode(source, start, end));
            Should.Throw<IndexOutOfRangeException>(() => StringHelper.GetHashCodeUnsafe(source, start, end));
        }

        [Theory]

        [InlineData(true, "", "", 0, 0)]
        [InlineData(true, "a", "a", 0, 1)]
        [InlineData(true, "abc", "abcdef", 0, 3)]
        [InlineData(true, "def", "abcdef", 3, 6)]

        [InlineData(false, "", "a", 0, 1)]
        [InlineData(false, "aXc", "abcdef", 0, 3)]
        [InlineData(false, "def", "abcdef", 2, 6)]
        public void Equals_Should_Work(bool expected, string value, string source, int start, int end)
        {
            StringHelper.Equals(value, source, start, end).ShouldBe(expected);
            StringHelper.EqualsUnsafe(value, source, start, end).ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, "0", 0, 1)]
        [InlineData(0, "-0", 0, 2)]
        [InlineData(1234, "aaa1234b", 3, 7)]
        [InlineData(-1234, "aaa-1234b", 3, 8)]
        [InlineData(100000000, "100000000", 0, 9)]
        [InlineData(214748364, "a214748364", 1, 10)]
        public void ParseInt(int expected, string source, int start, int end)
        {
            StringHelper.ParseInt(source, start, end).ShouldBe(expected);
        }
    }
}
