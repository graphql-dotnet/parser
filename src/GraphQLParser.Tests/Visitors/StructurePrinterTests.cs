using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class StructurePrinterTests
{
    private static readonly StructurePrinter _structPrinter1 = new(new StructurePrinterOptions { PrintNames = true });
    private static readonly StructurePrinter _structPrinter2 = new(new StructurePrinterOptions { PrintNames = false });
    private static readonly StructurePrinter _structPrinter3 = new(new StructurePrinterOptions { PrintNames = true, PrintLocations = true });

    [Fact]
    public void StructurePrinter_Should_Have_Default_Options()
    {
        var writer = new StructurePrinter();
        writer.Options.ShouldNotBeNull();
        writer.Options.PrintNames.ShouldBeTrue();
        writer.Options.PrintLocations.ShouldBeFalse();
    }

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
    [InlineData(""""
"""Very good type"""
type T {
# the best field ever
field: Int }
"""", @"Document
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
    [InlineData("extend schema @dir", @"Document
  SchemaExtension
    Directives
      Directive
        Name [dir]
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
    public async Task StructurePrinter_Should_Print_Tree(string text, string expected)
    {
        using var writer = new StringWriter();
        var document = text.Parse();
        await _structPrinter1.PrintAsync(document, writer);
        var actual = writer.ToString();
        actual.ShouldBe(expected);
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
    public async Task StructurePrinter_Should_Print_Tree_Without_Names(string text, string expected)
    {
        using var writer = new StringWriter();
        var document = text.Parse();
        await _structPrinter2.PrintAsync(document, writer);
        var actual = writer.ToString();
        actual.ShouldBe(expected);
    }

    [Theory]
    //           01234567890123456789012
    [InlineData("query a { name a: age }", @"Document (0,23)
  OperationDefinition (0,23)
    Name [a] (6,7)
    SelectionSet (8,23)
      Field (10,14)
        Name [name] (10,14)
      Field (15,21)
        Alias (15,17)
          Name [a] (15,16)
        Name [age] (18,21)
")]
    //           01234567890123456789
    [InlineData("directive @a on ENUM", @"Document (0,20)
  DirectiveDefinition (0,20)
    Name [a] (11,12)
    DirectiveLocations (16,20)
")]
    //           01234567890123456789
    [InlineData("enum A { RED GREEN }", @"Document (0,20)
  EnumTypeDefinition (0,20)
    Name [A] (5,6)
    EnumValuesDefinition (7,20)
      EnumValueDefinition (9,12)
        EnumValue (9,12)
          Name [RED] (9,12)
      EnumValueDefinition (13,18)
        EnumValue (13,18)
          Name [GREEN] (13,18)
")]
    //           012345678901234567890123456789012345 67 89012345678901
    [InlineData("{f(a:10,b:true,c:3.14,d:[],e:null,f:\"!\",g:{h:ENUM})}", @"Document (0,52)
  OperationDefinition (0,52)
    SelectionSet (0,52)
      Field (1,51)
        Name [f] (1,2)
        Arguments (2,51)
          Argument (3,7)
            Name [a] (3,4)
            IntValue (5,7)
          Argument (8,14)
            Name [b] (8,9)
            BooleanValue (10,14)
          Argument (15,21)
            Name [c] (15,16)
            FloatValue (17,21)
          Argument (22,26)
            Name [d] (22,23)
            ListValue (24,26)
          Argument (27,33)
            Name [e] (27,28)
            NullValue (29,33)
          Argument (34,39)
            Name [f] (34,35)
            StringValue (36,39)
          Argument (40,50)
            Name [g] (40,41)
            ObjectValue (42,50)
              ObjectField (43,49)
                Name [h] (43,44)
                EnumValue (45,49)
                  Name [ENUM] (45,49)
")]
    //           01234567890123456789
    [InlineData("type T {f(x:Id):Int}", @"Document (0,20)
  ObjectTypeDefinition (0,20)
    Name [T] (5,6)
    FieldsDefinition (7,20)
      FieldDefinition (8,19)
        Name [f] (8,9)
        ArgumentsDefinition (9,15)
          InputValueDefinition (10,14)
            Name [x] (10,11)
            NamedType (12,14)
              Name [Id] (12,14)
        NamedType (16,19)
          Name [Int] (16,19)
")]
    //            012345678  note that document node does not include comment node, comments are "out of grammar"
    [InlineData(@"#obsolete
""obsolete!""
scalar S", @"Document (10,30)
  ScalarTypeDefinition (10,30)
    Comment (0,10)
    Description (10,21)
    Name [S] (29,30)
", false)]
    //           0123456789012
    [InlineData("scalar S @vip", @"Document (0,13)
  ScalarTypeDefinition (0,13)
    Name [S] (7,8)
    Directives (9,13)
      Directive (9,13)
        Name [vip] (10,13)
")]
    //           012345678901234567890123456
    [InlineData("interface I implements Base", @"Document (0,27)
  InterfaceTypeDefinition (0,27)
    Name [I] (10,11)
    ImplementsInterfaces (12,27)
      NamedType (23,27)
        Name [Base] (23,27)
")]
    //           0123456789012345678901234
    [InlineData("input D {x: [Int!] = [3]}", @"Document (0,25)
  InputObjectTypeDefinition (0,25)
    Name [D] (6,7)
    InputFieldsDefinition (8,25)
      InputValueDefinition (9,24)
        Name [x] (9,10)
        ListType (12,18)
          NonNullType (13,17)
            NamedType (13,16)
              Name [Int] (13,16)
        ListValue (21,24)
          IntValue (22,23)
")]
    //           012345678901234
    [InlineData("union U = A | B", @"Document (0,15)
  UnionTypeDefinition (0,15)
    Name [U] (6,7)
    UnionMemberTypes (8,15)
      NamedType (10,11)
        Name [A] (10,11)
      NamedType (14,15)
        Name [B] (14,15)
")]
    //           012345678901234567
    [InlineData("extend enum E @vip", @"Document (0,18)
  EnumTypeExtension (0,18)
    Name [E] (12,13)
    Directives (14,18)
      Directive (14,18)
        Name [vip] (15,18)
")]
    //           0123456789012345678
    [InlineData("extend input D @vip", @"Document (0,19)
  InputObjectTypeExtension (0,19)
    Name [D] (13,14)
    Directives (15,19)
      Directive (15,19)
        Name [vip] (16,19)
")]
    //           0123456789012345678
    [InlineData("extend type T @vip", @"Document (0,18)
  ObjectTypeExtension (0,18)
    Name [T] (12,13)
    Directives (14,18)
      Directive (14,18)
        Name [vip] (15,18)
")]
    //           01234567890123456789012
    [InlineData("extend interface I @vip", @"Document (0,23)
  InterfaceTypeExtension (0,23)
    Name [I] (17,18)
    Directives (19,23)
      Directive (19,23)
        Name [vip] (20,23)
")]
    //           01234567890123456789
    [InlineData("extend scalar S @vip", @"Document (0,20)
  ScalarTypeExtension (0,20)
    Name [S] (14,15)
    Directives (16,20)
      Directive (16,20)
        Name [vip] (17,20)
")]
    //           0123456789012345678
    [InlineData("extend union U @vip", @"Document (0,19)
  UnionTypeExtension (0,19)
    Name [U] (13,14)
    Directives (15,19)
      Directive (15,19)
        Name [vip] (16,19)
")]
    //           0123456789012345678
    [InlineData("schema { query: Q }", @"Document (0,19)
  SchemaDefinition (0,19)
    RootOperationTypeDefinition (9,17)
      NamedType (16,17)
        Name [Q] (16,17)
")]
    //           012345678901234567890123456
    [InlineData("fragment F on User { name }", @"Document (0,27)
  FragmentDefinition (0,27)
    FragmentName (9,10)
      Name [F] (9,10)
    TypeCondition (11,18)
      NamedType (14,18)
        Name [User] (14,18)
    SelectionSet (19,27)
      Field (21,25)
        Name [name] (21,25)
")]
    //           0123456789012345678
    [InlineData("{ ...human ... {a}}", @"Document (0,19)
  OperationDefinition (0,19)
    SelectionSet (0,19)
      FragmentSpread (2,10)
        FragmentName (5,10)
          Name [human] (5,10)
      InlineFragment (11,18)
        SelectionSet (15,18)
          Field (16,17)
            Name [a] (16,17)
")]
    //           01234567890123456789012345678901234567890
    [InlineData("mutation M($id:ID = 5) { f(a:$id, b:42) }", @"Document (0,41)
  OperationDefinition (0,41)
    Name [M] (9,10)
    VariablesDefinition (10,22)
      VariableDefinition (11,21)
        Variable (11,14)
          Name [id] (12,14)
        NamedType (15,17)
          Name [ID] (15,17)
        IntValue (20,21)
    SelectionSet (23,41)
      Field (25,39)
        Name [f] (25,26)
        Arguments (26,39)
          Argument (27,32)
            Name [a] (27,28)
            Variable (29,32)
              Name [id] (30,32)
          Argument (34,38)
            Name [b] (34,35)
            IntValue (36,38)
")]
    public async Task StructurePrinter_Should_Print_Tree_With_Locations(string text, string expected, bool ignoreComments = true)
    {
        text = text.Replace("\r\n", "\n");
        foreach (var option in new[] { IgnoreOptions.None, IgnoreOptions.Comments })
        {
            if (option == IgnoreOptions.Comments && !ignoreComments)
                continue;

            using var writer = new StringWriter();

            var document = text.Parse(new ParserOptions { Ignore = option });
            await _structPrinter3.PrintAsync(document, writer);
            var actual = writer.ToString();
            actual.ShouldBe(expected);
        }
    }
}
