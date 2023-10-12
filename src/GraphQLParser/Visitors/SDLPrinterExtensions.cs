using System.Text;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Extension methods for <see cref="SDLPrinter"/>.
/// </summary>
public static class SDLPrinterExtensions
{
    /// <summary>
    /// Returns the specified AST printed as a SDL document.
    /// </summary>
    public static string Print(this SDLPrinter printer, ASTNode node)
    {
        var sb = new StringBuilder();
        printer.Print(node, sb);
        return sb.ToString();
    }

    /// <summary>
    /// Prints the specified AST into the specified <see cref="StringBuilder"/> as a SDL document.
    /// </summary>
    public static void Print(this SDLPrinter printer, ASTNode node, StringBuilder stringBuilder)
        => printer.PrintAsync(node, new StringWriter(stringBuilder), default).AsTask().GetAwaiter().GetResult();

#if NET462
    private static readonly Encoding _uTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
#endif

    /// <summary>
    /// Prints the specified AST into the specified <see cref="MemoryStream"/> as a SDL document.
    /// If no encoding is specified, the document is written in UTF-8 format without a byte order mark.
    /// </summary>
    public static void Print(this SDLPrinter printer, ASTNode node, MemoryStream memoryStream, Encoding? encoding = null)
    {
        int bufferSize = -1;
#if NET462
        if (encoding == null)
        {
            encoding = _uTF8NoBOM;
        }
        if (bufferSize == -1)
        {
            bufferSize = 1024;
        }
#endif
        using var streamWriter = new StreamWriter(memoryStream, encoding, bufferSize, true);
        printer.PrintAsync(node, streamWriter, default).AsTask().GetAwaiter().GetResult();
        // flush encoder state to stream
        streamWriter.Flush();
    }
}
