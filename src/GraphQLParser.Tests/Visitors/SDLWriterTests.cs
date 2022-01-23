using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class SDLWriterTests
{
    [Fact]
    public void SDLWriter_Should_Have_Default_Options()
    {
        var writer = new SDLWriter<DefaultWriteContext>();
        writer.Options.ShouldNotBeNull();
        writer.Options.WriteComments.ShouldBeFalse();
        writer.Options.EachDirectiveLocationOnNewLine.ShouldBeFalse();
        writer.Options.EachUnionMemberOnNewLine.ShouldBeFalse();
    }

    [Theory]
    [InlineData(1,
@"#comment that ignored
  scalar A     ",
@"scalar A
", false)]
    [InlineData(2,
@"{
  #
  field
}",
@"{
  #
  field
}
")]
    [InlineData(3,
@"{
  complicatedArgs {
    intArgField(intArg: 2)
  }
}",
@"{
  complicatedArgs {
    intArgField(intArg: 2)
  }
}
")]
    [InlineData(4,
@"mutation createUser($userInput: UserInput!) {
  createUser(userInput: $userInput) {
    id
    gender
    profileImage
  }
}
",
@"mutation createUser($userInput: UserInput!) {
  createUser(userInput: $userInput) {
    id
    gender
    profileImage
  }
}
")]
    [InlineData(5,
@"query users {
  users {
    id
    union {
      ... on UserType {
        username
      }
      ... on CustomerType {
        customername
      }
    }
  }
}
",
@"query users {
  users {
    id
    union {
      ... on UserType {
        username
      }
      ... on CustomerType {
        customername
      }
    }
  }
}
")]
    [InlineData(6,
@"{ a(list: [], obj: {}) }",
@"{
  a(list: [], obj: {})
}
")]
    [InlineData(7,
@"directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT",
@"directive @skip(if: Boolean!) on
  | FIELD
  | FRAGMENT_SPREAD
  | INLINE_FRAGMENT
", false, true)]
    [InlineData(8,
@"directive @twoArgs
(a: Int, b:
String!) repeatable on QUERY|MUTATION|SUBSCRIPTION|FIELD|FRAGMENT_DEFINITION|FRAGMENT_SPREAD|INLINE_FRAGMENT|VARIABLE_DEFINITION|SCHEMA|SCALAR|OBJECT|FIELD_DEFINITION|ARGUMENT_DEFINITION|INTERFACE|UNION|ENUM|ENUM_VALUE|INPUT_OBJECT|INPUT_FIELD_DEFINITION",
@"directive @twoArgs(a: Int, b: String!) repeatable on QUERY | MUTATION | SUBSCRIPTION | FIELD | FRAGMENT_DEFINITION | FRAGMENT_SPREAD | INLINE_FRAGMENT | VARIABLE_DEFINITION | SCHEMA | SCALAR | OBJECT | FIELD_DEFINITION | ARGUMENT_DEFINITION | INTERFACE | UNION | ENUM | ENUM_VALUE | INPUT_OBJECT | INPUT_FIELD_DEFINITION
")]
    [InlineData(9,
@"directive @exportable on | SCHEMA",
@"directive @exportable on SCHEMA
")]
    [InlineData(10,
@"directive @exportable on | SCHEMA | ENUM",
@"directive @exportable on SCHEMA | ENUM
")]
    [InlineData(11,
@"extend schema @exportable ",
@"extend schema @exportable
")]
    [InlineData(12,
@"extend schema @exportable { mutation: M }",
@"extend schema @exportable
{
  mutation: M
}
")]
    [InlineData(13,
@"extend scalar Foo @exportable",
@"extend scalar Foo @exportable
")]
    [InlineData(14,
@"extend type Foo @exportable",
@"extend type Foo @exportable
")]
    [InlineData(15,
@"extend interface Foo @exportable",
@"extend interface Foo @exportable
")]
    [InlineData(16,
@"extend union Foo @exportable",
@"extend union Foo @exportable
")]
    [InlineData(17,
@"extend enum Foo @exportable",
@"extend enum Foo @exportable
")]
    [InlineData(18,
@"extend input Foo @exportable",
@"extend input Foo @exportable
")]
    [InlineData(19,
@"#comment
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
    [InlineData(20,
@"query inlineFragmentTyping {
  profiles(handles: [""zuck"", ""coca - cola""])
{
    handle
    ... on User
   {
      friends           {
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
",
@"query inlineFragmentTyping {
  profiles(handles: [""zuck"", ""coca - cola""]) {
    handle
    ... on User {
      friends {
        count
      }
    }
    ... on Page {
      likers {
        count
      }
    }
  }
}
")]
    [InlineData(21,
@"scalar a scalar b scalar c",
@"scalar a

scalar b

scalar c
")]
    [InlineData(22,
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
",
@"{
  foo
  #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query {
  bar
  baz
}
")]
    [InlineData(23,
@"union Animal @immutable = |Cat | Dog",
@"union Animal @immutable = Cat | Dog
")]
    [InlineData(24,
@"query
    q
{
  a : name
 b
  c  :  age
}", @"query q {
  a: name
  b
  c: age
}
")]
    [InlineData(25,
@"schema @checked @documented { mutation: MyMutation subscription: MySub }",
@"schema @checked @documented
{
  mutation: MyMutation
  subscription: MySub
}
")]
    [InlineData(26,
@"interface Dog implements & Eat & Bark { volume: Int! }",
@"interface Dog implements Eat & Bark
{
  volume: Int!
}
")]
    [InlineData(27,
@"enum Color { RED,
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
    [InlineData(28,
@"# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true) { name(full:true,kind: UPPER) age1:age address { street @short(length:5,x:""a"", pi: 3.14)
#need
building } }",
@"# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true) {
  name(full: true, kind: UPPER)
  age1: age
  address {
    street @short(length: 5, x: ""a"", pi: 3.14)
    #need
    building
  }
}
")]
    [InlineData(29,
@"
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
    [InlineData(30,
@"query q
{
#field comment! not alias!
  a
#colon comment
  :
#field name (GraphQLName) comment
  name
}
",
@"query q {
  #field comment! not alias!
  a:
  #field name (GraphQLName) comment
  name
}
")]
    [InlineData(31,
@"query q
{
#field comment! not alias!
  a
#colon comment
  :
#field name (GraphQLName) comment
  name
}
",
@"query q {
  a: name
}
", false)]
    [InlineData(32,
@"{
  f
  #arguments comment
  (x:10,
  y:{
  #comment on object field
  z: 1
  }
  )
}
",
@"{
  f
  #arguments comment
  (x: 10, y: {
  #comment on object field
  z: 1})
}
")]
    [InlineData(33,
@"{
  f
  #arguments comment
  (x:10,
  y:{
  #comment on object field
  z: 1
  }
  )
}
",
@"{
  f(x: 10, y: {z: 1})
}
", false)]
    [InlineData(34,
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external
",
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external
")]
    [InlineData(35,
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external
",
@"scalar JSON

extend scalar JSON @external
", false)]
    [InlineData(36,
@"#very good union
union Unity
#comment for union members
= A | B

#forgot about C!
extend union Unity = C
",
@"#very good union
union Unity
#comment for union members
= A | B

#forgot about C!
extend union Unity = C
")]
    [InlineData(37,
@"#very good union
union Unity
#comment for union members
= A | B

#forgot about C!
extend union Unity = C
",
@"union Unity = A | B

extend union Unity = C
", false)]
    [InlineData(38,
@"union Unity
= A |    B

extend union Unity =   C
",
@"union Unity =
  | A
  | B

extend union Unity =
  | C
", true, false, true)]
    public async Task WriteDocumentVisitor_Should_Print_Document(
        int number,
        string text,
        string expected,
        bool writeComments = true,
        bool eachDirectiveLocationOnNewLine = false,
        bool eachUnionMemberOnNewLine = false)
    {
        var writer = new StringWriter();
        using (var document = text.Parse())
        {
            await document.ToSDL(writer, new SDLWriterOptions
            {
                WriteComments = writeComments,
                EachDirectiveLocationOnNewLine = eachDirectiveLocationOnNewLine,
                EachUnionMemberOnNewLine = eachUnionMemberOnNewLine
            }).ConfigureAwait(false);
            var actual = writer.ToString();
            actual.ShouldBe(expected, $"Test {number} failed");

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

        var writer = new StringWriter();

        using (var document = (input + " scalar a").Parse())
        {
            await document.ToSDL(writer).ConfigureAwait(false);
            var renderedOriginal = writer.ToString();

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
        string expected = @$"{{
  a(p: {stringValue})
}}
";
        var writer = new StringWriter();

        using (var document = query.Parse())
        {
            await document.ToSDL(writer).ConfigureAwait(false);
            var rendered = writer.ToString();
            rendered.ShouldBe(expected);

            using (rendered.Parse())
            {
                // should be parsed back
            }
        }
    }

    [Fact]
    public async Task SelectionSet_Without_Parent_Should_Be_Printed_On_New_Line()
    {
        var selectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() };
        var writer = new StringWriter();
        var options = new SDLWriterOptions { WriteComments = true };
        await selectionSet.ToSDL(writer, options);
        writer.ToString().ShouldBe(@"{
}
");
        selectionSet.Comment = new GraphQLComment("comment");
        writer = new StringWriter();
        await selectionSet.ToSDL(writer, options);
        writer.ToString().ShouldBe(@"#comment
{
}
");
    }

    [Fact]
    public async Task SelectionSet_Under_Operation_With_Null_Name_Should_Be_Printed_On_New_Line()
    {
        var def = new GraphQLOperationDefinition
        {
            SelectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() }
        };
        var writer = new StringWriter();
        var options = new SDLWriterOptions { WriteComments = true };
        await def.ToSDL(writer, options);
        writer.ToString().ShouldBe(@"{
}
");
        def.SelectionSet.Comment = new GraphQLComment("comment");
        writer = new StringWriter();
        await def.ToSDL(writer, options);
        writer.ToString().ShouldBe(@"#comment
{
}
");
    }

    [Theory]
    [InlineData("query a { name }")]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    public async Task WriteDocumentVisitor_Should_Throw_On_Unknown_Values(string text)
    {
        var writer = new StringWriter();
        using (var document = text.Parse())
        {
            await new DoBadThingsVisitor().VisitAsync(document, new Context());

            var ex = await Should.ThrowAsync<NotSupportedException>(async () => await document.ToSDL(writer));
            ex.Message.ShouldStartWith("Unknown ");
        }
    }

    private class Context : INodeVisitorContext
    {
        public CancellationToken CancellationToken => throw new NotImplementedException();
    }

    private sealed class DoBadThingsVisitor : DefaultNodeVisitor<Context>
    {
        public override ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, Context context)
        {
            operationDefinition.Operation = (OperationType)99;
            return base.VisitOperationDefinitionAsync(operationDefinition, context);
        }

        public override ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, Context context)
        {
            for (int i = 0; i < directiveLocations.Items.Count; ++i)
                directiveLocations.Items[i] = (DirectiveLocation)(100 + i);

            return base.VisitDirectiveLocationsAsync(directiveLocations, context);
        }
    }
}
