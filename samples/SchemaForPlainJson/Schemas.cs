using Enhanced.NRedisStack.Annotation;
using NRedisStack.Search;

namespace SchemaForPlainJson;

public static partial class Schemas
{
    [GeneratedSchema(typeof(Account))]
    public static partial Schema GetAccountSchema();
}
