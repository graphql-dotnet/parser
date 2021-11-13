using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
    /// <summary>
    /// Visitor which methods are called when traversing AST.
    /// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
    /// </summary>
    public interface INodeVisitor<TContext>
        where TContext : IVisitorContext
    {
        /// <summary>
        /// Visits <see cref="GraphQLName"/> node.
        /// </summary>
        void VisitName(GraphQLName name, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDocument"/> node.
        /// </summary>
        void VisitDocument(GraphQLDocument document, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLOperationDefinition"/> node.
        /// </summary>
        void VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLVariableDefinition"/> node.
        /// </summary>
        void VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLVariable"/> node.
        /// </summary>
        void VisitVariable(GraphQLVariable variable, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLSelectionSet"/> node.
        /// </summary>
        void VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLField"/> node.
        /// </summary>
        void VisitField(GraphQLField field, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLArgument"/> node.
        /// </summary>
        void VisitArgument(GraphQLArgument argument, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFragmentSpread"/> node.
        /// </summary>
        void VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInlineFragment"/> node.
        /// </summary>
        void VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFragmentDefinition"/> node.
        /// </summary>
        void VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitIntValue(GraphQLScalarValue intValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitFloatValue(GraphQLScalarValue floatValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitStringValue(GraphQLScalarValue stringValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitEnumValue(GraphQLScalarValue enumValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLListValue"/> node.
        /// </summary>
        void VisitListValue(GraphQLListValue listValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectValue"/> node.
        /// </summary>
        void VisitObjectValue(GraphQLObjectValue objectValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectField"/> node.
        /// </summary>
        void VisitObjectField(GraphQLObjectField objectField, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDirective"/> node.
        /// </summary>
        void VisitDirective(GraphQLDirective directive, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLNamedType"/> node.
        /// </summary>
        void VisitNamedType(GraphQLNamedType namedType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLListType"/> node.
        /// </summary>
        void VisitListType(GraphQLListType listType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLNonNullType"/> node.
        /// </summary>
        void VisitNonNullType(GraphQLNonNullType nonNullType, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarValue"/> node.
        /// </summary>
        void VisitNullValue(GraphQLScalarValue nullValue, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLSchemaDefinition"/> node.
        /// </summary>
        void VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLRootOperationTypeDefinition"/> node.
        /// </summary>
        void VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLScalarTypeDefinition"/> node.
        /// </summary>
        void VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLObjectTypeDefinition"/> node.
        /// </summary>
        void VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLFieldDefinition"/> node.
        /// </summary>
        void VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInputValueDefinition"/> node.
        /// </summary>
        void VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInterfaceTypeDefinition"/> node.
        /// </summary>
        void VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLUnionTypeDefinition"/> node.
        /// </summary>
        void VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLEnumTypeDefinition"/> node.
        /// </summary>
        void VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLEnumValueDefinition"/> node.
        /// </summary>
        void VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLInputObjectTypeDefinition"/> node.
        /// </summary>
        void VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context);

        // <summary>
        // Visits <see cref=""/> node.
        // </summary>
        //void VisitTypeExtensionDefinition(); //TODO

        /// <summary>
        /// Visits <see cref="GraphQLDirectiveDefinition"/> node.
        /// </summary>
        void VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLComment"/> node.
        /// </summary>
        void VisitComment(GraphQLComment comment, TContext context);

        /// <summary>
        /// Visits <see cref="GraphQLDescription"/> node.
        /// </summary>
        void VisitDescription(GraphQLDescription description, TContext context);
    }
}
