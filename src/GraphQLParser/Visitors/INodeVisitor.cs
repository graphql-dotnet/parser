using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
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
        ValueTask VisitName(GraphQLName name, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDocument"/> node.
        /// </summary>
        ValueTask VisitDocument(GraphQLDocument document, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLOperationDefinition"/> node.
        /// </summary>
        ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLVariableDefinition"/> node.
        /// </summary>
        ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLVariable"/> node.
        /// </summary>
        ValueTask VisitVariable(GraphQLVariable variable, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLSelectionSet"/> node.
        /// </summary>
        ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLField"/> node.
        /// </summary>
        ValueTask VisitField(GraphQLField field, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLArgument"/> node.
        /// </summary>
        ValueTask VisitArgument(GraphQLArgument argument, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFragmentSpread"/> node.
        /// </summary>
        ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInlineFragment"/> node.
        /// </summary>
        ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFragmentDefinition"/> node.
        /// </summary>
        ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitIntValue(GraphQLScalarValue intValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitFloatValue(GraphQLScalarValue floatValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitStringValue(GraphQLScalarValue stringValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitEnumValue(GraphQLScalarValue enumValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLListValue"/> node.
        /// </summary>
        ValueTask VisitListValue(GraphQLListValue listValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectValue"/> node.
        /// </summary>
        ValueTask VisitObjectValue(GraphQLObjectValue objectValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectField"/> node.
        /// </summary>
        ValueTask VisitObjectField(GraphQLObjectField objectField, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDirective"/> node.
        /// </summary>
        ValueTask VisitDirective(GraphQLDirective directive, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLNamedType"/> node.
        /// </summary>
        ValueTask VisitNamedType(GraphQLNamedType namedType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLListType"/> node.
        /// </summary>
        ValueTask VisitListType(GraphQLListType listType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLNonNullType"/> node.
        /// </summary>
        ValueTask VisitNonNullType(GraphQLNonNullType nonNullType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        ValueTask VisitNullValue(GraphQLScalarValue nullValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLSchemaDefinition"/> node.
        /// </summary>
        ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLRootOperationTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFieldDefinition"/> node.
        /// </summary>
        ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInputValueDefinition"/> node.
        /// </summary>
        ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInterfaceTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLUnionTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLEnumTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLEnumValueDefinition"/> node.
        /// </summary>
        ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInputObjectTypeDefinition"/> node.
        /// </summary>
        ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDirectiveDefinition"/> node.
        /// </summary>
        ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLComment"/> node.
        /// </summary>
        ValueTask VisitComment(GraphQLComment comment, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDescription"/> node.
        /// </summary>
        ValueTask VisitDescription(GraphQLDescription description, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLTypeCondition"/> node.
        /// </summary>
        ValueTask VisitTypeCondition(GraphQLTypeCondition typeCondition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLAlias"/> node.
        /// </summary>
        ValueTask VisitAlias(GraphQLAlias alias, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarTypeExtension"/> node.
        /// </summary>
        ValueTask VisitScalarTypeExtension(GraphQLScalarTypeExtension scalarTypeExtension, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectTypeExtension"/> node.
        /// </summary>
        ValueTask VisitObjectTypeExtension(GraphQLObjectTypeExtension objectTypeExtension, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInterfaceTypeExtension"/> node.
        /// </summary>
        ValueTask VisitInterfaceTypeExtension(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLUnionTypeExtension"/> node.
        /// </summary>
        ValueTask VisitUnionTypeExtension(GraphQLUnionTypeExtension unionTypeExtension, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLEnumTypeExtension"/> node.
        /// </summary>
        ValueTask VisitEnumTypeExtension(GraphQLEnumTypeExtension enumTypeExtension, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInputObjectTypeExtension"/> node.
        /// </summary>
        ValueTask VisitInputObjectTypeExtension(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context);
    }
}
