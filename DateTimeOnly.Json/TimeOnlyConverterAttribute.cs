using System.Text.Json.Serialization;
#if NET6_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace System.Text.Json;

/// <inheritdoc />
public sealed class TimeOnlyConverterAttribute : JsonConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter CreateConverter(
        Type typeToConvert)
    {
#if NET6_0_OR_GREATER
        return JsonMetadataServices.DateOnlyConverter;
#else
        return new TimeOnlyConverter();
#endif
    }
}
