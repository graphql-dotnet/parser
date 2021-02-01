using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLParser.AST;
using NSubstitute;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    public class GraphQLAstVisitorTests
    {
        private readonly List<GraphQLName> _visitedAliases;
        private readonly List<GraphQLArgument> _visitedArguments;
        private readonly List<ASTNode> _visitedDefinitions;
        private readonly List<GraphQLDirective> _visitedDirectives;
        private readonly List<GraphQLScalarValue> _visitedEnumValues;
        private readonly List<GraphQLFieldSelection> _visitedFieldSelections;
        private readonly List<GraphQLScalarValue> _visitedFloatValues;
        private readonly List<GraphQLFragmentDefinition> _visitedFragmentDefinitions;
        private readonly List<GraphQLFragmentSpread> _visitedFragmentSpreads;
        private readonly List<GraphQLNamedType> _visitedFragmentTypeConditions;
        private readonly List<GraphQLInlineFragment> _visitedInlineFragments;
        private readonly List<GraphQLScalarValue> _visitedIntValues;
        private readonly List<GraphQLName> _visitedNames;
        private readonly List<GraphQLSelectionSet> _visitedSelectionSets;
        private readonly List<GraphQLScalarValue> _visitedStringValues;
        private readonly List<GraphQLVariable> _visitedVariables;
        private readonly GraphQLAstVisitor _visitor;
        private readonly List<GraphQLScalarValue> _visitedBooleanValues;

        public GraphQLAstVisitorTests()
        {
            _visitor = Substitute.ForPartsOf<GraphQLAstVisitor>();

            _visitedDefinitions = MockVisitMethod<ASTNode>((visitor) => visitor.BeginVisitOperationDefinition(null));
            _visitedSelectionSets = MockVisitMethod<GraphQLSelectionSet>((visitor) => visitor.BeginVisitSelectionSet(null));
            _visitedFieldSelections = MockVisitMethod<GraphQLFieldSelection>((visitor) => visitor.BeginVisitFieldSelection(null));
            _visitedNames = MockVisitMethod<GraphQLName>((visitor) => visitor.BeginVisitName(null));
            _visitedArguments = MockVisitMethod<GraphQLArgument>((visitor) => visitor.BeginVisitArgument(null));
            _visitedAliases = MockVisitMethod<GraphQLName>((visitor) => visitor.BeginVisitAlias(null));
            _visitedFragmentSpreads = MockVisitMethod<GraphQLFragmentSpread>((visitor) => visitor.BeginVisitFragmentSpread(null));
            _visitedFragmentDefinitions = MockVisitMethod<GraphQLFragmentDefinition>((visitor) => visitor.BeginVisitFragmentDefinition(null));
            _visitedFragmentTypeConditions = MockVisitMethod<GraphQLNamedType>((visitor) => visitor.BeginVisitNamedType(null));
            _visitedInlineFragments = MockVisitMethod<GraphQLInlineFragment>((visitor) => visitor.BeginVisitInlineFragment(null));
            _visitedDirectives = MockVisitMethod<GraphQLDirective>((visitor) => visitor.BeginVisitDirective(null));
            _visitedVariables = MockVisitMethod<GraphQLVariable>((visitor) => visitor.BeginVisitVariable(null));
            _visitedIntValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitIntValue(null));
            _visitedFloatValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitFloatValue(null));
            _visitedStringValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitStringValue(null));
            _visitedBooleanValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitBooleanValue(null));
            _visitedEnumValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitEnumValue(null));
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_BooleanValueArgument_VisitsOneBooleanValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : true) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedBooleanValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsFragmentSpreadOneTime(IgnoreOptions options)
        {
            using var d = "{ foo { ...fragment } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFragmentSpreads.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsNameOfPropertyAndFragmentSpread(IgnoreOptions options)
        {
            using var d = "{ foo { ...fragment } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_DirectiveWithVariable_VisitsVariableOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedVariables.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_EnumValueArgument_VisitsOneEnumValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : TEST_ENUM) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedEnumValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_FloatValueArgument_VisitsOneFloatValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : 1.2) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFloatValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_FragmentWithTypeCondition_VisitsFragmentDefinitionOnce(IgnoreOptions options)
        {
            using var d = "fragment testFragment on Stuff { field }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFragmentDefinitions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_FragmentWithTypeCondition_VisitsTypeConditionOnce(IgnoreOptions options)
        {
            using var d = "fragment testFragment on Stuff { field }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsArgumentsOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsDirectiveOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedDirectives.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsNameThreeTimes(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(4);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithOneField_VisitsOneField(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFieldSelections.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithTypeCondition_VisitsInlineFragmentOnce(IgnoreOptions options)
        {
            using var d = "{ ... on Stuff { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedInlineFragments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_InlineFragmentWithTypeCondition_VisitsTypeConditionOnce(IgnoreOptions options)
        {
            using var d = "{ ... on Stuff { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_IntValueArgument_VisitsOneIntValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : 1) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedIntValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinition_CallsVisitDefinitionOnce(IgnoreOptions options)
        {
            using var d = "{ a }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedDefinitions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinition_ProvidesCorrectDefinitionAsParameter(IgnoreOptions options)
        {
            using var d = "{ a }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedDefinitions.Single().ShouldBe(d.Definitions.Single());
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinition_VisitsOneSelectionSet(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedSelectionSets.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinitionWithOneAliasedField_VisitsOneAlias(IgnoreOptions options)
        {
            using var d = "{ foo, foo : bar }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedAliases.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinitionWithOneArgument_VisitsOneArgument(IgnoreOptions options)
        {
            using var d = "{ foo(id : 1) { name } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_OneDefinitionWithOneNestedArgument_VisitsOneArgument(IgnoreOptions options)
        {
            using var d = "{ foo{ names(size: 10) } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_StringValueArgument_VisitsOneStringValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : \"abc\") }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedStringValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoDefinitions_CallsVisitDefinitionTwice(IgnoreOptions options)
        {
            using var d = "{ a }\n{ b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedDefinitions.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoFieldSelections_VisitsFieldSelectionTwice(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFieldSelections.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNames(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNamesAndDefinitionName(IgnoreOptions options)
        {
            using var d = "query foo { a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(3);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveFieldSelections(IgnoreOptions options)
        {
            using var d = "{a, nested { x,  y }, b}".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedFieldSelections.Count.ShouldBe(5);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveNames(IgnoreOptions options)
        {
            using var d = "{a, nested { x,  y }, b}".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(5);
        }

        private List<TEntity> MockVisitMethod<TEntity>(Action<GraphQLAstVisitor> visitorMethod)
        {
            var collection = new List<TEntity>();
            _visitor.WhenForAnyArgs(visitorMethod)
                .Do(e => collection.Add(e.Arg<TEntity>()));

            return collection;
        }
    }
}
