namespace Enhanced.NRedisStack.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public class RedisObjectAttribute : Attribute
{
    public string? Name { get; set; }
    public string? AliasPrefix { get; set; }
}