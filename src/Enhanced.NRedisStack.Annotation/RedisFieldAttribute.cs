namespace Enhanced.NRedisStack.Annotation;

/// <summary>
/// Base class containing common properties for any field of a Redis schema.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class RedisFieldAttribute : Attribute
{
    /// <summary>
    /// The field's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The field's alias.
    /// </summary>
    public string? Alias { get; set; }
}