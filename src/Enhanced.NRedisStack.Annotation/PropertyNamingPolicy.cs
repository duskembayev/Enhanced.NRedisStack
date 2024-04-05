namespace Enhanced.NRedisStack.Annotation;

/// <summary>
///     Determines the naming policy used to convert a string-based name to another format, such as a camel-casing format.
/// </summary>
public enum PropertyNamingPolicy
{
    /// <summary>
    ///     No naming policy is used.
    /// </summary>
    None,

    /// <summary>
    ///     Gets the naming policy for camel-casing.
    /// </summary>
    CamelCase,

    /// <summary>
    ///     Gets the naming policy for lowercase snake-casing.
    /// </summary>
    SnakeCaseLower,

    /// <summary>
    ///     Gets the naming policy for uppercase snake-casing.
    /// </summary>
    SnakeCaseUpper,

    /// <summary>
    ///     Gets the naming policy for lowercase kebab-casing.
    /// </summary>
    KebabCaseLower,

    /// <summary>
    ///     Gets the naming policy for uppercase kebab-casing.
    /// </summary>
    KebabCaseUpper
}
