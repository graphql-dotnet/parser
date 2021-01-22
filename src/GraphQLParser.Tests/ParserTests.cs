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
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Extra_Comments_Should_Read_Correctly(IgnoreOptions options)
        {
            const string query = @"
query _ {
    person {
        name
        #comment1
    }
    #comment2
    test {
        alt
    }
    #comment3
}
#comment4
";

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count().ShouldBe(1);
            // query
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count().ShouldBe(2);
            // person
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count().ShouldBe(1);
            // name
            var subField = field.SelectionSet.Selections.First() as GraphQLFieldSelection;
            subField.Comment.ShouldBeNull();
            // test
            field = def.SelectionSet.Selections.Last() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count().ShouldBe(1);
            field.Comment.ShouldNotBeNull().Text.ShouldBe("comment2");
            // alt
            subField = field.SelectionSet.Selections.First() as GraphQLFieldSelection;
            subField.Comment.ShouldBeNull();
            // extra document comments
            document.UnattachedComments.Count().ShouldBe(3);
            document.UnattachedComments[0].Text.ShouldBe("comment1");
            document.UnattachedComments[1].Text.ShouldBe("comment3");
            document.UnattachedComments[2].Text.ShouldBe("comment4");
        }

        [Theory]
        //[InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
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
            document.Definitions.Count().ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count().ShouldBe(1);
            def.Comment.ShouldBeNull();
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.Comment.ShouldBeNull();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Comments_on_FragmentSpread_Should_Read_Correclty(IgnoreOptions options)
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

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(2);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLFragmentSpread;
            fragment.Comment.ShouldNotBeNull().Text.ShouldBe("comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Comments_on_FragmentInline_Should_Read_Correclty(IgnoreOptions options)
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

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.SelectionSet.Selections.Count.ShouldBe(1);
            var field = def.SelectionSet.Selections.First() as GraphQLFieldSelection;
            field.SelectionSet.Selections.Count.ShouldBe(1);
            var fragment = field.SelectionSet.Selections.First() as GraphQLInlineFragment;
            fragment.Comment.ShouldNotBeNull().Text.ShouldBe("comment");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Comments_on_Variable_Should_Read_Correclty(IgnoreOptions options)
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

            using var document = query.Parse(new ParserOptions { Ignore = options });
            document.Definitions.Count.ShouldBe(1);
            var def = document.Definitions.First() as GraphQLOperationDefinition;
            def.VariableDefinitions.Count.ShouldBe(3);
            def.VariableDefinitions.First().Comment.ShouldNotBeNull().Text.ShouldBe("comment1");
            def.VariableDefinitions.Skip(1).First().Comment.ShouldBeNull();
            def.VariableDefinitions.Skip(2).First().Comment.ShouldNotBeNull().Text.ShouldBe("comment3");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
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
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Comments_On_Enums_Should_Read_Correctly(IgnoreOptions options)
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
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Unicode_Char_At_EOF_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{\"\\ue }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
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
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldInput_HasOneOperationDefinition(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            document.Definitions.First().Kind.ShouldBe(ASTNodeKind.OperationDefinition);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldInput_NameIsNull(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleOperationDefinition(document).Name.ShouldBeNull();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldInput_OperationIsQuery(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleOperationDefinition(document).Operation.ShouldBe(OperationType.Query);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldInput_ReturnsDocumentNode(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            document.Kind.ShouldBe(ASTNodeKind.Document);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldInput_SelectionSetContainsSingleFieldSelection(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldSource(options);

            GetSingleSelection(document).Kind.ShouldBe(ASTNodeKind.Field);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
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
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldWithOperationTypeAndNameInput_HasOneOperationDefinition(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            document.Definitions.First().Kind.ShouldBe(ASTNodeKind.OperationDefinition);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldWithOperationTypeAndNameInput_NameIsNull(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleOperationDefinition(document).Name.Value.ShouldBe("Foo");
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldWithOperationTypeAndNameInput_OperationIsQuery(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleOperationDefinition(document).Operation.ShouldBe(OperationType.Mutation);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldWithOperationTypeAndNameInput_ReturnsDocumentNode(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            document.Kind.ShouldBe(ASTNodeKind.Document);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_FieldWithOperationTypeAndNameInput_SelectionSetContainsSingleFieldWithOperationTypeAndNameSelection(IgnoreOptions options)
        {
            using var document = ParseGraphQLFieldWithOperationTypeAndNameSource(options);

            GetSingleSelection(document).Kind.ShouldBe(ASTNodeKind.Field);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        //[InlineData(IgnoreOptions.IgnoreComments)]
        //[InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_KitchenSink_DoesNotThrowError(IgnoreOptions options)
        {
            using var document = LoadKitchenSink().Parse(new ParserOptions { Ignore = options });
            var typeDef = document.Definitions.OfType<GraphQLObjectTypeDefinition>().First(d => d.Name.Value == "Foo");
            var fieldDef = typeDef.Fields.First(d => d.Name.Value == "three");
            fieldDef.Comment.ShouldNotBeNull().Text.ShouldBe($" multiline comments{_nl} with very importand description #{_nl} # and symbol # and ##");

            // Schema description
            // https://github.com/graphql/graphql-spec/pull/466
            var comment = document.Definitions.OfType<GraphQLSchemaDefinition>().First().Comment;
            comment.ShouldNotBeNull();
            ((string)comment.Text).StartsWith("﻿ Copyright (c) 2015, Facebook, Inc.").ShouldBeTrue();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_NullInput_EmptyDocument(IgnoreOptions options)
        {
            using var document = ((string)null).Parse(new ParserOptions { Ignore = options });

            document.Definitions.ShouldBeEmpty();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_VariableInlineValues_DoesNotThrowError(IgnoreOptions options)
        {
            using ("{ field(complex: { a: { b: [ $var ] } }) }".Parse(new ParserOptions { Ignore = options }))
            {
            }
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Empty_Field_Arguments_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{ a() }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Empty_Directive_Arguments_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "directive @dir() on FIELD_DEFINITION".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Empty_Enum_Values_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "enum Empty { }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Empty_SelectionSet_Should_Throw(IgnoreOptions options)
        {
            Should.Throw<GraphQLSyntaxErrorException>(() => "{ a { } }".Parse(new ParserOptions { Ignore = options }));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
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
    }
}
