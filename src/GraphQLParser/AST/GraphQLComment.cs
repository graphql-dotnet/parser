using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Comment"/>
    [DebuggerDisplay("{Text}")]
    public class GraphQLComment : ASTNode
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        /// <summary>
        /// Comment value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Text { get; set; }

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        // Comment itself can't have a comment
        //public override GraphQLComment? Comment
        //{
        //    get => _comment;
        //    set => _comment = value;
        //}
    }
}
