// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace System;

internal static class DateTimeParse
{
    private const string AppContextSwitchName = "Portable.System.DateTimeOnly.UseFastParsingLogic";

    private static readonly char[] ValidDateOnlyFormatSpecifiers = ['d', 'D', 'm', 'M', 'y', 'Y'];

    private static readonly char[] ValidTimeOnlyFormatSpecifiers = ['t', 'T'];

    internal static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
    {
        bool success = DateTime.TryParseExact(s.ToString(), format.ToString(), dtfi, style, out DateTime dt);
        result.parsedDate = dt;
        return success;
    }

    internal static bool TryParse(ReadOnlySpan<char> s, DateTimeFormatInfo dtfi, DateTimeStyles styles, ref DateTimeResult result)
    {
        bool success = DateTime.TryParse(
            s.ToString(), dtfi, styles | DateTimeStyles.NoCurrentDateDefault,
            out DateTime dt);
        result.parsedDate = dt;

        if (!success || IsFastParsingLogicEnabled())
        {
            return success;
        }

        if (dt.Date != DateTime.MinValue ||
            DateTime.TryParseExact(s.ToString(), GetValidDateOnlyFormatStrings(dtfi), dtfi, styles, out _))
        {
            result.flags |= ParseFlags.HaveDate;
        }

        if (dt.TimeOfDay != TimeSpan.Zero ||
            DateTime.TryParseExact(s.ToString(), GetValidTimeOnlyFormatStrings(dtfi), dtfi, styles, out _))
        {
            result.flags |= ParseFlags.HaveTime;
        }

        return success;
    }

    private static bool IsFastParsingLogicEnabled() =>
        AppContext.TryGetSwitch(AppContextSwitchName, out bool isEnabled) && isEnabled;

    private static string[] GetValidDateOnlyFormatStrings(DateTimeFormatInfo dtfi) =>
        ValidDateOnlyFormatSpecifiers.SelectMany(dtfi.GetAllDateTimePatterns).ToArray();

    private static string[] GetValidTimeOnlyFormatStrings(DateTimeFormatInfo dtfi) =>
        ValidTimeOnlyFormatSpecifiers.SelectMany(dtfi.GetAllDateTimePatterns).ToArray();
}

#pragma warning disable IDE0079
[SuppressMessage("ReSharper", "InconsistentNaming")]
#pragma warning restore IDE0079
internal enum ParseFailureKind
{
    None = 0,
    Argument_InvalidDateStyles = 3,
    Format_BadDateOnly = 4,
    Argument_BadFormatSpecifier = 5,
    Format_DateTimeOnlyContainsNoneDateParts = 8  // DateOnly and TimeOnly specific value. Unrelated date parts when parsing DateOnly or Unrelated time parts when parsing TimeOnly
}

[Flags]
internal enum ParseFlags
{
    HaveYear = 0x00000001,
    HaveMonth = 0x00000002,
    HaveDay = 0x00000004,
    HaveHour = 0x00000008,
    HaveMinute = 0x00000010,
    HaveSecond = 0x00000020,
    HaveTime = 0x00000040,
    HaveDate = 0x00000080,
    TimeZoneUsed = 0x00000100,
    TimeZoneUtc = 0x00000200,
    ParsedMonthName = 0x00000400,
    CaptureOffset = 0x00000800,
    UtcSortPattern = 0x00004000
}

#pragma warning disable IDE0079
[SuppressMessage("ReSharper", "InconsistentNaming")]
#pragma warning restore IDE0079
internal ref struct DateTimeResult
{
#pragma warning disable 649
    internal ParseFlags flags;
#pragma warning restore 649

    internal DateTime parsedDate;

    // ReSharper disable once UnusedParameter.Global
    internal static void Init(ReadOnlySpan<char> _) { }
}
