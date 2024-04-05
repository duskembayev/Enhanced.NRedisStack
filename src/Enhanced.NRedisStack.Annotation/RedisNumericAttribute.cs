namespace Enhanced.NRedisStack.Annotation;

/// <summary>
/// Marks a property as a NUMERIC field of a Redis schema.
/// </summary>
public sealed class RedisNumericAttribute : RedisFieldAttribute
{
    /// <summary>
    /// NUMERIC, TAG, TEXT, or GEO attributes can have an optional SORTABLE argument.
    /// As the user sorts the results by the value of this attribute, the results are available with very low latency. 
    /// Note that his adds memory overhead, so consider not declaring it on large text attributes.
    /// You can sort an attribute without the SORTABLE option, but the latency is not as good as with SORTABLE.
    /// </summary>
    public bool Sortable { get; set; } = false;

    /// <summary>
    /// Attributes can have the NOINDEX option, which means they will not be indexed.
    /// This is useful in conjunction with SORTABLE, to create attributes whose update using PARTIAL will not cause full reindexing of the document.
    /// If an attribute has NOINDEX and doesn't have SORTABLE, it will just be ignored by the index.
    /// </summary>
    public bool NoIndex { get; set; } = false;
}