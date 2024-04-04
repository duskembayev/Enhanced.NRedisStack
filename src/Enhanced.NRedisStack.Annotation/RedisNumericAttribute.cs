namespace Enhanced.NRedisStack.Annotation;

public sealed class RedisNumericAttribute : RedisPropertyAttribute
{
    public bool Sortable { get; set; } = false;

    public bool NoIndex { get; set; } = false;
}