namespace GraphQLParser.Tests
{
    using GraphQLParser;
    using GraphQLParser.AST;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class GraphQLAstVisitorTests
    {
        private readonly Parser parser;
        private readonly List<GraphQLName> visitedAliases;
        private readonly List<GraphQLArgument> visitedArguments;
        private readonly List<ASTNode> visitedDefinitions;
        private readonly List<GraphQLDirective> visitedDirectives;
        private readonly List<GraphQLScalarValue> visitedEnumValues;
        private readonly List<GraphQLFieldSelection> visitedFieldSelections;
        private readonly List<GraphQLScalarValue> visitedFloatValues;
        private readonly List<GraphQLFragmentDefinition> visitedFragmentDefinitions;
        private readonly List<GraphQLFragmentSpread> visitedFragmentSpreads;
        private readonly List<GraphQLNamedType> visitedFragmentTypeConditions;
        private readonly List<GraphQLInlineFragment> visitedInlineFragments;
        private readonly List<GraphQLScalarValue> visitedIntValues;
        private readonly List<GraphQLName> visitedNames;
        private readonly List<GraphQLSelectionSet> visitedSelectionSets;
        private readonly List<GraphQLScalarValue> visitedStringValues;
        private readonly List<GraphQLVariable> visitedVariables;
        private readonly GraphQLAstVisitor visitor;
        private readonly List<GraphQLScalarValue> visitedBooleanValues;

        public GraphQLAstVisitorTests()
        {
            parser = new Parser(new Lexer());
            visitor = Substitute.ForPartsOf<GraphQLAstVisitor>();

            visitedDefinitions = MockVisitMethod<ASTNode>((visitor) => visitor.BeginVisitOperationDefinition(null));
            visitedSelectionSets = MockVisitMethod<GraphQLSelectionSet>((visitor) => visitor.BeginVisitSelectionSet(null));
            visitedFieldSelections = MockVisitMethod<GraphQLFieldSelection>((visitor) => visitor.BeginVisitFieldSelection(null));
            visitedNames = MockVisitMethod<GraphQLName>((visitor) => visitor.BeginVisitName(null));
            visitedArguments = MockVisitMethod<GraphQLArgument>((visitor) => visitor.BeginVisitArgument(null));
            visitedAliases = MockVisitMethod<GraphQLName>((visitor) => visitor.BeginVisitAlias(null));
            visitedFragmentSpreads = MockVisitMethod<GraphQLFragmentSpread>((visitor) => visitor.BeginVisitFragmentSpread(null));
            visitedFragmentDefinitions = MockVisitMethod<GraphQLFragmentDefinition>((visitor) => visitor.BeginVisitFragmentDefinition(null));
            visitedFragmentTypeConditions = MockVisitMethod<GraphQLNamedType>((visitor) => visitor.BeginVisitNamedType(null));
            visitedInlineFragments = MockVisitMethod<GraphQLInlineFragment>((visitor) => visitor.BeginVisitInlineFragment(null));
            visitedDirectives = MockVisitMethod<GraphQLDirective>((visitor) => visitor.BeginVisitDirective(null));
            visitedVariables = MockVisitMethod<GraphQLVariable>((visitor) => visitor.BeginVisitVariable(null));
            visitedIntValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitIntValue(null));
            visitedFloatValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitFloatValue(null));
            visitedStringValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitStringValue(null));
            visitedBooleanValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitBooleanValue(null));
            visitedEnumValues = MockVisitMethod<GraphQLScalarValue>((visitor) => visitor.BeginVisitEnumValue(null));
        }

        [Fact]
        public void Visit_BooleanValueArgument_VisitsOneBooleanValue()
        {
            visitor.Visit(Parse("{ stuff(id : true) }"));

            Assert.Single(visitedBooleanValues);
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsFragmentSpreadOneTime()
        {
            visitor.Visit(Parse("{ foo { ...fragment } }"));

            Assert.Single(visitedFragmentSpreads);
        }

        [Fact]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsNameOfPropertyAndFragmentSpread()
        {
            visitor.Visit(Parse("{ foo { ...fragment } }"));

            Assert.Equal(2, visitedNames.Count);
        }

        [Fact]
        public void Visit_DirectiveWithVariable_VisitsVariableOnce()
        {
            visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            Assert.Single(visitedVariables);
        }

        [Fact]
        public void Visit_EnumValueArgument_VisitsOneEnumValue()
        {
            visitor.Visit(Parse("{ stuff(id : TEST_ENUM) }"));

            Assert.Single(visitedEnumValues);
        }

        [Fact]
        public void Visit_FloatValueArgument_VisitsOneFloatValue()
        {
            visitor.Visit(Parse("{ stuff(id : 1.2) }"));

            Assert.Single(visitedFloatValues);
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsFragmentDefinitionOnce()
        {
            visitor.Visit(Parse("fragment testFragment on Stuff { field }"));

            Assert.Single(visitedFragmentDefinitions);
        }

        [Fact]
        public void Visit_FragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            visitor.Visit(Parse("fragment testFragment on Stuff { field }"));

            Assert.Single(visitedFragmentTypeConditions);
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsArgumentsOnce()
        {
            visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            Assert.Single(visitedArguments);
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsDirectiveOnce()
        {
            visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            Assert.Single(visitedDirectives);
        }

        [Fact]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsNameThreeTimes()
        {
            visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            Assert.Equal(4, visitedNames.Count);
        }

        [Fact]
        public void Visit_InlineFragmentWithOneField_VisitsOneField()
        {
            visitor.Visit(Parse("{ ... @include(if : $stuff) { field } }"));

            Assert.Single(visitedFieldSelections);
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsInlineFragmentOnce()
        {
            visitor.Visit(Parse("{ ... on Stuff { field } }"));

            Assert.Single(visitedInlineFragments);
        }

        [Fact]
        public void Visit_InlineFragmentWithTypeCondition_VisitsTypeConditionOnce()
        {
            visitor.Visit(Parse("{ ... on Stuff { field } }"));

            Assert.Single(visitedFragmentTypeConditions);
        }

        [Fact]
        public void Visit_IntValueArgument_VisitsOneIntValue()
        {
            visitor.Visit(Parse("{ stuff(id : 1) }"));

            Assert.Single(visitedIntValues);
        }

        [Fact]
        public void Visit_OneDefinition_CallsVisitDefinitionOnce()
        {
            visitor.Visit(Parse("{ a }"));

            Assert.Single(visitedDefinitions);
        }

        [Fact]
        public void Visit_OneDefinition_ProvidesCorrectDefinitionAsParameter()
        {
            var ast = Parse("{ a }");
            visitor.Visit(ast);

            Assert.Equal(ast.Definitions.Single(), visitedDefinitions.Single());
        }

        [Fact]
        public void Visit_OneDefinition_VisitsOneSelectionSet()
        {
            visitor.Visit(Parse("{ a, b }"));

            Assert.Single(visitedSelectionSets);
        }

        [Fact]
        public void Visit_OneDefinitionWithOneAliasedField_VisitsOneAlias()
        {
            visitor.Visit(Parse("{ foo, foo : bar }"));

            Assert.Single(visitedAliases);
        }

        [Fact]
        public void Visit_OneDefinitionWithOneArgument_VisitsOneArgument()
        {
            visitor.Visit(Parse("{ foo(id : 1) { name } }"));

            Assert.Single(visitedArguments);
        }

        [Fact]
        public void Visit_OneDefinitionWithOneNestedArgument_VisitsOneArgument()
        {
            visitor.Visit(Parse("{ foo{ names(size: 10) } }"));

            Assert.Single(visitedArguments);
        }

        [Fact]
        public void Visit_StringValueArgument_VisitsOneStringValue()
        {
            visitor.Visit(Parse("{ stuff(id : \"abc\") }"));

            Assert.Single(visitedStringValues);
        }

        [Fact]
        public void Visit_TwoDefinitions_CallsVisitDefinitionTwice()
        {
            visitor.Visit(Parse("{ a }\n{ b }"));

            Assert.Equal(2, visitedDefinitions.Count);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsFieldSelectionTwice()
        {
            visitor.Visit(Parse("{ a, b }"));

            Assert.Equal(2, visitedFieldSelections.Count);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNames()
        {
            visitor.Visit(Parse("{ a, b }"));

            Assert.Equal(2, visitedNames.Count);
        }

        [Fact]
        public void Visit_TwoFieldSelections_VisitsTwoFieldNamesAndDefinitionName()
        {
            visitor.Visit(Parse("query foo { a, b }"));

            Assert.Equal(3, visitedNames.Count);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveFieldSelections()
        {
            visitor.Visit(Parse("{a, nested { x,  y }, b}"));

            Assert.Equal(5, visitedFieldSelections.Count);
        }

        [Fact]
        public void Visit_TwoFieldSelectionsWithOneNested_VisitsFiveNames()
        {
            visitor.Visit(Parse("{a, nested { x,  y }, b}"));

            Assert.Equal(5, visitedNames.Count);
        }

        private List<TEntity> MockVisitMethod<TEntity>(Action<GraphQLAstVisitor> visitorMethod)
        {
            var collection = new List<TEntity>();
            visitor.WhenForAnyArgs(visitorMethod)
                .Do(e => { collection.Add(e.Arg<TEntity>()); });

            return collection;
        }

        private GraphQLDocument Parse(string expression)
        {
            return parser.Parse(new Source(expression));
        }
    }
}