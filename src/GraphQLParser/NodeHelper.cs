using System.Runtime.CompilerServices;
using GraphQLParser.AST;

namespace GraphQLParser
{
    internal static class NodeHelper
    {
        #region ASTNodes that can not have comments, only locations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GraphQLComment CreateGraphQLComment(IgnoreOptions options)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLComment(),
                IgnoreOptions.Comments => new GraphQLCommentWithLocation(),
                IgnoreOptions.Locations => new GraphQLComment(),
                _ => new GraphQLCommentWithLocation(),
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
        public static GraphQLDescription CreateGraphQLDescription(IgnoreOptions options)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLDescription(),
                IgnoreOptions.Comments => new GraphQLDescriptionWithLocation(),
                IgnoreOptions.Locations => new GraphQLDescription(),
                _ => new GraphQLDescriptionWithLocation(),
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
        public static GraphQLScalarValue CreateGraphQLScalarValue(IgnoreOptions options, ASTNodeKind kind)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLScalarValue(kind),
                IgnoreOptions.Comments => new GraphQLScalarValueWithLocation(kind),
                IgnoreOptions.Locations => new GraphQLScalarValueWithComment(kind),
                _ => new GraphQLScalarValueFull(kind),
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
        public static GraphQLName CreateGraphQLName(IgnoreOptions options)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLName(),
                IgnoreOptions.Comments => new GraphQLNameWithLocation(),
                IgnoreOptions.Locations => new GraphQLNameWithComment(),
                _ => new GraphQLNameFull(),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GraphQLListValue CreateGraphQLListValue(IgnoreOptions options, ASTNodeKind kind)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLListValue(kind),
                IgnoreOptions.Comments => new GraphQLListValueWithLocation(kind),
                IgnoreOptions.Locations => new GraphQLListValueWithComment(kind),
                _ => new GraphQLListValueFull(kind),
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
        public static GraphQLTypeExtensionDefinition CreateGraphQLTypeExtensionDefinition(IgnoreOptions options)
        {
            return options switch
            {
                IgnoreOptions.All => new GraphQLTypeExtensionDefinition(),
                IgnoreOptions.Comments => new GraphQLTypeExtensionDefinitionWithLocation(),
                IgnoreOptions.Locations => new GraphQLTypeExtensionDefinitionWithComment(),
                _ => new GraphQLTypeExtensionDefinitionFull(),
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

        #endregion
    }
}
