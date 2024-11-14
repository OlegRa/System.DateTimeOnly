using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Resources;

// ReSharper disable once CheckNamespace
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltinTypes

namespace System;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class SR
{
    private static readonly bool s_usingResourceKeys =
        AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out bool usingResourceKeys) && usingResourceKeys;

    public static string UnsupportedFormat => Strings.UnsupportedFormat;

    public static string InvalidCast => Strings.InvalidCast;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string Format(string resourceFormat, object? p1) =>
        s_usingResourceKeys ? string.Join(", ", resourceFormat, p1) : string.Format(resourceFormat, p1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string Format(string resourceFormat, object? p1, object? p2) =>
        s_usingResourceKeys ? string.Join(", ", resourceFormat, p1, p2) : string.Format(resourceFormat, p1, p2);
}
