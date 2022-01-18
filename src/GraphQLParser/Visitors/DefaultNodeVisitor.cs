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
    public virtual async ValueTask VisitDocument(GraphQLDocument document, TContext context)
    {
        await Visit(document.Comment, context).ConfigureAwait(false); // Comment always null
        await Visit(document.Definitions, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitArgument(GraphQLArgument argument, TContext context)
    {
        await Visit(argument.Comment, context).ConfigureAwait(false);
        await Visit(argument.Name, context).ConfigureAwait(false);
        await Visit(argument.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitArgumentsDefinition(GraphQLArgumentsDefinition argumentsDefinition, TContext context)
    {
        await Visit(argumentsDefinition.Comment, context).ConfigureAwait(false);
        await Visit(argumentsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Visits <see cref="GraphQLArguments"/> node.
    /// </summary>
    public virtual async ValueTask VisitArguments(GraphQLArguments arguments, TContext context)
    {
        await Visit(arguments.Comment, context).ConfigureAwait(false);
        await Visit(arguments.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual ValueTask VisitComment(GraphQLComment comment, TContext context)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask VisitDescription(GraphQLDescription description, TContext context)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
    {
        await Visit(operationDefinition.Comment, context).ConfigureAwait(false);
        await Visit(operationDefinition.Name, context).ConfigureAwait(false);
        await Visit(operationDefinition.Variables, context).ConfigureAwait(false);
        await Visit(operationDefinition.Directives, context).ConfigureAwait(false);
        await Visit(operationDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitName(GraphQLName name, TContext context)
    {
        await Visit(name.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
    {
        await Visit(variableDefinition.Comment, context).ConfigureAwait(false);
        await Visit(variableDefinition.Variable, context).ConfigureAwait(false);
        await Visit(variableDefinition.Type, context).ConfigureAwait(false);
        await Visit(variableDefinition.DefaultValue, context).ConfigureAwait(false);
        await Visit(variableDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariablesDefinition(GraphQLVariablesDefinition variablesDefinition, TContext context)
    {
        await Visit(variablesDefinition.Comment, context).ConfigureAwait(false);
        await Visit(variablesDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitVariable(GraphQLVariable variable, TContext context)
    {
        await Visit(variable.Comment, context).ConfigureAwait(false);
        await Visit(variable.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
    {
        await Visit(selectionSet.Comment, context).ConfigureAwait(false);
        await Visit(selectionSet.Selections, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitAlias(GraphQLAlias alias, TContext context)
    {
        await Visit(alias.Comment, context).ConfigureAwait(false);
        await Visit(alias.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitField(GraphQLField field, TContext context)
    {
        await Visit(field.Comment, context).ConfigureAwait(false);
        await Visit(field.Alias, context).ConfigureAwait(false);
        await Visit(field.Name, context).ConfigureAwait(false);
        await Visit(field.Arguments, context).ConfigureAwait(false);
        await Visit(field.Directives, context).ConfigureAwait(false);
        await Visit(field.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async ValueTask VisitFragmentName(GraphQLFragmentName fragmentName, TContext context)
    {
        await Visit(fragmentName.Comment, context).ConfigureAwait(false);
        await Visit(fragmentName.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
    {
        await Visit(fragmentSpread.Comment, context).ConfigureAwait(false);
        await Visit(fragmentSpread.FragmentName, context).ConfigureAwait(false);
        await Visit(fragmentSpread.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
    {
        await Visit(inlineFragment.Comment, context).ConfigureAwait(false);
        await Visit(inlineFragment.TypeCondition, context).ConfigureAwait(false);
        await Visit(inlineFragment.Directives, context).ConfigureAwait(false);
        await Visit(inlineFragment.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitTypeCondition(GraphQLTypeCondition typeCondition, TContext context)
    {
        await Visit(typeCondition.Comment, context).ConfigureAwait(false);
        await Visit(typeCondition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitImplementsInterfaces(GraphQLImplementsInterfaces implementsInterfaces, TContext context)
    {
        await Visit(implementsInterfaces.Comment, context).ConfigureAwait(false);
        await Visit(implementsInterfaces.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
    {
        await Visit(fragmentDefinition.Comment, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.FragmentName, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.TypeCondition, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.Directives, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitIntValue(GraphQLIntValue intValue, TContext context)
    {
        await Visit(intValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFloatValue(GraphQLFloatValue floatValue, TContext context)
    {
        await Visit(floatValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitStringValue(GraphQLStringValue stringValue, TContext context)
    {
        await Visit(stringValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitBooleanValue(GraphQLBooleanValue booleanValue, TContext context)
    {
        await Visit(booleanValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValue(GraphQLEnumValue enumValue, TContext context)
    {
        await Visit(enumValue.Comment, context).ConfigureAwait(false);
        await Visit(enumValue.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitListValue(GraphQLListValue listValue, TContext context)
    {
        await Visit(listValue.Comment, context).ConfigureAwait(false);
        await Visit(listValue.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
    {
        await Visit(objectValue.Comment, context).ConfigureAwait(false);
        await Visit(objectValue.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectField(GraphQLObjectField objectField, TContext context)
    {
        await Visit(objectField.Comment, context).ConfigureAwait(false);
        await Visit(objectField.Name, context).ConfigureAwait(false);
        await Visit(objectField.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirective(GraphQLDirective directive, TContext context)
    {
        await Visit(directive.Comment, context).ConfigureAwait(false);
        await Visit(directive.Name, context).ConfigureAwait(false);
        await Visit(directive.Arguments, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirectives(GraphQLDirectives directives, TContext context)
    {
        await Visit(directives.Comment, context).ConfigureAwait(false); // Comment always null - see ParserContext.ParseDirectives
        await Visit(directives.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNamedType(GraphQLNamedType namedType, TContext context)
    {
        await Visit(namedType.Comment, context).ConfigureAwait(false);
        await Visit(namedType.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitListType(GraphQLListType listType, TContext context)
    {
        await Visit(listType.Comment, context).ConfigureAwait(false);
        await Visit(listType.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
    {
        await Visit(nonNullType.Comment, context).ConfigureAwait(false);
        await Visit(nonNullType.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitNullValue(GraphQLNullValue nullValue, TContext context)
    {
        await Visit(nullValue.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
    {
        await Visit(schemaDefinition.Comment, context).ConfigureAwait(false);
        await Visit(schemaDefinition.Description, context).ConfigureAwait(false);
        await Visit(schemaDefinition.Directives, context).ConfigureAwait(false);
        await Visit(schemaDefinition.OperationTypes, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
    {
        await Visit(rootOperationTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(rootOperationTypeDefinition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
    {
        await Visit(scalarTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
    {
        await Visit(objectTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
    {
        await Visit(fieldDefinition.Comment, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Description, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Name, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Arguments, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Type, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitFieldsDefinition(GraphQLFieldsDefinition fieldsDefinition, TContext context)
    {
        await Visit(fieldsDefinition.Comment, context).ConfigureAwait(false);
        await Visit(fieldsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
    {
        await Visit(inputValueDefinition.Comment, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.Description, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.Name, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.Type, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.DefaultValue, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputFieldsDefinition(GraphQLInputFieldsDefinition inputFieldsDefinition, TContext context)
    {
        await Visit(inputFieldsDefinition.Comment, context).ConfigureAwait(false);
        await Visit(inputFieldsDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
    {
        await Visit(interfaceTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
    {
        await Visit(unionTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionMemberTypes(GraphQLUnionMemberTypes unionMemberTypes, TContext context)
    {
        await Visit(unionMemberTypes.Comment, context).ConfigureAwait(false);
        await Visit(unionMemberTypes.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
    {
        await Visit(enumTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
    {
        await Visit(enumValueDefinition.Comment, context).ConfigureAwait(false);
        await Visit(enumValueDefinition.Description, context).ConfigureAwait(false);
        await Visit(enumValueDefinition.EnumValue, context).ConfigureAwait(false);
        await Visit(enumValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumValuesDefinition(GraphQLEnumValuesDefinition enumValuesDefinition, TContext context)
    {
        await Visit(enumValuesDefinition.Comment, context).ConfigureAwait(false);
        await Visit(enumValuesDefinition.Items, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
    {
        await Visit(inputObjectTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Description, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
    {
        await Visit(directiveDefinition.Comment, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Description, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Name, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Arguments, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Locations, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Visits <see cref="GraphQLDirectiveLocations"/> node.
    /// </summary>
    public virtual async ValueTask VisitDirectiveLocations(GraphQLDirectiveLocations directiveLocations, TContext context)
    {
        await Visit(directiveLocations.Comment, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitSchemaExtension(GraphQLSchemaExtension schemaExtension, TContext context)
    {
        await Visit(schemaExtension.Comment, context).ConfigureAwait(false);
        await Visit(schemaExtension.Directives, context).ConfigureAwait(false);
        await Visit(schemaExtension.OperationTypes, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitScalarTypeExtension(GraphQLScalarTypeExtension scalarTypeExtension, TContext context)
    {
        await Visit(scalarTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(scalarTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(scalarTypeExtension.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitObjectTypeExtension(GraphQLObjectTypeExtension objectTypeExtension, TContext context)
    {
        await Visit(objectTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Interfaces, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInterfaceTypeExtension(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context)
    {
        await Visit(interfaceTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Interfaces, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitUnionTypeExtension(GraphQLUnionTypeExtension unionTypeExtension, TContext context)
    {
        await Visit(unionTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(unionTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(unionTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(unionTypeExtension.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitEnumTypeExtension(GraphQLEnumTypeExtension enumTypeExtension, TContext context)
    {
        await Visit(enumTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(enumTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(enumTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(enumTypeExtension.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask VisitInputObjectTypeExtension(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context)
    {
        await Visit(inputObjectTypeExtension.Comment, context).ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Dispatches node to the appropriate VisitXXX method.
    /// </summary>
    /// <param name="node">AST node to dispatch.</param>
    /// <param name="context">Context passed into all INodeVisitor.VisitXXX methods.</param>
    public virtual ValueTask Visit(ASTNode? node, TContext context)
    {
        return node == null
            ? default
            : node switch
            {
                GraphQLArgument argument => VisitArgument(argument, context),
                GraphQLComment comment => VisitComment(comment, context),
                GraphQLDescription description => VisitDescription(description, context),
                GraphQLDirective directive => VisitDirective(directive, context),
                GraphQLDirectiveDefinition directiveDefinition => VisitDirectiveDefinition(directiveDefinition, context),
                GraphQLDirectiveLocations directiveLocations => VisitDirectiveLocations(directiveLocations, context),
                GraphQLDocument document => VisitDocument(document, context),
                GraphQLEnumTypeDefinition enumTypeDefinition => VisitEnumTypeDefinition(enumTypeDefinition, context),
                GraphQLEnumValueDefinition enumValueDefinition => VisitEnumValueDefinition(enumValueDefinition, context),
                GraphQLAlias alias => VisitAlias(alias, context),
                GraphQLField field => VisitField(field, context),
                GraphQLFieldDefinition fieldDefinition => VisitFieldDefinition(fieldDefinition, context),
                GraphQLFragmentDefinition fragmentDefinition => VisitFragmentDefinition(fragmentDefinition, context), // inherits from GraphQLInlineFragment so should be above
                GraphQLFragmentSpread fragmentSpread => VisitFragmentSpread(fragmentSpread, context),
                GraphQLInlineFragment inlineFragment => VisitInlineFragment(inlineFragment, context),
                GraphQLFragmentName fragmentName => VisitFragmentName(fragmentName, context),
                GraphQLTypeCondition typeCondition => VisitTypeCondition(typeCondition, context),
                GraphQLInputObjectTypeDefinition inputObjectTypeDefinition => VisitInputObjectTypeDefinition(inputObjectTypeDefinition, context),
                GraphQLInputValueDefinition inputValueDefinition => VisitInputValueDefinition(inputValueDefinition, context),
                GraphQLInterfaceTypeDefinition interfaceTypeDefinition => VisitInterfaceTypeDefinition(interfaceTypeDefinition, context),
                GraphQLListType listType => VisitListType(listType, context),
                GraphQLListValue listValue => VisitListValue(listValue, context),
                GraphQLName name => VisitName(name, context),
                GraphQLNamedType namedType => VisitNamedType(namedType, context),
                GraphQLNonNullType nonNullType => VisitNonNullType(nonNullType, context),
                GraphQLObjectField objectField => VisitObjectField(objectField, context),
                GraphQLObjectTypeDefinition objectTypeDefinition => VisitObjectTypeDefinition(objectTypeDefinition, context),
                GraphQLObjectValue objectValue => VisitObjectValue(objectValue, context),
                GraphQLOperationDefinition operationDefinition => VisitOperationDefinition(operationDefinition, context),
                GraphQLRootOperationTypeDefinition rootOperationTypeDefinition => VisitRootOperationTypeDefinition(rootOperationTypeDefinition, context),
                GraphQLScalarTypeDefinition scalarTypeDefinition => VisitScalarTypeDefinition(scalarTypeDefinition, context),
                GraphQLBooleanValue boolValue => VisitBooleanValue(boolValue, context),
                GraphQLEnumValue enumValue => VisitEnumValue(enumValue, context),
                GraphQLFloatValue floatValue => VisitFloatValue(floatValue, context),
                GraphQLIntValue intValue => VisitIntValue(intValue, context),
                GraphQLNullValue nullValue => VisitNullValue(nullValue, context),
                GraphQLStringValue stringValue => VisitStringValue(stringValue, context),
                GraphQLSchemaDefinition schemaDefinition => VisitSchemaDefinition(schemaDefinition, context),
                GraphQLSelectionSet selectionSet => VisitSelectionSet(selectionSet, context),
                GraphQLSchemaExtension schemaEx => VisitSchemaExtension(schemaEx, context),
                GraphQLScalarTypeExtension scalarEx => VisitScalarTypeExtension(scalarEx, context),
                GraphQLObjectTypeExtension objectEx => VisitObjectTypeExtension(objectEx, context),
                GraphQLInterfaceTypeExtension ifaceEx => VisitInterfaceTypeExtension(ifaceEx, context),
                GraphQLUnionTypeExtension unionEx => VisitUnionTypeExtension(unionEx, context),
                GraphQLEnumTypeExtension enumEx => VisitEnumTypeExtension(enumEx, context),
                GraphQLInputObjectTypeExtension inputEx => VisitInputObjectTypeExtension(inputEx, context),
                GraphQLUnionTypeDefinition unionTypeDefinition => VisitUnionTypeDefinition(unionTypeDefinition, context),
                GraphQLUnionMemberTypes unionMembers => VisitUnionMemberTypes(unionMembers, context),
                GraphQLVariable variable => VisitVariable(variable, context),
                GraphQLVariableDefinition variableDefinition => VisitVariableDefinition(variableDefinition, context),
                GraphQLArgumentsDefinition argsDef => VisitArgumentsDefinition(argsDef, context),
                GraphQLArguments args => VisitArguments(args, context),
                GraphQLInputFieldsDefinition inputFieldsDef => VisitInputFieldsDefinition(inputFieldsDef, context),
                GraphQLDirectives directives => VisitDirectives(directives, context),
                GraphQLVariablesDefinition varsDef => VisitVariablesDefinition(varsDef, context),
                GraphQLEnumValuesDefinition enumValuesDef => VisitEnumValuesDefinition(enumValuesDef, context),
                GraphQLFieldsDefinition fieldsDef => VisitFieldsDefinition(fieldsDef, context),
                GraphQLImplementsInterfaces implements => VisitImplementsInterfaces(implements, context),
                _ => throw new NotSupportedException($"Unknown node '{node.GetType().Name}'."),
            };
    }

    /// <summary>
    /// Visits all nodes from the provided list. As a rule, these are nested
    /// sibling nodes of some parent node, for example, argument nodes for
    /// parent field node or value nodes for parent list node.
    /// </summary>
    protected async ValueTask Visit<T>(List<T>? nodes, TContext context)
        where T : ASTNode
    {
        if (nodes != null)
        {
            // Visitor may change AST while being traversed so foreach yields
            // System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
            for (int i = 0; i < nodes.Count; ++i)
                await Visit(nodes[i], context).ConfigureAwait(false);
        }
    }
}
