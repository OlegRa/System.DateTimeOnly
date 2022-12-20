// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System;

internal static class ThrowHelper
{
    [DoesNotReturn]
    internal static void ThrowArgumentOutOfRange_DayNumber(int dayNumber) =>
        throw new ArgumentOutOfRangeException(nameof(dayNumber), dayNumber, SR.ArgumentOutOfRange_DayNumber);

    [DoesNotReturn]
    internal static void ThrowArgumentOutOfRange_BadHourMinuteSecond() =>
        throw new ArgumentOutOfRangeException(null, SR.ArgumentOutOfRange_BadHourMinuteSecond);

    [DoesNotReturn]
    internal static void ThrowArgumentNullException(ExceptionArgument argument) =>
        throw new ArgumentNullException(GetArgumentName(argument));

    private static string GetArgumentName(ExceptionArgument argument)
    {
        switch (argument)
        {
            case ExceptionArgument.s:
                return "s";
            case ExceptionArgument.format:
                return "format";
            default:
                Debug.Fail("The enum value is not defined, please check the ExceptionArgument Enum.");
                return "";
        }
    }
}
