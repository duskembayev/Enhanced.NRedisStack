using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public class SchemaGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (node, _) => node is MethodDeclarationSyntax {AttributeLists.Count: > 0, Modifiers: {Count: > 0}},
                (ctx, _) => GetMethodDeclarationForSourceGen(ctx))
            .Where(t => t.matches)
            .Select((t, _) => t.method);

        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
    }

    private static (MethodDeclarationSyntax method, bool matches) GetMethodDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;

        foreach (var attributeListSyntax in methodDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
            {
                continue;
            }

            var attributeType = attributeSymbol.ContainingType.ToDisplayString();

            if (attributeType != Constants.GeneratedSchemaAttributeFullName)
            {
                continue;
            }

            return (methodDeclarationSyntax, true);
        }

        return (methodDeclarationSyntax, false);
    }

    private static void GenerateCode(
        SourceProductionContext context, Compilation compilation,
        ImmutableArray<MethodDeclarationSyntax> methodDeclarations)
    {
        foreach (var methodDeclaration in methodDeclarations)
        {
            var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);
            var methodSymbol = (IMethodSymbol?)semanticModel.GetDeclaredSymbol(methodDeclaration);

            if (methodSymbol is null)
            {
                context.ReportDiagnostic(
                    Diagnostics.UnexpectedError.ToDiagnostic(
                        methodDeclaration.GetLocation(),
                        "Method symbol is null"));
                continue;
            }

            if (!methodSymbol.IsPartialDefinition)
            {
                context.ReportDiagnostic(Diagnostics.MethodNotPartial.ToDiagnostic(methodDeclaration.GetLocation()));
                continue;
            }

            if (methodSymbol.Parameters.Length > 0 || methodSymbol.TypeParameters.Length > 0)
            {
                context.ReportDiagnostic(Diagnostics.MethodHasParameters.ToDiagnostic(methodDeclaration.GetLocation()));
                continue;
            }

            var attribute = methodSymbol.GetAttribute(Constants.GeneratedSchemaAttributeFullName);
            var modelSymbol = attribute.GetCtorArgumentValue<INamedTypeSymbol>(0);
            var namingPolicy = attribute.GetNamedArgumentValue<PropertyNamingPolicy>(
                nameof(GeneratedSchemaAttribute.PropertyNamingPolicy));

            if (modelSymbol is null)
            {
                context.ReportDiagnostic(
                    Diagnostics.UnexpectedError.ToDiagnostic(
                        methodDeclaration.GetLocation(),
                        "Model symbol is null"));
                continue;
            }

            var sourceText = GenerateCore(methodSymbol, modelSymbol, new SchemaContext(context, namingPolicy));
            var sourceFileName = $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name}.g.cs";

            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static SourceText GenerateCore(
        IMethodSymbol methodSymbol,
        INamedTypeSymbol modelSymbol,
        SchemaContext context)
    {
        using var writer = new SchemaWriter();

        using (writer.DeclarePartialClass(methodSymbol.ContainingType))
        using (writer.DeclarePartialMethod(methodSymbol))
        {
            writer.WriteLine("Schema schema = new Schema();");
            modelSymbol.Accept(new SchemaMemberVisitor("schema", writer, context));
            writer.WriteLine("return schema;");
        }

        return writer.ToSourceText();
    }
}
