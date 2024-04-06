namespace Enhanced.NRedisStack.SourceGenerators;

public static class SymbolExtensions
{
    public static T? GetNamedArgumentValue<T>(this AttributeData attribute, string key, T? defaultValue = default)
    {
        var constant = attribute.NamedArguments
            .FirstOrDefault(namedArgument => namedArgument.Key == key)
            .Value;

        if (constant.Equals(default))
        {
            return defaultValue;
        }

        return (T?)constant.Value;
    }

    public static T? GetCtorArgumentValue<T>(this AttributeData attribute, int index, T? defaultValue = default)
    {
        var constant = attribute.ConstructorArguments
            .ElementAtOrDefault(index);

        if (constant.Equals(default))
        {
            return defaultValue;
        }

        return (T?)constant.Value;
    }

    public static AttributeData GetAttribute(this IMethodSymbol method, string attributeFullName) =>
        method.GetAttributes().First(attribute => attribute.AttributeClass?.ToDisplayString() == attributeFullName);
}
