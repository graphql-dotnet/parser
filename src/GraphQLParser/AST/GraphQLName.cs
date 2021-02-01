using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Name"/>
    [DebuggerDisplay("{Value}")]
    public class GraphQLName : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Name;

        /// <summary>
        /// Name value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value { get; set; }
    }

    internal sealed class GraphQLNameFull : GraphQLName
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        // TODO: this property is not set anywhere (yet), so it makes no sense to create a field for it
        //public override GraphQLComment? Comment
        //{
        //    get => _comment;
        //    set => _comment = value;
        //}
    }
}
