using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSelectionSet : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.SelectionSet;

        public List<ASTNode>? Selections { get; set; }
    }

    internal sealed class GraphQLSelectionSetFull : GraphQLSelectionSet
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
