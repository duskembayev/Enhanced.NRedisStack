using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Enhanced.NRedisStack.SourceGenerators;

public static class PropertySymbolExtensions
{
    private static readonly ImmutableHashSet<SpecialType> NumericSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_Int16, SpecialType.System_Int32, SpecialType.System_Int64,
        SpecialType.System_UInt16, SpecialType.System_UInt32, SpecialType.System_UInt64,
        SpecialType.System_Decimal, SpecialType.System_Single, SpecialType.System_Double,
        SpecialType.System_Byte, SpecialType.System_SByte, SpecialType.System_Enum
    );

    private static readonly ImmutableHashSet<SpecialType> TextSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_String, SpecialType.System_Char, SpecialType.System_DateTime
    );

    private static readonly ImmutableHashSet<SpecialType> TagSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_Boolean
    );

    public static string GetRedisName(this IPropertySymbol property, AttributeData? attribute)
    {
        var name = attribute?.NamedArguments
            .FirstOrDefault(x => x.Key == "Name")
            .Value;

        if (name?.Value is string redisName && !string.IsNullOrEmpty(redisName))
            return redisName;

        return property.Name;
    }

    public static RedisPropertyInfo ToRedisProperty(this IPropertySymbol property)
    {
        return property.GetAttributes().ResolveRedisPropertyFromAttributes()
               ?? new RedisPropertyInfo(property.Type.ToRedisType(), null);
    }

    private static RedisPropertyInfo? ResolveRedisPropertyFromAttributes(this IEnumerable<AttributeData> attributes)
    {
        var resultType = RedisPropertyType.Unknown;
        AttributeData? resultAttribute = null;

        foreach (var attr in attributes)
        {
            var attrTypeName = attr.AttributeClass?.ToDisplayString();
            var attrRedisType = attrTypeName switch
            {
                "Enhanced.NRedisStack.Annotation.RedisObjectAttribute" => RedisPropertyType.Object,
                "Enhanced.NRedisStack.Annotation.RedisNumericAttribute" => RedisPropertyType.Numeric,
                "Enhanced.NRedisStack.Annotation.RedisTextAttribute" => RedisPropertyType.Text,
                "Enhanced.NRedisStack.Annotation.RedisTagAttribute" => RedisPropertyType.Tag,
                _ => RedisPropertyType.Unknown
            };

            if (resultAttribute is not null)
            {
                // TODO report error
                continue;
            }

            resultType = attrRedisType;
            resultAttribute = attr;
        }

        if (resultAttribute is null)
            return null;

        return (resultType, resultAttribute);
    }

    private static RedisPropertyType ToRedisType(this ITypeSymbol type)
    {
        if (type is {SpecialType: SpecialType.None, IsReferenceType: true})
            return RedisPropertyType.Object;

        if (NumericSpecialTypes.Contains(type.SpecialType))
            return RedisPropertyType.Numeric;

        if (TextSpecialTypes.Contains(type.SpecialType))
            return RedisPropertyType.Text;

        if (TagSpecialTypes.Contains(type.SpecialType))
            return RedisPropertyType.Tag;

        return RedisPropertyType.Unknown;
    }
}