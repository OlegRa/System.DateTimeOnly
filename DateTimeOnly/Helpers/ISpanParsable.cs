// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System;

// ReSharper disable once UnusedType.Global
internal interface ISpanParsable<TSelf> : IParsable<TSelf>
    where TSelf : ISpanParsable<TSelf>?
{
    // ReSharper disable once UnusedMember.Global
    TSelf Parse(ReadOnlySpan<char> s, IFormatProvider? provider);

    // ReSharper disable once UnusedMember.Global
    bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(returnValue: false)] out TSelf result);
}
