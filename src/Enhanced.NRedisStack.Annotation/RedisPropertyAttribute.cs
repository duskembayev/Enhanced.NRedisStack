namespace Enhanced.NRedisStack.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public abstract class RedisPropertyAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Alias { get; set; }
}