using Enhanced.NRedisStack.Annotation;
using NRedisStack.Search;

namespace SchemaForPlainJson;

public static partial class Schemas
{
    [GeneratedSchema(typeof(Account))]
    public static partial Schema GetAccountSchema();

    public static Schema SchemaForAccount()
    {
        var schema = new Schema();

        schema.AddNumericField(new FieldName("", ""), sortable: false, noIndex: false);

        return schema;
    }
}