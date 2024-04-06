using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor : SymbolVisitor
{
    private readonly SchemaContext _context;
    private readonly string _variable;
    private readonly SchemaWriter _writer;
    private string _aliasPrefix;
    private string _path;

    public SchemaMemberVisitor(string variable, SchemaWriter writer, SchemaContext context)
    {
        _variable = variable;
        _writer = writer;
        _context = context;

        _path = "$";
        _aliasPrefix = string.Empty;
    }

    public override void VisitNamedType(INamedTypeSymbol symbol)
    {
        var members = symbol.GetMembers();

        foreach (var member in members)
        {
            member.Accept(this);
        }
    }

    public override void VisitProperty(IPropertySymbol symbol)
    {
        if (!symbol.DeclaredAccessibility.HasFlag(Accessibility.Public)
            || !symbol.DeclaredAccessibility.HasFlag(Accessibility.Internal))
        {
            return;
        }

        var redisProperty = symbol.ToRedisProperty(_context);

        if (redisProperty is {Type: RedisPropertyType.Ignore})
        {
            return;
        }

        _path = string.Concat(_path, ".", redisProperty.Name);

        switch (redisProperty.Type)
        {
            case RedisPropertyType.Unknown:
                HandleUnknownProperty(symbol, redisProperty.Attribute);
                break;
            case RedisPropertyType.Object:
                HandleObjectProperty(symbol, redisProperty.Attribute);
                break;
            case RedisPropertyType.Text:
                HandleTextProperty(symbol, redisProperty.Attribute);
                break;
            case RedisPropertyType.Numeric:
                HandleNumericProperty(symbol, redisProperty.Attribute);
                break;
            case RedisPropertyType.Tag:
                HandleTagProperty(symbol, redisProperty.Attribute);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _path = _path.Substring(0, _path.Length - redisProperty.Name.Length - 1);
    }

    private void HandleObjectProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        var aliasSubPrefix = attribute?.GetNamedArgumentValue<string>(nameof(RedisObjectAttribute.AliasPrefix));

        if (!string.IsNullOrEmpty(aliasSubPrefix))
        {
            _aliasPrefix = string.Concat(_aliasPrefix, aliasSubPrefix);
        }

        symbol.Type.Accept(this);

        if (!string.IsNullOrEmpty(aliasSubPrefix))
        {
            _aliasPrefix = _aliasPrefix.Substring(0, _aliasPrefix.Length - aliasSubPrefix!.Length);
        }
    }

    private void HandleUnknownProperty(IPropertySymbol symbol, AttributeData? _)
    {
        var propertyType = symbol.Type.ToDisplayString();

        _context.ReportDiagnostic(Diagnostics.UnknownPropertyType
            .ToDiagnostic(symbol.Locations.FirstOrDefault(), propertyType));
    }
}
