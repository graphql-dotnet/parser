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
    [DebuggerDisplay("{ValueString}")]
    public class GraphQLScalarValue : GraphQLValue
    {
        private readonly ASTNodeKind _kind;
        private ROM _value;
        private string? _valueString;

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
        public ROM Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueString = null;
            }
        }

        /// <summary>
        /// Gets scalar value represented as string. The value of this property is cached and in sync with <see cref="Value"/>.
        /// The first time this property is accessed, memory in the managed heap will be allocated for it.
        /// In scenarios where minimum memory consumption is required, use the <see cref="Value"/> property.
        /// </summary>
        public string ValueString => _valueString ??= (string)Value;

        /// <inheritdoc/>
        public override string? ToString() => Kind == ASTNodeKind.StringValue ? $"\"{Value}\"" : Value.ToString();
    }
}
