using System.Text;
using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class SDLPrinterTests
{
    [Fact]
    public async Task Print_Matches_PrintAsync_String()
    {
        var query = "KitchenSink".ReadGraphQLFile().Parse();
        var writer = new SDLPrinter();
        var sw = new StringWriter();
        await writer.PrintAsync(query, sw);
        sw.Flush();
        var txt = sw.ToString();
        writer.Print(query).ShouldBe(txt);
    }

    [Fact]
    public async Task Print_Matches_PrintAsync_Bytes()
    {
        var query = "KitchenSink".ReadGraphQLFile().Parse();
        var writer = new SDLPrinter();
        var ms1 = new MemoryStream();
        var sw = new StreamWriter(ms1);
        await writer.PrintAsync(query, sw);
        sw.Flush();

        var ms2 = new MemoryStream();
        writer.Print(query, ms2);
        var bytes1 = ms1.ToArray();
        var bytes2 = ms2.ToArray();
        bytes1.ShouldBe(bytes2);
    }

    [Fact]
    public void SDLPrinter_Should_Have_Default_Options()
    {
        var writer = new SDLPrinter();
        writer.Options.ShouldNotBeNull();
        writer.Options.PrintComments.ShouldBeFalse();
        writer.Options.EachDirectiveLocationOnNewLine.ShouldBeFalse();
        writer.Options.EachUnionMemberOnNewLine.ShouldBeFalse();
    }

    [Theory]
    [InlineData(1,
@"#comment that ignored
  scalar A     ",
@"scalar A", false)]
    [InlineData(2,
@"{
  #
  field
}",
@"{
  #
  field
}")]
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
}")]
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
}")]
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
}")]
    [InlineData(6,
@"{ a(list: [], obj: {}) }",
@"{
  a(list: [], obj: {})
}")]
    [InlineData(7,
@"directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT",
@"directive @skip(if: Boolean!) on
  | FIELD
  | FRAGMENT_SPREAD
  | INLINE_FRAGMENT", false, true)]
    [InlineData(8,
@"directive @twoArgs
(a: Int, b:
String!) repeatable on QUERY|MUTATION|SUBSCRIPTION|FIELD|FRAGMENT_DEFINITION|FRAGMENT_SPREAD|INLINE_FRAGMENT|VARIABLE_DEFINITION|SCHEMA|SCALAR|OBJECT|FIELD_DEFINITION|ARGUMENT_DEFINITION|INTERFACE|UNION|ENUM|ENUM_VALUE|INPUT_OBJECT|INPUT_FIELD_DEFINITION",
@"directive @twoArgs(a: Int, b: String!) repeatable on QUERY | MUTATION | SUBSCRIPTION | FIELD | FRAGMENT_DEFINITION | FRAGMENT_SPREAD | INLINE_FRAGMENT | VARIABLE_DEFINITION | SCHEMA | SCALAR | OBJECT | FIELD_DEFINITION | ARGUMENT_DEFINITION | INTERFACE | UNION | ENUM | ENUM_VALUE | INPUT_OBJECT | INPUT_FIELD_DEFINITION")]
    [InlineData(9,
@"directive @exportable on | SCHEMA",
@"directive @exportable on SCHEMA")]
    [InlineData(10,
@"directive @exportable on | SCHEMA | ENUM",
@"directive @exportable on SCHEMA | ENUM")]
    [InlineData(11,
@"extend schema @exportable ",
@"extend schema @exportable")]
    [InlineData(12,
@"extend schema @exportable { mutation: M }",
@"extend schema @exportable {
  mutation: M
}")]
    [InlineData(13,
@"extend scalar Foo @exportable",
@"extend scalar Foo @exportable")]
    [InlineData(14,
@"extend type Foo @exportable",
@"extend type Foo @exportable")]
    [InlineData(15,
@"extend interface Foo @exportable",
@"extend interface Foo @exportable")]
    [InlineData(16,
@"extend union Foo @exportable",
@"extend union Foo @exportable")]
    [InlineData(17,
@"extend enum Foo @exportable",
@"extend enum Foo @exportable")]
    [InlineData(18,
@"extend input Foo @exportable",
@"extend input Foo @exportable")]
    [InlineData(19, """
#comment
input Example @x
#comment on fields
{
  self: [Example!]!
  value: String = "xyz"
}
input B
input C
""", """
#comment
input Example @x
#comment on fields
{
  self: [Example!]!
  value: String = "xyz"
}

input B

input C
""")]
    [InlineData(20, """
query inlineFragmentTyping {
  profiles(handles: ["zuck", "coca - cola"])
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
""", """
query inlineFragmentTyping {
  profiles(handles: ["zuck", "coca - cola"]) {
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
""")]
    [InlineData(21,
@"scalar a scalar b scalar c",
@"scalar a

scalar b

scalar c")]
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
}",
@"{
  foo
  #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query {
  bar
  baz
}")]
    [InlineData(23,
@"union Animal @immutable = |Cat | Dog",
@"union Animal @immutable = Cat | Dog")]
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
}")]
    [InlineData(25,
@"schema @checked @documented { mutation: MyMutation subscription: MySub }",
@"schema @checked @documented {
  mutation: MyMutation
  subscription: MySub
}")]
    [InlineData(26,
@"interface Dog implements & Eat & Bark { volume: Int! }",
@"interface Dog implements Eat & Bark {
  volume: Int!
}")]
    [InlineData(27, """"
enum Color { RED,
#good color
GREEN @directive(list: [1,2.7,3,null,{}, {name:"tom" age:42}]),
"""
another good color
"""
BLUE }
"""", """
enum Color {
  RED
  #good color
  GREEN @directive(list: [1, 2.7, 3, null, {}, {name: "tom", age: 42}])
  "another good color"
  BLUE
}
""")]
    [InlineData(28, """
# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true) { name(full:true,kind: UPPER) age1:age address { street @short(length:5,x:"a", pi: 3.14)
#need
building } }
""", """
# super query
#
# multiline
query summary($id: ID!, $detailed: Boolean! = true) {
  name(full: true, kind: UPPER)
  age1: age
  address {
    street @short(length: 5, x: "a", pi: 3.14)
    #need
    building
  }
}
""")]
    [InlineData(29, """"
"""
  description
    indent 2
      indent4
"""
scalar JSON @exportable
# A dog
type Dog implements &Animal
     #comment on fields
 {
  """inline docs"""
  volume: Float
  """
 multiline
 docs
  """
  friends: [Dog!]
  age(precise: Boolean!): Int!
}
"""", """"
"""
description
  indent 2
    indent4
"""
scalar JSON @exportable

# A dog
type Dog implements Animal
#comment on fields
{
  "inline docs"
  volume: Float
  """
  multiline
  docs
  """
  friends: [Dog!]
  age(precise: Boolean!): Int!
}
"""")]
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
}")]
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
}", false)]
    [InlineData(32,
@"{
  f
  #arguments comment
  #multilined
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
  #multilined
  (x: 10, y: {
  #comment on object field
  z: 1})
}")]
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
}", false)]
    [InlineData(34,
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external
",
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external")]
    [InlineData(35,
@"#very good scalar
scalar JSON

#forgot about external!
extend scalar JSON @external
",
@"scalar JSON

extend scalar JSON @external", false)]
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
extend union Unity = C")]
    [InlineData(37,
@"#very good union
union Unity
#comment for union members
= A | B

#forgot about C!
extend union Unity = C
",
@"union Unity = A | B

extend union Unity = C", false)]
    [InlineData(38,
@"union Unity
= A |    B

extend union Unity =   C
",
@"union Unity =
  | A
  | B

extend union Unity =
  | C", true, false, true)]
    [InlineData(38,
@"enum Color
    #comment
 {
     GREEN   RED }

extend enum Color
#comment
{   YELLOW }
",
@"enum Color
#comment
{
  GREEN
  RED
}

extend enum Color
#comment
{
  YELLOW
}", true)]
    [InlineData(39,
@"type T  {
    data(
#comment
rendered: Boolean) : String
}",
@"type T {
  data(
  #comment
  rendered: Boolean): String
}", true)]
    [InlineData(40, """
"This is a Foo object type"
type Foo {
  "This is of type Integer"
  int: Int
  "This is of type String"
  str: String
}

type Query
{
    foo: Foo
}
""", """
"This is a Foo object type"
type Foo {
  "This is of type Integer"
  int: Int
  "This is of type String"
  str: String
}

type Query {
  foo: Foo
}
""", true)]
    [InlineData(41,
"""directive @skip("Skipped when true." if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT""",
@"directive @skip(
  ""Skipped when true.""
  if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    [InlineData(42,
@"directive @skip(""Skipped when true."" if: Boolean!, x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT",
@"directive @skip(
  ""Skipped when true.""
  if: Boolean!, x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    [InlineData(43,
"""directive @skip("Skipped when true." if: Boolean!, "Second argument" x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT""", """
directive @skip(
  "Skipped when true."
  if: Boolean!,
  "Second argument"
  x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT
""")]
    [InlineData(44,
"schema { query: Q mutation: M subscription: S }",
"""
schema {
     query: Q
     mutation: M
     subscription: S
}
""", true, false, false, 5)]
    [InlineData(45,
"""
"A component contains the parametric details of a PCB part."
input DesComponentFilterInput {
    and: [DesComponentFilterInput!]
    or: [DesComponentFilterInput!]
    "The library label for this component."
    name: StringOperationFilterInput
    "The additional information for this component."
    comment: StringOperationFilterInput
    "The summary of function or other performance details for this component."
    description: StringOperationFilterInput
    "The component revision."
    revision: DesRevisionFilterInput
}
""",
"""
"A component contains the parametric details of a PCB part."
input DesComponentFilterInput {
  and: [DesComponentFilterInput!]
  or: [DesComponentFilterInput!]
  "The library label for this component."
  name: StringOperationFilterInput
  "The additional information for this component."
  comment: StringOperationFilterInput
  "The summary of function or other performance details for this component."
  description: StringOperationFilterInput
  "The component revision."
  revision: DesRevisionFilterInput
}
""")]
    [InlineData(46,
"""
# comment
directive @my on FIELD
""",
"""
# comment
directive @my on FIELD
""")]
    [InlineData(47,
"""
query q
# comment
($a: Int) { x }
""",
"""
query q
# comment
($a: Int) {
  x
}
""")]
    [InlineData(48,
"""
query q
(
# comment
$a: Int) { x }
""",
"""
query q(
# comment
$a: Int) {
  x
}
""")]
    public async Task SDLPrinter_Should_Print_Document(
        int number,
        string text,
        string expected,
        bool writeComments = true,
        bool eachDirectiveLocationOnNewLine = false,
        bool eachUnionMemberOnNewLine = false,
        int indentSize = 2)
    {
        var printer = new SDLPrinter(new SDLPrinterOptions
        {
            PrintComments = writeComments,
            EachDirectiveLocationOnNewLine = eachDirectiveLocationOnNewLine,
            EachUnionMemberOnNewLine = eachUnionMemberOnNewLine,
            IndentSize = indentSize,
        });
        var writer = new StringWriter();
        var document = text.Parse();

        await printer.PrintAsync(document, writer).ConfigureAwait(false);
        var actual = writer.ToString();
        actual.ShouldBe(expected, $"Test {number} failed");

        actual.Parse(); // should be parsed back
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
    [InlineData(31, "t\"est\n  line2", "\nt\"est\nline2\n", true)] // BlockString with double quote inside
    [InlineData(32, "t\\\"\"\"est\n  line2", "\nt\\\"\"\"est\nline2\n", true)] // BlockString with triple double quote inside
    public async Task SDLPrinter_Should_Print_BlockStrings(int number, string input, string expected, bool isBlockString)
    {
        number.ShouldBeGreaterThan(0);

        input = input.Replace("___", new string('_', 9000));
        expected = expected.Replace("___", new string('_', 9000));

        input = "\"\"\"" + input + "\"\"\"";
        expected = isBlockString
            ? "\"\"\"" + expected + "\"\"\""
            : "\"" + expected + "\"";

        var writer = new StringWriter();

        var document = (input + " scalar a").Parse();

        var printer = new SDLPrinter();
        await printer.PrintAsync(document, writer).ConfigureAwait(false);
        var renderedOriginal = writer.ToString();

        var lines = renderedOriginal.Split(Environment.NewLine);
        var renderedDescription = string.Join(Environment.NewLine, lines.SkipLast(1));
        renderedDescription = renderedDescription.Replace("\r\n", "\n");
        renderedDescription.ShouldBe(expected);

        renderedOriginal.Parse(); // should be parsed back
    }

    [Theory]
    [InlineData("\"\"")]
    [InlineData("\"\\\\\"")]
    [InlineData("\"\\n\\b\\f\\r\\t\"")]
    [InlineData("\"\\tX\\t\"")]
    [InlineData("\" \u1234 \"")] // unicode > ' ' (32)
    [InlineData("\" \\u001F \"")] // unicode < ' ' (32)
    [InlineData("\" \\u0000 \"")] // unicode < ' ' (32)
    [InlineData("\"normal text\"")]
    public async Task SDLPrinter_Should_Print_EscapedStrings(string stringValue)
    {
        string query = $"{{a(p:{stringValue})}}";
        string expected = @$"{{
  a(p: {stringValue})
}}";
        var writer = new StringWriter();

        var document = query.Parse();

        var printer = new SDLPrinter();
        await printer.PrintAsync(document, writer).ConfigureAwait(false);
        var rendered = writer.ToString();
        rendered.ShouldBe(expected);

        rendered.Parse(); // should be parsed back
    }

    [Fact]
    public async Task SelectionSet_Without_Parent_Should_Be_Printed_On_New_Line()
    {
        var selectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() };
        var writer = new StringWriter();
        var printer = new SDLPrinter(new SDLPrinterOptions { PrintComments = true });
        await printer.PrintAsync(selectionSet, writer);
        writer.ToString().ShouldBe(@"{
}");
        selectionSet.Comments = new List<GraphQLComment> { new GraphQLComment("comment") };
        writer = new StringWriter();
        await printer.PrintAsync(selectionSet, writer);
        writer.ToString().ShouldBe(@"#comment
{
}");
    }

    [Fact]
    public async Task SelectionSet_Under_Operation_With_Null_Name_Should_Be_Printed_On_New_Line()
    {
        var def = new GraphQLOperationDefinition
        {
            SelectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() }
        };
        var writer = new StringWriter();
        var printer = new SDLPrinter(new SDLPrinterOptions { PrintComments = true });
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe(@"{
}");
        def.SelectionSet.Comments = new List<GraphQLComment> { new GraphQLComment("comment") };
        writer = new StringWriter();
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe(@"#comment
{
}");
    }

    [Theory]
    [InlineData(
@"description",
@"""description""
")]
    [InlineData(
@"description
multilined",
@"""""""
description
multilined
""""""
")]
    public async Task Description_Without_Parent_Should_Be_Printed(string text, string expected)
    {
        var description = new GraphQLDescription(text);
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(description, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(
"a \u0000\u0001\u0002\u0003\u0004\u0005\u0006\u0007\u0008\u0009\u000A\u000B\u000C\u000D\u000E\u000F b",
@"""a \u0000\u0001\u0002\u0003\u0004\u0005\u0006\u0007\b\t\n\u000B\f\r\u000E\u000F b""
")]
    [InlineData(
"a \u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F b",
@"""a \u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F b""
")]
    [InlineData(                    // TODO: Change test condition?
"Test\r\nLine 2\rLine 3\nLine 4", """"
"""
Test
Line 2Line 3
Line 4
"""

"""")]
    public async Task Description_With_Escaped_Unicode_Should_Be_Printed(string text, string expected)
    {
        var description = new GraphQLDescription(text);
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(description, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public async Task InputValueDefinition_Without_Parent_Should_Be_Printed()
    {
        var def = new GraphQLInputValueDefinition
        {
            Name = new GraphQLName("field"),
            Type = new GraphQLNamedType { Name = new GraphQLName("String") },
            DefaultValue = new GraphQLStringValue("abc")
        };

        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe("field: String = \"abc\"");
    }

    [Fact]
    public async Task Directive_Without_Parent_Should_Be_Printed()
    {
        var directive = new GraphQLDirective { Name = new GraphQLName("upper") };
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(directive, writer);
        writer.ToString().ShouldBe("@upper");
    }

    [Fact]
    public void StringBuilder_Runs_Synchronously()
    {
        var document = "KitchenSink".ReadGraphQLFile().Parse();
        var sb = new StringBuilder();
        using var writer = new StringWriter(sb);
        var printer = new SDLPrinter();
        printer.PrintAsync(document, writer).IsCompletedSuccessfully.ShouldBeTrue();
    }

    [Fact]
    public void UTF8_MemoryStream_Runs_Synchronously()
    {
        var document = "KitchenSink".ReadGraphQLFile().Parse();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms);
        var printer = new SDLPrinter();
        printer.PrintAsync(document, writer).IsCompletedSuccessfully.ShouldBeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Printer_Should_Print_DirectiveDefinition_Without_Locations(bool eachDirectiveLocationOnNewLine)
    {
        var printer = new SDLPrinter(new SDLPrinterOptions
        {
            EachDirectiveLocationOnNewLine = eachDirectiveLocationOnNewLine,
        });
        var document = new GraphQLDocument
        {
            Definitions = new List<ASTNode>
            {
                new GraphQLDirectiveDefinition
                {
                    Name = new GraphQLName("null_locations"),
                    Locations = new GraphQLDirectiveLocations() // Items is null
                },
                new GraphQLDirectiveDefinition
                {
                    Name = new GraphQLName("empty_locations"),
                    Locations = new GraphQLDirectiveLocations() { Items = new List<DirectiveLocation>() } // Items is empty
                },
                new GraphQLScalarTypeDefinition
                {
                    Name = new GraphQLName("AAA")
                }
            }
        };

        var actual = printer.Print(document);
        actual.ShouldBe("""
            directive @null_locations on

            directive @empty_locations on

            scalar AAA
            """);
    }

    [Theory]
    [InlineData("query a { name }")]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    public async Task SDLPrinter_Should_Throw_On_Unknown_Values(string text)
    {
        var writer = new StringWriter();
        var document = text.Parse();

        await new DoBadThingsVisitor().VisitAsync(document, new Context());

        var printer = new SDLPrinter();
        var ex = await Should.ThrowAsync<NotSupportedException>(async () => await printer.PrintAsync(document, writer));
        ex.Message.ShouldStartWith("Unknown ");
    }

    private class Context : IASTVisitorContext
    {
        public CancellationToken CancellationToken { get; set; }
    }

    private sealed class DoBadThingsVisitor : ASTVisitor<Context>
    {
        protected override ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, Context context)
        {
            operationDefinition.Operation = (OperationType)99;
            return base.VisitOperationDefinitionAsync(operationDefinition, context);
        }

        protected override ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, Context context)
        {
            for (int i = 0; i < directiveLocations.Items.Count; ++i)
                directiveLocations.Items[i] = (DirectiveLocation)(100 + i);

            return base.VisitDirectiveLocationsAsync(directiveLocations, context);
        }
    }
}
