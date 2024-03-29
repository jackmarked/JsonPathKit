﻿using JetDevel.JsonPath.CodeAnalysis;
using System.Text;

namespace JetDevel.JsonPath;

public static class SyntaxFactory
{
    public static JsonPathQuerySyntax Parse(string s)
    {
        var reader = new Utf8BytesUnicodeCharacterReader(Encoding.UTF8.GetBytes(s));
        var lexer = new Lexer(reader);
        var parser = new Parser(lexer);
        var result = parser.ParseQuery();
        if(result.JsonPathQuery != null)
            return result.JsonPathQuery;
        throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors));
    }
}