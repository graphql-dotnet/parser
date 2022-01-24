using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2012:Use ValueTasks correctly", Justification = "CountVisitor is sync")]
public class GraphQLAstVisitorTests
{
    public class CountVisitor : ASTVisitor<CountContext>
    {
        protected override async ValueTask VisitBooleanValueAsync(GraphQLBooleanValue booleanValue, CountContext context)
        {
            context.VisitedBooleanValues.Add(booleanValue);
            await base.VisitBooleanValueAsync(booleanValue, context);
        }

        protected override async ValueTask VisitIntValueAsync(GraphQLIntValue intValue, CountContext context)
        {
            context.VisitedIntValues.Add(intValue);
            await base.VisitIntValueAsync(intValue, context);
        }

        protected override async ValueTask VisitFragmentSpreadAsync(GraphQLFragmentSpread fragmentSpread, CountContext context)
        {
            context.VisitedFragmentSpreads.Add(fragmentSpread);
            await base.VisitFragmentSpreadAsync(fragmentSpread, context);
        }

        protected override async ValueTask VisitArgumentAsync(GraphQLArgument argument, CountContext context)
        {
            context.VisitedArguments.Add(argument);
            await base.VisitArgumentAsync(argument, context);
        }

        protected override async ValueTask VisitVariableAsync(GraphQLVariable variable, CountContext context)
        {
            context.VisitedVariables.Add(variable);
            await base.VisitVariableAsync(variable, context);
        }

        protected override async ValueTask VisitSelectionSetAsync(GraphQLSelectionSet selectionSet, CountContext context)
        {
            context.VisitedSelectionSets.Add(selectionSet);
            await base.VisitSelectionSetAsync(selectionSet, context);
        }

        protected override async ValueTask VisitDirectiveAsync(GraphQLDirective directive, CountContext context)
        {
            context.VisitedDirectives.Add(directive);
            await base.VisitDirectiveAsync(directive, context);
        }

        protected override async ValueTask VisitEnumValueAsync(GraphQLEnumValue enumValue, CountContext context)
        {
            context.VisitedEnumValues.Add(enumValue);
            await base.VisitEnumValueAsync(enumValue, context);
        }

        protected override async ValueTask VisitStringValueAsync(GraphQLStringValue stringValue, CountContext context)
        {
            context.VisitedStringValues.Add(stringValue);
            await base.VisitStringValueAsync(stringValue, context);
        }

        protected override async ValueTask VisitNameAsync(GraphQLName name, CountContext context)
        {
            context.VisitedNames.Add(name);
            await base.VisitNameAsync(name, context);
        }

        protected override async ValueTask VisitFieldAsync(GraphQLField field, CountContext context)
        {
            context.VisitedFields.Add(field);
            if (field.Alias != null)
                context.VisitedAliases.Add(field.Alias);
            await base.VisitFieldAsync(field, context);
        }

        protected override async ValueTask VisitFloatValueAsync(GraphQLFloatValue floatValue, CountContext context)
        {
            context.VisitedFloatValues.Add(floatValue);
            await base.VisitFloatValueAsync(floatValue, context);
        }

        protected override async ValueTask VisitEnumTypeDefinitionAsync(GraphQLEnumTypeDefinition enumTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(enumTypeDefinition);
            await base.VisitEnumTypeDefinitionAsync(enumTypeDefinition, context);
        }

        protected override async ValueTask VisitInlineFragmentAsync(GraphQLInlineFragment inlineFragment, CountContext context)
        {
            context.VisitedInlineFragments.Add(inlineFragment);
            context.VisitedFragmentTypeConditions.Add(inlineFragment.TypeCondition);
            await base.VisitInlineFragmentAsync(inlineFragment, context);
        }

        protected override async ValueTask VisitFragmentDefinitionAsync(GraphQLFragmentDefinition fragmentDefinition, CountContext context)
        {
            context.VisitedFragmentDefinitions.Add(fragmentDefinition);
            context.VisitedFragmentTypeConditions.Add(fragmentDefinition.TypeCondition);
            await base.VisitFragmentDefinitionAsync(fragmentDefinition, context);
        }

        protected override async ValueTask VisitFieldDefinitionAsync(GraphQLFieldDefinition fieldDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(fieldDefinition);
            await base.VisitFieldDefinitionAsync(fieldDefinition, context);
        }

        protected override async ValueTask VisitDirectiveDefinitionAsync(GraphQLDirectiveDefinition directiveDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(directiveDefinition);
            await base.VisitDirectiveDefinitionAsync(directiveDefinition, context);
        }

        protected override async ValueTask VisitEnumValueDefinitionAsync(GraphQLEnumValueDefinition enumValueDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(enumValueDefinition);
            await base.VisitEnumValueDefinitionAsync(enumValueDefinition, context);
        }

        protected override async ValueTask VisitInputObjectTypeDefinitionAsync(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(inputObjectTypeDefinition);
            await base.VisitInputObjectTypeDefinitionAsync(inputObjectTypeDefinition, context);
        }

        protected override async ValueTask VisitInputValueDefinitionAsync(GraphQLInputValueDefinition inputValueDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(inputValueDefinition);
            await base.VisitInputValueDefinitionAsync(inputValueDefinition, context);
        }

        protected override async ValueTask VisitInterfaceTypeDefinitionAsync(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(interfaceTypeDefinition);
            await base.VisitInterfaceTypeDefinitionAsync(interfaceTypeDefinition, context);
        }

        protected override async ValueTask VisitObjectTypeDefinitionAsync(GraphQLObjectTypeDefinition objectTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(objectTypeDefinition);
            await base.VisitObjectTypeDefinitionAsync(objectTypeDefinition, context);
        }

        protected override async ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(operationDefinition);
            await base.VisitOperationDefinitionAsync(operationDefinition, context);
        }

        protected override async ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(scalarTypeDefinition);
            await base.VisitScalarTypeDefinitionAsync(scalarTypeDefinition, context);
        }

        protected override async ValueTask VisitRootOperationTypeDefinitionAsync(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(rootOperationTypeDefinition);
            await base.VisitRootOperationTypeDefinitionAsync(rootOperationTypeDefinition, context);
        }

        protected override async ValueTask VisitVariableDefinitionAsync(GraphQLVariableDefinition variableDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(variableDefinition);
            await base.VisitVariableDefinitionAsync(variableDefinition, context);
        }

        protected override async ValueTask VisitUnionTypeDefinitionAsync(GraphQLUnionTypeDefinition unionTypeDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(unionTypeDefinition);
            await base.VisitUnionTypeDefinitionAsync(unionTypeDefinition, context);
        }

        protected override async ValueTask VisitSchemaDefinitionAsync(GraphQLSchemaDefinition schemaDefinition, CountContext context)
        {
            context.VisitedDefinitions.Add(schemaDefinition);
            await base.VisitSchemaDefinitionAsync(schemaDefinition, context);
        }
    }

    public class CountContext : IASTVisitorContext
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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

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
        _visitor.VisitAsync(d, Context);

        Context.VisitedNames.Count.ShouldBe(5);
    }
}
