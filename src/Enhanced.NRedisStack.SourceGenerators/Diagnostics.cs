using System.Diagnostics.CodeAnalysis;

namespace Enhanced.NRedisStack.SourceGenerators;

[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
public static class Diagnostics
{
    public static readonly DiagnosticDescriptor UnexpectedError = new DiagnosticDescriptor(
        "ENRS0001",
        "Unexpected Error",
        "An unexpected error occurred: {0}",
        "Unknown",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MethodNotPartial = new DiagnosticDescriptor(
        "ENRS0100",
        "Method Not Partial",
        "Method must be declared as partial",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MethodHasParameters = new DiagnosticDescriptor(
        "ENRS0101",
        "Method Has Parameters",
        "Method must not have parameters",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor UnknownPropertyType = new DiagnosticDescriptor(
        "ENRS0102",
        "Unknown Property Type",
        "Unknown or unsupported property type: {0}",
        "Schema",
        DiagnosticSeverity.Error,
        true);

    public static Diagnostic ToDiagnostic(this DiagnosticDescriptor descriptor, Location? location,
        params object[] messageArgs)
    {
        return Diagnostic.Create(descriptor, location, messageArgs);
    }
}