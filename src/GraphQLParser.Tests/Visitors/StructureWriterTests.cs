using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class StructureWriterTests
{
    private class TestContext : IWriteContext
    {
        public TextWriter Writer { get; set; } = new StringWriter();

        public Stack<AST.ASTNode> Parents { get; set; } = new Stack<AST.ASTNode>();

        public CancellationToken CancellationToken { get; set; }
    }

    private static readonly StructureWriter<TestContext> _structWriter1 = new(new StructureWriterOptions { WriteNames = true });
    private static readonly StructureWriter<TestContext> _structWriter2 = new(new StructureWriterOptions { WriteNames = false });
    private static readonly StructureWriter<TestContext> _structWriter3 = new(new StructureWriterOptions { WriteNames = true, WriteLocations = true });

    [Theory]
    [InlineData("query a { name age }", @"Document
  OperationDefinition
    Name [a]
    SelectionSet
      Field
        Name [name]
      Field
        Name [age]
")]
    [InlineData("scalar Test", @"Document
  ScalarTypeDefinition
    Name [Test]
")]
    [InlineData("scalar JSON @exportable", @"Document
  ScalarTypeDefinition
    Name [JSON]
    Directives
      Directive
        Name [exportable]
")]
    [InlineData("{a}", @"Document
  OperationDefinition
    SelectionSet
      Field
        Name [a]
")]
    [InlineData(@"{
  a {
    b {
      c
    }
    d {
      e {
        f
      }
    }
    g {
      h {
        i {
          k
        }
      }
    }
  }
}", @"Document
  OperationDefinition
    SelectionSet
      Field
        Name [a]
        SelectionSet
          Field
            Name [b]
            SelectionSet
              Field
                Name [c]
          Field
            Name [d]
            SelectionSet
              Field
                Name [e]
                SelectionSet
                  Field
                    Name [f]
          Field
            Name [g]
            SelectionSet
              Field
                Name [h]
                SelectionSet
                  Field
                    Name [i]
                    SelectionSet
                      Field
                        Name [k]
")]
    [InlineData(@"
""""""Very good type""""""
type T {
# the best field ever
field: Int }", @"Document
  ObjectTypeDefinition
    Description
    Name [T]
    FieldsDefinition
      FieldDefinition
        Comment
        Name [field]
        NamedType
          Name [Int]
")]
    [InlineData("mutation add($a: Int!, $b: [Int]) { result }", @"Document
  OperationDefinition
    Name [add]
    VariablesDefinition
      VariableDefinition
        Variable
          Name [a]
        NonNullType
          NamedType
            Name [Int]
      VariableDefinition
        Variable
          Name [b]
        ListType
          NamedType
            Name [Int]
    SelectionSet
      Field
        Name [result]
")]
    [InlineData("type T implements I & K { f: ID }", @"Document
  ObjectTypeDefinition
    Name [T]
    ImplementsInterfaces
      NamedType
        Name [I]
      NamedType
        Name [K]
    FieldsDefinition
      FieldDefinition
        Name [f]
        NamedType
          Name [ID]
")]
    [InlineData("interface I implements K", @"Document
  InterfaceTypeDefinition
    Name [I]
    ImplementsInterfaces
      NamedType
        Name [K]
")]
    [InlineData("query { field(list: [1, null, 2], obj: { x: true }, empty: { }) }", @"Document
  OperationDefinition
    SelectionSet
      Field
        Name [field]
        Arguments
          Argument
            Name [list]
            ListValue
              IntValue
              NullValue
              IntValue
          Argument
            Name [obj]
            ObjectValue
              ObjectField
                Name [x]
                BooleanValue
          Argument
            Name [empty]
            ObjectValue
")]
    [InlineData("schema @exportable { query: Q mutation: M subscription: S }", @"Document
  SchemaDefinition
    Directives
      Directive
        Name [exportable]
    RootOperationTypeDefinition
      NamedType
        Name [Q]
    RootOperationTypeDefinition
      NamedType
        Name [M]
    RootOperationTypeDefinition
      NamedType
        Name [S]
")]
    [InlineData("union U = A | B", @"Document
  UnionTypeDefinition
    Name [U]
    UnionMemberTypes
      NamedType
        Name [A]
      NamedType
        Name [B]
")]
    [InlineData("enum Color { RED GREEN }", @"Document
  EnumTypeDefinition
    Name [Color]
    EnumValuesDefinition
      EnumValueDefinition
        EnumValue
          Name [RED]
      EnumValueDefinition
        EnumValue
          Name [GREEN]
")]
    [InlineData("input UserData { Login: String! Password: String! }", @"Document
  InputObjectTypeDefinition
    Name [UserData]
    InputFieldsDefinition
      InputValueDefinition
        Name [Login]
        NonNullType
          NamedType
            Name [String]
      InputValueDefinition
        Name [Password]
        NonNullType
          NamedType
            Name [String]
")]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT", @"Document
  DirectiveDefinition
    Name [skip]
    ArgumentsDefinition
      InputValueDefinition
        Name [if]
        NonNullType
          NamedType
            Name [Boolean]
    DirectiveLocations
")]
    [InlineData("extend scalar S @dir", @"Document
  ScalarTypeExtension
    Name [S]
    Directives
      Directive
        Name [dir]
")]
    [InlineData("extend type T @dir", @"Document
  ObjectTypeExtension
    Name [T]
    Directives
      Directive
        Name [dir]
")]
    [InlineData("extend interface I @dir", @"Document
  InterfaceTypeExtension
    Name [I]
    Directives
      Directive
        Name [dir]
")]
    [InlineData("extend union U @dir", @"Document
  UnionTypeExtension
    Name [U]
    Directives
      Directive
        Name [dir]
")]
    [InlineData("extend enum E @dir", @"Document
  EnumTypeExtension
    Name [E]
    Directives
      Directive
        Name [dir]
")]
    [InlineData("extend input P @dir", @"Document
  InputObjectTypeExtension
    Name [P]
    Directives
      Directive
        Name [dir]
")]
    public async Task WriteTreeVisitor_Should_Print_Tree(string text, string expected)
    {
        var context = new TestContext();

        using (var document = text.Parse())
        {
            await _structWriter1.Visit(document, context).ConfigureAwait(false);
            var actual = context.Writer.ToString();
            actual.ShouldBe(expected);
        }
    }

    [Theory]
    [InlineData("query a { name age }", @"Document
  OperationDefinition
    Name
    SelectionSet
      Field
        Name
      Field
        Name
")]
    public async Task WriteTreeVisitor_Should_Print_Tree_Without_Names(string text, string expected)
    {
        var context = new TestContext();

        using (var document = text.Parse())
        {
            await _structWriter2.Visit(document, context).ConfigureAwait(false);
            var actual = context.Writer.ToString();
            actual.ShouldBe(expected);
        }
    }

    [Theory]
    //           01234567890123456789
    [InlineData("query a { name age }", @"Document (0,20)
  OperationDefinition (0,20)
    Name [a] (6,7)
    SelectionSet (8,20)
      Field (10,14)
        Name [name] (10,14)
      Field (15,18)
        Name [age] (15,18)
")]
    //           01234567890123456789
    [InlineData("directive @a on ENUM", @"Document (0,20)
  DirectiveDefinition (0,20)
    Name [a] (11,12)
    DirectiveLocations (16,20)
")]
    public async Task WriteTreeVisitor_Should_Print_Tree_With_Locations(string text, string expected)
    {
        var context = new TestContext();

        using (var document = text.Parse())
        {
            await _structWriter3.Visit(document, context).ConfigureAwait(false);
            var actual = context.Writer.ToString();
            actual.ShouldBe(expected);
        }
    }
}
