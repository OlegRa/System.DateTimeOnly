// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Custom converter for handling the <see cref="DateOnly"/> data type with the <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// </summary>
/// <remarks>
/// This class backported from:
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/DateOnlyConverter.cs">
/// System.Text.Json.Serialization.Converters.DateOnlyConverter</see>
/// </remarks>
public sealed class DateOnlyConverter : JsonConverter<DateOnly>
{
    private const int FormatLength = 10; // YYYY-MM-DD

    private const int MaxEscapedFormatLength = FormatLength * JsonConstants.MaxExpansionFactorWhileEscaping;

    /// <inheritdoc />
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            ThrowHelper.ThrowInvalidOperationException_ExpectedString(reader.TokenType);
        }

        return ReadCore(ref reader);
    }

    /// <inheritdoc />
    public override DateOnly ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
        return ReadCore(ref reader);
    }

    private static DateOnly ReadCore(ref Utf8JsonReader reader)
    {
        if (!JsonHelpers.IsInRangeInclusive(reader.ValueLength(), FormatLength, MaxEscapedFormatLength))
        {
            ThrowHelper.ThrowFormatException(DataType.DateOnly);
        }

        scoped ReadOnlySpan<byte> source;
        if (!reader.HasValueSequence && !reader.ValueIsEscaped)
        {
            source = reader.ValueSpan;
        }
        else
        {
            Span<byte> stackSpan = stackalloc byte[MaxEscapedFormatLength];
            int bytesWritten = reader.CopyString(stackSpan);
            source = stackSpan.Slice(0, bytesWritten);
        }

        if (!JsonHelpers.TryParseAsIso(source, out DateOnly value))
        {
            ThrowHelper.ThrowFormatException(DataType.DateOnly);
        }

        return value;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
#if NET8_0_OR_GREATER
        Span<byte> buffer = stackalloc byte[FormatLength];
#else
        Span<char> buffer = stackalloc char[FormatLength];
#endif
        // ReSharper disable once RedundantAssignment
        bool formattedSuccessfully = value.TryFormat(buffer, out int charsWritten, "O".AsSpan(), CultureInfo.InvariantCulture);
        Debug.Assert(formattedSuccessfully && charsWritten == FormatLength);
        writer.WriteStringValue(buffer);
    }

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
#if NET8_0_OR_GREATER
        Span<byte> buffer = stackalloc byte[FormatLength];
#else
        Span<char> buffer = stackalloc char[FormatLength];
#endif
        // ReSharper disable once RedundantAssignment
        bool formattedSuccessfully = value.TryFormat(buffer, out int charsWritten, "O".AsSpan(), CultureInfo.InvariantCulture);
        Debug.Assert(formattedSuccessfully && charsWritten == FormatLength);
        writer.WritePropertyName(buffer);
    }}
