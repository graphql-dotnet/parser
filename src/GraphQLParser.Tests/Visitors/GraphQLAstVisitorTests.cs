using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class GraphQLAstVisitorTests
{
    public class CountVisitor : DefaultNodeVisitor<CountContext>
    {
        public override async ValueTask VisitBooleanValue(GraphQLBooleanValue booleanValue, CountContext context)
        {
            context.VisitedBooleanValues.Add(booleanValue);
            await base.VisitBooleanValue(booleanValue, context);
        }

        public override async ValueTask VisitIntValue(GraphQLIntValue intValue, CountContext context)
        {
            context.VisitedIntValues.Add(intValue);
            await base.VisitIntValue(intValue, context);
        }

        public override async ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, CountContext context)
        {
            context.VisitedFragmentSpreads.Add(fragmentSpread);
            await base.VisitFragmentSpread(fragmentSpread, context);
        }

        public override async ValueTask VisitArgument(GraphQLArgument argument, CountContext context)
        {
            context.VisitedArguments.Add(argument);
            await base.VisitArgument(argument, context);
        }

        public override async ValueTask VisitVariable(GraphQLVariable variable, CountContext context)
        {
            context.VisitedVariables.Add(variable);
            await base.VisitVariable(variable, context);
        }

        public override async ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, CountContext context)
        {
            context.VisitedSelectionSets.Add(selectionSet);
            await base.VisitSelectionSet(selectionSet, context);
        }

        public override async ValueTask VisitDirective(GraphQLDirective directive, CountContext context)
        {
            context.VisitedDirectives.Add(directive);
            await base.VisitDirective(directive, context);
        }

        public override async ValueTask VisitEnumValue(GraphQLEnumValue enumValue, CountContext context)
        {
            context.VisitedEnumValues.Add(enumValue);
            await base.VisitEnumValue(enumValue, context);
        }

        public override async ValueTask VisitStringValue(GraphQLStringValue stringValue, CountContext context)
        {
            context.VisitedStringValues.Add(stringValue);
            await base.VisitStringValue(stringValue, context);
        }

        public override async ValueTask VisitName(GraphQLName name, CountContext context)
        {
            context.VisitedNames.Add(name);
            await base.VisitName(name, context);
        }

        public override async ValueTask VisitField(GraphQLField field, CountContext context)
        {
            context.VisitedFields.Add(field);
            if (field.Alias != null)
                context.VisitedAliases.Add(field.Alias);
            await base.VisitField(field, context);
        }

        public override async ValueTask VisitFloatValue(GraphQLFloatValue floatValue, CountContext context)
        {
            context.VisitedFloatValues.Add(floatValue);
            await base.VisitFloatValue(floatValue, context);
        }

        public override async ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(enumTypeDefinition);
            await base.VisitEnumTypeDefinition(enumTypeDefinition, context);
        }

        public override async ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, CountContext context)
        {
            context.VisitedInlineFragments.Add(inlineFragment);
            context.VisitedFragmentTypeConditions.Add(inlineFragment.TypeCondition);
            await base.VisitInlineFragment(inlineFragment, context);
        }

        public override async ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, CountContext context)
        {
            context.VisitedFragmentDefinitions.Add(fragmentDefinition);
            context.VisitedFragmentTypeConditions.Add(fragmentDefinition.TypeCondition);
            await base.VisitFragmentDefinition(fragmentDefinition, context);
        }

        public override async ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(fieldDefinition);
            await base.VisitFieldDefinition(fieldDefinition, context);
        }

        public override async ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(directiveDefinition);
            await base.VisitDirectiveDefinition(directiveDefinition, context);
        }

        public override async ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(enumValueDefinition);
            await base.VisitEnumValueDefinition(enumValueDefinition, context);
        }

        public override async ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(inputObjectTypeDefinition);
            await base.VisitInputObjectTypeDefinition(inputObjectTypeDefinition, context);
        }

        public override async ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(inputValueDefinition);
            await base.VisitInputValueDefinition(inputValueDefinition, context);
        }

        public override async ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(interfaceTypeDefinition);
            await base.VisitInterfaceTypeDefinition(interfaceTypeDefinition, context);
        }

        public override async ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(objectTypeDefinition);
            await base.VisitObjectTypeDefinition(objectTypeDefinition, context);
        }

        public override async ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(operationDefinition);
            await base.VisitOperationDefinition(operationDefinition, context);
        }

        public override async ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(scalarTypeDefinition);
            await base.VisitScalarTypeDefinition(scalarTypeDefinition, context);
        }

        public override async ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(rootOperationTypeDefinition);
            await base.VisitRootOperationTypeDefinition(rootOperationTypeDefinition, context);
        }

        public override async ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(variableDefinition);
            await base.VisitVariableDefinition(variableDefinition, context);
        }

        public override async ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(unionTypeDefinition);
            await base.VisitUnionTypeDefinition(unionTypeDefinition, context);
        }

        public override async ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(schemaDefinition);
            await base.VisitSchemaDefinition(schemaDefinition, context);
        }
    }

    public class CountContext : INodeVisitorContext
    {
        public List<GraphQLAlias> VisitedAliases = new();
        public List<GraphQLArgument> VisitedArguments = new();
        public List<ASTNode> VisitedDefinitions = new();
        public List<GraphQLDirective> VisitedDirectives = new();
        public List<GraphQLEnumValue> VisitedEnumValues = new();
        public List<GraphQLField> VisitedFields = new();
        public List<GraphQLFloatValue> VisitedFloatValues = new();
        public List<GraphQLFragmentDefinition> VisitedFragmentDefinitions = new();
        public List<GraphQLFragmentSpread> VisitedFragmentSpreads = new();
        public List<GraphQLTypeCondition> VisitedFragmentTypeConditions = new();
        public List<GraphQLInlineFragment> VisitedInlineFragments = new();
        public List<GraphQLIntValue> VisitedIntValues = new();
        public List<GraphQLName> VisitedNames = new();
        public List<GraphQLSelectionSet> VisitedSelectionSets = new();
        public List<GraphQLStringValue> VisitedStringValues = new();
        public List<GraphQLVariable> VisitedVariables = new();
        public List<GraphQLBooleanValue> VisitedBooleanValues = new();

        public CancellationToken CancellationToken { get; set; }
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
