using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor : SymbolVisitor
{
    private readonly SchemaContext _context;
    private readonly string _variable;
    private readonly SchemaWriter _writer;
    private string _path;
    private int _depthMax;
    private int _depth;

    public SchemaMemberVisitor(string variable, SchemaWriter writer, SchemaContext context)
    {
        _variable = variable;
        _writer = writer;
        _context = context;

        _path = "$";
        _depthMax = 1;
    }

    public override void VisitNamedType(INamedTypeSymbol symbol)
    {
        _depth++;

        if (_depth <= _depthMax)
        {
            var members = symbol.GetMembers();

            foreach (var member in members)
            {
                member.Accept(this);
            }
        }

        _depth--;
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
                HandleUnknownProperty(symbol);
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
            case RedisPropertyType.Ignore:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _path = _path.Substring(0, _path.Length - redisProperty.Name.Length - 1);
    }

    private void HandleObjectProperty(IPropertySymbol symbol, AttributeData? attribute)
    {
        var cascadeDepth = attribute?.GetNamedArgumentValue(nameof(RedisObjectAttribute.CascadeDepth), 1);

        if (cascadeDepth is null or < 1)
        {
            _context.ReportDiagnostic(
                Diagnostics.CascadeDepthNotSupported.ToDiagnostic(symbol.Locations.FirstOrDefault()));
            return;
        }

        _depthMax += cascadeDepth.Value;

        symbol.Type.Accept(this);

        _depthMax -= cascadeDepth.Value;
    }

    private void HandleUnknownProperty(IPropertySymbol symbol)
    {
        var propertyType = symbol.Type.ToDisplayString();

        _context.ReportDiagnostic(Diagnostics.UnknownPropertyType
            .ToDiagnostic(symbol.Locations.FirstOrDefault(), propertyType));
    }
}
