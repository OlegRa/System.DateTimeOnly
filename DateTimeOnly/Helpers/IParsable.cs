// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System;

internal interface IParsable<TSelf>
    where TSelf : IParsable<TSelf>?
{
    // ReSharper disable once UnusedMember.Global
    TSelf Parse(string s, IFormatProvider? provider);

    // ReSharper disable once UnusedMember.Global
    bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(returnValue: false)] out TSelf result);
}
