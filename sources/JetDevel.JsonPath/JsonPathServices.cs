using JetDevel.JsonPath.CodeAnalysis;
using System.Text.Unicode;

namespace JetDevel.JsonPath;
public sealed partial class JsonPathServices
{
    readonly Dictionary<string, FunctionDefinition?> functionMap;
    public JsonPathServices()
    {
        functionMap = [];
        RegisterFunctionDefinition(KnownFunctions.Length);
        RegisterFunctionDefinition(KnownFunctions.Count);
        RegisterFunctionDefinition(KnownFunctions.Value);
        RegisterFunctionDefinition(KnownFunctions.Match);
        RegisterFunctionDefinition(KnownFunctions.Search);
    }
    void RegisterFunctionDefinition(FunctionDefinition function)
    {
        ArgumentNullException.ThrowIfNull(function);
        functionMap[function.Name] = function;
    }
    internal FunctionDefinition? GetFunction(string functionName)
    {
        if(string.IsNullOrEmpty(functionName))
            return null;
        return functionMap.GetValueOrDefault(functionName);

    }
    public JsonPathQuery FromSource(string source)
    {
        ReadOnlySpan<char> sourceSpan = source;
        var maxLength = sourceSpan.Length * 2;
        var destination = maxLength > 160 ? new byte[maxLength] : stackalloc byte[maxLength];
        Utf8.FromUtf16(source, destination, out _, out var length);
        return FromUtf8(destination[..length]);
    }
    public static bool TryParse(ReadOnlySpan<byte> utf8Bytes, out JsonPathQuerySyntax? query)
    {
        var charReader = new Utf8BytesUnicodeCharacterReader(utf8Bytes.ToArray());
        var lexer = new Lexer(charReader);
        var parser = new Parser(lexer);
        var result = parser.ParseQuery();
        query = result.JsonPathQuery;
        return query != null;
    }
    public JsonPathQuery FromUtf8(ReadOnlySpan<byte> utf8Bytes)
    {
        var charReader = new Utf8BytesUnicodeCharacterReader(utf8Bytes.ToArray());
        var lexer = new Lexer(charReader);
        Parser parser = new Parser(lexer);
        return FromSyntax(parser.ParseQuery().JsonPathQuery!);
    }
    public JsonPathQuery FromSyntax(JsonPathQuerySyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        return new SyntaxBasedJsonPathQuery(syntax, this);
    }
}