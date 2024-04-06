using System.Diagnostics.CodeAnalysis;

namespace Enhanced.NRedisStack.SourceGenerators;

[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
public static class Diagnostics
{
    public static readonly DiagnosticDescriptor UnexpectedError = new DiagnosticDescriptor(
        "ENRS001",
        "Unexpected Error",
        "An unexpected error occurred: {0}",
        "Unknown",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MethodNotPartial = new DiagnosticDescriptor(
        "ENRS100",
        "Method Not Partial",
        "Method must be declared as partial",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MethodHasParameters = new DiagnosticDescriptor(
        "ENRS101",
        "Method Has Parameters",
        "Method must not have parameters",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor UnknownPropertyType = new DiagnosticDescriptor(
        "ENRS102",
        "Unknown Property Type",
        "Unknown or unsupported property type: {0}",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MultipleRedisAttributes = new DiagnosticDescriptor(
        "ENRS103",
        "Multiple Redis Attributes",
        "Multiple Redis attributes found on property",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static Diagnostic ToDiagnostic(this DiagnosticDescriptor descriptor, Location? location,
        params object[] messageArgs) =>
        Diagnostic.Create(descriptor, location, messageArgs);
}
