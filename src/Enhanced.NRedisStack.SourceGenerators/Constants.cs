using Enhanced.NRedisStack.Annotation;

namespace Enhanced.NRedisStack.SourceGenerators;

public static class Constants
{
    public const string AnnotationNamespace = "Enhanced.NRedisStack.Annotation";
    public const string GeneratedSchemaAttributeFullName = AnnotationNamespace + "." + nameof(GeneratedSchemaAttribute);
    public const string RedisIgnoreAttributeFullName = AnnotationNamespace + "." + nameof(RedisIgnoreAttribute);
    public const string RedisNumericAttributeFullName = AnnotationNamespace + "." + nameof(RedisNumericAttribute);
    public const string RedisObjectAttributeFullName = AnnotationNamespace + "." + nameof(RedisObjectAttribute);
    public const string RedisTagAttributeFullName = AnnotationNamespace + "." + nameof(RedisTagAttribute);
    public const string RedisTextAttributeFullName = AnnotationNamespace + "." + nameof(RedisTextAttribute);
}
