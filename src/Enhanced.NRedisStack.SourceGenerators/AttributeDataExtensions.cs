namespace Enhanced.NRedisStack.SourceGenerators;

public static class AttributeDataExtensions
{
    public static T? GetNamedArgumentValue<T>(this AttributeData attribute, string key, T? defaultValue = default)
    {
        var constant = attribute.NamedArguments
            .FirstOrDefault(x => x.Key == key)
            .Value;

        return constant.Value is T value ? value : defaultValue;
    }
}
