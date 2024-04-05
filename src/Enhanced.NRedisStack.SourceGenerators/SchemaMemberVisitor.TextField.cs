using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor
{
    private void HandleTextProperty(IPropertySymbol _, AttributeData? attribute)
    {
        // https://github.com/redis/NRedisStack/blob/03162ee3a68e01db0c420786f7e15c98aceeb7a3/src/NRedisStack/Search/Schema.cs#L318
        var propertyWriter = new PropertyWriter(_variable, _path, "AddTextField");

        if (attribute is not null)
        {
            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.Key)
                {
                    case nameof(RedisTextAttribute.Name):
                    {
                        break;
                    }
                    case nameof(RedisTextAttribute.Alias) when argument.Value.Value is null:
                    {
                        propertyWriter.SetAlias(_aliasPrefix, null);
                        break;
                    }
                    case nameof(RedisTextAttribute.Alias) when argument.Value.Value is string alias:
                    {
                        propertyWriter.SetAlias(_aliasPrefix, alias);
                        break;
                    }
                    case nameof(RedisTextAttribute.Weight) when argument.Value.Value is double weight:
                    {
                        propertyWriter.AddDoubleArgument("weight", weight);
                        break;
                    }
                    case nameof(RedisTextAttribute.Sortable) when argument.Value.Value is bool sortable:
                    {
                        propertyWriter.AddBoolArgument("sortable", sortable);
                        break;
                    }
                    case nameof(RedisTextAttribute.Unf) when argument.Value.Value is bool unf:
                    {
                        propertyWriter.AddBoolArgument("unf", unf);
                        break;
                    }
                    case nameof(RedisTextAttribute.NoStem) when argument.Value.Value is bool noStem:
                    {
                        propertyWriter.AddBoolArgument("noStem", noStem);
                        break;
                    }
                    case nameof(RedisTextAttribute.Phonetic) when argument.Value.Value is string phonetic:
                    {
                        propertyWriter.AddStringArgument("phonetic", phonetic);
                        break;
                    }
                    case nameof(RedisTextAttribute.NoIndex) when argument.Value.Value is bool noIndex:
                    {
                        propertyWriter.AddBoolArgument("noIndex", noIndex);
                        break;
                    }
                    case nameof(RedisTextAttribute.WithSuffixTrie) when argument.Value.Value is bool withSuffixTrie:
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