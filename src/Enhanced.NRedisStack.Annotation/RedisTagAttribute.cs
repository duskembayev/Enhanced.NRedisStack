namespace Enhanced.NRedisStack.Annotation;

public sealed class RedisTagAttribute : RedisPropertyAttribute
{
    public bool Sortable { get; set; } = false;

    public bool Unf { get; set; } = false;

    public bool NoIndex { get; set; } = false;

    public string Separator { get; set; } = ",";

    public bool CaseSensitive { get; set; } = false;

    public bool WithSuffixTrie { get; set; } = false;
}