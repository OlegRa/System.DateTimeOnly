using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Helper attribute for the proper <see cref="TimeOnly"/> struct instance handling by the
/// <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// </summary>
public sealed class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
{
    private static readonly TimeOnlyConverter TimeOnlyConverter = new ();

    /// <inheritdoc />
    public override TimeOnly Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TimeOnlyConverter.Read(ref reader, typeToConvert, options);

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
        TimeOnlyConverter.Write(writer, value, options);
}
