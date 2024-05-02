namespace System.Tests;

internal static class SpanExtensions
{
    public static ReadOnlySpan<char> TrimEnd(this Span<char> source) =>
        new ReadOnlySpan<char>(source.ToArray()).TrimEnd();
}
