namespace Enhanced.NRedisStack.Annotation;

public sealed class RedisTextAttribute : RedisPropertyAttribute
{
    public double Weight { get; set; } = 1.0;

    public bool NoStem { get; set; } = false;

    public string? Phonetic { get; set; } = null;

    public bool Sortable { get; set; } = false;

    public bool Unf { get; set; } = false;

    public bool NoIndex { get; set; } = false;

    public bool WithSuffixTrie { get; set; } = false;
}