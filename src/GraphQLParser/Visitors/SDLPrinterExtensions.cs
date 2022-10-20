using System.Text;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Extension methods for <see cref="SDLPrinter"/>.
/// </summary>
public static class SDLPrinterExtensions
{
    /// <summary>
    /// Prints the specified AST into the specified <see cref="StringBuilder"/> as a SDL document.
    /// </summary>
    public static void Print(this SDLPrinter printer, ASTNode node, StringBuilder stringBuilder)
#pragma warning disable CA2012 // Use ValueTasks correctly
        => printer.PrintAsync(node, new StringWriter(stringBuilder), default).GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly

    /// <summary>
    /// Prints the specified AST into the specified <see cref="MemoryStream"/> as a SDL document.
    /// If no encoding is specified, the document is written in UTF-8 format without a byte order mark.
    /// </summary>
    public static void Print(this SDLPrinter printer, ASTNode node, MemoryStream memoryStream, Encoding? encoding = null)
    {
        using var streamWriter = new StreamWriter(memoryStream, encoding, -1 /* default */, true);
#pragma warning disable CA2012 // Use ValueTasks correctly
        printer.PrintAsync(node, streamWriter, default).GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly
        // flush encoder state to stream
        streamWriter.Flush();
    }
}
