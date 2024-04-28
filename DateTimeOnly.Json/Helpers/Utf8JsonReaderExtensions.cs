namespace System.Text.Json;

internal static class Utf8JsonReaderExtensions
{
    internal static int ValueLength(
        this Utf8JsonReader reader) =>
        reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;
}
