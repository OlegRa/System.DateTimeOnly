﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Text;
using System.Diagnostics;
using System.Text.Json.Serialization;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace System.Text.Json;

/// <summary>
/// Custom converter for handling the <see cref="TimeOnly"/> data type with the <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library.
/// </summary>
/// <remarks>
/// This class backported from:
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/TimeOnlyConverter.cs">
/// System.Text.Json.Serialization.Converters.TimeOnlyConverter</see>
/// </remarks>
public sealed class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const int MinimumTimeOnlyFormatLength = 8; // hh:mm:ss
    // ReSharper disable once CommentTypo
    private const int MaximumTimeOnlyFormatLength = 16; // hh:mm:ss.fffffff
    private const int MaximumEscapedTimeOnlyFormatLength = JsonConstants.MaxExpansionFactorWhileEscaping * MaximumTimeOnlyFormatLength;

    /// <inheritdoc />
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            ThrowHelper.ThrowInvalidOperationException_ExpectedString(reader.TokenType);
        }

        return ReadCore(ref reader);
    }

    private static TimeOnly ReadCore(ref Utf8JsonReader reader)
    {
        Debug.Assert(reader.TokenType is JsonTokenType.String or JsonTokenType.PropertyName);

        if (!JsonHelpers.IsInRangeInclusive(reader.ValueLength(), MinimumTimeOnlyFormatLength, MaximumEscapedTimeOnlyFormatLength))
        {
            ThrowHelper.ThrowFormatException(DataType.TimeOnly);
        }

        scoped ReadOnlySpan<byte> source;
        if (!reader.HasValueSequence && !reader.ValueIsEscaped)
        {
            source = reader.ValueSpan;
        }
        else
        {
            Span<byte> stackSpan = stackalloc byte[MaximumEscapedTimeOnlyFormatLength];
            int bytesWritten = reader.CopyString(stackSpan);
            source = stackSpan.Slice(0, bytesWritten);
        }

        byte firstChar = source[0];
        int firstSeparator = source.IndexOfAny((byte)'.', (byte)':');
        if (!JsonHelpers.IsDigit(firstChar) || firstSeparator < 0 || source[firstSeparator] == (byte)'.')
        {
            // Note: Utf8Parser.TryParse permits leading whitespace, negative values
            // and numbers of days, so we need to exclude these cases here.
            ThrowHelper.ThrowFormatException(DataType.TimeOnly);
        }

        bool result = Utf8Parser.TryParse(source, out TimeSpan timespan, out int bytesConsumed, 'c');

        // Note: Utf8Parser.TryParse will return true for invalid input so
        // long as it starts with an integer. Example: "2021-06-18" or
        // "1$$$$$$$$$$". We need to check bytesConsumed to know if the
        // entire source was actually valid.

        if (!result || source.Length != bytesConsumed)
        {
            ThrowHelper.ThrowFormatException(DataType.TimeOnly);
        }

        Debug.Assert(TimeOnly.MinValue.ToTimeSpan() <= timespan && timespan <= TimeOnly.MaxValue.ToTimeSpan());
        return TimeOnly.FromTimeSpan(timespan);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        Span<byte> output = stackalloc byte[MaximumTimeOnlyFormatLength];

        // ReSharper disable once RedundantAssignment
        bool result = Utf8Formatter.TryFormat(value.ToTimeSpan(), output, out int bytesWritten, 'c');
        Debug.Assert(result);

        writer.WriteStringValue(output.Slice(0, bytesWritten));
    }
}
