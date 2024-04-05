#pragma warning disable CS9113

namespace Enhanced.NRedisStack.Annotation;
/// <summary>
/// Attribute used to generate a schema for a given type.
/// </summary>
/// <param name="type">The type to generate a schema for.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class GeneratedSchemaAttribute(Type type) : Attribute
{
    /// <summary>
    /// Determines the property naming policy used to generate the schema.
    /// </summary>
    public PropertyNamingPolicy PropertyNamingPolicy { get; set; }
}
