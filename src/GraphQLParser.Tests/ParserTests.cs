using System;
using System.Linq;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "Tests")]
    public class ParserTests
    {
        private static readonly string _nl = Environment.NewLine;

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Extra_Comments_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "ExtraComments".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            // query
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(2);
            // person
            var field = def.SelectionSet.Selections.First() as GraphQLField;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            // name
            var subField = field.SelectionSet.Selections.First() as GraphQLField;
            subField.Comment.ShouldBeNull();
            // test
            field = def.SelectionSet.Selections.Last() as GraphQLField;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            field.Comment.ShouldNotBeNull().Text.ShouldBe("comment2");
            // alt
            subField = field.SelectionSet.Selections.First() as GraphQLField;
            subField.Comment.ShouldBeNull();
            // extra document comments
            document.UnattachedComments.Count.ShouldBe(3);
            document.UnattachedComments[0].Text.ShouldBe("comment1");
            document.UnattachedComments[1].Text.ShouldBe("comment3");
            document.UnattachedComments[2].Text.ShouldBe("comment4");
        }

        [Theory]
        //[InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        //[InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Comments_Can_Be_Ignored(IgnoreOptions options)
        {
            const string query = @"
{
    #comment
    person
    # comment2
}";

            var document = query.Parse(new ParserOptions { Ignore = options });
            document.UnattachedComments.ShouldBeNull();
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            def.Comment.ShouldBeNull();
            var field = def.SelectionSet.Selections.First() as GraphQLField;
            field.Comment.ShouldBeNull();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_FragmentSpread_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnFragmentSpread".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(2);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLField;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLFragmentSpread;
            fragment.Comment.ShouldNotBeNull().Text.ShouldBe("comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_Values_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnValues".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLField;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            field.Arguments.Count.ShouldBe(9);

            var boolValue = field.Arguments[0].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            boolValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for bool");

            var nullValue = field.Arguments[1].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            nullValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for null");

            var enumValue = field.Arguments[2].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            enumValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for enum");

            var listValue = field.Arguments[3].Value.ShouldBeAssignableTo<GraphQLListValue>();
            listValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for list");

            var objValue = field.Arguments[4].Value.ShouldBeAssignableTo<GraphQLObjectValue>();
            objValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for object");

            var intValue = field.Arguments[5].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            intValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for int");

            var floatValue = field.Arguments[6].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            floatValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for float");

            var stringValue = field.Arguments[7].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            stringValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for string");

            var varValue = field.Arguments[8].Value.ShouldBeAssignableTo<GraphQLVariable>();
            varValue.Comment.ShouldNotBeNull().Text.ShouldBe("comment for variable");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_FragmentInline_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnInlineFragment".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLField;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLInlineFragment;
            fragment.Comment.ShouldNotBeNull().Text.ShouldBe("comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_NamedTypes_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnNamedType".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(5);

            var def1 = document.Definitions[0] as GraphQLOperationDefinition;
            var field = def1.SelectionSet.Selections[0] as GraphQLField;
            var frag = field.SelectionSet.Selections[0] as GraphQLInlineFragment;
            frag.TypeCondition.Type.Comment.Text.ShouldBe("comment for named type from TypeCondition");

            var def2 = document.Definitions[1] as GraphQLObjectTypeDefinition;
            def2.Interfaces[0].Comment.Text.ShouldBe("comment for named type from ImplementsInterfaces");

            var def3 = document.Definitions[2] as GraphQLSchemaDefinition;
            def3.OperationTypes[0].Type.Comment.Text.ShouldBe("comment for named type from RootOperationTypeDefinition");

            var def4 = document.Definitions[3] as GraphQLObjectTypeDefinition;
            def4.Fields[0].Type.Comment.Text.ShouldBe("comment for named type from Type");

            var def5 = document.Definitions[4] as GraphQLUnionTypeDefinition;
            def5.Types[1].Comment.Text.ShouldBe("comment for named type from UnionMemberTypes");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_SelectionSet_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnSelectionSet".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0] as GraphQLOperationDefinition;
            def.SelectionSet.Comment.Text.ShouldBe("comment on selection set");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_RootOperationType_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnRootOperationType".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0] as GraphQLSchemaDefinition;
            def.OperationTypes[0].Comment.Text.ShouldBe("comment for root operation type");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_Directive_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnDirective".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0] as GraphQLScalarTypeDefinition;
            def.Directives[0].Comment.Text.ShouldBe("comment for directive");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_Type_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnType".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0] as GraphQLObjectTypeDefinition;
            def.Fields[0].Type.Comment.Text.ShouldBe("comment for named type");
            def.Fields[1].Type.Comment.Text.ShouldBe("comment for nonnull type");
            def.Fields[2].Type.Comment.Text.ShouldBe("comment for list type");
            (def.Fields[2].Type as GraphQLListType).Type.Comment.Text.ShouldBe("comment for item type");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_Alias_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnAlias".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0] as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections[0].ShouldBeAssignableTo<GraphQLField>();
            field.Name.Value.ShouldBe("name");
            field.Comment.Text.ShouldBe("field comment");
            field.Alias.Name.Value.ShouldBe("a");
            field.Alias.Comment.Text.ShouldBe("alias comment");

            document.UnattachedComments.Count.ShouldBe(1);
            document.UnattachedComments[0].Text.ShouldBe("colon comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_on_Variable_Should_Read_Correctly(IgnoreOptions options)
        {
            string query = "CommentsOnVariables".ReadGraphQLFile();

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.Variables.Count.ShouldBe(3);
            def.Variables.First().Comment.ShouldNotBeNull().Text.ShouldBe("comment1");
            def.Variables.Skip(1).First().Comment.ShouldBeNull();
            def.Variables.Skip(2).First().Comment.ShouldNotBeNull().Text.ShouldBe("comment3");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_On_SelectionSet_Should_Read_Correctly(IgnoreOptions options)
        {
            using var document = @"
query {
    # a comment below query
    field1
    field2
    #second comment
    field3
}
".Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(3);
            def.SelectionSet.Selections.First().Comment.ShouldNotBeNull().Text.ShouldBe(" a comment below query");
            def.SelectionSet.Selections.Skip(1).First().Comment.ShouldBe(null);
            def.SelectionSet.Selections.Skip(2).First().Comment.ShouldNotBeNull().Text.ShouldBe("second comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Comments_On_Enum_Definitions_Should_Read_Correctly(IgnoreOptions options)
        {
            using var document = @"
# different animals
enum Animal {
    #a cat
    Cat
    #a dog
    Dog
    Octopus
    #bird is the word
    Bird
}

input Parameter {
    #any value
    Value: String
}

scalar JSON
".Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(3);
            var d1 = document.Definitions.First() as GraphQLEnumTypeDefinition;
            d1.Name.Value.ShouldBe("Animal");
            d1.Comment.ShouldNotBeNull().Text.ShouldBe(" different animals");
            d1.Values.First().Name.Value.ShouldBe("Cat");
            d1.Values.First().Comment.ShouldNotBeNull();
            d1.Values.First().Comment.Text.ShouldBe("a cat");
            d1.Values.Skip(2).First().Name.Value.ShouldBe("Octopus");
            d1.Values.Skip(2).First().Comment.ShouldBeNull();

            var d2 = document.Definitions.Skip(1).First() as GraphQLInputObjectTypeDefinition;
            d2.Name.Value.ShouldBe("Parameter");
            d2.Comment.ShouldBeNull();
            d2.Fields.Count.ShouldBe(1);
            d2.Fields.First().Comment.Text.ShouldBe("any value");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Unicode_Char_At_EOF_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{\"\\ue }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        //[InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_HasCorrectLocations(IgnoreOptions options)
        {
            // { field }
            using var document = ParseGraphQLFieldSource(options);

            document.Location.ShouldBe(new GraphQLLocation(0, 9)); // { field }
            document.Definitions.First().Location.ShouldBe(new GraphQLLocation(0, 9)); // { field }
            (document.Definitions.First() as GraphQLOperationDefinition).SelectionSet.Location.ShouldBe(new GraphQLLocation(0, 9)); // { field }
            (document.Definitions.First() as GraphQLOperationDefinition).SelectionSet.Selections.First().Location.ShouldBe(new GraphQLLocation(2, 7)); // field
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_HasOneOperationDefinition(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            document.Definitions.First().Kind.ShouldBe(ASTNodeKind.OperationDefinition);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_NameIsNull(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleOperationDefinition(document).Name.ShouldBeNull();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_OperationIsQuery(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleOperationDefinition(document).Operation.ShouldBe(OperationType.Query);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_ReturnsDocumentNode(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            document.Kind.ShouldBe(ASTNodeKind.Document);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldInput_SelectionSetContainsSingleField(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleSelection(document).Kind.ShouldBe(ASTNodeKind.Field);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        //[InlineData(IgnoreOptions.Locations)]
        //[InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_HasCorrectLocations(IgnoreOptions options)
        {
            // mutation Foo { field }
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            document.Location.ShouldBe(new GraphQLLocation(0, 22));
            document.Definitions.First().Location.ShouldBe(new GraphQLLocation(0, 22));
            (document.Definitions.First() as GraphQLOperationDefinition).Name.Location.ShouldBe(new GraphQLLocation(9, 12)); // Foo
            (document.Definitions.First() as GraphQLOperationDefinition).SelectionSet.Location.ShouldBe(new GraphQLLocation(13, 22)); // { field }
            (document.Definitions.First() as GraphQLOperationDefinition).SelectionSet.Selections.First().Location.ShouldBe(new GraphQLLocation(15, 20)); // field
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_HasOneOperationDefinition(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            document.Definitions.First().Kind.ShouldBe(ASTNodeKind.OperationDefinition);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_NameIsNull(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleOperationDefinition(document).Name.Value.ShouldBe("Foo");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_OperationIsQuery(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleOperationDefinition(document).Operation.ShouldBe(OperationType.Mutation);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_ReturnsDocumentNode(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            document.Kind.ShouldBe(ASTNodeKind.Document);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_FieldWithOperationTypeAndNameInput_SelectionSetContainsSingleFieldWithOperationTypeAndNameSelection(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleSelection(document).Kind.ShouldBe(ASTNodeKind.Field);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_KitchenSink_DoesNotThrowError(IgnoreOptions options)
        {
            using var document = "KitchenSink".ReadGraphQLFile().Parse(new ParserOptions { Ignore = options });
            var typeDef = document.Definitions.OfType<GraphQLObjectTypeDefinition>().First(d => d.Name.Value == "Foo");
            var fieldDef = typeDef.Fields.First(d => d.Name.Value == "three");
            if (options.HasFlag(IgnoreOptions.Comments))
                fieldDef.Comment.ShouldBeNull();
            else
                fieldDef.Comment.ShouldNotBeNull().Text.ShouldBe($" multiline comments{_nl} with very importand description #{_nl} # and symbol # and ##");

            // Schema description
            // https://github.com/graphql/graphql-spec/pull/466
            var comment = document.Definitions.OfType<GraphQLSchemaDefinition>().First().Comment;
            if (options.HasFlag(IgnoreOptions.Comments))
            {
                comment.ShouldBeNull();
            }
            else
            {
                comment.ShouldNotBeNull();
                ((string)comment.Text).StartsWith("﻿ Copyright (c) 2015, Facebook, Inc.").ShouldBeTrue();
            }
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_NullInput_EmptyDocument(IgnoreOptions options)
        {
            using var document = ((string)null).Parse(new ParserOptions { Ignore = options });

            document.Definitions.ShouldBeEmpty();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_VariableInlineValues_DoesNotThrowError(IgnoreOptions options)
        {
            using ("{ field(complex: { a: { b: [ $var ] } }) }".Parse(new ParserOptions { Ignore = options }))
            {
            }
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Should_Read_Directives_on_VariableDefinition(IgnoreOptions options)
        {
            using (var document = "query A($id: String @a @b(priority: 1, managed: true)) { name }".Parse(new ParserOptions { Ignore = options }))
            {
                document.Definitions.Count.ShouldBe(1);
                var def = document.Definitions[0].ShouldBeAssignableTo<GraphQLOperationDefinition>();
                def.Variables.Count.ShouldBe(1);
                def.Variables[0].Directives.Count.ShouldBe(2);
                def.Variables[0].Directives[0].Name.Value.ShouldBe("a");
                def.Variables[0].Directives[1].Name.Value.ShouldBe("b");
                def.Variables[0].Directives[1].Arguments.Count.ShouldBe(2);
            }
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Empty_Field_Arguments_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{ a() }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Empty_Directive_Arguments_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "directive @dir() on FIELD_DEFINITION".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Empty_Enum_Values_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "enum Empty { }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Empty_SelectionSet_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{ a { } }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Parse_Empty_VariableDefinitions_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "query test() { a }".Parse(new ParserOptions { Ignore = options }));
        }

        private static GraphQLOperationDefinition GetSingleOperationDefinition(GraphQLDocument document)
        {
            return (GraphQLOperationDefinition)document.Definitions.Single();
        }

        private static ASTNode GetSingleSelection(GraphQLDocument document)
        {
            return GetSingleOperationDefinition(document).SelectionSet.Selections.Single();
        }

        private static GraphQLDocument ParseGraphQLFieldSource(IgnoreOptions options) => "{ field }".Parse(new ParserOptions { Ignore = options });

        private static GraphQLDocument ParseGraphQLFieldWithOperationTypeAndNameSource(IgnoreOptions options) => "mutation Foo { field }".Parse(new ParserOptions { Ignore = options });

        [Theory]
        [InlineData("directive @dir repeatable on FIELD_DEFINITION", true)]
        [InlineData("directive @dir(a: Int) repeatable on FIELD_DEFINITION", true)]
        [InlineData("directive @dir on FIELD_DEFINITION | ENUM_VALUE", false)]
        [InlineData("directive @dir on | FIELD_DEFINITION | ENUM_VALUE", false)]
        [InlineData(@"directive @dir on
FIELD_DEFINITION | ENUM_VALUE", false)]
        [InlineData(@"directive @dir on
FIELD_DEFINITION
| ENUM_VALUE", false)]
        [InlineData(@"directive @dir on
| FIELD_DEFINITION
| ENUM_VALUE", false)]
        [InlineData(@"directive @dir on
|  FIELD_DEFINITION
|          ENUM_VALUE", false)]
        public void Should_Parse_Directives(string text, bool repeatable)
        {
            using var document = text.Parse();
            document.ShouldNotBeNull();
            document.Definitions.Count.ShouldBe(1);
            document.Definitions[0].ShouldBeAssignableTo<GraphQLDirectiveDefinition>().Repeatable.ShouldBe(repeatable);
        }

        // http://spec.graphql.org/October2021/#sec--specifiedBy
        [Fact]
        public void Should_Parse_SpecifiedBy()
        {
            string text = @"scalar UUID @specifiedBy(url: ""https://tools.ietf.org/html/rfc4122"")";
            using var document = text.Parse();
            document.ShouldNotBeNull();
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions[0].ShouldBeAssignableTo<GraphQLScalarTypeDefinition>();
            def.Directives.Count.ShouldBe(1);
            def.Directives[0].Name.Value.ShouldBe("specifiedBy");
            def.Directives[0].Arguments.Count.ShouldBe(1);
            def.Directives[0].Arguments[0].Name.Value.ShouldBe("url");
            var value = def.Directives[0].Arguments[0].Value.ShouldBeAssignableTo<GraphQLScalarValue>();
            value.Value.ShouldBe("https://tools.ietf.org/html/rfc4122");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Should_Fail_On_Empty_Fields(IgnoreOptions options)
        {
            string text = "interface Dog { }";
            Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse(new ParserOptions { Ignore = options })).Message.ShouldContain("Expected Name, found }");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Should_Parse_Interfaces_Implemented_By_Interface(IgnoreOptions options)
        {
            string text = "InterfacesOnInterface".ReadGraphQLFile();

            using var document = text.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(4);
            var def1 = document.Definitions[0].ShouldBeAssignableTo<GraphQLInterfaceTypeDefinition>();
            def1.Name.Value.ShouldBe("Dog");
            def1.Interfaces.ShouldBeNull();

            var def2 = document.Definitions[1].ShouldBeAssignableTo<GraphQLInterfaceTypeDefinition>();
            def2.Name.Value.ShouldBe("Dog");
            def2.Interfaces.Count.ShouldBe(1);
            def2.Interfaces[0].Name.Value.ShouldBe("Eat");

            var def3 = document.Definitions[2].ShouldBeAssignableTo<GraphQLInterfaceTypeDefinition>();
            def3.Name.Value.ShouldBe("Dog");
            def3.Interfaces.Count.ShouldBe(2);
            def3.Interfaces[0].Name.Value.ShouldBe("Eat");
            def3.Interfaces[1].Name.Value.ShouldBe("Sleep");

            var def4 = document.Definitions[3].ShouldBeAssignableTo<GraphQLInterfaceTypeDefinition>();
            def4.Name.Value.ShouldBe("Dog");
            def4.Interfaces.Count.ShouldBe(2);
            def4.Interfaces[0].Name.Value.ShouldBe("Eat");
            def4.Interfaces[1].Name.Value.ShouldBe("Sleep");
            def4.Fields.Count.ShouldBe(1);
            def4.Fields[0].Name.Value.ShouldBe("name");
        }

        [Theory]
        [InlineData("directive @dir On FIELD_DEFINITION")]
        [InlineData("directive @dir onn FIELD_DEFINITION")]
        [InlineData("directive @dir Repeatable on FIELD_DEFINITION")]
        [InlineData("directive @dir repeatablee on FIELD_DEFINITION")]
        [InlineData("directive @dir repeatable On FIELD_DEFINITION")]
        [InlineData("directive @dir repeatable onn FIELD_DEFINITION")]
        public void Should_Throw_GraphQLSyntaxErrorException(string text)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        }

        [Theory]
        [InlineData("union Animal = Cat | Dog")]
        [InlineData("union Animal = | Cat | Dog")]
        [InlineData(@"union Animal =
Cat | Dog")]
        [InlineData(@"union Animal =
Cat
| Dog")]
        [InlineData(@"union Animal =
| Cat
| Dog")]
        [InlineData(@"union Animal =   
|  Cat
|       Dog")]
        public void Should_Parse_Unions(string text)
        {
            using var document = text.Parse();
            document.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("extend scalar Foo @exportable", ASTNodeKind.ScalarTypeExtension)]
        [InlineData("extend type Foo implements Bar @exportable { a: String }", ASTNodeKind.ObjectTypeExtension)]
        [InlineData("extend type Foo implements Bar @exportable", ASTNodeKind.ObjectTypeExtension)]
        [InlineData("extend type Foo implements Bar { a: String }", ASTNodeKind.ObjectTypeExtension)]
        [InlineData("extend type Foo implements Bar", ASTNodeKind.ObjectTypeExtension)]
        [InlineData("extend type Foo { a: String }", ASTNodeKind.ObjectTypeExtension)]
        [InlineData("extend interface Foo implements Bar @exportable { a: String }", ASTNodeKind.InterfaceTypeExtension)]
        [InlineData("extend interface Foo implements Bar @exportable", ASTNodeKind.InterfaceTypeExtension)]
        [InlineData("extend interface Foo implements Bar { a: String }", ASTNodeKind.InterfaceTypeExtension)]
        [InlineData("extend interface Foo { a: String }", ASTNodeKind.InterfaceTypeExtension)]
        [InlineData("extend interface Foo implements Bar", ASTNodeKind.InterfaceTypeExtension)]
        [InlineData("extend union Foo @exportable = A | B", ASTNodeKind.UnionTypeExtension)]
        [InlineData("extend union Foo = A | B", ASTNodeKind.UnionTypeExtension)]
        [InlineData("extend union Foo @exportable", ASTNodeKind.UnionTypeExtension)]
        [InlineData("extend enum Foo @exportable { ONE TWO }", ASTNodeKind.EnumTypeExtension)]
        [InlineData("extend enum Foo { ONE TWO }", ASTNodeKind.EnumTypeExtension)]
        [InlineData("extend enum Foo @exportable", ASTNodeKind.EnumTypeExtension)]
        [InlineData("extend input Foo @exportable { a: String }", ASTNodeKind.InputObjectTypeExtension)]
        [InlineData("extend input Foo { a: String }", ASTNodeKind.InputObjectTypeExtension)]
        [InlineData("extend input Foo @exportable", ASTNodeKind.InputObjectTypeExtension)]
        public void Should_Parse_Extensions(string text, ASTNodeKind kind)
        {
            using var document = text.Parse();
            document.ShouldNotBeNull();
            document.Definitions[0].Kind.ShouldBe(kind);
        }

        [Theory]
        [InlineData("extend")]
        [InlineData("extend scalar")]
        [InlineData("extend scalar A")]
        [InlineData("extend scalar A B")]
        [InlineData("extend type")]
        [InlineData("extend type A")]
        [InlineData("extend type A B")]
        [InlineData("extend interface")]
        [InlineData("extend interface A")]
        [InlineData("extend interface A B")]
        [InlineData("extend union")]
        [InlineData("extend union A")]
        [InlineData("extend enum")]
        [InlineData("extend enum A")]
        [InlineData("extend input")]
        [InlineData("extend input A")]
        [InlineData("extend variable")]
        public void Should_Throw_Extensions(string text)
        {
            var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        }

        [Theory]
        [InlineData("scalar Empty", ASTNodeKind.ScalarTypeDefinition)]
        [InlineData("union Empty", ASTNodeKind.UnionTypeDefinition)]
        [InlineData("type Empty", ASTNodeKind.ObjectTypeDefinition)]
        [InlineData("input Empty", ASTNodeKind.InputObjectTypeDefinition)]
        [InlineData("interface Empty", ASTNodeKind.InterfaceTypeDefinition)]
        [InlineData("enum Empty", ASTNodeKind.EnumTypeDefinition)]
        public void Should_Parse_Empty_Types(string text, ASTNodeKind kind)
        {
            using var document = text.Parse();
            document.ShouldNotBeNull();
            document.Definitions[0].Kind.ShouldBe(kind);
        }

        [Theory]
        [InlineData("type Query { }", 1, 14)]
        [InlineData("extend type Query { }", 1, 21)]
        [InlineData("input Empty { }", 1, 15)]
        [InlineData("interface Empty { }", 1, 19)]
        [InlineData("enum Empty { }", 1, 14)]
        [InlineData("extend type Type implements Interface { }", 1, 41)]
        public void Should_Throw_On_Empty_Types_With_Braces(string text, int line, int column)
        {
            var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
            ex.Line.ShouldBe(line);
            ex.Column.ShouldBe(column);
            ex.Message.ShouldContain("Expected Name, found }");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Descriptions_Should_Read_Correctly(IgnoreOptions options)
        {
            using var document = @"
""Super schema""
schema {
  query: String
}

""A JSON scalar""
scalar JSON

""""""
Human type
""""""
type Human {
  """"""
  Name of human
  """"""
  name: String

  ""Test""
  test(
    ""desc""
    arg: Int
  ): Int
}

""Test interface""
interface TestInterface {
  ""Object name""
  name: String
}

""""""
Test union
""""""
union TestUnion = Test1 | Test2

""Example enum""
enum Colors {
  ""Red"" RED
  ""Blue"" BLUE
}

""""""
This is an example input object
Line two of the description
""""""
input TestInputObject {
    """"""
    The value of the input object
      (any JSON value is accepted)
    """"""
    Value: JSON
}

""Test directive""
directive @TestDirective (
  ""Example""
  Value: Int
) on QUERY
".Parse(new ParserOptions { Ignore = options });
            var defs = document.Definitions;
            defs.Count.ShouldBe(8);

            var schemaDef = defs.Single(x => x is GraphQLSchemaDefinition) as GraphQLSchemaDefinition;
            schemaDef.Description.Value.ShouldBe("Super schema");

            var scalarDef = defs.Single(x => x is GraphQLScalarTypeDefinition) as GraphQLScalarTypeDefinition;
            scalarDef.Name.Value.ShouldBe("JSON");
            scalarDef.Description.Value.ShouldBe("A JSON scalar");

            var objectDef = defs.Single(x => x is GraphQLObjectTypeDefinition) as GraphQLObjectTypeDefinition;
            objectDef.Name.Value.ShouldBe("Human");
            objectDef.Description.Value.ShouldBe("Human type");
            objectDef.Fields.Count.ShouldBe(2);
            objectDef.Fields[0].Name.Value.ShouldBe("name");
            objectDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            objectDef.Fields[0].Description.Value.ShouldBe("Name of human");
            objectDef.Fields[1].Name.Value.ShouldBe("test");
            objectDef.Fields[1].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            objectDef.Fields[1].Description.Value.ShouldBe("Test");
            objectDef.Fields[1].Arguments.Count.ShouldBe(1);
            objectDef.Fields[1].Arguments[0].Name.Value.ShouldBe("arg");
            objectDef.Fields[1].Arguments[0].Description.Value.ShouldBe("desc");
            objectDef.Fields[1].Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");

            var interfaceDef = defs.Single(x => x is GraphQLInterfaceTypeDefinition) as GraphQLInterfaceTypeDefinition;
            interfaceDef.Name.Value.ShouldBe("TestInterface");
            interfaceDef.Description.Value.ShouldBe("Test interface");
            interfaceDef.Fields.Count.ShouldBe(1);
            interfaceDef.Fields[0].Name.Value.ShouldBe("name");
            interfaceDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            interfaceDef.Fields[0].Description.Value.ShouldBe("Object name");

            var unionDef = defs.Single(x => x is GraphQLUnionTypeDefinition) as GraphQLUnionTypeDefinition;
            unionDef.Name.Value.ShouldBe("TestUnion");
            unionDef.Description.Value.ShouldBe("Test union");
            unionDef.Types.Count.ShouldBe(2);
            unionDef.Types[0].Name.Value.ShouldBe("Test1");
            unionDef.Types[1].Name.Value.ShouldBe("Test2");

            var enumDef = defs.Single(x => x is GraphQLEnumTypeDefinition) as GraphQLEnumTypeDefinition;
            enumDef.Name.Value.ShouldBe("Colors");
            enumDef.Description.Value.ShouldBe("Example enum");
            enumDef.Values.Count.ShouldBe(2);
            enumDef.Values[0].Name.Value.ShouldBe("RED");
            enumDef.Values[0].Description.Value.ShouldBe("Red");
            enumDef.Values[1].Name.Value.ShouldBe("BLUE");
            enumDef.Values[1].Description.Value.ShouldBe("Blue");

            var inputDef = defs.Single(x => x is GraphQLInputObjectTypeDefinition) as GraphQLInputObjectTypeDefinition;
            inputDef.Name.Value.ShouldBe("TestInputObject");
            inputDef.Description.Value.ShouldBe("This is an example input object\nLine two of the description");
            inputDef.Fields.Count.ShouldBe(1);
            inputDef.Fields[0].Name.Value.ShouldBe("Value");
            inputDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("JSON");
            inputDef.Fields[0].Description.Value.ShouldBe("The value of the input object\n  (any JSON value is accepted)");

            var directiveDef = defs.Single(x => x is GraphQLDirectiveDefinition) as GraphQLDirectiveDefinition;
            directiveDef.Name.Value.ShouldBe("TestDirective");
            directiveDef.Description.Value.ShouldBe("Test directive");
            directiveDef.Arguments.Count.ShouldBe(1);
            directiveDef.Arguments[0].Name.Value.ShouldBe("Value");
            directiveDef.Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            directiveDef.Arguments[0].Description.Value.ShouldBe("Example");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Descriptions_WithComments_Should_Read_Correctly_1(IgnoreOptions options)
        {
            using var document = @"
# comment -1
""Super schema""
# comment 0
schema {
  query: String
}

# comment 1
""A JSON scalar""
# comment 2
scalar JSON

# comment 3
""""""
Human type
""""""
# comment 4
type Human {
  # comment 5
  """"""
  Name of human
  """"""
  # comment 6
  name: String

  # comment 7
  ""Test""
  # comment 8
  test(
    # comment 9
    ""desc""
    # comment 10
    arg: Int
  ): Int
}

# comment 11
""Test interface""
# comment 12
interface TestInterface {
  # comment 13
  ""Object name""
  # comment 14
  name: String
}

# comment 15
""""""
Test union
""""""
# comment 16
union TestUnion = Test1 | Test2

# comment 17
""Example enum""
# comment 18
enum Colors {
  # comment 19
  ""Red""
  # comment 20
  RED
  # comment 21
  ""Blue""
  # comment 22
  BLUE
}

# comment 23
""""""
This is an example input object
Line two of the description
""""""
# comment 24
input TestInputObject {
    # comment 25
    """"""
    The value of the input object
      (any JSON value is accepted)
    """"""
    # comment 26
    Value: JSON
}

# comment 27
""Test directive""
# comment 28
directive @TestDirective (
  # comment 29
  ""Example""
  # comment 30
  Value: Int
) on QUERY
".Parse(new ParserOptions { Ignore = options });
            var defs = document.Definitions;
            defs.Count.ShouldBe(8);
            var parseComments = !options.HasFlag(IgnoreOptions.Comments);

            var schemaDef = defs.Single(x => x is GraphQLSchemaDefinition) as GraphQLSchemaDefinition;
            schemaDef.Description.Value.ShouldBe("Super schema");
            if (parseComments)
                schemaDef.Comment.Text.ShouldBe(" comment 0");

            var scalarDef = defs.Single(x => x is GraphQLScalarTypeDefinition) as GraphQLScalarTypeDefinition;
            scalarDef.Name.Value.ShouldBe("JSON");
            scalarDef.Description.Value.ShouldBe("A JSON scalar");
            if (parseComments)
                scalarDef.Comment.Text.ShouldBe(" comment 2");

            var objectDef = defs.Single(x => x is GraphQLObjectTypeDefinition) as GraphQLObjectTypeDefinition;
            objectDef.Name.Value.ShouldBe("Human");
            objectDef.Description.Value.ShouldBe("Human type");
            if (parseComments)
                objectDef.Comment.Text.ShouldBe(" comment 4");
            objectDef.Fields.Count.ShouldBe(2);
            objectDef.Fields[0].Name.Value.ShouldBe("name");
            objectDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            objectDef.Fields[0].Description.Value.ShouldBe("Name of human");
            if (parseComments)
                objectDef.Fields[0].Comment.Text.ShouldBe(" comment 6");
            objectDef.Fields[1].Name.Value.ShouldBe("test");
            objectDef.Fields[1].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            objectDef.Fields[1].Description.Value.ShouldBe("Test");
            if (parseComments)
                objectDef.Fields[1].Comment.Text.ShouldBe(" comment 8");
            objectDef.Fields[1].Arguments.Count.ShouldBe(1);
            objectDef.Fields[1].Arguments[0].Name.Value.ShouldBe("arg");
            objectDef.Fields[1].Arguments[0].Description.Value.ShouldBe("desc");
            objectDef.Fields[1].Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            if (parseComments)
                objectDef.Fields[1].Arguments[0].Comment.Text.ShouldBe(" comment 10");

            var interfaceDef = defs.Single(x => x is GraphQLInterfaceTypeDefinition) as GraphQLInterfaceTypeDefinition;
            interfaceDef.Name.Value.ShouldBe("TestInterface");
            interfaceDef.Description.Value.ShouldBe("Test interface");
            if (parseComments)
                interfaceDef.Comment.Text.ShouldBe(" comment 12");
            interfaceDef.Fields.Count.ShouldBe(1);
            interfaceDef.Fields[0].Name.Value.ShouldBe("name");
            interfaceDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            interfaceDef.Fields[0].Description.Value.ShouldBe("Object name");
            if (parseComments)
                interfaceDef.Fields[0].Comment.Text.ShouldBe(" comment 14");

            var unionDef = defs.Single(x => x is GraphQLUnionTypeDefinition) as GraphQLUnionTypeDefinition;
            unionDef.Name.Value.ShouldBe("TestUnion");
            unionDef.Description.Value.ShouldBe("Test union");
            if (parseComments)
                unionDef.Comment.Text.ShouldBe(" comment 16");
            unionDef.Types.Count.ShouldBe(2);
            unionDef.Types[0].Name.Value.ShouldBe("Test1");
            unionDef.Types[1].Name.Value.ShouldBe("Test2");

            var enumDef = defs.Single(x => x is GraphQLEnumTypeDefinition) as GraphQLEnumTypeDefinition;
            enumDef.Name.Value.ShouldBe("Colors");
            enumDef.Description.Value.ShouldBe("Example enum");
            if (parseComments)
                enumDef.Comment.Text.ShouldBe(" comment 18");
            enumDef.Values.Count.ShouldBe(2);
            enumDef.Values[0].Name.Value.ShouldBe("RED");
            enumDef.Values[0].Description.Value.ShouldBe("Red");
            if (parseComments)
                enumDef.Values[0].Comment.Text.ShouldBe(" comment 20");
            enumDef.Values[1].Name.Value.ShouldBe("BLUE");
            enumDef.Values[1].Description.Value.ShouldBe("Blue");
            if (parseComments)
                enumDef.Values[1].Comment.Text.ShouldBe(" comment 22");

            var inputDef = defs.Single(x => x is GraphQLInputObjectTypeDefinition) as GraphQLInputObjectTypeDefinition;
            inputDef.Name.Value.ShouldBe("TestInputObject");
            inputDef.Description.Value.ShouldBe("This is an example input object\nLine two of the description");
            if (parseComments)
                inputDef.Comment.Text.ShouldBe(" comment 24");
            inputDef.Fields.Count.ShouldBe(1);
            inputDef.Fields[0].Name.Value.ShouldBe("Value");
            inputDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("JSON");
            inputDef.Fields[0].Description.Value.ShouldBe("The value of the input object\n  (any JSON value is accepted)");
            if (parseComments)
                inputDef.Fields[0].Comment.Text.ShouldBe(" comment 26");

            var directiveDef = defs.Single(x => x is GraphQLDirectiveDefinition) as GraphQLDirectiveDefinition;
            directiveDef.Name.Value.ShouldBe("TestDirective");
            directiveDef.Description.Value.ShouldBe("Test directive");
            if (parseComments)
                directiveDef.Comment.Text.ShouldBe(" comment 28");
            directiveDef.Arguments.Count.ShouldBe(1);
            directiveDef.Arguments[0].Name.Value.ShouldBe("Value");
            directiveDef.Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            directiveDef.Arguments[0].Description.Value.ShouldBe("Example");
            if (parseComments)
                directiveDef.Arguments[0].Comment.Text.ShouldBe(" comment 30");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Descriptions_WithComments_Should_Read_Correctly_2(IgnoreOptions options)
        {
            using var document = @"
""Super schema""
# comment 1
schema {
  query: String
}

""A JSON scalar""
# comment 2
scalar JSON

""""""
Human type
""""""
# comment 4
type Human {
  """"""
  Name of human
  """"""
  # comment 6
  name: String

  ""Test""
  # comment 8
  test(
    ""desc""
    # comment 10
    arg: Int
  ): Int
}

""Test interface""
# comment 12
interface TestInterface {
  ""Object name""
  # comment 14
  name: String
}

""""""
Test union
""""""
# comment 16
union TestUnion = Test1 | Test2

""Example enum""
# comment 18
enum Colors {
  ""Red""
  # comment 20
  RED
  ""Blue""
  # comment 22
  BLUE
}

""""""
This is an example input object
Line two of the description
""""""
# comment 24
input TestInputObject {
    """"""
    The value of the input object
      (any JSON value is accepted)
    """"""
    # comment 26
    Value: JSON
}

""Test directive""
# comment 28
directive @TestDirective (
  ""Example""
  # comment 30
  Value: Int
) on QUERY
".Parse(new ParserOptions { Ignore = options });
            var defs = document.Definitions;
            defs.Count.ShouldBe(8);
            var parseComments = !options.HasFlag(IgnoreOptions.Comments);

            var schemaDef = defs.Single(x => x is GraphQLSchemaDefinition) as GraphQLSchemaDefinition;
            schemaDef.Description.Value.ShouldBe("Super schema");
            if (parseComments)
                schemaDef.Comment.Text.ShouldBe(" comment 1");

            var scalarDef = defs.Single(x => x is GraphQLScalarTypeDefinition) as GraphQLScalarTypeDefinition;
            scalarDef.Name.Value.ShouldBe("JSON");
            scalarDef.Description.Value.ShouldBe("A JSON scalar");
            if (parseComments)
                scalarDef.Comment.Text.ShouldBe(" comment 2");

            var objectDef = defs.Single(x => x is GraphQLObjectTypeDefinition) as GraphQLObjectTypeDefinition;
            objectDef.Name.Value.ShouldBe("Human");
            objectDef.Description.Value.ShouldBe("Human type");
            if (parseComments)
                objectDef.Comment.Text.ShouldBe(" comment 4");
            objectDef.Fields.Count.ShouldBe(2);
            objectDef.Fields[0].Name.Value.ShouldBe("name");
            objectDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            objectDef.Fields[0].Description.Value.ShouldBe("Name of human");
            if (parseComments)
                objectDef.Fields[0].Comment.Text.ShouldBe(" comment 6");
            objectDef.Fields[1].Name.Value.ShouldBe("test");
            objectDef.Fields[1].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            objectDef.Fields[1].Description.Value.ShouldBe("Test");
            if (parseComments)
                objectDef.Fields[1].Comment.Text.ShouldBe(" comment 8");
            objectDef.Fields[1].Arguments.Count.ShouldBe(1);
            objectDef.Fields[1].Arguments[0].Name.Value.ShouldBe("arg");
            objectDef.Fields[1].Arguments[0].Description.Value.ShouldBe("desc");
            objectDef.Fields[1].Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            if (parseComments)
                objectDef.Fields[1].Arguments[0].Comment.Text.ShouldBe(" comment 10");

            var interfaceDef = defs.Single(x => x is GraphQLInterfaceTypeDefinition) as GraphQLInterfaceTypeDefinition;
            interfaceDef.Name.Value.ShouldBe("TestInterface");
            interfaceDef.Description.Value.ShouldBe("Test interface");
            if (parseComments)
                interfaceDef.Comment.Text.ShouldBe(" comment 12");
            interfaceDef.Fields.Count.ShouldBe(1);
            interfaceDef.Fields[0].Name.Value.ShouldBe("name");
            interfaceDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            interfaceDef.Fields[0].Description.Value.ShouldBe("Object name");
            if (parseComments)
                interfaceDef.Fields[0].Comment.Text.ShouldBe(" comment 14");

            var unionDef = defs.Single(x => x is GraphQLUnionTypeDefinition) as GraphQLUnionTypeDefinition;
            unionDef.Name.Value.ShouldBe("TestUnion");
            unionDef.Description.Value.ShouldBe("Test union");
            if (parseComments)
                unionDef.Comment.Text.ShouldBe(" comment 16");
            unionDef.Types.Count.ShouldBe(2);
            unionDef.Types[0].Name.Value.ShouldBe("Test1");
            unionDef.Types[1].Name.Value.ShouldBe("Test2");

            var enumDef = defs.Single(x => x is GraphQLEnumTypeDefinition) as GraphQLEnumTypeDefinition;
            enumDef.Name.Value.ShouldBe("Colors");
            enumDef.Description.Value.ShouldBe("Example enum");
            if (parseComments)
                enumDef.Comment.Text.ShouldBe(" comment 18");
            enumDef.Values.Count.ShouldBe(2);
            enumDef.Values[0].Name.Value.ShouldBe("RED");
            enumDef.Values[0].Description.Value.ShouldBe("Red");
            if (parseComments)
                enumDef.Values[0].Comment.Text.ShouldBe(" comment 20");
            enumDef.Values[1].Name.Value.ShouldBe("BLUE");
            enumDef.Values[1].Description.Value.ShouldBe("Blue");
            if (parseComments)
                enumDef.Values[1].Comment.Text.ShouldBe(" comment 22");

            var inputDef = defs.Single(x => x is GraphQLInputObjectTypeDefinition) as GraphQLInputObjectTypeDefinition;
            inputDef.Name.Value.ShouldBe("TestInputObject");
            inputDef.Description.Value.ShouldBe("This is an example input object\nLine two of the description");
            if (parseComments)
                inputDef.Comment.Text.ShouldBe(" comment 24");
            inputDef.Fields.Count.ShouldBe(1);
            inputDef.Fields[0].Name.Value.ShouldBe("Value");
            inputDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("JSON");
            inputDef.Fields[0].Description.Value.ShouldBe("The value of the input object\n  (any JSON value is accepted)");
            if (parseComments)
                inputDef.Fields[0].Comment.Text.ShouldBe(" comment 26");

            var directiveDef = defs.Single(x => x is GraphQLDirectiveDefinition) as GraphQLDirectiveDefinition;
            directiveDef.Name.Value.ShouldBe("TestDirective");
            directiveDef.Description.Value.ShouldBe("Test directive");
            if (parseComments)
                directiveDef.Comment.Text.ShouldBe(" comment 28");
            directiveDef.Arguments.Count.ShouldBe(1);
            directiveDef.Arguments[0].Name.Value.ShouldBe("Value");
            directiveDef.Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            directiveDef.Arguments[0].Description.Value.ShouldBe("Example");
            if (parseComments)
                directiveDef.Arguments[0].Comment.Text.ShouldBe(" comment 30");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Descriptions_WithComments_Should_Read_Correctly_3(IgnoreOptions options)
        {
            using var document = @"
# comment 0
""Super schema""
schema {
  query: String
}

# comment 1
""A JSON scalar""
scalar JSON

# comment 3
""""""
Human type
""""""
type Human {
  # comment 5
  """"""
  Name of human
  """"""
  name: String

  # comment 7
  ""Test""
  test(
    # comment 9
    ""desc""
    arg: Int
  ): Int
}

# comment 11
""Test interface""
interface TestInterface {
  # comment 13
  ""Object name""
  name: String
}

# comment 15
""""""
Test union
""""""
union TestUnion = Test1 | Test2

# comment 17
""Example enum""
enum Colors {
  # comment 19
  ""Red""
  RED
  # comment 21
  ""Blue""
  BLUE
}

# comment 23
""""""
This is an example input object
Line two of the description
""""""
input TestInputObject {
    # comment 25
    """"""
    The value of the input object
      (any JSON value is accepted)
    """"""
    Value: JSON
}

# comment 27
""Test directive""
directive @TestDirective (
  # comment 29
  ""Example""
  Value: Int
) on QUERY
".Parse(new ParserOptions { Ignore = options });
            var defs = document.Definitions;
            defs.Count.ShouldBe(8);
            var parseComments = !options.HasFlag(IgnoreOptions.Comments);

            var schemaDef = defs.Single(x => x is GraphQLSchemaDefinition) as GraphQLSchemaDefinition;
            schemaDef.Description.Value.ShouldBe("Super schema");
            if (parseComments)
                schemaDef.Comment.Text.ShouldBe(" comment 0");

            var scalarDef = defs.Single(x => x is GraphQLScalarTypeDefinition) as GraphQLScalarTypeDefinition;
            scalarDef.Name.Value.ShouldBe("JSON");
            scalarDef.Description.Value.ShouldBe("A JSON scalar");
            if (parseComments)
                scalarDef.Comment.Text.ShouldBe(" comment 1");

            var objectDef = defs.Single(x => x is GraphQLObjectTypeDefinition) as GraphQLObjectTypeDefinition;
            objectDef.Name.Value.ShouldBe("Human");
            objectDef.Description.Value.ShouldBe("Human type");
            if (parseComments)
                objectDef.Comment.Text.ShouldBe(" comment 3");
            objectDef.Fields.Count.ShouldBe(2);
            objectDef.Fields[0].Name.Value.ShouldBe("name");
            objectDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            objectDef.Fields[0].Description.Value.ShouldBe("Name of human");
            if (parseComments)
                objectDef.Fields[0].Comment.Text.ShouldBe(" comment 5");
            objectDef.Fields[1].Name.Value.ShouldBe("test");
            objectDef.Fields[1].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            objectDef.Fields[1].Description.Value.ShouldBe("Test");
            if (parseComments)
                objectDef.Fields[1].Comment.Text.ShouldBe(" comment 7");
            objectDef.Fields[1].Arguments.Count.ShouldBe(1);
            objectDef.Fields[1].Arguments[0].Name.Value.ShouldBe("arg");
            objectDef.Fields[1].Arguments[0].Description.Value.ShouldBe("desc");
            objectDef.Fields[1].Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            if (parseComments)
                objectDef.Fields[1].Arguments[0].Comment.Text.ShouldBe(" comment 9");

            var interfaceDef = defs.Single(x => x is GraphQLInterfaceTypeDefinition) as GraphQLInterfaceTypeDefinition;
            interfaceDef.Name.Value.ShouldBe("TestInterface");
            interfaceDef.Description.Value.ShouldBe("Test interface");
            if (parseComments)
                interfaceDef.Comment.Text.ShouldBe(" comment 11");
            interfaceDef.Fields.Count.ShouldBe(1);
            interfaceDef.Fields[0].Name.Value.ShouldBe("name");
            interfaceDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            interfaceDef.Fields[0].Description.Value.ShouldBe("Object name");
            if (parseComments)
                interfaceDef.Fields[0].Comment.Text.ShouldBe(" comment 13");

            var unionDef = defs.Single(x => x is GraphQLUnionTypeDefinition) as GraphQLUnionTypeDefinition;
            unionDef.Name.Value.ShouldBe("TestUnion");
            unionDef.Description.Value.ShouldBe("Test union");
            if (parseComments)
                unionDef.Comment.Text.ShouldBe(" comment 15");
            unionDef.Types.Count.ShouldBe(2);
            unionDef.Types[0].Name.Value.ShouldBe("Test1");
            unionDef.Types[1].Name.Value.ShouldBe("Test2");

            var enumDef = defs.Single(x => x is GraphQLEnumTypeDefinition) as GraphQLEnumTypeDefinition;
            enumDef.Name.Value.ShouldBe("Colors");
            enumDef.Description.Value.ShouldBe("Example enum");
            if (parseComments)
                enumDef.Comment.Text.ShouldBe(" comment 17");
            enumDef.Values.Count.ShouldBe(2);
            enumDef.Values[0].Name.Value.ShouldBe("RED");
            enumDef.Values[0].Description.Value.ShouldBe("Red");
            if (parseComments)
                enumDef.Values[0].Comment.Text.ShouldBe(" comment 19");
            enumDef.Values[1].Name.Value.ShouldBe("BLUE");
            enumDef.Values[1].Description.Value.ShouldBe("Blue");
            if (parseComments)
                enumDef.Values[1].Comment.Text.ShouldBe(" comment 21");

            var inputDef = defs.Single(x => x is GraphQLInputObjectTypeDefinition) as GraphQLInputObjectTypeDefinition;
            inputDef.Name.Value.ShouldBe("TestInputObject");
            inputDef.Description.Value.ShouldBe("This is an example input object\nLine two of the description");
            if (parseComments)
                inputDef.Comment.Text.ShouldBe(" comment 23");
            inputDef.Fields.Count.ShouldBe(1);
            inputDef.Fields[0].Name.Value.ShouldBe("Value");
            inputDef.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("JSON");
            inputDef.Fields[0].Description.Value.ShouldBe("The value of the input object\n  (any JSON value is accepted)");
            if (parseComments)
                inputDef.Fields[0].Comment.Text.ShouldBe(" comment 25");

            var directiveDef = defs.Single(x => x is GraphQLDirectiveDefinition) as GraphQLDirectiveDefinition;
            directiveDef.Name.Value.ShouldBe("TestDirective");
            directiveDef.Description.Value.ShouldBe("Test directive");
            if (parseComments)
                directiveDef.Comment.Text.ShouldBe(" comment 27");
            directiveDef.Arguments.Count.ShouldBe(1);
            directiveDef.Arguments[0].Name.Value.ShouldBe("Value");
            directiveDef.Arguments[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("Int");
            directiveDef.Arguments[0].Description.Value.ShouldBe("Example");
            if (parseComments)
                directiveDef.Arguments[0].Comment.Text.ShouldBe(" comment 29");
        }
    }
}
