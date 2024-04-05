using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor
{
    private void HandleTagProperty(IPropertySymbol _, AttributeData? attribute)
    {
        // https://github.com/redis/NRedisStack/blob/03162ee3a68e01db0c420786f7e15c98aceeb7a3/src/NRedisStack/Search/Schema.cs#L413
        var propertyWriter = new PropertyWriter(_variable, _path, "AddTagField");

        if (attribute is not null)
        {
            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.Key)
                {
                    case nameof(RedisTagAttribute.Name):
                    {
                        break;
                    }
                    case nameof(RedisTagAttribute.Alias) when argument.Value.Value is null:
                    {
                        propertyWriter.SetAlias(_aliasPrefix, null);
                        break;
                    }
                    case nameof(RedisTagAttribute.Alias) when argument.Value.Value is string alias:
                    {
                        propertyWriter.SetAlias(_aliasPrefix, alias);
                        break;
                    }
                    case nameof(RedisTagAttribute.Sortable) when argument.Value.Value is bool sortable:
                    {
                        propertyWriter.AddBoolArgument("sortable", sortable);
                        break;
                    }
                    case nameof(RedisTagAttribute.Unf) when argument.Value.Value is bool unf:
                    {
                        propertyWriter.AddBoolArgument("unf", unf);
                        break;
                    }
                    case nameof(RedisTagAttribute.NoIndex) when argument.Value.Value is bool noIndex:
                    {
                        propertyWriter.AddBoolArgument("noIndex", noIndex);
                        break;
                    }
                    case nameof(RedisTagAttribute.Separator) when argument.Value.Value is string separator:
                    {
                        propertyWriter.AddStringArgument("separator", separator);
                        break;
                    }
                    case nameof(RedisTagAttribute.CaseSensitive) when argument.Value.Value is bool caseSensitive:
                    {
                        propertyWriter.AddBoolArgument("caseSensitive", caseSensitive);
                        break;
                    }
                    case nameof(RedisTagAttribute.WithSuffixTrie) when argument.Value.Value is bool withSuffixTrie:
                    {
                        propertyWriter.AddBoolArgument("withSuffixTrie", withSuffixTrie);
                        break;
                    }
                }
            }
        }

        propertyWriter.Write(_writer);
    }
}