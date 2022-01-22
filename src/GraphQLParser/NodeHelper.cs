using System.Runtime.CompilerServices;
using GraphQLParser.AST;

namespace GraphQLParser;

internal static class NodeHelper
{
    #region ASTNodes that can not have comments, only locations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLComment CreateGraphQLComment(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLComment(value),
            IgnoreOptions.Comments => new GraphQLCommentWithLocation(value),
            IgnoreOptions.Locations => new GraphQLComment(value),
            _ => new GraphQLCommentWithLocation(value),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLDocument CreateGraphQLDocument(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDocument(),
            IgnoreOptions.Comments => new GraphQLDocumentWithLocation(),
            IgnoreOptions.Locations => new GraphQLDocument(),
            _ => new GraphQLDocumentWithLocation(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLDescription CreateGraphQLDescription(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDescription(value),
            IgnoreOptions.Comments => new GraphQLDescriptionWithLocation(value),
            IgnoreOptions.Locations => new GraphQLDescription(value),
            _ => new GraphQLDescriptionWithLocation(value),
        };
    }

    // Directives go one after another without any "list prefix", so it is impossible
    // to distinguish the comment of the first directive from the comment to the entire
    // list of directives. Therefore, a comment for the directive itself is used.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLDirectives CreateGraphQLDirectives(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDirectives(),
            IgnoreOptions.Comments => new GraphQLDirectivesWithLocation(),
            IgnoreOptions.Locations => new GraphQLDirectives(),
            _ => new GraphQLDirectivesWithLocation(),
        };
    }

    #endregion

    #region ASTNodes that can have comments and locations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLArgument CreateGraphQLArgument(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLArgument(),
            IgnoreOptions.Comments => new GraphQLArgumentWithLocation(),
            IgnoreOptions.Locations => new GraphQLArgumentWithComment(),
            _ => new GraphQLArgumentFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLDirective CreateGraphQLDirective(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDirective(),
            IgnoreOptions.Comments => new GraphQLDirectiveWithLocation(),
            IgnoreOptions.Locations => new GraphQLDirectiveWithComment(),
            _ => new GraphQLDirectiveFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLVariableDefinition CreateGraphQLVariableDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLVariableDefinition(),
            IgnoreOptions.Comments => new GraphQLVariableDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLVariableDefinitionWithComment(),
            _ => new GraphQLVariableDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLEnumTypeDefinition CreateGraphQLEnumTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLEnumTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLEnumTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLEnumTypeDefinitionWithComment(),
            _ => new GraphQLEnumTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLEnumValueDefinition CreateGraphQLEnumValueDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLEnumValueDefinition(),
            IgnoreOptions.Comments => new GraphQLEnumValueDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLEnumValueDefinitionWithComment(),
            _ => new GraphQLEnumValueDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLFieldDefinition CreateGraphQLFieldDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFieldDefinition(),
            IgnoreOptions.Comments => new GraphQLFieldDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLFieldDefinitionWithComment(),
            _ => new GraphQLFieldDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLSelectionSet CreateGraphQLSelectionSet(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLSelectionSet(),
            IgnoreOptions.Comments => new GraphQLSelectionSetWithLocation(),
            IgnoreOptions.Locations => new GraphQLSelectionSetWithComment(),
            _ => new GraphQLSelectionSetFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLVariable CreateGraphQLVariable(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLVariable(),
            IgnoreOptions.Comments => new GraphQLVariableWithLocation(),
            IgnoreOptions.Locations => new GraphQLVariableWithComment(),
            _ => new GraphQLVariableFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLRootOperationTypeDefinition CreateGraphQLOperationTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLRootOperationTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLRootOperationTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLRootOperationTypeDefinitionWithComment(),
            _ => new GraphQLRootOperationTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLNamedType CreateGraphQLNamedType(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLNamedType(),
            IgnoreOptions.Comments => new GraphQLNamedTypeWithLocation(),
            IgnoreOptions.Locations => new GraphQLNamedTypeWithComment(),
            _ => new GraphQLNamedTypeFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLName CreateGraphQLName(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLName(value),
            IgnoreOptions.Comments => new GraphQLNameWithLocation(value),
            IgnoreOptions.Locations => new GraphQLNameWithComment(value),
            _ => new GraphQLNameFull(value),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLListValue CreateGraphQLListValue(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLListValue(),
            IgnoreOptions.Comments => new GraphQLListValueWithLocation(),
            IgnoreOptions.Locations => new GraphQLListValueWithComment(),
            _ => new GraphQLListValueFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLListType CreateGraphQLListType(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLListType(),
            IgnoreOptions.Comments => new GraphQLListTypeWithLocation(),
            IgnoreOptions.Locations => new GraphQLListTypeWithComment(),
            _ => new GraphQLListTypeFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLDirectiveDefinition CreateGraphQLDirectiveDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDirectiveDefinition(),
            IgnoreOptions.Comments => new GraphQLDirectiveDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLDirectiveDefinitionWithComment(),
            _ => new GraphQLDirectiveDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLField CreateGraphQLField(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLField(),
            IgnoreOptions.Comments => new GraphQLFieldWithLocation(),
            IgnoreOptions.Locations => new GraphQLFieldWithComment(),
            _ => new GraphQLFieldFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLFragmentDefinition CreateGraphQLFragmentDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFragmentDefinition(),
            IgnoreOptions.Comments => new GraphQLFragmentDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLFragmentDefinitionWithComment(),
            _ => new GraphQLFragmentDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLUnionTypeDefinition CreateGraphQLUnionTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLUnionTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLUnionTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLUnionTypeDefinitionWithComment(),
            _ => new GraphQLUnionTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLObjectTypeExtension CreateGraphQLObjectTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLObjectTypeExtension(),
            IgnoreOptions.Comments => new GraphQLObjectTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLObjectTypeExtensionWithComment(),
            _ => new GraphQLObjectTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLScalarTypeExtension CreateGraphQLScalarTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLScalarTypeExtension(),
            IgnoreOptions.Comments => new GraphQLScalarTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLScalarTypeExtensionWithComment(),
            _ => new GraphQLScalarTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInterfaceTypeExtension CreateGraphQLInterfaceTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInterfaceTypeExtension(),
            IgnoreOptions.Comments => new GraphQLInterfaceTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInterfaceTypeExtensionWithComment(),
            _ => new GraphQLInterfaceTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLUnionTypeExtension CreateGraphQLUnionTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLUnionTypeExtension(),
            IgnoreOptions.Comments => new GraphQLUnionTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLUnionTypeExtensionWithComment(),
            _ => new GraphQLUnionTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLEnumTypeExtension CreateGraphQLEnumTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLEnumTypeExtension(),
            IgnoreOptions.Comments => new GraphQLEnumTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLEnumTypeExtensionWithComment(),
            _ => new GraphQLEnumTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInputObjectTypeExtension CreateGraphQLInputObjectTypeExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInputObjectTypeExtension(),
            IgnoreOptions.Comments => new GraphQLInputObjectTypeExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInputObjectTypeExtensionWithComment(),
            _ => new GraphQLInputObjectTypeExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLSchemaExtension CreateGraphQLSchemaExtension(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLSchemaExtension(),
            IgnoreOptions.Comments => new GraphQLSchemaExtensionWithLocation(),
            IgnoreOptions.Locations => new GraphQLSchemaExtensionWithComment(),
            _ => new GraphQLSchemaExtensionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLFragmentSpread CreateGraphQLFragmentSpread(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFragmentSpread(),
            IgnoreOptions.Comments => new GraphQLFragmentSpreadWithLocation(),
            IgnoreOptions.Locations => new GraphQLFragmentSpreadWithComment(),
            _ => new GraphQLFragmentSpreadFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInlineFragment CreateGraphQLInlineFragment(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInlineFragment(),
            IgnoreOptions.Comments => new GraphQLInlineFragmentWithLocation(),
            IgnoreOptions.Locations => new GraphQLInlineFragmentWithComment(),
            _ => new GraphQLInlineFragmentFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInputObjectTypeDefinition CreateGraphQLInputObjectTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInputObjectTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLInputObjectTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInputObjectTypeDefinitionWithComment(),
            _ => new GraphQLInputObjectTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInterfaceTypeDefinition CreateGraphQLInterfaceTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInterfaceTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLInterfaceTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInterfaceTypeDefinitionWithComment(),
            _ => new GraphQLInterfaceTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLNonNullType CreateGraphQLNonNullType(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLNonNullType(),
            IgnoreOptions.Comments => new GraphQLNonNullTypeWithLocation(),
            IgnoreOptions.Locations => new GraphQLNonNullTypeWithComment(),
            _ => new GraphQLNonNullTypeFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLSchemaDefinition CreateGraphQLSchemaDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLSchemaDefinition(),
            IgnoreOptions.Comments => new GraphQLSchemaDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLSchemaDefinitionWithComment(),
            _ => new GraphQLSchemaDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLScalarTypeDefinition CreateGraphQLScalarTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLScalarTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLScalarTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLScalarTypeDefinitionWithComment(),
            _ => new GraphQLScalarTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLOperationDefinition CreateGraphQLOperationDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLOperationDefinition(),
            IgnoreOptions.Comments => new GraphQLOperationDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLOperationDefinitionWithComment(),
            _ => new GraphQLOperationDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLObjectTypeDefinition CreateGraphQLObjectTypeDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLObjectTypeDefinition(),
            IgnoreOptions.Comments => new GraphQLObjectTypeDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLObjectTypeDefinitionWithComment(),
            _ => new GraphQLObjectTypeDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLObjectField CreateGraphQLObjectField(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLObjectField(),
            IgnoreOptions.Comments => new GraphQLObjectFieldWithLocation(),
            IgnoreOptions.Locations => new GraphQLObjectFieldWithComment(),
            _ => new GraphQLObjectFieldFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLObjectValue CreateGraphQLObjectValue(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLObjectValue(),
            IgnoreOptions.Comments => new GraphQLObjectValueWithLocation(),
            IgnoreOptions.Locations => new GraphQLObjectValueWithComment(),
            _ => new GraphQLObjectValueFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInputValueDefinition CreateGraphQLInputValueDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInputValueDefinition(),
            IgnoreOptions.Comments => new GraphQLInputValueDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInputValueDefinitionWithComment(),
            _ => new GraphQLInputValueDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLTypeCondition CreateGraphQLTypeCondition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLTypeCondition(),
            IgnoreOptions.Comments => new GraphQLTypeConditionWithLocation(),
            IgnoreOptions.Locations => new GraphQLTypeConditionWithComment(),
            _ => new GraphQLTypeConditionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLAlias CreateGraphQLAlias(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLAlias(),
            IgnoreOptions.Comments => new GraphQLAliasWithLocation(),
            IgnoreOptions.Locations => new GraphQLAliasWithComment(),
            _ => new GraphQLAliasFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLArgumentsDefinition CreateGraphQLArgumentsDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLArgumentsDefinition(),
            IgnoreOptions.Comments => new GraphQLArgumentsDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLArgumentsDefinitionWithComment(),
            _ => new GraphQLArgumentsDefinitionFull(),
        };
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLArguments CreateGraphQLArguments(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLArguments(),
            IgnoreOptions.Comments => new GraphQLArgumentsWithLocation(),
            IgnoreOptions.Locations => new GraphQLArgumentsWithComment(),
            _ => new GraphQLArgumentsFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLInputFieldsDefinition CreateGraphQLInputFieldsDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLInputFieldsDefinition(),
            IgnoreOptions.Comments => new GraphQLInputFieldsDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLInputFieldsDefinitionWithComment(),
            _ => new GraphQLInputFieldsDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLVariablesDefinition CreateGraphQLVariablesDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLVariablesDefinition(),
            IgnoreOptions.Comments => new GraphQLVariablesDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLVariablesDefinitionWithComment(),
            _ => new GraphQLVariablesDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLEnumValuesDefinition CreateGraphQLEnumValuesDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLEnumValuesDefinition(),
            IgnoreOptions.Comments => new GraphQLEnumValuesDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLEnumValuesDefinitionWithComment(),
            _ => new GraphQLEnumValuesDefinitionFull(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GraphQLFieldsDefinition CreateGraphQLFieldsDefinition(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFieldsDefinition(),
            IgnoreOptions.Comments => new GraphQLFieldsDefinitionWithLocation(),
            IgnoreOptions.Locations => new GraphQLFieldsDefinitionWithComment(),
            _ => new GraphQLFieldsDefinitionFull(),
        };
    }

    public static GraphQLImplementsInterfaces CreateGraphQLImplementsInterfaces(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLImplementsInterfaces(),
            IgnoreOptions.Comments => new GraphQLImplementsInterfacesWithLocation(),
            IgnoreOptions.Locations => new GraphQLImplementsInterfacesWithComment(),
            _ => new GraphQLImplementsInterfacesFull(),
        };
    }

    public static GraphQLDirectiveLocations CreateGraphQLDirectiveLocations(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLDirectiveLocations(),
            IgnoreOptions.Comments => new GraphQLDirectiveLocationsWithLocation(),
            IgnoreOptions.Locations => new GraphQLDirectiveLocationsWithComment(),
            _ => new GraphQLDirectiveLocationsFull(),
        };
    }

    public static GraphQLUnionMemberTypes CreateGraphQLUnionMemberTypes(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLUnionMemberTypes(),
            IgnoreOptions.Comments => new GraphQLUnionMemberTypesWithLocation(),
            IgnoreOptions.Locations => new GraphQLUnionMemberTypesWithComment(),
            _ => new GraphQLUnionMemberTypesFull(),
        };
    }

    public static GraphQLEnumValue CreateGraphQLEnumValue(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLEnumValue(),
            IgnoreOptions.Comments => new GraphQLEnumValueWithLocation(),
            IgnoreOptions.Locations => new GraphQLEnumValueWithComment(),
            _ => new GraphQLEnumValueFull(),
        };
    }

    public static GraphQLNullValue CreateGraphQLNullValue(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLNullValue(),
            IgnoreOptions.Comments => new GraphQLNullValueWithLocation(),
            IgnoreOptions.Locations => new GraphQLNullValueWithComment(),
            _ => new GraphQLNullValueFull(),
        };
    }

    public static GraphQLBooleanValue CreateGraphQLBooleanValue(IgnoreOptions options, bool value)
    {
        return value switch
        {
            true => options switch
            {
                IgnoreOptions.All => new GraphQLTrueBooleanValue(),
                IgnoreOptions.Comments => new GraphQLTrueBooleanValueWithLocation(),
                IgnoreOptions.Locations => new GraphQLTrueBooleanValueWithComment(),
                _ => new GraphQLTrueBooleanValueFull(),
            },
            false => options switch
            {
                IgnoreOptions.All => new GraphQLFalseBooleanValue(),
                IgnoreOptions.Comments => new GraphQLFalseBooleanValueWithLocation(),
                IgnoreOptions.Locations => new GraphQLFalseBooleanValueWithComment(),
                _ => new GraphQLFalseBooleanValueFull(),
            }
        };
    }

    public static GraphQLFloatValue CreateGraphQLFloatValue(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFloatValue(value),
            IgnoreOptions.Comments => new GraphQLFloatValueWithLocation(value),
            IgnoreOptions.Locations => new GraphQLFloatValueWithComment(value),
            _ => new GraphQLFloatValueFull(value),
        };
    }

    public static GraphQLIntValue CreateGraphQLIntValue(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLIntValue(value),
            IgnoreOptions.Comments => new GraphQLIntValueWithLocation(value),
            IgnoreOptions.Locations => new GraphQLIntValueWithComment(value),
            _ => new GraphQLIntValueFull(value),
        };
    }

    public static GraphQLStringValue CreateGraphQLStringValue(IgnoreOptions options, ROM value)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLStringValue(value),
            IgnoreOptions.Comments => new GraphQLStringValueWithLocation(value),
            IgnoreOptions.Locations => new GraphQLStringValueWithComment(value),
            _ => new GraphQLStringValueFull(value),
        };
    }

    public static GraphQLFragmentName CreateGraphQLFragmentName(IgnoreOptions options)
    {
        return options switch
        {
            IgnoreOptions.All => new GraphQLFragmentName(),
            IgnoreOptions.Comments => new GraphQLFragmentNameWithLocation(),
            IgnoreOptions.Locations => new GraphQLFragmentNameWithComment(),
            _ => new GraphQLFragmentNameFull(),
        };
    }

    #endregion
}
