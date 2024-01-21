﻿namespace JetDevel.JsonPath.CodeAnalysis;

public sealed class MemberNameShorthandSelectorSyntax: SelectorSyntax
{
    internal MemberNameShorthandSelectorSyntax(string memberName)
    {
        MemberName = memberName;
    }
    public string MemberName { get; }
    public override string ToString()
    {
        return MemberName;
    }
}