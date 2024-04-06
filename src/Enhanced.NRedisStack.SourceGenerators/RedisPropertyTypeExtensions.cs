using Enhanced.NRedisStack.Annotation;
using static Enhanced.NRedisStack.SourceGenerators.Constants;

namespace Enhanced.NRedisStack.SourceGenerators;

public static class RedisPropertyTypeExtensions
{
    private static readonly ImmutableHashSet<SpecialType> _numericSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_Int16, SpecialType.System_Int32, SpecialType.System_Int64,
        SpecialType.System_UInt16, SpecialType.System_UInt32, SpecialType.System_UInt64,
        SpecialType.System_Decimal, SpecialType.System_Single, SpecialType.System_Double,
        SpecialType.System_Byte, SpecialType.System_SByte, SpecialType.System_Enum
    );

    private static readonly ImmutableHashSet<SpecialType> _textSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_String, SpecialType.System_Char, SpecialType.System_DateTime
    );

    private static readonly ImmutableHashSet<SpecialType> _tagSpecialTypes = ImmutableHashSet.Create(
        SpecialType.System_Boolean
    );

    public static RedisPropertyInfo ToRedisProperty(this IPropertySymbol property, SchemaContext context)
    {
        var (propertyType, attribute) = property.FindRedisProperty(context);
        var propertyName = property.GetRedisPropertyName(attribute, context);

        if (propertyType is RedisPropertyType.Unknown)
        {
            propertyType = property.Type.ToRedisType();
        }

        return new RedisPropertyInfo(propertyName, propertyType, attribute);
    }

    private static string GetRedisPropertyName(
        this IPropertySymbol property,
        AttributeData? attribute,
        SchemaContext context)
    {
        var value = attribute?.GetNamedArgumentValue<string>(nameof(RedisFieldAttribute.Name));

        if (string.IsNullOrEmpty(value))
        {
            value = property.Name;
        }

        return context.ConvertName(value!);
    }

    private static (RedisPropertyType, AttributeData?) FindRedisProperty(
        this IPropertySymbol property,
        SchemaContext context)
    {
        var attributes = property.GetAttributes();
        var resultType = RedisPropertyType.Unknown;

        AttributeData? resultAttribute = null;

        foreach (var attr in attributes)
        {
            var attrTypeName = attr.AttributeClass?.ToDisplayString();
            var attrRedisType = attrTypeName switch
            {
                RedisNumericAttributeFullName => RedisPropertyType.Numeric,
                RedisTextAttributeFullName => RedisPropertyType.Text,
                RedisTagAttributeFullName => RedisPropertyType.Tag,
                RedisObjectAttributeFullName => RedisPropertyType.Object,
                RedisIgnoreAttributeFullName => RedisPropertyType.Ignore,
                _ => RedisPropertyType.Unknown
            };

            if (attrRedisType is RedisPropertyType.Unknown)
            {
                continue;
            }

            if (resultAttribute is not null)
            {
                context.ReportDiagnostic(Diagnostics.MultipleRedisAttributes.ToDiagnostic(property.Locations.First()));
                continue;
            }

            resultType = attrRedisType;
            resultAttribute = attr;
        }

        return (resultType, resultAttribute);
    }

    private static RedisPropertyType ToRedisType(this ITypeSymbol type)
    {
        if (type is {SpecialType: SpecialType.None, IsReferenceType: true})
        {
            return RedisPropertyType.Object;
        }

        if (_numericSpecialTypes.Contains(type.SpecialType))
        {
            return RedisPropertyType.Numeric;
        }

        if (_textSpecialTypes.Contains(type.SpecialType))
        {
            return RedisPropertyType.Text;
        }

        if (_tagSpecialTypes.Contains(type.SpecialType))
        {
            return RedisPropertyType.Tag;
        }

        return RedisPropertyType.Unknown;
    }
}
