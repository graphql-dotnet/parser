using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents a directive definition.
    /// </summary>
    public class GraphQLDirectiveDefinition : GraphQLTypeDefinitionWithDescription
    {
        public List<GraphQLInputValueDefinition>? Arguments { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.DirectiveDefinition;

        /// <summary>
        /// Returns a list of locations representing the valid locations this directive may be placed.
        /// </summary>
        public List<GraphQLName>? Locations { get; set; }

        /// <summary>
        /// Indicates if the directive may be used repeatedly at a single location.
        /// <br/><br/>
        /// Repeatable directives are often useful when the same directive
        /// should be used with different arguments at a single location,
        /// especially in cases where additional information needs to be
        /// provided to a type or schema extension via a directive
        /// </summary>
        public bool Repeatable { get; set; }
    }

    internal sealed class GraphQLDirectiveDefinitionFull : GraphQLDirectiveDefinition
    {
        private GraphQLLocation _location;
        private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }
}
