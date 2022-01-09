using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class SDLWriterTests
{
    private class TestContext : IWriteContext
    {
        public TextWriter Writer { get; set; } = new StringWriter();

        public Stack<AST.ASTNode> Parents { get; set; } = new Stack<AST.ASTNode>();

        public CancellationToken CancellationToken { get; set; }
    }

    private static readonly SDLWriter<TestContext> _sdlWriter = new();

    [Theory]
    [InlineData("{ a(list: [], obj: {}) }", @"
{
  a(list: [], obj: {})
}
")]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT", @"directive @skip(
  if: Boolean!
) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    [InlineData("directive @twoArgs(a: Int, b: String!) repeatable on QUERY|MUTATION|SUBSCRIPTION|FIELD|FRAGMENT_DEFINITION|FRAGMENT_SPREAD|INLINE_FRAGMENT|VARIABLE_DEFINITION|SCHEMA|SCALAR|OBJECT|FIELD_DEFINITION|ARGUMENT_DEFINITION|INTERFACE|UNION|ENUM|ENUM_VALUE|INPUT_OBJECT|INPUT_FIELD_DEFINITION", @"directive @twoArgs(
  a: Int
  b: String!
) repeatable on QUERY | MUTATION | SUBSCRIPTION | FIELD | FRAGMENT_DEFINITION | FRAGMENT_SPREAD | INLINE_FRAGMENT | VARIABLE_DEFINITION | SCHEMA | SCALAR | OBJECT | FIELD_DEFINITION | ARGUMENT_DEFINITION | INTERFACE | UNION | ENUM | ENUM_VALUE | INPUT_OBJECT | INPUT_FIELD_DEFINITION")]
    [InlineData("directive @exportable on | SCHEMA", @"directive @exportable on SCHEMA")]
    [InlineData("directive @exportable on | SCHEMA | ENUM", @"directive @exportable on SCHEMA | ENUM")]
    [InlineData("extend scalar Foo @exportable", @"extend scalar Foo @exportable
")]
    [InlineData("extend type Foo @exportable", @"extend type Foo @exportable
")]
    [InlineData("extend interface Foo @exportable", "extend interface Foo @exportable")]
    [InlineData("extend union Foo @exportable", "extend union Foo @exportable")]
    [InlineData("extend enum Foo @exportable", "extend enum Foo @exportable")]
    [InlineData("extend input Foo @exportable", @"extend input Foo @exportable
")]
    [InlineData(@"#comment
input Example @x {
  self: [Example!]!
  value: String = ""xyz""
}
input B
input C
",
@"#comment
input Example @x
{
  self: [Example!]!
  value: String = ""xyz""
}

input B

input C
")]
    [InlineData(@"query inlineFragmentTyping {
  profiles(handles: [""zuck"", ""coca - cola""]) {
    handle
    ... on User {
      friends {
        count
      }
}
... on Page
{
    likers
    {
        count
      }
}
  }
}
", @"query inlineFragmentTyping
{
  profiles(handles: [""zuck"", ""coca - cola""])
  {
    handle
    ... on User
    {
      friends
      {
        count
      }
    }
    ... on Page
    {
      likers
      {
        count
      }
    }
  }
}
")]
    [InlineData(@"scalar a scalar b scalar c", @"scalar a

scalar b

scalar c
")]
    [InlineData(@"{
  foo
    #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query {
  bar
  baz
}
",

@"
{
  foo
  #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query
{
  bar
  baz
}
")]
    [InlineData(@"union Animal @immutable = |Cat | Dog", @"union Animal @immutable = Cat | Dog")]
    [InlineData(@"query
    q
{
  a : name
 b
  c  :  age
}", @"query q
{
  a: name
  b
  c: age
}
")]
    [InlineData(@"schema @checked @documented { mutation: MyMutation subscription: MySub }", @"schema @checked @documented
{
  mutation: MyMutation
  subscription: MySub
}
")]
    [InlineData(@"interface Dog implements & Eat & Bark { volume: Int! }",
        @"interface Dog implements Eat & Bark
{
  volume: Int!
}
")]
    [InlineData(@"enum Color { RED,
#good color
GREEN @directive(list: [1,2.7,3,null,{}, {name:""tom"" age:42}]),
""""""
another good color
""""""
BLUE }",
@"enum Color
{
  RED
  #good color
  GREEN @directive(list: [1, 2.7, 3, null, {}, {name: ""tom"", age: 42}])
  ""another good color""
  BLUE
}
")]
    [InlineData(@"# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true) { name(full:true,kind: UPPER) age1:age address { street @short(length:5,x:""a"", pi: 3.14)
#need
building } }",

@"# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true)
{
  name(full: true, kind: UPPER)
  age1: age
  address
  {
    street @short(length: 5, x: ""a"", pi: 3.14)
    #need
    building
  }
}
")]
    [InlineData(@"
""""""
  description
    indent 2
      indent4
""""""
scalar JSON @exportable
# A dog
type Dog implements &Animal {
  """"""inline docs""""""
  volume: Float
  """"""
 multiline
 docs
  """"""
  friends: [Dog!]
  age: Int!
}",

@"""""""
description
  indent 2
    indent4
""""""
scalar JSON @exportable

# A dog
type Dog implements Animal
{
  ""inline docs""
  volume: Float
  """"""
  multiline
  docs
  """"""
  friends: [Dog!]
  age: Int!
}
")]
    public async Task WriteDocumentVisitor_Should_Print_Document(string text, string expected)
    {
        var context = new TestContext();

        using (var document = text.Parse())
        {
            await _sdlWriter.Visit(document, context).ConfigureAwait(false);
            var actual = context.Writer.ToString();
            actual.ShouldBe(expected);

            using (actual.Parse())
            {
                // should be parsed back
            }
        }
    }

    [Theory]
    [InlineData(1, "test", "test", false)]
    [InlineData(2, "te\\\"\"\"st", "te\\\"\\\"\\\"st", false)]
    [InlineData(3, "\ntest", "test", false)]
    [InlineData(4, "\r\ntest", "test", false)]
    [InlineData(5, " \ntest", "test", false)]
    [InlineData(6, "\t\ntest", "test", false)]
    [InlineData(7, "\n\ntest", "test", false)]
    [InlineData(8, "\ntest\nline2", "\ntest\nline2\n", true)]
    [InlineData(9, "test\rline2", "\ntest\nline2\n", true)]
    [InlineData(10, "test\r\nline2", "\ntest\nline2\n", true)]
    [InlineData(11, "test\r\r\nline2", "\ntest\n\nline2\n", true)]
    [InlineData(12, "test\r\n\nline2", "\ntest\n\nline2\n", true)]
    [InlineData(13, "test\n", "test", false)]
    [InlineData(14, "test\n ", "test", false)]
    [InlineData(15, "test\n\t", "test", false)]
    [InlineData(16, "test\n\n", "test", false)]
    [InlineData(17, "test\n  line2", "\ntest\nline2\n", true)]
    [InlineData(18, "test\n\t\tline2", "\ntest\nline2\n", true)]
    [InlineData(19, "test\n \tline2", "\ntest\nline2\n", true)]
    [InlineData(20, "  test\nline2", "\n  test\nline2\n", true)]
    [InlineData(21, "  test\n  line2", "\n  test\nline2\n", true)]
    [InlineData(22, "\n  test\n  line2", "\ntest\nline2\n", true)]
    [InlineData(23, "  test\n line2\n\t\tline3\n  line4", "\n  test\nline2\n\tline3\n line4\n", true)]
    [InlineData(24, "  test\n  Hello,\n\n    world!\n ", "\n  test\nHello,\n\n  world!\n", true)]
    [InlineData(25, "  \n  Hello,\r\n\n    world!\n ", "\nHello,\n\n  world!\n", true)]
    [InlineData(26, "  \n  Hello,\r\n\n    wor___ld!\n ", "\nHello,\n\n  wor___ld!\n", true)]
    [InlineData(27, "\r\n    Hello,\r\n      World!\r\n\r\n    Yours,\r\n      GraphQL.\r\n  ", "\nHello,\n  World!\n\nYours,\n  GraphQL.\n", true)]
    [InlineData(28, "Test \\n escaping", "Test \\\\n escaping", false)]
    [InlineData(29, "Test \\u1234 escaping", "Test \\\\u1234 escaping", false)]
    [InlineData(30, "Test \\ escaping", "Test \\\\ escaping", false)]
    public async Task WriteDocumentVisitor_Should_Print_BlockStrings(int number, string input, string expected, bool isBlockString)
    {
        number.ShouldBeGreaterThan(0);

        input = input.Replace("___", new string('_', 9000));
        expected = expected.Replace("___", new string('_', 9000));

        input = "\"\"\"" + input + "\"\"\"";
        expected = isBlockString
            ? "\"\"\"" + expected + "\"\"\""
            : "\"" + expected + "\"";

        var context = new TestContext();

        using (var document = (input + " scalar a").Parse())
        {
            await _sdlWriter.Visit(document, context).ConfigureAwait(false);
            var renderedOriginal = context.Writer.ToString();

            var lines = renderedOriginal.Split(Environment.NewLine);
            var renderedDescription = string.Join(Environment.NewLine, lines.SkipLast(2));
            renderedDescription = renderedDescription.Replace("\r\n", "\n");
            renderedDescription.ShouldBe(expected);

            using (renderedOriginal.Parse())
            {
                // should be parsed back
            }
        }
    }

    [Theory]
    [InlineData("\"\"")]
    [InlineData("\"\\\\\"")]
    [InlineData("\"\\n\\b\\f\\r\\t\"")]
    [InlineData("\" \u1234 \"")]
    [InlineData("\"normal text\"")]
    public async Task WriteDocumentVisitor_Should_Print_EscapedStrings(string stringValue)
    {
        string query = $"{{a(p:{stringValue})}}";
        string expected = @$"
{{
  a(p: {stringValue})
}}
";
        var context = new TestContext();

        using (var document = query.Parse())
        {
            await _sdlWriter.Visit(document, context).ConfigureAwait(false);
            var rendered = context.Writer.ToString();
            rendered.ShouldBe(expected);

            using (rendered.Parse())
            {
                // should be parsed back
            }
        }
    }
}
