using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Description"/>
    [DebuggerDisplay("{Value}")]
    public class GraphQLDescription : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Description;

        /// <summary>
        /// Description value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value { get; set; }
    }

    internal sealed class GraphQLDescriptionWithLocation : GraphQLDescription
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }
}
