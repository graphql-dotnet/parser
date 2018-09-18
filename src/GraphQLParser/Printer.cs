using GraphQLParser.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLParser
{
    public class Printer
    {
        public string Print(ASTNode node)
        {
            if (node == null) return string.Empty;

            switch (node.Kind)
            {
                case ASTNodeKind.Document: return this.PrintDocument((GraphQLDocument)node);
                case ASTNodeKind.OperationDefinition: return this.PrintOperationDefinition((GraphQLOperationDefinition)node);
                case ASTNodeKind.SelectionSet: return this.PrintSelectionSet((GraphQLSelectionSet)node);
                case ASTNodeKind.Field: return this.PrintFieldSelection((GraphQLFieldSelection)node);
                case ASTNodeKind.Name: return this.PrintName((GraphQLName)node);
                case ASTNodeKind.Argument: return this.PrintArgument((GraphQLArgument)node);
                case ASTNodeKind.FragmentSpread: return this.PrintFragmentSpread((GraphQLFragmentSpread)node);
                case ASTNodeKind.FragmentDefinition: return this.PrintFragmentDefinition((GraphQLFragmentDefinition)node);
                case ASTNodeKind.InlineFragment: return this.PrintInlineFragment((GraphQLInlineFragment)node);
                case ASTNodeKind.NamedType: return this.PrintNamedType((GraphQLNamedType)node);
                case ASTNodeKind.Directive: return this.PrintDirective((GraphQLDirective)node);
                case ASTNodeKind.Variable: return this.PrintVariable((GraphQLVariable)node);
                case ASTNodeKind.IntValue: return this.PrintIntValue((GraphQLScalarValue)node);
                case ASTNodeKind.FloatValue: return this.PrintFloatValue((GraphQLScalarValue)node);
                case ASTNodeKind.StringValue: return this.PrintStringValue((GraphQLScalarValue)node);
                case ASTNodeKind.BooleanValue: return this.PrintBooleanValue((GraphQLScalarValue)node);
                case ASTNodeKind.EnumValue: return this.PrintEnumValue((GraphQLScalarValue)node);
                case ASTNodeKind.ListValue: return this.PrintListValue((GraphQLListValue)node);
                case ASTNodeKind.ObjectValue: return this.PrintObjectValue((GraphQLObjectValue)node);
                case ASTNodeKind.ObjectField: return this.PrintObjectField((GraphQLObjectField)node);
                case ASTNodeKind.VariableDefinition: return this.PrintVariableDefinition((GraphQLVariableDefinition)node);
                case ASTNodeKind.NullValue: return this.PrintNullValue((GraphQLScalarValue)node);
                case ASTNodeKind.SchemaDefinition: return this.PrintSchemaDefinition((GraphQLSchemaDefinition)node);
                case ASTNodeKind.ListType: return this.PrintListType((GraphQLListType)node);
                case ASTNodeKind.NonNullType: return this.PrintNonNullType((GraphQLNonNullType)node);
                case ASTNodeKind.OperationTypeDefinition: return this.PrintOperationTypeDefinition((GraphQLOperationTypeDefinition)node);
                case ASTNodeKind.ScalarTypeDefinition: return this.PrintScalarTypeDefinition((GraphQLScalarTypeDefinition)node);
                case ASTNodeKind.ObjectTypeDefinition: return this.PrintObjectTypeDefinition((GraphQLObjectTypeDefinition)node);
                case ASTNodeKind.FieldDefinition: return this.PrintFieldDefinition((GraphQLFieldDefinition)node);
                case ASTNodeKind.InputValueDefinition: return this.PrintInputValueDefinition((GraphQLInputValueDefinition)node);
                case ASTNodeKind.InterfaceTypeDefinition: return this.PrintInterfaceTypeDefinition((GraphQLInterfaceTypeDefinition)node);
                case ASTNodeKind.UnionTypeDefinition: return this.PrintUnionTypeDefinition((GraphQLUnionTypeDefinition)node);
                case ASTNodeKind.EnumTypeDefinition: return this.PrintEnumTypeDefinition((GraphQLEnumTypeDefinition)node);
                case ASTNodeKind.EnumValueDefinition: return this.PrintEnumValueDefinition((GraphQLEnumValueDefinition)node);
                case ASTNodeKind.InputObjectTypeDefinition: return this.PrintInputObjectTypeDefinition((GraphQLInputObjectTypeDefinition)node);
                case ASTNodeKind.TypeExtensionDefinition: return this.PrintTypeExtensionDefinition((GraphQLTypeExtensionDefinition)node);
                case ASTNodeKind.DirectiveDefinition: return this.PrintDirectiveDefinition((GraphQLDirectiveDefinition)node);
            }

            return string.Empty;
        }

        private string Block(IEnumerable<string> enumerable)
        {
            return enumerable?.Any() == true
                ? "{" + Environment.NewLine + this.Indent(this.Join(enumerable, Environment.NewLine)) + Environment.NewLine + "}"
                : null;
        }

        private string Indent(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? null
                : $"  {input.Replace(Environment.NewLine, $"{Environment.NewLine}  ")}";
        }

        private string Join(IEnumerable<string> collection, string separator = "")
        {
            collection = collection?.Where(e => !string.IsNullOrWhiteSpace(e));

            return collection?.Any() == true
                ? string.Join(separator, collection)
                : string.Empty;
        }

        private string PrintArgument(GraphQLArgument argument)
        {
            var name = this.PrintName(argument.Name);
            var value = this.Print(argument.Value);

            return $"{name}: {value}";
        }

        private string PrintBooleanValue(GraphQLScalarValue node)
        {
            return node.Value;
        }

        private string PrintDirective(GraphQLDirective directive)
        {
            var name = this.PrintName(directive.Name);
            var args = directive.Arguments?.Select(this.PrintArgument);

            return $"@{name}{this.Wrap("(", this.Join(args, ", "), ")")}";
        }

        private string PrintDirectiveDefinition(GraphQLDirectiveDefinition node)
        {
            var name = this.PrintName(node.Name);
            var args = node.Arguments?.Select(this.Print);
            var locations = node.Locations?.Select(this.PrintName);

            return this.Join(new[]
            {
                "directive @",
                name,
                args.All(e => !e.Contains(Environment.NewLine))
                    ? this.Wrap("(", this.Join(args, ", "), ")")
                    : this.Wrap(
                        $"({Environment.NewLine}",
                        this.Indent(this.Join(args, Environment.NewLine)),
                        $"{Environment.NewLine})"),
                " on ",
                this.Join(locations, " | ")
            });
        }

        private string PrintDocument(GraphQLDocument node)
        {
            var definitions = node.Definitions?.Select(this.Print);

            return this.Join(definitions, $"{Environment.NewLine}{Environment.NewLine}");
        }

        private string PrintEnumTypeDefinition(GraphQLEnumTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);
            var values = node.Values?.Select(this.PrintEnumValueDefinition);

            return this.Join(new[]
            {
                "enum",
                name,
                this.Join(directives, " "),
                this.Block(values) ?? "{ }"
            },
            " ");
        }

        private string PrintEnumValue(GraphQLScalarValue node)
        {
            return node.Value;
        }

        private string PrintEnumValueDefinition(GraphQLEnumValueDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);

            return this.Join(new[]
            {
                name,
                this.Join(directives, " ")
            },
            " ");
        }

        private string PrintFieldDefinition(GraphQLFieldDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);
            var args = node.Arguments?.Select(this.PrintInputValueDefinition);
            var type = this.Print(node.Type);

            return this.Join(new[]
            {
                name,
                args.All(e => !e.Contains(Environment.NewLine))
                    ? this.Wrap("(", this.Join(args, ", "), ")")
                    : this.Wrap(
                        $"({Environment.NewLine}",
                        this.Indent(this.Join(args, Environment.NewLine)),
                        $"{Environment.NewLine}aaa)"),
                ": ",
                type,
                this.Wrap(" ", this.Join(directives, " "))
            });
        }

        private string PrintFieldSelection(GraphQLFieldSelection node)
        {
            var alias = this.PrintName(node.Alias);
            var name = this.PrintName(node.Name);
            var args = node.Arguments?.Select(this.PrintArgument);
            var directives = node.Directives?.Select(this.PrintDirective);
            var selectionSet = this.PrintSelectionSet(node.SelectionSet);

            return this.Join(new[]
            {
                $"{this.Wrap(string.Empty, alias, ": ")}{name}{this.Wrap("(", this.Join(args, ", "), ")")}",
                this.Join(directives, " "),
                selectionSet
            });
        }

        private string PrintFloatValue(GraphQLScalarValue node)
        {
            return node.Value;
        }

        private string PrintFragmentDefinition(GraphQLFragmentDefinition node)
        {
            var name = this.PrintName(node.Name);
            var typeCondition = this.PrintNamedType(node.TypeCondition);
            var directives = node.Directives?.Select(this.PrintDirective);
            var selectionSet = this.PrintSelectionSet(node.SelectionSet);

            return $"fragment {name} on {typeCondition} {this.Wrap(string.Empty, this.Join(directives, " "), " ")}{selectionSet}";
        }

        private string PrintFragmentSpread(GraphQLFragmentSpread node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);

            return $"...{name}{this.Wrap(string.Empty, this.Join(directives, " "))}";
        }

        private string PrintInlineFragment(GraphQLInlineFragment node)
        {
            var typeCondition = this.PrintNamedType(node.TypeCondition);
            var directives = node.Directives?.Select(this.PrintDirective);
            var selectionSet = this.PrintSelectionSet(node.SelectionSet);

            return this.Join(new[]
            {
                "...",
                this.Wrap("on ", typeCondition),
                this.Join(directives, " "),
                selectionSet
            },
            " ");
        }

        private string PrintInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);
            var fields = node.Fields?.Select(this.PrintInputValueDefinition);

            return this.Join(new[]
            {
                "input",
                name,
                this.Join(directives, " "),
                this.Block(fields) ?? "{ }"
            },
            " ");
        }

        private string PrintInputValueDefinition(GraphQLInputValueDefinition node)
        {
            var name = this.PrintName(node.Name);
            var type = this.Print(node.Type);
            var directives = node.Directives?.Select(this.PrintDirective);
            var defaultValue = this.Print(node.DefaultValue);

            return this.Join(new[]
            {
                $"{name}: {type}",
                this.Wrap("= ", defaultValue),
                this.Join(directives, " "),
            },
            " ");
        }

        private string PrintInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);
            var fields = node.Fields?.Select(this.PrintFieldDefinition);

            return this.Join(new[]
            {
                "interface",
                name,
                this.Join(directives, " "),
                this.Block(fields) ?? "{ }"
            },
            " ");
        }
        private string PrintIntValue(GraphQLScalarValue node)
        {
            return node.Value;
        }

        private string PrintListType(GraphQLListType node)
        {
            var type = this.Print(node.Type);

            return $"[{type}]";
        }

        private string PrintListValue(GraphQLListValue node)
        {
            var values = node.Values?.Select(this.Print);

            return $"[{this.Join(values, ", ")}]";
        }

        private string PrintName(GraphQLName name)
        {
            return name?.Value ?? string.Empty;
        }

        private string PrintNamedType(GraphQLNamedType node)
        {
            if (node == null) return string.Empty;

            return this.PrintName(node.Name);
        }

        private string PrintNonNullType(GraphQLNonNullType node)
        {
            var type = this.Print(node.Type);

            return $"{type}!";
        }

        private string PrintNullValue(GraphQLScalarValue node)
        {
            return "null";
        }

        private string PrintObjectField(GraphQLObjectField node)
        {
            var name = this.PrintName(node.Name);
            var value = this.Print(node.Value);

            return $"{name}: {value}";
        }

        private string PrintObjectTypeDefinition(GraphQLObjectTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var interfaces = node.Interfaces?.Select(this.PrintNamedType);
            var directives = node.Directives?.Select(this.PrintDirective);
            var fields = node.Fields?.Select(this.PrintFieldDefinition);

            return this.Join(new[]
            {
                "type",
                name,
                this.Wrap("implements ", this.Join(interfaces, " & ")),
                this.Join(directives, " "),
                this.Block(fields) ?? "{ }"
            },
            " ");
        }
        private string PrintObjectValue(GraphQLObjectValue node)
        {
            var fields = node.Fields?.Select(this.PrintObjectField);

            return "{" + this.Join(fields, ", ") + "}";
        }

        private string PrintOperationDefinition(GraphQLOperationDefinition definition)
        {
            var name = PrintName(definition.Name);
            var directives = Join(definition.Directives?.Select(this.PrintDirective), " ");
            var selectionSet = this.PrintSelectionSet(definition.SelectionSet);

            var variableDefinitions = Wrap(
                "(",
                this.Join(definition.VariableDefinitions?.Select(this.PrintVariableDefinition), ", "), ")");

            var operation = definition.Operation
                .ToString()
                .ToLower();

            return string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(name) &&
                definition.Operation == OperationType.Query
                ? selectionSet
                : this.Join(
                    new[]
                    {
                        operation,
                        this.Join(new[] { name, variableDefinitions }),
                        directives,
                        selectionSet
                    },
                    " ");
        }

        private string PrintOperationType(GraphQLOperationTypeDefinition operationType)
        {
            var operation = operationType.Operation.ToString().ToLower();
            var type = this.PrintNamedType(operationType.Type);

            return $"{operation}: {type}";
        }

        private string PrintOperationTypeDefinition(GraphQLOperationTypeDefinition node)
        {
            var operation = node.Operation.ToString();
            var type = this.PrintNamedType(node.Type);

            return $"{operation}: {type}";
        }

        private string PrintScalarTypeDefinition(GraphQLScalarTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);

            return this.Join(new[]
            {
                "scalar",
                name,
                this.Join(directives, " ")
            },
            " ");
        }

        private string PrintSchemaDefinition(GraphQLSchemaDefinition node)
        {
            var directives = node.Directives?.Select(this.PrintDirective);
            var operationTypes = node.OperationTypes?.Select(this.PrintOperationType);

            return this.Join(new[]
            {
                "schema",
                this.Join(directives, " "),
                this.Block(operationTypes) ?? "{ }"
            },
            " ");
        }

        private string PrintSelectionSet(GraphQLSelectionSet selectionSet)
        {
            if (selectionSet == null) return string.Empty;

            return this.Block(selectionSet.Selections?.Select(this.Print));
        }

        private string PrintStringValue(GraphQLScalarValue node)
        {
            return $"\"{node.Value}\"";
        }

        private string PrintTypeExtensionDefinition(GraphQLTypeExtensionDefinition node)
        {
            return $"extend {this.Print(node.Definition)}";
        }
        private string PrintUnionTypeDefinition(GraphQLUnionTypeDefinition node)
        {
            var name = this.PrintName(node.Name);
            var directives = node.Directives?.Select(this.PrintDirective);
            var types = node.Types?.Select(this.PrintNamedType);

            return this.Join(new[]
            {
                "union",
                name,
                this.Join(directives, " "),
                types?.Any() == true
                    ? "= " + this.Join(types, " | ")
                    : string.Empty
            },
            " ");
        }
        private string PrintVariable(GraphQLVariable variable)
        {
            return $"${variable.Name.Value}";
        }
        private string PrintVariableDefinition(GraphQLVariableDefinition variableDefinition)
        {
            var variable = this.PrintVariable(variableDefinition.Variable);
            var type = this.Print(variableDefinition.Type);
            var defaultValue = variableDefinition.DefaultValue?.ToString();

            return this.Join(new[]
            {
                variable,
                ": ",
                type,
                Wrap(" = ", defaultValue)
            });
        }
        private string Wrap(string start, string maybeString, string end = "")
        {
            return string.IsNullOrWhiteSpace(maybeString)
                ? null
                : $"{start}{maybeString}{end}";
        }
    }
}