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
        private static readonly string NL = Environment.NewLine;

        [Fact]
        public void Comments_on_FragmentSpread_Should_Read_Correclty()
        {
            const string query = @"
query _ {
    person {
        #comment
        ...human
    }
}

fragment human on person {
        name
}";

            var parser = new Parser(new Lexer());
            var document = parser.Parse(new Source(query));
            document.Definitions.Count().ShouldBe(2);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count().ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count().ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLFragmentSpread;
            fragment.Comment.Text.ShouldBe("comment");
        }

        [Fact]
        public void Comments_on_FragmentInline_Should_Read_Correclty()
        {
            const string query = @"
query _ {
    person {
        #comment
        ... on human {
            name
        }
    }
}";

            var parser = new Parser(new Lexer());
            var document = parser.Parse(new Source(query));
            document.Definitions.Count().ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count().ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count().ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLInlineFragment;
            fragment.Comment.Text.ShouldBe("comment");
        }

        [Fact]
        public void Comments_on_Variable_Should_Read_Correclty()
        {
            const string query = @"
query _(
    #comment1
    $id: ID,
    $id2: String!,
    #comment3
    $id3: String) {
    person {
        name
    }
}";

            var parser = new Parser(new Lexer());
            var document = parser.Parse(new Source(query));
            document.Definitions.Count().ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.VariableDefinitions.Count().ShouldBe(3);
            def.VariableDefinitions.First().Comment.Text.ShouldBe("comment1");
            def.VariableDefinitions.Skip(1).First().Comment.ShouldBeNull();
            def.VariableDefinitions.Skip(2).First().Comment.Text.ShouldBe("comment3");
        }

        [Fact]
        public void Comments_On_SelectionSet_Should_Read_Correctly()
        {
            var parser = new Parser(new Lexer());
            var document = parser.Parse(new Source(@"
query {
    # a comment below query
    field1
    field2
    #second comment
    field3
}
"));
            document.Definitions.Count().ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count().ShouldBe(3);
            def.SelectionSet.Selections.First().Comment.Text.ShouldBe(" a comment below query");
            def.SelectionSet.Selections.Skip(1).First().Comment.ShouldBe(null);
            def.SelectionSet.Selections.Skip(2).First().Comment.Text.ShouldBe("second comment");
        }

        [Fact]
        public void Comments_On_Enums_Should_Read_Correctly()
        {
            var parser = new Parser(new Lexer());
            var document = parser.Parse(new Source(@"
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
"));
            document.Definitions.Count().ShouldBe(3);
            var d1 = document.Definitions.First() as GraphQLEnumTypeDefinition;
            d1.Name.Value.ShouldBe("Animal");
            d1.Comment.Text.ShouldBe(" different animals");
            d1.Values.First().Name.Value.ShouldBe("Cat");
            d1.Values.First().Comment.ShouldNotBeNull();
            d1.Values.First().Comment.Text.ShouldBe("a cat");
            d1.Values.Skip(2).First().Name.Value.ShouldBe("Octopus");
            d1.Values.Skip(2).First().Comment.ShouldBeNull();

            var d2 = document.Definitions.Skip(1).First() as GraphQLInputObjectTypeDefinition;
            d2.Name.Value.ShouldBe("Parameter");
            d2.Comment.ShouldBeNull();
            d2.Fields.Count().ShouldBe(1);
            d2.Fields.First().Comment.Text.ShouldBe("any value");
        }

        [Fact]
        public void Parse_Unicode_Char_At_EOF_Should_Throw()
        {
            var parser = new Parser(new Lexer());
            Assert.Throws<GraphQLSyntaxErrorException>(() => parser.Parse(new Source("{\"\\ue }")));
        }

        [Fact]
        public void Parse_FieldInput_HasCorrectEndLocationAttribute()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(9, document.Location.End);
        }

        [Fact]
        public void Parse_FieldInput_HasCorrectStartLocationAttribute()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(0, document.Location.Start);
        }

        [Fact]
        public void Parse_FieldInput_HasOneOperationDefinition()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(ASTNodeKind.OperationDefinition, document.Definitions.First().Kind);
        }

        [Fact]
        public void Parse_FieldInput_NameIsNull()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Null(GetSingleOperationDefinition(document).Name);
        }

        [Fact]
        public void Parse_FieldInput_OperationIsQuery()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(OperationType.Query, GetSingleOperationDefinition(document).Operation);
        }

        [Fact]
        public void Parse_FieldInput_ReturnsDocumentNode()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(ASTNodeKind.Document, document.Kind);
        }

        [Fact]
        public void Parse_FieldInput_SelectionSetContainsSingleFieldSelection()
        {
            var document = ParseGraphQLFieldSource();

            Assert.Equal(ASTNodeKind.Field, GetSingleSelection(document).Kind);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_HasCorrectEndLocationAttribute()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(22, document.Location.End);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_HasCorrectStartLocationAttribute()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(0, document.Location.Start);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_HasOneOperationDefinition()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(ASTNodeKind.OperationDefinition, document.Definitions.First().Kind);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_NameIsNull()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal("Foo", GetSingleOperationDefinition(document).Name.Value);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_OperationIsQuery()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(OperationType.Mutation, GetSingleOperationDefinition(document).Operation);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_ReturnsDocumentNode()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(ASTNodeKind.Document, document.Kind);
        }

        [Fact]
        public void Parse_FieldWithOperationTypeAndNameInput_SelectionSetContainsSingleFieldWithOperationTypeAndNameSelection()
        {
            var document = ParseGraphQLFieldWithOperationTypeAndNameSource();

            Assert.Equal(ASTNodeKind.Field, GetSingleSelection(document).Kind);
        }

        [Fact]
        public void Parse_KitchenSink_DoesNotThrowError()
        {
            var document = new Parser(new Lexer()).Parse(new Source(LoadKitchenSink()));
            if (document != null)
            {
                var typeDef = document.Definitions.OfType<GraphQLObjectTypeDefinition>().First(d => d.Name.Value == "Foo");
                var fieldDef = typeDef.Fields.First(d => d.Name.Value == "three");
                Assert.Equal($" multiline comments{NL} with very importand description #{NL} # and symbol # and ##", fieldDef.Comment.Text);

                // Schema description
                // https://github.com/graphql/graphql-spec/pull/466
                var comment = document.Definitions.OfType<GraphQLSchemaDefinition>().First().Comment;
                comment.ShouldNotBeNull();
                comment.Text.ShouldNotBeNull();
                comment.Text.StartsWith("﻿ Copyright (c) 2015, Facebook, Inc.").ShouldBeTrue();
            }
        }

        [Fact]
        public void Parse_NullInput_EmptyDocument()
        {
            var document = new Parser(new Lexer()).Parse(new Source(null));

            Assert.Empty(document.Definitions);
        }

        [Fact]
        public void Parse_Fields_Object_LocationCorrect()
        {
            var document = ParseGraphQLFieldsSource();
            var query = GetSingleOperationDefinition(document);
            var field = GetSingleFieldSelection(query.SelectionSet, "object");

            Assert.Equal(2, field.Location.Start);
            Assert.Equal(22, field.Location.End);
        }

        [Fact]
        public void Parse_Fields_Scalar_LocationCorrect()
        {
            var document = ParseGraphQLFieldsSource();
            var query = GetSingleOperationDefinition(document);
            var field = GetSingleFieldSelection(query.SelectionSet, "scalar");

            Assert.Equal(22, field.Location.Start);
            Assert.Equal(29, field.Location.End);
        }

        [Fact]
        public void Parse_VariableInlineValues_DoesNotThrowError()
        {
            new Parser(new Lexer()).Parse(new Source("{ field(complex: { a: { b: [ $var ] } }) }"));
        }

        private static GraphQLFieldSelection GetSingleFieldSelection(GraphQLSelectionSet selectionSet, string name)
        {
            return selectionSet
                .Selections
                .OfType<GraphQLFieldSelection>()
                .First(x => x.Name?.Value == name);
        }

        private static GraphQLOperationDefinition GetSingleOperationDefinition(GraphQLDocument document)
        {
            return (GraphQLOperationDefinition)document.Definitions.Single();
        }

        private static ASTNode GetSingleSelection(GraphQLDocument document)
        {
            return GetSingleOperationDefinition(document).SelectionSet.Selections.Single();
        }

        private static string LoadKitchenSink()
        {
            return @"﻿# Copyright (c) 2015, Facebook, Inc.
# All rights reserved.
#
# This source code is licensed under the BSD-style license found in the
# LICENSE file in the root directory of this source tree. An additional grant
# of patent rights can be found in the PATENTS file in the same directory.

query queryName($foo: ComplexType, $site: Site = MOBILE) {
  whoever123is: node(id: [123, 456]) {
    id ,
    ... on User @defer {
      field2 {
        id ,
        alias: field1(first:10, after:$foo,) @include(if: $foo) {
          id,
          ...frag
        }
      }
    }
    ... @skip(unless: $foo) {
      id
    }
    ... {
      id
    }
  }
}

mutation updateStory {
  like(story: {id: 123, EndDate: null}) {
    story {
      id
    }
  }
}

mutation likeStory {
  like(story: 123) @defer {
    story {
      id
    }
  }
}

subscription StoryLikeSubscription($input: StoryLikeSubscribeInput) {
  storyLikeSubscribe(input: $input) {
    story {
      likers {
        count
      }
      likeSentence {
        text
      }
    }
  }
}

fragment frag on Friend {
  foo(size: $size, bar: $b, obj: {key: ""value""})
}

{
  unnamed(truthy: true, falsey: false),
  query
    }

# Copyright (c) 2015, Facebook, Inc.
# All rights reserved.
#
# This source code is licensed under the BSD-style license found in the
# LICENSE file in the root directory of this source tree. An additional grant
# of patent rights can be found in the PATENTS file in the same directory.

schema {
  query: QueryType
  mutation: MutationType
}

type Foo implements Bar
{
  # comment 1
  one: Type
  # comment 2
  two(argument: InputType!): Type
  # multiline comments
  # with very importand description #
  # # and symbol # and ##
  three(argument: InputType, other: String): Int
  four(argument: String = ""string""): String
  five(argument: [String] = [""string"", ""string""]): String
  six(argument: InputType = { key: ""value""}): Type
}

type AnnotatedObject @onObject(arg: ""value"")
{
    # a comment
    annotatedField(arg: Type = ""default"" @onArg): Type @onField
}

interface Bar
{
    one: Type
    four(argument: String = ""string""): String
}

interface AnnotatedInterface @onInterface {
  annotatedField(arg: Type @onArg): Type @onField
}

union Feed = Story | Article | Advert

union AnnotatedUnion @onUnion = A | B

scalar CustomScalar

scalar AnnotatedScalar @onScalar

enum Site
{
    DESKTOP
  MOBILE
}

enum AnnotatedEnum @onEnum {
  ANNOTATED_VALUE @onEnumValue
  OTHER_VALUE
}

input InputType
{
    key: String!
  answer: Int = 42
}

input AnnotatedInput @onInputObjectType {
  annotatedField: Type @onField
}

extend type Foo {
  seven(argument: [String]): Type
}

extend type Foo @onType { }

type NoFields { }

directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT

directive @include(if: Boolean!)
  on FIELD
   | FRAGMENT_SPREAD
   | INLINE_FRAGMENT";
        }

        private static GraphQLDocument ParseGraphQLFieldSource()
        {
            return new Parser(new Lexer()).Parse(new Source("{ field }"));
        }

        private static GraphQLDocument ParseGraphQLFieldsSource()
        {
            return new Parser(new Lexer()).Parse(new Source("{ object { subfield } scalar }"));
        }

        private static GraphQLDocument ParseGraphQLFieldWithOperationTypeAndNameSource()
        {
            return new Parser(new Lexer()).Parse(new Source("mutation Foo { field }"));
        }

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
            var document = new Parser(new Lexer()).Parse(new Source(text));
            document.ShouldNotBeNull();
            document.Definitions.Count.ShouldBe(1);
            document.Definitions[0].ShouldBeOfType<GraphQLDirectiveDefinition>().Repeatable.ShouldBe(repeatable);
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
            Should.Throw<GraphQLSyntaxErrorException>(() => new Parser(new Lexer()).Parse(new Source(text)));
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
            new Parser(new Lexer()).Parse(new Source(text)).ShouldNotBeNull();
        }
    }
}
