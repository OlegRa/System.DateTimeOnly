using System.Text.Json.Serialization;
#if NET6_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace System.Text.Json;

#if NET6_0_OR_GREATER
/// <summary>
/// Attribute for handling the <see cref="DateOnly"/> data type with the <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// At run-time selecting the built-in <see cref="JsonMetadataServices.DateOnlyConverter"/> converter using the converters factory method.
/// </summary>
#else
/// <summary>
/// Attribute for handling the <see cref="DateOnly"/> data type with the <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// At run-time selecting the backported <see cref="DateOnlyConverter"/> converter using the converters factory method.
/// </summary>
#endif
// ReSharper disable once UnusedType.Global
public sealed class DateOnlyConverterAttribute : JsonConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter CreateConverter(
        Type typeToConvert)
    {
#if NET6_0_OR_GREATER
        return JsonMetadataServices.DateOnlyConverter;
#else
        return new DateOnlyConverter();
#endif
    }
}
