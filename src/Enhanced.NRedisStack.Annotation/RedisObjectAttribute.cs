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
    ///     The depth into the object graph.
    /// </summary>
    public short CascadeDepth { get; set; }
}
