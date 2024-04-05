namespace Enhanced.NRedisStack.Annotation;

/// <summary>
///     Marks a property as a TEXT field in a Redis schema.
/// </summary>
public sealed class RedisTextAttribute : RedisFieldAttribute
{
    /// <summary>
    ///     Declares the importance of this attribute when calculating result accuracy.
    ///     This is a multiplication factor, and defaults to 1 if not specified.
    /// </summary>
    public double Weight { get; set; } = 1.0;

    /// <summary>
    ///     Text attributes can have the NOSTEM argument that disables stemming when indexing its values.
    ///     This may be ideal for things like proper names.
    /// </summary>
    public bool NoStem { get; set; } = false;

    /// <summary>
    ///     Declaring a text attribute as PHONETIC will perform phonetic matching on it in searches by default.
    ///     The obligatory {matcher} argument specifies the phonetic algorithm and language used.
    ///     <list type="bullet">
    ///         <listheader>
    ///             <description>The following matchers are supported:</description>
    ///         </listheader>
    ///         <item>
    ///             <description>dm:en - Double metaphone for English</description>
    ///         </item>
    ///         <item>
    ///             <description>dm:fr - Double metaphone for French</description>
    ///         </item>
    ///         <item>
    ///             <description>dm:pt - Double metaphone for Portuguese</description>
    ///         </item>
    ///         <item>
    ///             <description>dm:es - Double metaphone for Spanish</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public string? Phonetic { get; set; } = null;

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
    ///     Keeps a suffix trie with all terms which match the suffix.
    ///     It is used to optimize contains (foo) and suffix (*foo) queries.
    ///     Otherwise, a brute-force search on the trie is performed.
    ///     If suffix trie exists for some fields, these queries will be disabled for other fields.
    /// </summary>
    public bool WithSuffixTrie { get; set; } = false;
}
