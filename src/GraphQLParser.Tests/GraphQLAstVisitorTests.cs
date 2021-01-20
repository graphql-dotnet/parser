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

        [Fact]
        public void Visit_BooleanValueArgument_VisitsOneBooleanValue()
        {
            using var d = "{ stuff(id : true) }".Parse();
            _visitor.Visit(d);

            _visitedBooleanValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsFragmentSpreadOneTime()
        {
            using var d = "{ foo { ...fragment } }".Parse();
            _visitor.Visit(d);

            _visitedFragmentSpreads.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsNameOfPropertyAndFragmentSpread()
        {
            using var d = "{ foo { ...fragment } }".Parse();
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_DirectiveWithVariable_VisitsVariableOnce()
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse();
            _visitor.Visit(d);

            _visitedVariables.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_EnumValueArgument_VisitsOneEnumValue()
        {
            using var d = "{ stuff(id : TEST_ENUM) }".Parse();
            _visitor.Visit(d);

            _visitedEnumValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FloatValueArgument_VisitsOneFloatValue()
        {
            using var d = "{ stuff(id : 1.2) }".Parse();
            _visitor.Visit(d);

            _visitedFloatValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsFragmentDefinitionOnce()
        {
            using var d = "fragment testFragment on Stuff { field }".Parse();
            _visitor.Visit(d);

            _visitedFragmentDefinitions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            using var d = "fragment testFragment on Stuff { field }".Parse();
            _visitor.Visit(d);

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsArgumentsOnce()
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse();
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsDirectiveOnce()
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse();
            _visitor.Visit(d);

            _visitedDirectives.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsNameThreeTimes()
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse();
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(4);
        }

        [Fact]
        public void Visit_InlineFragmentWithOneField_VisitsOneField()
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse();
            _visitor.Visit(d);

            _visitedFieldSelections.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsInlineFragmentOnce()
        {
            using var d = "{ ... on Stuff { field } }".Parse();
            _visitor.Visit(d);

            _visitedInlineFragments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            using var d = "{ ... on Stuff { field } }".Parse();
            _visitor.Visit(d);

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_IntValueArgument_VisitsOneIntValue()
        {
            using var d = "{ stuff(id : 1) }".Parse();
            _visitor.Visit(d);

            _visitedIntValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinition_CallsVisitDefinitionOnce()
        {
            using var d = "{ a }".Parse();
            _visitor.Visit(d);

            _visitedDefinitions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinition_ProvidesCorrectDefinitionAsParameter()
        {
            using var d = "{ a }".Parse();
            _visitor.Visit(d);

            _visitedDefinitions.Single().ShouldBe(d.Definitions.Single());
        }

        [Fact]
        public void Visit_OneDefinition_VisitsOneSelectionSet()
        {
            using var d = "{ a, b }".Parse();
            _visitor.Visit(d);

            _visitedSelectionSets.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneAliasedField_VisitsOneAlias()
        {
            using var d = "{ foo, foo : bar }".Parse();
            _visitor.Visit(d);

            _visitedAliases.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneArgument_VisitsOneArgument()
        {
            using var d = "{ foo(id : 1) { name } }".Parse();
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneNestedArgument_VisitsOneArgument()
        {
            using var d = "{ foo{ names(size: 10) } }".Parse();
            _visitor.Visit(d);

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_StringValueArgument_VisitsOneStringValue()
        {
            using var d = "{ stuff(id : \"abc\") }".Parse();
            _visitor.Visit(d);

            _visitedStringValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_TwoDefinitions_CallsVisitDefinitionTwice()
        {
            using var d = "{ a }\n{ b }".Parse();
            _visitor.Visit(d);

            _visitedDefinitions.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsFieldSelectionTwice()
        {
            using var d = "{ a, b }".Parse();
            _visitor.Visit(d);

            _visitedFieldSelections.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNames()
        {
            using var d = "{ a, b }".Parse();
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNamesAndDefinitionName()
        {
            using var d = "query foo { a, b }".Parse();
            _visitor.Visit(d);

            _visitedNames.Count.ShouldBe(3);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveFieldSelections()
        {
            using var d = "{a, nested { x,  y }, b}".Parse();
            _visitor.Visit(d);

            _visitedFieldSelections.Count.ShouldBe(5);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveNames()
        {
            using var d = "{a, nested { x,  y }, b}".Parse();
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
