using System.Text;
using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class SDLPrinterFromParsedTextTests
{
    [Fact]
    public async Task Print_Matches_PrintAsync_String()
    {
        var query = "KitchenSink".ReadGraphQLFile().Parse();
        var writer = new SDLPrinter();
        using var sw = new StringWriter();
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
  | INLINE_FRAGMENT", false, true, true)]
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
  | C", true, true, false, true)]
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
  if: Boolean!, x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT", true, true, false, false, 2, SDLPrinterArgumentsMode.None)]
    [InlineData(43,
@"directive @skip(if: Boolean!, x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT",
@"directive @skip(
  if: Boolean!,
  x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT", true, true, false, false, 2, SDLPrinterArgumentsMode.ForceNewLine)]
    [InlineData(44,
@"directive @skip(""Skipped when true."" if: Boolean!, x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT",
@"directive @skip(
  ""Skipped when true.""
  if: Boolean!,
  x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    [InlineData(45,
"""directive @skip("Skipped when true." if: Boolean!, "Second argument" x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT""", """
directive @skip(
  "Skipped when true."
  if: Boolean!,
  "Second argument"
  x: Some) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT
""")]
    [InlineData(46,
"schema { query: Q mutation: M subscription: S }",
"""
schema {
     query: Q
     mutation: M
     subscription: S
}
""", true, true, false, false, 5)]
    [InlineData(47,
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
    [InlineData(48,
"""
# comment
directive @my on FIELD
""",
"""
# comment
directive @my on FIELD
""")]
    [InlineData(49,
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
    [InlineData(50,
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
    [InlineData(51,
"""
"description"
schema {
  query: Query
}
""",
"""
"description"
schema {
  query: Query
}
""")]
    [InlineData(52,
"""
type Query {
"Fetches an object given its ID."
node(
"ID of the object."
id: ID!): Node
"Lookup nodes by a list of IDs."
nodes(
"The list of node IDs."
ids: [ID!]!): [Node]!
"Search for workspaces associated with this account."
desWorkspaces(where: DesWorkspaceFilterInput): [DesWorkspace!]!
"Search a specific workspace by its unique identifier."
desWorkspaceById(
"The node identifier for a workspace." id: ID!): DesWorkspace
}
""",
"""
type Query {
  "Fetches an object given its ID."
  node(
    "ID of the object."
    id: ID!): Node
  "Lookup nodes by a list of IDs."
  nodes(
    "The list of node IDs."
    ids: [ID!]!): [Node]!
  "Search for workspaces associated with this account."
  desWorkspaces(where: DesWorkspaceFilterInput): [DesWorkspace!]!
  "Search a specific workspace by its unique identifier."
  desWorkspaceById(
    "The node identifier for a workspace."
    id: ID!): DesWorkspace
}
""")]
    [InlineData(53,
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!
    name: Name!): Node
}
""",
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!, name: Name!): Node
}
""", true, true, false, false, 2, SDLPrinterArgumentsMode.None)]
    [InlineData(54,
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!
    name: Name!): Node
}
""",
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!,
    name: Name!): Node
}
""", true, true, false, false, 2, SDLPrinterArgumentsMode.ForceNewLine)]
    [InlineData(55,
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!
    name: Name!): Node
}
""",
"""
type Query {
  user
  # comment 1
  (
    # comment 2
    id: ID!,
    name: Name!): Node
}
""")]
    [InlineData(56,
"""
directive @my
  # comment 1
  (
    # comment 2
    arg: Boolean!) on FIELD
""",
"""
directive @my
  # comment 1
  (
    # comment 2
    arg: Boolean!) on FIELD
""")]
    [InlineData(57,
"""
query Q {
  field1(arg1: 1) {
    field2(arg2: 2) {
      field3(arg3: 3)
    }
  }
}
""",
"""
query Q {
  field1(arg1: 1) {
    field2(arg2: 2) {
      field3(arg3: 3)
    }
  }
}
""")]
    [InlineData(58,
"""
query Q {
  field1
  #comment
  (
    #comment
    arg1: 1
  ) {
    field2
    #comment
    (
      #comment
      arg2: 2
    ) {
      field3
      #comment
      (
        #comment
        arg3: 3
      )
    }
  }
}
""",
"""
query Q {
  field1
  #comment
  (
    #comment
    arg1: 1) {
    field2
    #comment
    (
      #comment
      arg2: 2) {
      field3
      #comment
      (
        #comment
        arg3: 3)
    }
  }
}
""")]
    [InlineData(59,
"""
fragment f
#comment
on Person { name }
""",
"""
fragment f
#comment
on Person {
  name
}
""")]
    [InlineData(60,
"""
type Person
#comment
implements Entity { name: String }
""",
"""
type Person
#comment
implements Entity {
  name: String
}
""")]
    [InlineData(61,
"""
type Person
#comment
implements Entity &
#comment
Entity2 { name: String }
""",
"""
type Person
#comment
implements Entity &
#comment
Entity2 {
  name: String
}
""")]
    [InlineData(62,
""""
"description"
type Person {
"""description"""
name: String }
"""",
"""
type Person {
  name: String
}
""", false, false)]
    [InlineData(63, // https://github.com/graphql-dotnet/parser/issues/330
""""
type DesPcb {
  designItems("An optional array of designators to search." designators: [String!] "Returns the first _n_ elements from the list." first: Int "Returns the elements in the list that come after the specified cursor." after: String "Returns the last _n_ elements from the list." last: Int "Returns the elements in the list that come before the specified cursor." before: String where: DesDesignItemFilterInput): DesDesignItemConnection
}
"""",
"""
type DesPcb {
  designItems(
    "An optional array of designators to search."
    designators: [String!],
    "Returns the first _n_ elements from the list."
    first: Int,
    "Returns the elements in the list that come after the specified cursor."
    after: String,
    "Returns the last _n_ elements from the list."
    last: Int,
    "Returns the elements in the list that come before the specified cursor."
    before: String,
    where: DesDesignItemFilterInput): DesDesignItemConnection
}
""")]
    public async Task SDLPrinter_Should_Print_Document(
        int number,
        string text,
        string expected,
        bool writeComments = true,
        bool writeDescriptions = true,
        bool eachDirectiveLocationOnNewLine = false,
        bool eachUnionMemberOnNewLine = false,
        int indentSize = 2,
        SDLPrinterArgumentsMode mode = SDLPrinterArgumentsMode.PreferNewLine)
    {
        var printer = new SDLPrinter(new SDLPrinterOptions
        {
            PrintComments = writeComments,
            PrintDescriptions = writeDescriptions,
            EachDirectiveLocationOnNewLine = eachDirectiveLocationOnNewLine,
            EachUnionMemberOnNewLine = eachUnionMemberOnNewLine,
            IndentSize = indentSize,
            ArgumentsPrintMode = mode,
        });
        using var writer = new StringWriter();
        var document = text.Parse();

        await printer.PrintAsync(document, writer);
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
    [InlineData(32, "t\\\"\"\"\"est\n  line2", "\nt\\\"\"\"\"est\nline2\n", true)] // BlockString with quad double quote inside
    public async Task SDLPrinter_Should_Print_BlockStrings(int number, string input, string expected, bool isBlockString)
    {
        number.ShouldBeGreaterThan(0);

        input = input.Replace("___", new string('_', 9000));
        expected = expected.Replace("___", new string('_', 9000));

        input = "\"\"\"" + input + "\"\"\"";
        expected = isBlockString
            ? "\"\"\"" + expected + "\"\"\""
            : "\"" + expected + "\"";

        using var writer = new StringWriter();

        var document = (input + " scalar a").Parse();

        var printer = new SDLPrinter();
        await printer.PrintAsync(document, writer);
        var renderedOriginal = writer.ToString();

        var lines = renderedOriginal.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        var renderedDescription = string.Join(Environment.NewLine, lines.Take(lines.Length - 1));
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
        using var writer = new StringWriter();

        var document = query.Parse();

        var printer = new SDLPrinter();
        await printer.PrintAsync(document, writer);
        var rendered = writer.ToString();
        rendered.ShouldBe(expected);

        rendered.Parse(); // should be parsed back
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
    [InlineData("{ field1 }", "{\n  field1\n}")]
    [InlineData("query { field1 }", "{\n  field1\n}")]
    [InlineData("query q1 { field1 }", "query q1 {\n  field1\n}")]
    [InlineData("mutation { field1 }", "mutation {\n  field1\n}")]
    [InlineData("mutation m1 { field1 }", "mutation m1 {\n  field1\n}")]
    public void OperationPrints(string input, string expected)
    {
        new SDLPrinter().Print(Parser.Parse(input)).ShouldBe(expected, StringCompareShould.IgnoreLineEndings);
    }

    [Theory]
    [InlineData("query a { name }")]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT")]
    public async Task SDLPrinter_Should_Throw_On_Unknown_Values(string text)
    {
        using var writer = new StringWriter();
        var document = text.Parse();

        await new DoBadThingsVisitor().VisitAsync(document, new Context());

        var printer = new SDLPrinter();
        var ex = await Should.ThrowAsync<NotSupportedException>(async () => await printer.PrintAsync(document, writer));
        ex.Message.ShouldStartWith("Unknown ");
    }

    private sealed class Context : IASTVisitorContext
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
