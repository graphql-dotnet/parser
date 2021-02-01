using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Scalar nodes represent primitive leaf values in a GraphQL document.
    /// <br/>
    /// There are 6 kinds of scalar nodes:
    /// <br/>
    /// <see cref="ASTNodeKind.StringValue">String</see>
    /// <br/>
    /// <see cref="ASTNodeKind.BooleanValue">Boolean</see>
    /// <br/>
    /// <see cref="ASTNodeKind.IntValue">Int</see>
    /// <br/>
    /// <see cref="ASTNodeKind.FloatValue">Float</see>
    /// <br/>
    /// <see cref="ASTNodeKind.EnumValue">Enumeration</see>
    /// <br/>
    /// <see cref="ASTNodeKind.NullValue">Null</see>
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class GraphQLScalarValue : GraphQLValue
    {
        private readonly ASTNodeKind _kind;

        /// <summary>
        /// Creates scalar node with the specified kind.
        /// </summary>
        /// <param name="kind">One of six kinds of scalar nodes.</param>
        public GraphQLScalarValue(ASTNodeKind kind)
        {
            _kind = kind;
        }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => _kind;

        /// <summary>
        /// Scalar value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value { get; set; }

        /// <inheritdoc/>
        public override string? ToString() => Kind == ASTNodeKind.StringValue ? $"\"{Value}\"" : Value.ToString();
    }

    internal sealed class GraphQLScalarValueFull : GraphQLScalarValue
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        public GraphQLScalarValueFull(ASTNodeKind kind)
            :base(kind)
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
