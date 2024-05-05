using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Helper attribute for the proper <see cref="DateOnly"/> struct instance handling by the
/// <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// </summary>
public sealed class JsonDateOnlyConverter : JsonConverter<DateOnly>
{
    private static readonly DateOnlyConverter DateOnlyConverter = new ();

    /// <inheritdoc />
    public override DateOnly Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateOnlyConverter.Read(ref reader, typeToConvert, options);

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        DateOnlyConverter.Write(writer, value, options);
}
