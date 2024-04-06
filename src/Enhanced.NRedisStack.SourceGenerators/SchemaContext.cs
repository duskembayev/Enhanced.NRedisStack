using System.Text.Json;
using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

public class SchemaContext
{
    private readonly SourceProductionContext _context;
    private readonly JsonNamingPolicy? _namePolicy;

    public SchemaContext(SourceProductionContext context, PropertyNamingPolicy namingPolicy)
    {
        _context = context;
        _namePolicy = namingPolicy switch
        {
            PropertyNamingPolicy.None => null,
            PropertyNamingPolicy.CamelCase => JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy.SnakeCaseLower => JsonNamingPolicy.SnakeCaseLower,
            PropertyNamingPolicy.SnakeCaseUpper => JsonNamingPolicy.SnakeCaseUpper,
            PropertyNamingPolicy.KebabCaseLower => JsonNamingPolicy.KebabCaseLower,
            PropertyNamingPolicy.KebabCaseUpper => JsonNamingPolicy.KebabCaseUpper,
            _ => throw new ArgumentOutOfRangeException(nameof(namingPolicy), namingPolicy, null)
        };
    }

    public void ReportDiagnostic(Diagnostic diagnostic) => _context.ReportDiagnostic(diagnostic);
    public string ConvertName(string name) => _namePolicy?.ConvertName(name) ?? name;
}
