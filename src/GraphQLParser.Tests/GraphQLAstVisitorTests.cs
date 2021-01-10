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
        private readonly Parser _parser;
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
            _parser = new Parser(new Lexer());
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
            _visitor.Visit(Parse("{ stuff(id : true) }"));

            _visitedBooleanValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsFragmentSpreadOneTime()
        {
            _visitor.Visit(Parse("{ foo { ...fragment } }"));

            _visitedFragmentSpreads.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsNameOfPropertyAndFragmentSpread()
        {
            _visitor.Visit(Parse("{ foo { ...fragment } }"));

            _visitedNames.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_DirectiveWithVariable_VisitsVariableOnce()
        {
            _visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            _visitedVariables.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_EnumValueArgument_VisitsOneEnumValue()
        {
            _visitor.Visit(Parse("{ stuff(id : TEST_ENUM) }"));

            _visitedEnumValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FloatValueArgument_VisitsOneFloatValue()
        {
            _visitor.Visit(Parse("{ stuff(id : 1.2) }"));

            _visitedFloatValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsFragmentDefinitionOnce()
        {
            _visitor.Visit(Parse("fragment testFragment on Stuff { field }"));

            _visitedFragmentDefinitions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            _visitor.Visit(Parse("fragment testFragment on Stuff { field }"));

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsArgumentsOnce()
        {
            _visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsDirectiveOnce()
        {
            _visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            _visitedDirectives.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsNameThreeTimes()
        {
            _visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            _visitedNames.Count.ShouldBe(4);
        }

        [Fact]
        public void Visit_InlineFragmentWithOneField_VisitsOneField()
        {
            _visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            _visitedFieldSelections.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsInlineFragmentOnce()
        {
            _visitor.Visit(Parse("{ ... on Stuff { field } }"));

            _visitedInlineFragments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            _visitor.Visit(Parse("{ ... on Stuff { field } }"));

            _visitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_IntValueArgument_VisitsOneIntValue()
        {
            _visitor.Visit(Parse("{ stuff(id : 1) }"));

            _visitedIntValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinition_CallsVisitDefinitionOnce()
        {
            _visitor.Visit(Parse("{ a }"));

            _visitedDefinitions.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinition_ProvidesCorrectDefinitionAsParameter()
        {
            var ast = Parse("{ a }");
            _visitor.Visit(ast);

            _visitedDefinitions.Single().ShouldBe(ast.Definitions.Single());
        }

        [Fact]
        public void Visit_OneDefinition_VisitsOneSelectionSet()
        {
            _visitor.Visit(Parse("{ a, b }"));

            _visitedSelectionSets.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneAliasedField_VisitsOneAlias()
        {
            _visitor.Visit(Parse("{ foo, foo : bar }"));

            _visitedAliases.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneArgument_VisitsOneArgument()
        {
            _visitor.Visit(Parse("{ foo(id : 1) { name } }"));

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_OneDefinitionWithOneNestedArgument_VisitsOneArgument()
        {
            _visitor.Visit(Parse("{ foo{ names(size: 10) } }"));

            _visitedArguments.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_StringValueArgument_VisitsOneStringValue()
        {
            _visitor.Visit(Parse("{ stuff(id : \"abc\") }"));

            _visitedStringValues.ShouldHaveSingleItem();
        }

        [Fact]
        public void Visit_TwoDefinitions_CallsVisitDefinitionTwice()
        {
            _visitor.Visit(Parse("{ a }\n{ b }"));

            _visitedDefinitions.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsFieldSelectionTwice()
        {
            _visitor.Visit(Parse("{ a, b }"));

            _visitedFieldSelections.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNames()
        {
            _visitor.Visit(Parse("{ a, b }"));

            _visitedNames.Count.ShouldBe(2);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNamesAndDefinitionName()
        {
            _visitor.Visit(Parse("query foo { a, b }"));

            _visitedNames.Count.ShouldBe(3);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveFieldSelections()
        {
            _visitor.Visit(Parse("{a, nested { x,  y }, b}"));

            _visitedFieldSelections.Count.ShouldBe(5);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveNames()
        {
            _visitor.Visit(Parse("{a, nested { x,  y }, b}"));

            _visitedNames.Count.ShouldBe(5);
        }

        private List<TEntity> MockVisitMethod<TEntity>(Action<GraphQLAstVisitor> visitorMethod)
        {
            var collection = new List<TEntity>();
            _visitor.WhenForAnyArgs(visitorMethod)
                .Do(e => collection.Add(e.Arg<TEntity>()));

            return collection;
        }

        private GraphQLDocument Parse(string expression) => _parser.Parse(expression);
    }
}
