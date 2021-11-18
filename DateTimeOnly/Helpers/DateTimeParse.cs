// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System
{
    internal static class DateTimeParse
    {
        internal static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
        {
            var success = DateTime.TryParseExact(s.ToString(), format.ToString(), dtfi, style, out var dt);
            result.parsedDate = dt;
            return success;
        }

        internal static bool TryParse(ReadOnlySpan<char> s, DateTimeFormatInfo dtfi, DateTimeStyles styles, ref DateTimeResult result)
        {
            var success = DateTime.TryParse(s.ToString(), dtfi, styles, out var dt);
            result.parsedDate = dt;
            return success;
        }
    }

    internal enum ParseFailureKind
    {
        None = 0,
        FormatWithParameter = 3,
        FormatWithOriginalDateTime = 4,
        FormatWithFormatSpecifier = 5,
        WrongParts = 8,  // DateOnly and TimeOnly specific value. Unrelated date parts when parsing DateOnly or Unrelated time parts when parsing TimeOnly
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
        UtcSortPattern = 0x00004000,
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal ref struct DateTimeResult
    {
#pragma warning disable 649
        internal ParseFlags flags;
#pragma warning restore 649

        internal DateTime parsedDate;

        internal void Init(ReadOnlySpan<char> _) { }
    }
}