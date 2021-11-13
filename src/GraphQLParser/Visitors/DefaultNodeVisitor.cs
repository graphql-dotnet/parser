using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
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
            await Visit(document.Definitions, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitArgument(GraphQLArgument argument, TContext context)
        {
            await Visit(argument.Comment, context);
            await Visit(argument.Name, context);
            await Visit(argument.Value, context);
        }

        /// <inheritdoc/>
        public virtual ValueTask VisitComment(GraphQLComment comment, TContext context)
        {
            return new ValueTask(Task.CompletedTask);
        }

        /// <inheritdoc/>
        public virtual ValueTask VisitDescription(GraphQLDescription description, TContext context)
        {
            return new ValueTask(Task.CompletedTask);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
        {
            await Visit(operationDefinition.Comment, context);
            await Visit(operationDefinition.Name, context);
            await Visit(operationDefinition.VariableDefinitions, context);
            await Visit(operationDefinition.Directives, context);
            await Visit(operationDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitName(GraphQLName name, TContext context)
        {
            await Visit(name.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
        {
            await Visit(variableDefinition.Comment, context);
            await Visit(variableDefinition.Variable, context);
            await Visit(variableDefinition.Type, context);
            await Visit(variableDefinition.DefaultValue, context);
            await Visit(variableDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitVariable(GraphQLVariable variable, TContext context)
        {
            await Visit(variable.Comment, context);
            await Visit(variable.Name, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
        {
            await Visit(selectionSet.Comment, context);
            await Visit(selectionSet.Selections, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitField(GraphQLField field, TContext context)
        {
            await Visit(field.Comment, context);
            await Visit(field.Alias, context);
            await Visit(field.Name, context);
            await Visit(field.Arguments, context);
            await Visit(field.Directives, context);
            await Visit(field.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
        {
            await Visit(fragmentSpread.Comment, context);
            await Visit(fragmentSpread.Name, context);
            await Visit(fragmentSpread.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
        {
            await Visit(inlineFragment.Comment, context);
            await Visit(inlineFragment.TypeCondition, context);
            await Visit(inlineFragment.Directives, context);
            await Visit(inlineFragment.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitTypeCondition(GraphQLTypeCondition typeCondition, TContext context)
        {
            await Visit(typeCondition.Comment, context);
            await Visit(typeCondition.Type, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
        {
            await Visit(fragmentDefinition.Comment, context);
            await Visit(fragmentDefinition.Name, context);
            await Visit(fragmentDefinition.TypeCondition, context);
            await Visit(fragmentDefinition.Directives, context);
            await Visit(fragmentDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitIntValue(GraphQLScalarValue intValue, TContext context)
        {
            await Visit(intValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitFloatValue(GraphQLScalarValue floatValue, TContext context)
        {
            await Visit(floatValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitStringValue(GraphQLScalarValue stringValue, TContext context)
        {
            await Visit(stringValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context)
        {
            await Visit(booleanValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitEnumValue(GraphQLScalarValue enumValue, TContext context)
        {
            await Visit(enumValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitListValue(GraphQLListValue listValue, TContext context)
        {
            await Visit(listValue.Comment, context);
            await Visit(listValue.Values, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
        {
            await Visit(objectValue.Comment, context);
            await Visit(objectValue.Fields, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitObjectField(GraphQLObjectField objectField, TContext context)
        {
            await Visit(objectField.Comment, context);
            await Visit(objectField.Name, context);
            await Visit(objectField.Value, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitDirective(GraphQLDirective directive, TContext context)
        {
            await Visit(directive.Comment, context);
            await Visit(directive.Name, context);
            await Visit(directive.Arguments, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitNamedType(GraphQLNamedType namedType, TContext context)
        {
            await Visit(namedType.Comment, context);
            await Visit(namedType.Name, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitListType(GraphQLListType listType, TContext context)
        {
            await Visit(listType.Comment, context);
            await Visit(listType.Type, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
        {
            await Visit(nonNullType.Comment, context);
            await Visit(nonNullType.Type, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitNullValue(GraphQLScalarValue nullValue, TContext context)
        {
            await Visit(nullValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
        {
            await Visit(schemaDefinition.Comment, context);
            await Visit(schemaDefinition.Description, context);
            await Visit(schemaDefinition.Directives, context);
            await Visit(schemaDefinition.OperationTypes, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
        {
            await Visit(rootOperationTypeDefinition.Comment, context);
            await Visit(rootOperationTypeDefinition.Type, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
        {
            await Visit(scalarTypeDefinition.Comment, context);
            await Visit(scalarTypeDefinition.Description, context);
            await Visit(scalarTypeDefinition.Name, context);
            await Visit(scalarTypeDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
        {
            await Visit(objectTypeDefinition.Comment, context);
            await Visit(objectTypeDefinition.Description, context);
            await Visit(objectTypeDefinition.Name, context);
            await Visit(objectTypeDefinition.Interfaces, context);
            await Visit(objectTypeDefinition.Directives, context);
            await Visit(objectTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
        {
            await Visit(fieldDefinition.Comment, context);
            await Visit(fieldDefinition.Description, context);
            await Visit(fieldDefinition.Name, context);
            await Visit(fieldDefinition.Arguments, context);
            await Visit(fieldDefinition.Type, context);
            await Visit(fieldDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
        {
            await Visit(inputValueDefinition.Comment, context);
            await Visit(inputValueDefinition.Description, context);
            await Visit(inputValueDefinition.Name, context);
            await Visit(inputValueDefinition.Type, context);
            await Visit(inputValueDefinition.DefaultValue, context);
            await Visit(inputValueDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
        {
            await Visit(interfaceTypeDefinition.Comment, context);
            await Visit(interfaceTypeDefinition.Description, context);
            await Visit(interfaceTypeDefinition.Name, context);
            await Visit(interfaceTypeDefinition.Interfaces, context);
            await Visit(interfaceTypeDefinition.Directives, context);
            await Visit(interfaceTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
        {
            await Visit(unionTypeDefinition.Comment, context);
            await Visit(unionTypeDefinition.Description, context);
            await Visit(unionTypeDefinition.Name, context);
            await Visit(unionTypeDefinition.Directives, context);
            await Visit(unionTypeDefinition.Types, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
        {
            await Visit(enumTypeDefinition.Comment, context);
            await Visit(enumTypeDefinition.Description, context);
            await Visit(enumTypeDefinition.Name, context);
            await Visit(enumTypeDefinition.Directives, context);
            await Visit(enumTypeDefinition.Values, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
        {
            await Visit(enumValueDefinition.Comment, context);
            await Visit(enumValueDefinition.Description, context);
            await Visit(enumValueDefinition.Name, context);
            await Visit(enumValueDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
        {
            await Visit(inputObjectTypeDefinition.Comment, context);
            await Visit(inputObjectTypeDefinition.Description, context);
            await Visit(inputObjectTypeDefinition.Name, context);
            await Visit(inputObjectTypeDefinition.Directives, context);
            await Visit(inputObjectTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual async ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
        {
            await Visit(directiveDefinition.Comment, context);
            await Visit(directiveDefinition.Description, context);
            await Visit(directiveDefinition.Name, context);
            await Visit(directiveDefinition.Arguments, context);
        }

        /// <summary>
        /// Dispatches node to the appropriate VisitXXX method.
        /// </summary>
        /// <param name="node">AST node to dispatch.</param>
        /// <param name="context">Context passed into all INodeVisitor.VisitXXX methods.</param>
        public virtual ValueTask Visit(ASTNode? node, TContext context)
        {
            if (node == null)
                return new ValueTask(Task.CompletedTask);

            return node switch
            {
                GraphQLArgument argument => VisitArgument(argument, context),
                GraphQLComment comment => VisitComment(comment, context),
                GraphQLDescription description => VisitDescription(description, context),
                GraphQLDirective directive => VisitDirective(directive, context),
                GraphQLDirectiveDefinition directiveDefinition => VisitDirectiveDefinition(directiveDefinition, context),
                GraphQLDocument document => VisitDocument(document, context),
                GraphQLEnumTypeDefinition enumTypeDefinition => VisitEnumTypeDefinition(enumTypeDefinition, context),
                GraphQLEnumValueDefinition enumValueDefinition => VisitEnumValueDefinition(enumValueDefinition, context),
                GraphQLField field => VisitField(field, context),
                GraphQLFieldDefinition fieldDefinition => VisitFieldDefinition(fieldDefinition, context),
                GraphQLFragmentDefinition fragmentDefinition => VisitFragmentDefinition(fragmentDefinition, context), // inherits from GraphQLInlineFragment so should be above
                GraphQLFragmentSpread fragmentSpread => VisitFragmentSpread(fragmentSpread, context),
                GraphQLInlineFragment inlineFragment => VisitInlineFragment(inlineFragment, context),
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
                GraphQLScalarValue scalarValue => scalarValue.Kind switch
                {
                    ASTNodeKind.BooleanValue => VisitBooleanValue(scalarValue, context),
                    ASTNodeKind.EnumValue => VisitEnumValue(scalarValue, context),
                    ASTNodeKind.FloatValue => VisitFloatValue(scalarValue, context),
                    ASTNodeKind.IntValue => VisitIntValue(scalarValue, context),
                    ASTNodeKind.NullValue => VisitNullValue(scalarValue, context),
                    ASTNodeKind.StringValue => VisitStringValue(scalarValue, context),
                    _ => throw new NotSupportedException($"Unknown GraphQLScalarValue of kind '{scalarValue.Kind}'."),

                },
                GraphQLSchemaDefinition schemaDefinition => VisitSchemaDefinition(schemaDefinition, context),
                GraphQLSelectionSet selectionSet => VisitSelectionSet(selectionSet, context),
                //GraphQLTypeExtensionDefinition n => VisitTypeDE
                GraphQLUnionTypeDefinition unionTypeDefinition => VisitUnionTypeDefinition(unionTypeDefinition, context),
                GraphQLVariable variable => VisitVariable(variable, context),
                GraphQLVariableDefinition variableDefinition => VisitVariableDefinition(variableDefinition, context),
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
                foreach (var node in nodes)
                    await Visit(node, context);
            }
        }
    }
}
