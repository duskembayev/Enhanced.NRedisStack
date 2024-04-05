using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

internal partial class SchemaMemberVisitor
{
    private void HandleNumericProperty(IPropertySymbol _, AttributeData? attribute)
    {
        // https://github.com/redis/NRedisStack/blob/03162ee3a68e01db0c420786f7e15c98aceeb7a3/src/NRedisStack/Search/Schema.cs#L382
        var propertyWriter = new PropertyWriter(_variable, _path, "AddNumericField");

        if (attribute is not null)
        {
            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.Key)
                {
                    case nameof(RedisNumericAttribute.Name):
                        {
                            break;
                        }
                    case nameof(RedisNumericAttribute.Alias) when argument.Value.Value is null:
                        {
                            propertyWriter.SetAlias(_aliasPrefix, null);
                            break;
                        }
                    case nameof(RedisNumericAttribute.Alias) when argument.Value.Value is string alias:
                        {
                            propertyWriter.SetAlias(_aliasPrefix, alias);
                            break;
                        }
                    case nameof(RedisNumericAttribute.Sortable) when argument.Value.Value is bool sortable:
                        {
                            propertyWriter.AddBoolArgument("sortable", sortable);
                            break;
                        }
                    case nameof(RedisNumericAttribute.NoIndex) when argument.Value.Value is bool noIndex:
                        {
                            propertyWriter.AddBoolArgument("noIndex", noIndex);
                            break;
                        }
                }
            }
        }

        propertyWriter.Write(_writer);
    }
}
