using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.ImplementsInterfaces"/>.
    /// </summary>
    public class GraphQLImplementsInterfaces : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ImplementsInterfaces;

        public List<GraphQLNamedType> Items { get; set; } = null!;
    }

    internal sealed class GraphQLImplementsInterfacesWithLocation : GraphQLImplementsInterfaces
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLImplementsInterfacesWithComment : GraphQLImplementsInterfaces
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLImplementsInterfacesFull : GraphQLImplementsInterfaces
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
