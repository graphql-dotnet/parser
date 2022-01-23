using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Default implementation of <see cref="INodeVisitor{TContext}"/>.
/// Traverses all AST nodes of the provided one.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class DefaultNodeVisitor<TContext> : INodeVisitor<TContext>
    where TContext : INodeVisitorContext
{
    /// <inheritdoc/>
    public virtual async ValueTask VisitDocumentAsync(GraphQLDocument document, TContext context)
    {
        await VisitAsync(document.Comment, context).ConfigureAwait(false); // Comment always null
        await VisitAsync(document.Definitions, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitArgumentAsync(GraphQLArgument argument, TContext context)
    {
        await VisitAsync(argument.Comment, context).ConfigureAwait(false);
        await VisitAsync(argument.Name, context).ConfigureAwait(false);
        await VisitAsync(argument.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitArgumentsDefinitionAsync(GraphQLArgumentsDefinition argumentsDefinition, TContext context)
    {
        await VisitAsync(argumentsDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(argumentsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Visits <see cref="GraphQLArguments"/> node.
    /// </summary>
    public virtual async ValueTask VisitArgumentsAsync(GraphQLArguments arguments, TContext context)
    {
        await VisitAsync(arguments.Comment, context).ConfigureAwait(false);
        await VisitAsync(arguments.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual ValueTask VisitCommentAsync(GraphQLComment comment, TContext context)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask VisitDescriptionAsync(GraphQLDescription description, TContext context)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, TContext context)
    {
        await VisitAsync(operationDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.Variables, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNameAsync(GraphQLName name, TContext context)
    {
        await VisitAsync(name.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariableDefinitionAsync(GraphQLVariableDefinition variableDefinition, TContext context)
    {
        await VisitAsync(variableDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(variableDefinition.Variable, context).ConfigureAwait(false);
        await VisitAsync(variableDefinition.Type, context).ConfigureAwait(false);
        await VisitAsync(variableDefinition.DefaultValue, context).ConfigureAwait(false);
        await VisitAsync(variableDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariablesDefinitionAsync(GraphQLVariablesDefinition variablesDefinition, TContext context)
    {
        await VisitAsync(variablesDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(variablesDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariableAsync(GraphQLVariable variable, TContext context)
    {
        await VisitAsync(variable.Comment, context).ConfigureAwait(false);
        await VisitAsync(variable.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSelectionSetAsync(GraphQLSelectionSet selectionSet, TContext context)
    {
        await VisitAsync(selectionSet.Comment, context).ConfigureAwait(false);
        await VisitAsync(selectionSet.Selections, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitAliasAsync(GraphQLAlias alias, TContext context)
    {
        await VisitAsync(alias.Comment, context).ConfigureAwait(false);
        await VisitAsync(alias.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFieldAsync(GraphQLField field, TContext context)
    {
        await VisitAsync(field.Comment, context).ConfigureAwait(false);
        await VisitAsync(field.Alias, context).ConfigureAwait(false);
        await VisitAsync(field.Name, context).ConfigureAwait(false);
        await VisitAsync(field.Arguments, context).ConfigureAwait(false);
        await VisitAsync(field.Directives, context).ConfigureAwait(false);
        await VisitAsync(field.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async ValueTask VisitFragmentNameAsync(GraphQLFragmentName fragmentName, TContext context)
    {
        await VisitAsync(fragmentName.Comment, context).ConfigureAwait(false);
        await VisitAsync(fragmentName.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFragmentSpreadAsync(GraphQLFragmentSpread fragmentSpread, TContext context)
    {
        await VisitAsync(fragmentSpread.Comment, context).ConfigureAwait(false);
        await VisitAsync(fragmentSpread.FragmentName, context).ConfigureAwait(false);
        await VisitAsync(fragmentSpread.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInlineFragmentAsync(GraphQLInlineFragment inlineFragment, TContext context)
    {
        await VisitAsync(inlineFragment.Comment, context).ConfigureAwait(false);
        await VisitAsync(inlineFragment.TypeCondition, context).ConfigureAwait(false);
        await VisitAsync(inlineFragment.Directives, context).ConfigureAwait(false);
        await VisitAsync(inlineFragment.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitTypeConditionAsync(GraphQLTypeCondition typeCondition, TContext context)
    {
        await VisitAsync(typeCondition.Comment, context).ConfigureAwait(false);
        await VisitAsync(typeCondition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitImplementsInterfacesAsync(GraphQLImplementsInterfaces implementsInterfaces, TContext context)
    {
        await VisitAsync(implementsInterfaces.Comment, context).ConfigureAwait(false);
        await VisitAsync(implementsInterfaces.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFragmentDefinitionAsync(GraphQLFragmentDefinition fragmentDefinition, TContext context)
    {
        await VisitAsync(fragmentDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.FragmentName, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.TypeCondition, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitIntValueAsync(GraphQLIntValue intValue, TContext context)
    {
        await VisitAsync(intValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFloatValueAsync(GraphQLFloatValue floatValue, TContext context)
    {
        await VisitAsync(floatValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitStringValueAsync(GraphQLStringValue stringValue, TContext context)
    {
        await VisitAsync(stringValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitBooleanValueAsync(GraphQLBooleanValue booleanValue, TContext context)
    {
        await VisitAsync(booleanValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValueAsync(GraphQLEnumValue enumValue, TContext context)
    {
        await VisitAsync(enumValue.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumValue.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitListValueAsync(GraphQLListValue listValue, TContext context)
    {
        await VisitAsync(listValue.Comment, context).ConfigureAwait(false);
        await VisitAsync(listValue.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectValueAsync(GraphQLObjectValue objectValue, TContext context)
    {
        await VisitAsync(objectValue.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectValue.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectFieldAsync(GraphQLObjectField objectField, TContext context)
    {
        await VisitAsync(objectField.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectField.Name, context).ConfigureAwait(false);
        await VisitAsync(objectField.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirectiveAsync(GraphQLDirective directive, TContext context)
    {
        await VisitAsync(directive.Comment, context).ConfigureAwait(false);
        await VisitAsync(directive.Name, context).ConfigureAwait(false);
        await VisitAsync(directive.Arguments, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirectivesAsync(GraphQLDirectives directives, TContext context)
    {
        await VisitAsync(directives.Comment, context).ConfigureAwait(false); // Comment always null - see ParserContext.ParseDirectives
        await VisitAsync(directives.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNamedTypeAsync(GraphQLNamedType namedType, TContext context)
    {
        await VisitAsync(namedType.Comment, context).ConfigureAwait(false);
        await VisitAsync(namedType.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitListTypeAsync(GraphQLListType listType, TContext context)
    {
        await VisitAsync(listType.Comment, context).ConfigureAwait(false);
        await VisitAsync(listType.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNonNullTypeAsync(GraphQLNonNullType nonNullType, TContext context)
    {
        await VisitAsync(nonNullType.Comment, context).ConfigureAwait(false);
        await VisitAsync(nonNullType.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNullValueAsync(GraphQLNullValue nullValue, TContext context)
    {
        await VisitAsync(nullValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSchemaDefinitionAsync(GraphQLSchemaDefinition schemaDefinition, TContext context)
    {
        await VisitAsync(schemaDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(schemaDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(schemaDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(schemaDefinition.OperationTypes, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitRootOperationTypeDefinitionAsync(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
    {
        await VisitAsync(rootOperationTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(rootOperationTypeDefinition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
    {
        await VisitAsync(scalarTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectTypeDefinitionAsync(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
    {
        await VisitAsync(objectTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFieldDefinitionAsync(GraphQLFieldDefinition fieldDefinition, TContext context)
    {
        await VisitAsync(fieldDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Arguments, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Type, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFieldsDefinitionAsync(GraphQLFieldsDefinition fieldsDefinition, TContext context)
    {
        await VisitAsync(fieldsDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(fieldsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputValueDefinitionAsync(GraphQLInputValueDefinition inputValueDefinition, TContext context)
    {
        await VisitAsync(inputValueDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Type, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.DefaultValue, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputFieldsDefinitionAsync(GraphQLInputFieldsDefinition inputFieldsDefinition, TContext context)
    {
        await VisitAsync(inputFieldsDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputFieldsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInterfaceTypeDefinitionAsync(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
    {
        await VisitAsync(interfaceTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionTypeDefinitionAsync(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
    {
        await VisitAsync(unionTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionMemberTypesAsync(GraphQLUnionMemberTypes unionMemberTypes, TContext context)
    {
        await VisitAsync(unionMemberTypes.Comment, context).ConfigureAwait(false);
        await VisitAsync(unionMemberTypes.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumTypeDefinitionAsync(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
    {
        await VisitAsync(enumTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValueDefinitionAsync(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
    {
        await VisitAsync(enumValueDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumValueDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(enumValueDefinition.EnumValue, context).ConfigureAwait(false);
        await VisitAsync(enumValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValuesDefinitionAsync(GraphQLEnumValuesDefinition enumValuesDefinition, TContext context)
    {
        await VisitAsync(enumValuesDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumValuesDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputObjectTypeDefinitionAsync(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
    {
        await VisitAsync(inputObjectTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirectiveDefinitionAsync(GraphQLDirectiveDefinition directiveDefinition, TContext context)
    {
        await VisitAsync(directiveDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Description, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Arguments, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Locations, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Visits <see cref="GraphQLDirectiveLocations"/> node.
    /// </summary>
    public virtual async ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, TContext context)
    {
        await VisitAsync(directiveLocations.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSchemaExtensionAsync(GraphQLSchemaExtension schemaExtension, TContext context)
    {
        await VisitAsync(schemaExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(schemaExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(schemaExtension.OperationTypes, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitScalarTypeExtensionAsync(GraphQLScalarTypeExtension scalarTypeExtension, TContext context)
    {
        await VisitAsync(scalarTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeExtension.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectTypeExtensionAsync(GraphQLObjectTypeExtension objectTypeExtension, TContext context)
    {
        await VisitAsync(objectTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInterfaceTypeExtensionAsync(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context)
    {
        await VisitAsync(interfaceTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionTypeExtensionAsync(GraphQLUnionTypeExtension unionTypeExtension, TContext context)
    {
        await VisitAsync(unionTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumTypeExtensionAsync(GraphQLEnumTypeExtension enumTypeExtension, TContext context)
    {
        await VisitAsync(enumTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputObjectTypeExtensionAsync(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context)
    {
        await VisitAsync(inputObjectTypeExtension.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Dispatches node to the appropriate VisitXXX method.
    /// </summary>
    /// <param name="node">AST node to dispatch.</param>
    /// <param name="context">Context passed into all INodeVisitor.VisitXXX methods.</param>
    public virtual ValueTask VisitAsync(ASTNode? node, TContext context)
    {
        return node == null
            ? default
            : node switch
            {
                GraphQLArgument argument => VisitArgumentAsync(argument, context),
                GraphQLComment comment => VisitCommentAsync(comment, context),
                GraphQLDescription description => VisitDescriptionAsync(description, context),
                GraphQLDirective directive => VisitDirectiveAsync(directive, context),
                GraphQLDirectiveDefinition directiveDefinition => VisitDirectiveDefinitionAsync(directiveDefinition, context),
                GraphQLDirectiveLocations directiveLocations => VisitDirectiveLocationsAsync(directiveLocations, context),
                GraphQLDocument document => VisitDocumentAsync(document, context),
                GraphQLEnumTypeDefinition enumTypeDefinition => VisitEnumTypeDefinitionAsync(enumTypeDefinition, context),
                GraphQLEnumValueDefinition enumValueDefinition => VisitEnumValueDefinitionAsync(enumValueDefinition, context),
                GraphQLAlias alias => VisitAliasAsync(alias, context),
                GraphQLField field => VisitFieldAsync(field, context),
                GraphQLFieldDefinition fieldDefinition => VisitFieldDefinitionAsync(fieldDefinition, context),
                GraphQLFragmentDefinition fragmentDefinition => VisitFragmentDefinitionAsync(fragmentDefinition, context), // inherits from GraphQLInlineFragment so should be above
                GraphQLFragmentSpread fragmentSpread => VisitFragmentSpreadAsync(fragmentSpread, context),
                GraphQLInlineFragment inlineFragment => VisitInlineFragmentAsync(inlineFragment, context),
                GraphQLFragmentName fragmentName => VisitFragmentNameAsync(fragmentName, context),
                GraphQLTypeCondition typeCondition => VisitTypeConditionAsync(typeCondition, context),
                GraphQLInputObjectTypeDefinition inputObjectTypeDefinition => VisitInputObjectTypeDefinitionAsync(inputObjectTypeDefinition, context),
                GraphQLInputValueDefinition inputValueDefinition => VisitInputValueDefinitionAsync(inputValueDefinition, context),
                GraphQLInterfaceTypeDefinition interfaceTypeDefinition => VisitInterfaceTypeDefinitionAsync(interfaceTypeDefinition, context),
                GraphQLListType listType => VisitListTypeAsync(listType, context),
                GraphQLListValue listValue => VisitListValueAsync(listValue, context),
                GraphQLName name => VisitNameAsync(name, context),
                GraphQLNamedType namedType => VisitNamedTypeAsync(namedType, context),
                GraphQLNonNullType nonNullType => VisitNonNullTypeAsync(nonNullType, context),
                GraphQLObjectField objectField => VisitObjectFieldAsync(objectField, context),
                GraphQLObjectTypeDefinition objectTypeDefinition => VisitObjectTypeDefinitionAsync(objectTypeDefinition, context),
                GraphQLObjectValue objectValue => VisitObjectValueAsync(objectValue, context),
                GraphQLOperationDefinition operationDefinition => VisitOperationDefinitionAsync(operationDefinition, context),
                GraphQLRootOperationTypeDefinition rootOperationTypeDefinition => VisitRootOperationTypeDefinitionAsync(rootOperationTypeDefinition, context),
                GraphQLScalarTypeDefinition scalarTypeDefinition => VisitScalarTypeDefinitionAsync(scalarTypeDefinition, context),
                GraphQLBooleanValue boolValue => VisitBooleanValueAsync(boolValue, context),
                GraphQLEnumValue enumValue => VisitEnumValueAsync(enumValue, context),
                GraphQLFloatValue floatValue => VisitFloatValueAsync(floatValue, context),
                GraphQLIntValue intValue => VisitIntValueAsync(intValue, context),
                GraphQLNullValue nullValue => VisitNullValueAsync(nullValue, context),
                GraphQLStringValue stringValue => VisitStringValueAsync(stringValue, context),
                GraphQLSchemaDefinition schemaDefinition => VisitSchemaDefinitionAsync(schemaDefinition, context),
                GraphQLSelectionSet selectionSet => VisitSelectionSetAsync(selectionSet, context),
                GraphQLSchemaExtension schemaEx => VisitSchemaExtensionAsync(schemaEx, context),
                GraphQLScalarTypeExtension scalarEx => VisitScalarTypeExtensionAsync(scalarEx, context),
                GraphQLObjectTypeExtension objectEx => VisitObjectTypeExtensionAsync(objectEx, context),
                GraphQLInterfaceTypeExtension ifaceEx => VisitInterfaceTypeExtensionAsync(ifaceEx, context),
                GraphQLUnionTypeExtension unionEx => VisitUnionTypeExtensionAsync(unionEx, context),
                GraphQLEnumTypeExtension enumEx => VisitEnumTypeExtensionAsync(enumEx, context),
                GraphQLInputObjectTypeExtension inputEx => VisitInputObjectTypeExtensionAsync(inputEx, context),
                GraphQLUnionTypeDefinition unionTypeDefinition => VisitUnionTypeDefinitionAsync(unionTypeDefinition, context),
                GraphQLUnionMemberTypes unionMembers => VisitUnionMemberTypesAsync(unionMembers, context),
                GraphQLVariable variable => VisitVariableAsync(variable, context),
                GraphQLVariableDefinition variableDefinition => VisitVariableDefinitionAsync(variableDefinition, context),
                GraphQLArgumentsDefinition argsDef => VisitArgumentsDefinitionAsync(argsDef, context),
                GraphQLArguments args => VisitArgumentsAsync(args, context),
                GraphQLInputFieldsDefinition inputFieldsDef => VisitInputFieldsDefinitionAsync(inputFieldsDef, context),
                GraphQLDirectives directives => VisitDirectivesAsync(directives, context),
                GraphQLVariablesDefinition varsDef => VisitVariablesDefinitionAsync(varsDef, context),
                GraphQLEnumValuesDefinition enumValuesDef => VisitEnumValuesDefinitionAsync(enumValuesDef, context),
                GraphQLFieldsDefinition fieldsDef => VisitFieldsDefinitionAsync(fieldsDef, context),
                GraphQLImplementsInterfaces implements => VisitImplementsInterfacesAsync(implements, context),
                _ => throw new NotSupportedException($"Unknown node '{node.GetType().Name}'."),
            };
    }

    /// <summary>
    /// Visits all nodes from the provided list. As a rule, these are nested
    /// sibling nodes of some parent node, for example, argument nodes for
    /// parent field node or value nodes for parent list node.
    /// </summary>
    protected async ValueTask VisitAsync<T>(List<T>? nodes, TContext context)
        where T : ASTNode
    {
        if (nodes != null)
        {
            // Visitor may change AST while being traversed so foreach yields
            // System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
            for (int i = 0; i < nodes.Count; ++i)
                await VisitAsync(nodes[i], context).ConfigureAwait(false);
        }
    }
}
