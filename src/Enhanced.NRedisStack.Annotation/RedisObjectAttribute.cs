namespace Enhanced.NRedisStack.Annotation;

/// <summary>
///     Marks a property as a Redis object containing child set of properties.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RedisObjectAttribute : Attribute
{
    /// <summary>
    ///     The field's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     The prefix to use for the containing field's alias.
    /// </summary>
    public string? AliasPrefix { get; set; }
}
