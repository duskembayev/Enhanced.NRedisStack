namespace Enhanced.NRedisStack.Annotation;

/// <summary>
///     Marks a property as a TAG field of a Redis schema.
/// </summary>
public sealed class RedisTagAttribute : RedisFieldAttribute
{
    /// <summary>
    ///     NUMERIC, TAG, TEXT, or GEO attributes can have an optional SORTABLE argument.
    ///     As the user sorts the results by the value of this attribute, the results are available with very low latency.
    ///     Note that his adds memory overhead, so consider not declaring it on large text attributes.
    ///     You can sort an attribute without the SORTABLE option, but the latency is not as good as with SORTABLE.
    /// </summary>
    public bool Sortable { get; set; } = false;

    /// <summary>
    ///     By default, for hashes (not with JSON) SORTABLE applies a normalization to the indexed value (characters set to
    ///     lowercase, removal of diacritics).
    ///     When using the unnormalized form (UNF), you can disable the normalization and keep the original form of the value.
    ///     With JSON, UNF is implicit with SORTABLE (normalization is disabled).
    /// </summary>
    public bool Unf { get; set; } = false;

    /// <summary>
    ///     Attributes can have the NOINDEX option, which means they will not be indexed.
    ///     This is useful in conjunction with SORTABLE, to create attributes whose update using PARTIAL will not cause full
    ///     reindexing of the document.
    ///     If an attribute has NOINDEX and doesn't have SORTABLE, it will just be ignored by the index.
    /// </summary>
    public bool NoIndex { get; set; } = false;

    /// <summary>
    ///     Indicates how the text contained in the attribute is to be split into individual tags.
    ///     The default is <c>,</c>. The value must be a single character.
    /// </summary>
    public string Separator { get; set; } = ",";

    /// <summary>
    ///     Keeps the original letter cases of the tags. If not specified, the characters are converted to lowercase.
    /// </summary>
    public bool CaseSensitive { get; set; } = false;

    /// <summary>
    ///     Keeps a suffix trie with all terms which match the suffix.
    ///     It is used to optimize contains (foo) and suffix (*foo) queries.
    ///     Otherwise, a brute-force search on the trie is performed.
    ///     If suffix trie exists for some fields, these queries will be disabled for other fields.
    /// </summary>
    public bool WithSuffixTrie { get; set; } = false;
}
