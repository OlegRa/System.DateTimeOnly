// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Text.Json;

internal static class JsonConstants
{
    // The maximum number of fraction digits the Json DateTime parser allows
    public const int DateTimeParseNumFractionDigits = 16;

    // In the worst case, an ASCII character represented as a single utf-8 byte could expand 6x when escaped.
    public const int MaxExpansionFactorWhileEscaping = 6;

    // The largest fraction expressible by TimeSpan and DateTime formats
    public const int MaxDateTimeFraction = 9_999_999;

    // TimeSpan and DateTime formats allow exactly up to many digits for specifying the fraction after the seconds.
    public const int DateTimeNumFractionDigits = 7;

    public const byte UtcOffsetToken = (byte)'Z';

    public const byte TimePrefix = (byte)'T';

    public const byte Period = (byte)'.';

    public const byte Hyphen = (byte)'-';

    public const byte Colon = (byte)':';

    public const byte Plus = (byte)'+';
}
