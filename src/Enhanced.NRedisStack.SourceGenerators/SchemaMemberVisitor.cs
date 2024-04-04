﻿using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor : SymbolVisitor
{
    private readonly string _variable;
    private readonly SchemaWriter _writer;
    private string _path;
    private string _aliasPrefix;

    public SchemaMemberVisitor(string variable, SchemaWriter writer)
    {
        _variable = variable;
        _writer = writer;

        _path = "$";
        _aliasPrefix = string.Empty;
    }

    public override void VisitNamedType(INamedTypeSymbol symbol)
    {
        var members = symbol.GetMembers();

        foreach (var member in members)
            member.Accept(this);
    }

    public override void VisitProperty(IPropertySymbol symbol)
    {
        if (!symbol.DeclaredAccessibility.HasFlag(Accessibility.Public)
            || !symbol.DeclaredAccessibility.HasFlag(Accessibility.Internal))
            return;

        var (type, attribute) = symbol.ToRedisProperty();

        if (type == RedisPropertyType.Ignore)
            return;

        var name = symbol.GetRedisName(attribute);
        _path = string.Concat(_path, ".", name);

        switch (type)
        {
            case RedisPropertyType.Unknown:
                HandleUnknownProperty(symbol, attribute);
                break;
            case RedisPropertyType.Object:
                HandleObjectProperty(symbol, attribute);
                break;
            case RedisPropertyType.Text:
                HandleTextProperty(symbol, attribute);
                break;
            case RedisPropertyType.Numeric:
                HandleNumericProperty(symbol, attribute);
                break;
            case RedisPropertyType.Tag:
                HandleTagProperty(symbol, attribute);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _path = _path.Substring(0, _path.Length - name.Length - 1);
    }

    private void HandleObjectProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        var aliasSubPrefix = attribute?.GetNamedArgumentValue<string>(nameof(RedisObjectAttribute.AliasPrefix));

        if (!string.IsNullOrEmpty(aliasSubPrefix))
            _aliasPrefix = string.Concat(_aliasPrefix, aliasSubPrefix);
        
        symbol.Type.Accept(this);
        
        if (!string.IsNullOrEmpty(aliasSubPrefix))
            _aliasPrefix = _aliasPrefix.Substring(0, _aliasPrefix.Length - aliasSubPrefix!.Length);
    }

    private void HandleUnknownProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        // TODO report error
        Debug.Fail("Unknown property type");
    }
}