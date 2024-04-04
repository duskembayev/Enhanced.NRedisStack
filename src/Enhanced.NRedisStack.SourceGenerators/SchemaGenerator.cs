using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Enhanced.NRedisStack.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public class SchemaGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Filter abstract methods annotated with the [SchemaOf] attribute. Only filtered Syntax Nodes can trigger code generation.
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (node, _) => node is MethodDeclarationSyntax {AttributeLists.Count: > 0, Modifiers: {Count: > 0}},
                (ctx, _) => GetMethodDeclarationForSourceGen(ctx))
            .Where(t => t.matches)
            .Select((t, _) => t.method);

        // Generate the source code.
        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            ((ctx, t) => GenerateCode(ctx, t.Left, t.Right)));
    }

    private static (MethodDeclarationSyntax method, bool matches) GetMethodDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var methodDeclarationSyntax = (MethodDeclarationSyntax) context.Node;

        foreach (AttributeListSyntax attributeListSyntax in methodDeclarationSyntax.AttributeLists)
        foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue;

            var attributeType = attributeSymbol.ContainingType.ToDisplayString();

            if (attributeType != Constants.GeneratedSchemaAttributeFullName)
                continue;

            return (methodDeclarationSyntax, true);
        }

        return (methodDeclarationSyntax, false);
    }

    private void GenerateCode(
        SourceProductionContext context, Compilation compilation,
        ImmutableArray<MethodDeclarationSyntax> methodDeclarations)
    {
        foreach (var methodDeclaration in methodDeclarations)
        {
            var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);
            var declaredSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

            if (declaredSymbol is not IMethodSymbol methodSymbol)
            {
                // Log error
                continue;
            }

            if (!methodSymbol.IsPartialDefinition)
            {
                // Log error
                continue;
            }

            if (methodSymbol.Parameters.Length > 0)
            {
                // Log error
                continue;
            }

            if (methodSymbol.TypeParameters.Length > 0)
            {
                // Log error
                continue;
            }

            var attributeValue = methodSymbol
                .GetAttributes()
                .First(data => data.AttributeClass?.ToDisplayString() == Constants.GeneratedSchemaAttributeFullName)
                .ConstructorArguments
                .First();

            if (attributeValue is not {Kind: TypedConstantKind.Type})
            {
                // Log error
                continue;
            }

            if (attributeValue.Value is not INamedTypeSymbol modelSymbol)
            {
                // Log error
                continue;
            }

            var sourceText = GenerateCore(methodSymbol, modelSymbol);
            var sourceFileName = $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name}.g.cs";

            context.AddSource(sourceFileName, sourceText);
        }
    }

    private static SourceText GenerateCore(IMethodSymbol methodSymbol, INamedTypeSymbol modelSymbol)
    {
        using var writer = new SchemaWriter();

        using (writer.DeclarePartialClass(methodSymbol.ContainingType))
        using (writer.DeclarePartialMethod(methodSymbol))
        {
            writer.WriteLine("Schema schema = new Schema();");
            modelSymbol.Accept(new SchemaMemberVisitor("schema", writer));
            writer.WriteLine("return schema;");
        }

        return writer.ToSourceText();
    }
}