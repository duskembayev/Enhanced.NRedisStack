using System.Text;
using Enhanced.NRedisStack.Annotation;
using Enhanced.NRedisStack.SourceGenerators.PropertyWriters;
using Microsoft.CodeAnalysis;

namespace Enhanced.NRedisStack.SourceGenerators;

internal class SchemaMemberVisitor : SymbolVisitor
{
    private readonly string _variable;
    private readonly SchemaWriter _writer;
    private readonly StringBuilder _path;

    public SchemaMemberVisitor(string variable, SchemaWriter writer)
    {
        _variable = variable;
        _writer = writer;

        _path = new StringBuilder("$");
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
        var name = symbol.GetRedisName(attribute);

        _path.Append('.').Append(name);
        

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
        
        _path.Remove(_path.Length - name.Length - 1, name.Length + 1);
    }

    private void HandleTagProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        
    }

    private void HandleNumericProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        _writer.Write(_variable);
        _writer.Write(".AddNumericField(new FieldName(\"");
        _writer.Write($"{_variable}.AddNumericField(new FieldName(\"{_path}\", \"\"));");
    }

    private void HandleTextProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        var propertyWriter = new PropertyWriter(_variable, _path.ToString(), RedisPropertyType.Text);
        
        if (attribute is not null)
        {
            var alias = attribute.NamedArguments
                .FirstOrDefault(x => x.Key == "Alias")
                .Value;

            if (alias.Value is string aliasValue && !string.IsNullOrEmpty(aliasValue))
                propertyWriter.SetAlias(aliasValue);
        }
        
        propertyWriter.Write(_writer);
    }

    private void HandleObjectProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        throw new NotImplementedException();
    }

    private void HandleUnknownProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        throw new NotImplementedException();
    }
}