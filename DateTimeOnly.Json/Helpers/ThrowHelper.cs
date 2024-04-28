// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json;

internal static class ThrowHelper
{
    private const string ExceptionSourceValueToRethrowAsJsonException = "System.Text.Json.Rethrowable";

    [DoesNotReturn]
    public static void ThrowInvalidOperationException_ExpectedString(
        JsonTokenType tokenType)
    {
        throw GetInvalidOperationException("string", tokenType);
    }

    public static void ThrowFormatException(
        DataType dataType)
    {
        throw new FormatException(SR.Format(SR.UnsupportedFormat, dataType))
            { Source = ExceptionSourceValueToRethrowAsJsonException };
    }

    private static Exception GetInvalidOperationException(string message, JsonTokenType tokenType)
    {
        return GetInvalidOperationException(SR.Format(SR.InvalidCast, tokenType, message));
    }

    private static InvalidOperationException GetInvalidOperationException(string message)
    {
        return new InvalidOperationException(message) { Source = ExceptionSourceValueToRethrowAsJsonException };
    }
}
