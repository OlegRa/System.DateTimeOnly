using System.Diagnostics.CodeAnalysis;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class SR
{
    private static readonly bool s_usingResourceKeys = 
        AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var usingResourceKeys) && usingResourceKeys;

    public static string Argument_BadFormatSpecifier => Strings.Argument_BadFormatSpecifier;

    public static string Argument_InvalidDateStyles => Strings.Argument_InvalidDateStyles;

    public static string ArgumentOutOfRange_Range => Strings.ArgumentOutOfRange_Range;

    public static string ArgumentOutOfRange_AddValue => Strings.ArgumentOutOfRange_AddValue;

    public static string ArgumentOutOfRange_BadHourMinuteSecond => Strings.ArgumentOutOfRange_BadHourMinuteSecond;

    public static string ArgumentOutOfRange_DayNumber => Strings.ArgumentOutOfRange_DayNumber;

    public static string ArgumentOutOfRange_TimeOnlyBadTicks => Strings.ArgumentOutOfRange_TimeOnlyBadTicks;

    public static string Arg_MustBeDateOnly => Strings.Arg_MustBeDateOnly;

    public static string Arg_MustBeTimeOnly => Strings.Arg_MustBeTimeOnly;

    public static string Format_BadDateOnly => Strings.Format_BadDateOnly;

    public static string Format_BadQuote => Strings.Format_BadQuote;

    public static string Format_BadTimeOnly => Strings.Format_BadTimeOnly;

    public static string Format_DateTimeOnlyContainsNoneDateParts => Strings.Format_DateTimeOnlyContainsNoneDateParts;

    public static string Format_InvalidString => Strings.Format_InvalidString;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string Format(string resourceFormat, object? p1) =>
        s_usingResourceKeys ? string.Join(", ", resourceFormat, p1) : string.Format(resourceFormat, p1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string Format(string resourceFormat, object? p1, object? p2) =>
        s_usingResourceKeys ? string.Join(", ", resourceFormat, p1, p2) : string.Format(resourceFormat, p1, p2);
}
