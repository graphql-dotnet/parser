using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLListValue : GraphQLValue
    {
        private readonly ASTNodeKind _kind;

        public GraphQLListValue(ASTNodeKind kind)
        {
            _kind = kind;
        }

        public ROM AstValue { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => _kind;

        public List<GraphQLValue>? Values { get; set; }

        /// <inheritdoc/>
        public override string ToString() => AstValue.ToString();
    }

    internal sealed class GraphQLListValueFull : GraphQLListValue
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        public GraphQLListValueFull(ASTNodeKind kind)
            : base(kind)
        {
        }

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
