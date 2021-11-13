using System.Collections.Generic;
using System.Linq;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors
{
    public class GraphQLAstVisitorTests
    {
        public class CountVisitor : DefaultNodeVisitor<CountContext>
        {
            public override void VisitBooleanValue(GraphQLScalarValue booleanValue, CountContext context)
            {
                context.VisitedBooleanValues.Add(booleanValue);
                base.VisitBooleanValue(booleanValue, context);
            }

            public override void VisitIntValue(GraphQLScalarValue intValue, CountContext context)
            {
                context.VisitedIntValues.Add(intValue);
                base.VisitIntValue(intValue, context);
            }

            public override void VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, CountContext context)
            {
                context.VisitedFragmentSpreads.Add(fragmentSpread);
                base.VisitFragmentSpread(fragmentSpread, context);
            }

            public override void VisitArgument(GraphQLArgument argument, CountContext context)
            {
                context.VisitedArguments.Add(argument);
                base.VisitArgument(argument, context);
            }

            public override void VisitVariable(GraphQLVariable variable, CountContext context)
            {
                context.VisitedVariables.Add(variable);
                base.VisitVariable(variable, context);
            }

            public override void VisitSelectionSet(GraphQLSelectionSet selectionSet, CountContext context)
            {
                context.VisitedSelectionSets.Add(selectionSet);
                base.VisitSelectionSet(selectionSet, context);
            }

            public override void VisitDirective(GraphQLDirective directive, CountContext context)
            {
                context.VisitedDirectives.Add(directive);
                base.VisitDirective(directive, context);
            }

            public override void VisitEnumValue(GraphQLScalarValue enumValue, CountContext context)
            {
                context.VisitedEnumValues.Add(enumValue);
                base.VisitEnumValue(enumValue, context);
            }

            public override void VisitStringValue(GraphQLScalarValue stringValue, CountContext context)
            {
                context.VisitedStringValues.Add(stringValue);
                base.VisitStringValue(stringValue, context);
            }

            public override void VisitName(GraphQLName name, CountContext context)
            {
                context.VisitedNames.Add(name);
                base.VisitName(name, context);
            }

            public override void VisitField(GraphQLField field, CountContext context)
            {
                context.VisitedFields.Add(field);
                if (field.Alias != null)
                    context.VisitedAliases.Add(field.Alias);
                base.VisitField(field, context);
            }

            public override void VisitFloatValue(GraphQLScalarValue floatValue, CountContext context)
            {
                context.VisitedFloatValues.Add(floatValue);
                base.VisitFloatValue(floatValue, context);
            }

            public override void VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(enumTypeDefinition);
                base.VisitEnumTypeDefinition(enumTypeDefinition, context);
            }

            public override void VisitInlineFragment(GraphQLInlineFragment inlineFragment, CountContext context)
            {
                context.VisitedInlineFragments.Add(inlineFragment);
                context.VisitedFragmentTypeConditions.Add(inlineFragment.TypeCondition);
                base.VisitInlineFragment(inlineFragment, context);
            }

            public override void VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, CountContext context)
            {
                context.VisitedFragmentDefinitions.Add(fragmentDefinition);
                context.VisitedFragmentTypeConditions.Add(fragmentDefinition.TypeCondition);
                base.VisitFragmentDefinition(fragmentDefinition, context);
            }

            public override void VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(fieldDefinition);
                base.VisitFieldDefinition(fieldDefinition, context);
            }

            public override void VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(directiveDefinition);
                base.VisitDirectiveDefinition(directiveDefinition, context);
            }

            public override void VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(enumValueDefinition);
                base.VisitEnumValueDefinition(enumValueDefinition, context);
            }

            public override void VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(inputObjectTypeDefinition);
                base.VisitInputObjectTypeDefinition(inputObjectTypeDefinition, context);
            }

            public override void VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(inputValueDefinition);
                base.VisitInputValueDefinition(inputValueDefinition, context);
            }

            public override void VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(interfaceTypeDefinition);
                base.VisitInterfaceTypeDefinition(interfaceTypeDefinition, context);
            }

            public override void VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(objectTypeDefinition);
                base.VisitObjectTypeDefinition(objectTypeDefinition, context);
            }

            public override void VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(operationDefinition);
                base.VisitOperationDefinition(operationDefinition, context);
            }

            public override void VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(scalarTypeDefinition);
                base.VisitScalarTypeDefinition(scalarTypeDefinition, context);
            }

            public override void VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(rootOperationTypeDefinition);
                base.VisitRootOperationTypeDefinition(rootOperationTypeDefinition, context);
            }

            public override void VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(variableDefinition);
                base.VisitVariableDefinition(variableDefinition, context);
            }

            public override void VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(unionTypeDefinition);
                base.VisitUnionTypeDefinition(unionTypeDefinition, context);
            }

            public override void VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, CountContext context)
            {
                context.VisitedDefinitions.Add(schemaDefinition);
                base.VisitSchemaDefinition(schemaDefinition, context);
            }
        }

        public class CountContext : IVisitorContext
        {
            public List<GraphQLName> VisitedAliases = new();
            public List<GraphQLArgument> VisitedArguments = new();
            public List<ASTNode> VisitedDefinitions = new();
            public List<GraphQLDirective> VisitedDirectives = new();
            public List<GraphQLScalarValue> VisitedEnumValues = new();
            public List<GraphQLField> VisitedFields = new();
            public List<GraphQLScalarValue> VisitedFloatValues = new();
            public List<GraphQLFragmentDefinition> VisitedFragmentDefinitions = new();
            public List<GraphQLFragmentSpread> VisitedFragmentSpreads = new();
            public List<GraphQLNamedType> VisitedFragmentTypeConditions = new();
            public List<GraphQLInlineFragment> VisitedInlineFragments = new();
            public List<GraphQLScalarValue> VisitedIntValues = new();
            public List<GraphQLName> VisitedNames = new();
            public List<GraphQLSelectionSet> VisitedSelectionSets = new();
            public List<GraphQLScalarValue> VisitedStringValues = new();
            public List<GraphQLVariable> VisitedVariables = new();
            public List<GraphQLScalarValue> VisitedBooleanValues = new();
        }

        private readonly CountVisitor _visitor = new();

        public CountContext Context = new();

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_BooleanValueArgument_VisitsOneBooleanValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : true) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedBooleanValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsFragmentSpreadOneTime(IgnoreOptions options)
        {
            using var d = "{ foo { ...fragment } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFragmentSpreads.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_DefinitionWithSingleFragmentSpread_VisitsNameOfPropertyAndFragmentSpread(IgnoreOptions options)
        {
            using var d = "{ foo { ...fragment } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedNames.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_DirectiveWithVariable_VisitsVariableOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedVariables.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_EnumValueArgument_VisitsOneEnumValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : TEST_ENUM) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedEnumValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_FloatValueArgument_VisitsOneFloatValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : 1.2) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFloatValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_FragmentWithTypeCondition_VisitsFragmentDefinitionOnce(IgnoreOptions options)
        {
            using var d = "fragment testFragment on Stuff { field }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFragmentDefinitions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_FragmentWithTypeCondition_VisitsTypeConditionOnce(IgnoreOptions options)
        {
            using var d = "fragment testFragment on Stuff { field }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsArgumentsOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsDirectiveOnce(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedDirectives.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithDirectiveAndArgument_VisitsNameThreeTimes(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedNames.Count.ShouldBe(4);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithOneField_VisitsOneField(IgnoreOptions options)
        {
            using var d = "{ ... @include(if : $stuff) { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFields.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithTypeCondition_VisitsInlineFragmentOnce(IgnoreOptions options)
        {
            using var d = "{ ... on Stuff { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedInlineFragments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_InlineFragmentWithTypeCondition_VisitsTypeConditionOnce(IgnoreOptions options)
        {
            using var d = "{ ... on Stuff { field } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFragmentTypeConditions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_IntValueArgument_VisitsOneIntValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : 1) }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedIntValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinition_CallsVisitDefinitionOnce(IgnoreOptions options)
        {
            using var d = "{ a }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedDefinitions.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinition_ProvidesCorrectDefinitionAsParameter(IgnoreOptions options)
        {
            using var d = "{ a }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedDefinitions.Single().ShouldBe(d.Definitions.Single());
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinition_VisitsOneSelectionSet(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedSelectionSets.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinitionWithOneAliasedField_VisitsOneAlias(IgnoreOptions options)
        {
            using var d = "{ foo, foo : bar }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedAliases.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinitionWithOneArgument_VisitsOneArgument(IgnoreOptions options)
        {
            using var d = "{ foo(id : 1) { name } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_OneDefinitionWithOneNestedArgument_VisitsOneArgument(IgnoreOptions options)
        {
            using var d = "{ foo{ names(size: 10) } }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedArguments.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_StringValueArgument_VisitsOneStringValue(IgnoreOptions options)
        {
            using var d = "{ stuff(id : \"abc\") }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedStringValues.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoDefinitions_CallsVisitDefinitionTwice(IgnoreOptions options)
        {
            using var d = "{ a }\n{ b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedDefinitions.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoFields_VisitsFieldTwice(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFields.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoFields_VisitsTwoFieldNames(IgnoreOptions options)
        {
            using var d = "{ a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedNames.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoFields_VisitsTwoFieldNamesAndDefinitionName(IgnoreOptions options)
        {
            using var d = "query foo { a, b }".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedNames.Count.ShouldBe(3);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoFieldsWithOneNested_VisitsFiveFields(IgnoreOptions options)
        {
            using var d = "{a, nested { x,  y }, b}".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedFields.Count.ShouldBe(5);
        }

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.Comments)]
        [InlineData(IgnoreOptions.Locations)]
        [InlineData(IgnoreOptions.All)]
        public void Visit_TwoFieldsWithOneNested_VisitsFiveNames(IgnoreOptions options)
        {
            using var d = "{a, nested { x,  y }, b}".Parse(new ParserOptions { Ignore = options });
            _visitor.Visit(d, Context);

            Context.VisitedNames.Count.ShouldBe(5);
        }
    }
}
