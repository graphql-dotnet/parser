using System;
using System.Collections.Generic;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
    /// <summary>
    /// Default implementation of <see cref="INodeVisitor{TContext}"/>.
    /// Traverses all AST nodes of the provided one.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
    public class DefaultNodeVisitor<TContext> : INodeVisitor<TContext>
        where TContext : IVisitorContext
    {
        /// <inheritdoc/>
        public virtual void VisitDocument(GraphQLDocument document, TContext context)
        {
            Visit(document.Definitions, context);
        }

        /// <inheritdoc/>
        public virtual void VisitArgument(GraphQLArgument argument, TContext context)
        {
            Visit(argument.Comment, context);
            Visit(argument.Name, context);
            Visit(argument.Value, context);
        }

        /// <inheritdoc/>
        public virtual void VisitComment(GraphQLComment comment, TContext context)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitDescription(GraphQLDescription description, TContext context)
        {
        }

        /// <inheritdoc/>
        public virtual void VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
        {
            Visit(operationDefinition.Comment, context);
            Visit(operationDefinition.Name, context);
            Visit(operationDefinition.VariableDefinitions, context);
            Visit(operationDefinition.Directives, context);
            Visit(operationDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual void VisitName(GraphQLName name, TContext context)
        {
            Visit(name.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
        {
            Visit(variableDefinition.Comment, context);
            Visit(variableDefinition.Variable, context);
            Visit(variableDefinition.Type, context);
            Visit(variableDefinition.DefaultValue, context);
            Visit(variableDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitVariable(GraphQLVariable variable, TContext context)
        {
            Visit(variable.Comment, context);
            Visit(variable.Name, context);
        }

        /// <inheritdoc/>
        public virtual void VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
        {
            Visit(selectionSet.Comment, context);
            Visit(selectionSet.Selections, context);
        }

        /// <inheritdoc/>
        public virtual void VisitField(GraphQLField field, TContext context)
        {
            Visit(field.Comment, context);
            Visit(field.Alias, context);
            Visit(field.Name, context);
            Visit(field.Arguments, context);
            Visit(field.Directives, context);
            Visit(field.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual void VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
        {
            Visit(fragmentSpread.Comment, context);
            Visit(fragmentSpread.Name, context);
            Visit(fragmentSpread.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
        {
            Visit(inlineFragment.Comment, context);
            Visit(inlineFragment.TypeCondition, context);
            Visit(inlineFragment.Directives, context);
            Visit(inlineFragment.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual void VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
        {
            Visit(fragmentDefinition.Comment, context);
            Visit(fragmentDefinition.Name, context);
            Visit(fragmentDefinition.TypeCondition, context);
            Visit(fragmentDefinition.Directives, context);
            Visit(fragmentDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public virtual void VisitIntValue(GraphQLScalarValue intValue, TContext context)
        {
            Visit(intValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitFloatValue(GraphQLScalarValue floatValue, TContext context)
        {
            Visit(floatValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitStringValue(GraphQLScalarValue stringValue, TContext context)
        {
            Visit(stringValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context)
        {
            Visit(booleanValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitEnumValue(GraphQLScalarValue enumValue, TContext context)
        {
            Visit(enumValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitListValue(GraphQLListValue listValue, TContext context)
        {
            Visit(listValue.Comment, context);
            Visit(listValue.Values, context);
        }

        /// <inheritdoc/>
        public virtual void VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
        {
            Visit(objectValue.Comment, context);
            Visit(objectValue.Fields, context);
        }

        /// <inheritdoc/>
        public virtual void VisitObjectField(GraphQLObjectField objectField, TContext context)
        {
            Visit(objectField.Comment, context);
            Visit(objectField.Name, context);
            Visit(objectField.Value, context);
        }

        /// <inheritdoc/>
        public virtual void VisitDirective(GraphQLDirective directive, TContext context)
        {
            Visit(directive.Comment, context);
            Visit(directive.Name, context);
            Visit(directive.Arguments, context);
        }

        /// <inheritdoc/>
        public virtual void VisitNamedType(GraphQLNamedType namedType, TContext context)
        {
            Visit(namedType.Comment, context);
            Visit(namedType.Name, context);
        }

        /// <inheritdoc/>
        public virtual void VisitListType(GraphQLListType listType, TContext context)
        {
            Visit(listType.Comment, context);
            Visit(listType.Type, context);
        }

        /// <inheritdoc/>
        public virtual void VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
        {
            Visit(nonNullType.Comment, context);
            Visit(nonNullType.Type, context);
        }

        /// <inheritdoc/>
        public virtual void VisitNullValue(GraphQLScalarValue nullValue, TContext context)
        {
            Visit(nullValue.Comment, context);
        }

        /// <inheritdoc/>
        public virtual void VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
        {
            Visit(schemaDefinition.Comment, context);
            Visit(schemaDefinition.Description, context);
            Visit(schemaDefinition.Directives, context);
            Visit(schemaDefinition.OperationTypes, context);
        }

        /// <inheritdoc/>
        public virtual void VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
        {
            Visit(rootOperationTypeDefinition.Comment, context);
            Visit(rootOperationTypeDefinition.Type, context);
        }

        /// <inheritdoc/>
        public virtual void VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
        {
            Visit(scalarTypeDefinition.Comment, context);
            Visit(scalarTypeDefinition.Description, context);
            Visit(scalarTypeDefinition.Name, context);
            Visit(scalarTypeDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
        {
            Visit(objectTypeDefinition.Comment, context);
            Visit(objectTypeDefinition.Description, context);
            Visit(objectTypeDefinition.Name, context);
            Visit(objectTypeDefinition.Interfaces, context);
            Visit(objectTypeDefinition.Directives, context);
            Visit(objectTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual void VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
        {
            Visit(fieldDefinition.Comment, context);
            Visit(fieldDefinition.Description, context);
            Visit(fieldDefinition.Name, context);
            Visit(fieldDefinition.Arguments, context);
            Visit(fieldDefinition.Type, context);
            Visit(fieldDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
        {
            Visit(inputValueDefinition.Comment, context);
            Visit(inputValueDefinition.Description, context);
            Visit(inputValueDefinition.Name, context);
            Visit(inputValueDefinition.Type, context);
            Visit(inputValueDefinition.DefaultValue, context);
            Visit(inputValueDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
        {
            Visit(interfaceTypeDefinition.Comment, context);
            Visit(interfaceTypeDefinition.Description, context);
            Visit(interfaceTypeDefinition.Name, context);
            Visit(interfaceTypeDefinition.Interfaces, context);
            Visit(interfaceTypeDefinition.Directives, context);
            Visit(interfaceTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual void VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
        {
            Visit(unionTypeDefinition.Comment, context);
            Visit(unionTypeDefinition.Description, context);
            Visit(unionTypeDefinition.Name, context);
            Visit(unionTypeDefinition.Directives, context);
            Visit(unionTypeDefinition.Types, context);
        }

        /// <inheritdoc/>
        public virtual void VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
        {
            Visit(enumTypeDefinition.Comment, context);
            Visit(enumTypeDefinition.Description, context);
            Visit(enumTypeDefinition.Name, context);
            Visit(enumTypeDefinition.Directives, context);
            Visit(enumTypeDefinition.Values, context);
        }

        /// <inheritdoc/>
        public virtual void VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
        {
            Visit(enumValueDefinition.Comment, context);
            Visit(enumValueDefinition.Description, context);
            Visit(enumValueDefinition.Name, context);
            Visit(enumValueDefinition.Directives, context);
        }

        /// <inheritdoc/>
        public virtual void VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
        {
            Visit(inputObjectTypeDefinition.Comment, context);
            Visit(inputObjectTypeDefinition.Description, context);
            Visit(inputObjectTypeDefinition.Name, context);
            Visit(inputObjectTypeDefinition.Directives, context);
            Visit(inputObjectTypeDefinition.Fields, context);
        }

        /// <inheritdoc/>
        public virtual void VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
        {
            Visit(directiveDefinition.Comment, context);
            Visit(directiveDefinition.Description, context);
            Visit(directiveDefinition.Name, context);
            Visit(directiveDefinition.Arguments, context);
        }

        /// <summary>
        /// Dispatches node to the appropriate VisitXXX method.
        /// </summary>
        /// <param name="node">AST node to dispatch.</param>
        /// <param name="context">Context passed into all INodeVisitor.VisitXXX methods.</param>
        public virtual void Visit(ASTNode? node, TContext context)
        {
            if (node == null)
                return;

            switch (node)
            {
                case GraphQLArgument argument:
                    VisitArgument(argument, context);
                    break;

                case GraphQLComment comment:
                    VisitComment(comment, context);
                    break;

                case GraphQLDescription description:
                    VisitDescription(description, context);
                    break;

                case GraphQLDirective directive:
                    VisitDirective(directive, context);
                    break;

                case GraphQLDirectiveDefinition directiveDefinition:
                    VisitDirectiveDefinition(directiveDefinition, context);
                    break;

                case GraphQLDocument document:
                    VisitDocument(document, context);
                    break;

                case GraphQLEnumTypeDefinition enumTypeDefinition:
                    VisitEnumTypeDefinition(enumTypeDefinition, context);
                    break;

                case GraphQLEnumValueDefinition enumValueDefinition:
                    VisitEnumValueDefinition(enumValueDefinition, context);
                    break;

                case GraphQLField field:
                    VisitField(field, context);
                    break;

                case GraphQLFieldDefinition fieldDefinition:
                    VisitFieldDefinition(fieldDefinition, context);
                    break;

                case GraphQLFragmentDefinition fragmentDefinition:
                    VisitFragmentDefinition(fragmentDefinition, context);
                    break;

                case GraphQLFragmentSpread fragmentSpread:
                    VisitFragmentSpread(fragmentSpread, context);
                    break;

                case GraphQLInlineFragment inlineFragment:
                    VisitInlineFragment(inlineFragment, context);
                    break;

                case GraphQLInputObjectTypeDefinition inputObjectTypeDefinition:
                    VisitInputObjectTypeDefinition(inputObjectTypeDefinition, context);
                    break;

                case GraphQLInputValueDefinition inputValueDefinition:
                    VisitInputValueDefinition(inputValueDefinition, context);
                    break;

                case GraphQLInterfaceTypeDefinition interfaceTypeDefinition:
                    VisitInterfaceTypeDefinition(interfaceTypeDefinition, context);
                    break;

                case GraphQLListType listType:
                    VisitListType(listType, context);
                    break;

                case GraphQLListValue listValue:
                    VisitListValue(listValue, context);
                    break;

                case GraphQLName name:
                    VisitName(name, context);
                    break;

                case GraphQLNamedType namedType:
                    VisitNamedType(namedType, context);
                    break;

                case GraphQLNonNullType nonNullType:
                    VisitNonNullType(nonNullType, context);
                    break;

                case GraphQLObjectField objectField:
                    VisitObjectField(objectField, context);
                    break;

                case GraphQLObjectTypeDefinition objectTypeDefinition:
                    VisitObjectTypeDefinition(objectTypeDefinition, context);
                    break;

                case GraphQLObjectValue objectValue:
                    VisitObjectValue(objectValue, context);
                    break;

                case GraphQLOperationDefinition operationDefinition:
                    VisitOperationDefinition(operationDefinition, context);
                    break;

                case GraphQLRootOperationTypeDefinition rootOperationTypeDefinition:
                    VisitRootOperationTypeDefinition(rootOperationTypeDefinition, context);
                    break;

                case GraphQLScalarTypeDefinition scalarTypeDefinition:
                    VisitScalarTypeDefinition(scalarTypeDefinition, context);
                    break;

                case GraphQLScalarValue scalarValue:
                    switch (scalarValue.Kind)
                    {
                        case ASTNodeKind.BooleanValue:
                            VisitBooleanValue(scalarValue, context);
                            break;

                        case ASTNodeKind.EnumValue:
                            VisitEnumValue(scalarValue, context);
                            break;

                        case ASTNodeKind.FloatValue:
                            VisitFloatValue(scalarValue, context);
                            break;

                        case ASTNodeKind.IntValue:
                            VisitIntValue(scalarValue, context);
                            break;

                        case ASTNodeKind.NullValue:
                            VisitNullValue(scalarValue, context);
                            break;

                        case ASTNodeKind.StringValue:
                            VisitStringValue(scalarValue, context);
                            break;

                        default:
                            throw new NotSupportedException($"Unknown GraphQLScalarValue of kind '{scalarValue.Kind}'.");
                    }
                    break;

                case GraphQLSchemaDefinition schemaDefinition:
                    VisitSchemaDefinition(schemaDefinition, context);
                    break;

                case GraphQLSelectionSet selectionSet:
                    VisitSelectionSet(selectionSet, context);
                    break;

                // case GraphQLTypeExtensionDefinition n => VisitTypeDE
                case GraphQLUnionTypeDefinition unionTypeDefinition:
                    VisitUnionTypeDefinition(unionTypeDefinition, context);
                    break;

                case GraphQLVariable variable:
                    VisitVariable(variable, context);
                    break;

                case GraphQLVariableDefinition variableDefinition:
                    VisitVariableDefinition(variableDefinition, context);
                    break;

                default:
                    throw new NotSupportedException($"Unknown node '{node.GetType().Name}'.");
            };
        }

        protected void Visit<T>(List<T>? nodes, TContext context)
            where T : ASTNode
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                    Visit(node, context);
            }
        }
    }
}
