namespace Enhanced.NRedisStack.Annotation;

/// <summary>
///     Marks a property as ignored when generating a Redis schema.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class RedisIgnoreAttribute : Attribute;
