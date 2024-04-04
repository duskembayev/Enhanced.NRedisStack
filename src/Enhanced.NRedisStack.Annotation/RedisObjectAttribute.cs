namespace Enhanced.NRedisStack.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public class RedisObjectAttribute
{
    public string? Name { get; set; }
    public string? AliasPrefix { get; set; }
}