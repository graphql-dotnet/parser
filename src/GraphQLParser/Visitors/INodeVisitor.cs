using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Visitor which methods are called when traversing AST.
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
/// </summary>
public interface INodeVisitor<TContext>
    where TContext : INodeVisitorContext
{
    /// <summary>
    /// Visits <see cref="GraphQLName"/> node.
    /// </summary>
    ValueTask VisitNameAsync(GraphQLName name, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDocument"/> node.
    /// </summary>
    ValueTask VisitDocumentAsync(GraphQLDocument document, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLOperationDefinition"/> node.
    /// </summary>
    ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLVariableDefinition"/> node.
    /// </summary>
    ValueTask VisitVariableDefinitionAsync(GraphQLVariableDefinition variableDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLVariablesDefinition"/> node.
    /// </summary>
    ValueTask VisitVariablesDefinitionAsync(GraphQLVariablesDefinition variablesDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLVariable"/> node.
    /// </summary>
    ValueTask VisitVariableAsync(GraphQLVariable variable, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLSelectionSet"/> node.
    /// </summary>
    ValueTask VisitSelectionSetAsync(GraphQLSelectionSet selectionSet, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLField"/> node.
    /// </summary>
    ValueTask VisitFieldAsync(GraphQLField field, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLArgument"/> node.
    /// </summary>
    ValueTask VisitArgumentAsync(GraphQLArgument argument, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLArgumentsDefinition"/> node.
    /// </summary>
    ValueTask VisitArgumentsDefinitionAsync(GraphQLArgumentsDefinition argumentsDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLArguments"/> node.
    /// </summary>
    ValueTask VisitArgumentsAsync(GraphQLArguments arguments, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFragmentSpread"/> node.
    /// </summary>
    ValueTask VisitFragmentSpreadAsync(GraphQLFragmentSpread fragmentSpread, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInlineFragment"/> node.
    /// </summary>
    ValueTask VisitInlineFragmentAsync(GraphQLInlineFragment inlineFragment, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFragmentDefinition"/> node.
    /// </summary>
    ValueTask VisitFragmentDefinitionAsync(GraphQLFragmentDefinition fragmentDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFragmentName"/> node.
    /// </summary>
    ValueTask VisitFragmentNameAsync(GraphQLFragmentName fragmentName, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLIntValue"/> node.
    /// </summary>
    ValueTask VisitIntValueAsync(GraphQLIntValue intValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFloatValue"/> node.
    /// </summary>
    ValueTask VisitFloatValueAsync(GraphQLFloatValue floatValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLStringValue"/> node.
    /// </summary>
    ValueTask VisitStringValueAsync(GraphQLStringValue stringValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLBooleanValue"/> node.
    /// </summary>
    ValueTask VisitBooleanValueAsync(GraphQLBooleanValue booleanValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLEnumValue"/> node.
    /// </summary>
    ValueTask VisitEnumValueAsync(GraphQLEnumValue enumValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLListValue"/> node.
    /// </summary>
    ValueTask VisitListValueAsync(GraphQLListValue listValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLObjectValue"/> node.
    /// </summary>
    ValueTask VisitObjectValueAsync(GraphQLObjectValue objectValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLObjectField"/> node.
    /// </summary>
    ValueTask VisitObjectFieldAsync(GraphQLObjectField objectField, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDirective"/> node.
    /// </summary>
    ValueTask VisitDirectiveAsync(GraphQLDirective directive, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDirectives"/> node.
    /// </summary>
    ValueTask VisitDirectivesAsync(GraphQLDirectives directives, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLNamedType"/> node.
    /// </summary>
    ValueTask VisitNamedTypeAsync(GraphQLNamedType namedType, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLListType"/> node.
    /// </summary>
    ValueTask VisitListTypeAsync(GraphQLListType listType, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLNonNullType"/> node.
    /// </summary>
    ValueTask VisitNonNullTypeAsync(GraphQLNonNullType nonNullType, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLNullValue"/> node.
    /// </summary>
    ValueTask VisitNullValueAsync(GraphQLNullValue nullValue, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLSchemaDefinition"/> node.
    /// </summary>
    ValueTask VisitSchemaDefinitionAsync(GraphQLSchemaDefinition schemaDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLRootOperationTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitRootOperationTypeDefinitionAsync(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLScalarTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLObjectTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitObjectTypeDefinitionAsync(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFieldDefinition"/> node.
    /// </summary>
    ValueTask VisitFieldDefinitionAsync(GraphQLFieldDefinition fieldDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLFieldsDefinition"/> node.
    /// </summary>
    ValueTask VisitFieldsDefinitionAsync(GraphQLFieldsDefinition fieldsDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInputValueDefinition"/> node.
    /// </summary>
    ValueTask VisitInputValueDefinitionAsync(GraphQLInputValueDefinition inputValueDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInputFieldsDefinition"/> node.
    /// </summary>
    ValueTask VisitInputFieldsDefinitionAsync(GraphQLInputFieldsDefinition inputFieldsDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInterfaceTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitInterfaceTypeDefinitionAsync(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLUnionTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitUnionTypeDefinitionAsync(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLUnionMemberTypes"/> node.
    /// </summary>
    ValueTask VisitUnionMemberTypesAsync(GraphQLUnionMemberTypes unionMemberTypes, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLEnumTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitEnumTypeDefinitionAsync(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLEnumValueDefinition"/> node.
    /// </summary>
    ValueTask VisitEnumValueDefinitionAsync(GraphQLEnumValueDefinition enumValueDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLEnumValuesDefinition"/> node.
    /// </summary>
    ValueTask VisitEnumValuesDefinitionAsync(GraphQLEnumValuesDefinition enumValuesDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInputObjectTypeDefinition"/> node.
    /// </summary>
    ValueTask VisitInputObjectTypeDefinitionAsync(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDirectiveDefinition"/> node.
    /// </summary>
    ValueTask VisitDirectiveDefinitionAsync(GraphQLDirectiveDefinition directiveDefinition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDirectiveLocations"/> node.
    /// </summary>
    ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLComment"/> node.
    /// </summary>
    ValueTask VisitCommentAsync(GraphQLComment comment, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLDescription"/> node.
    /// </summary>
    ValueTask VisitDescriptionAsync(GraphQLDescription description, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLTypeCondition"/> node.
    /// </summary>
    ValueTask VisitTypeConditionAsync(GraphQLTypeCondition typeCondition, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLImplementsInterfaces"/> node.
    /// </summary>
    ValueTask VisitImplementsInterfacesAsync(GraphQLImplementsInterfaces implementsInterfaces, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLAlias"/> node.
    /// </summary>
    ValueTask VisitAliasAsync(GraphQLAlias alias, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLSchemaExtension"/> node.
    /// </summary>
    ValueTask VisitSchemaExtensionAsync(GraphQLSchemaExtension schemaExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLScalarTypeExtension"/> node.
    /// </summary>
    ValueTask VisitScalarTypeExtensionAsync(GraphQLScalarTypeExtension scalarTypeExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLObjectTypeExtension"/> node.
    /// </summary>
    ValueTask VisitObjectTypeExtensionAsync(GraphQLObjectTypeExtension objectTypeExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInterfaceTypeExtension"/> node.
    /// </summary>
    ValueTask VisitInterfaceTypeExtensionAsync(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLUnionTypeExtension"/> node.
    /// </summary>
    ValueTask VisitUnionTypeExtensionAsync(GraphQLUnionTypeExtension unionTypeExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLEnumTypeExtension"/> node.
    /// </summary>
    ValueTask VisitEnumTypeExtensionAsync(GraphQLEnumTypeExtension enumTypeExtension, TContext context);

    /// <summary>
    /// Visits <see cref="GraphQLInputObjectTypeExtension"/> node.
    /// </summary>
    ValueTask VisitInputObjectTypeExtensionAsync(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context);
}
