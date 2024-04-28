// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>Specifies the syntax used in a string.</summary>
/// <remarks>Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.</remarks>
/// <param name="syntax">The syntax identifier.</param>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class StringSyntaxAttribute(string syntax) : Attribute
{

    /// <summary>Gets the identifier of the syntax used.</summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Syntax { get; } = syntax;

    /// <summary>Optional arguments associated with the specific syntax employed.</summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public object?[] Arguments { get; } = [];

    /// <summary>The syntax identifier for strings containing date format specifiers.</summary>
    public const string DateOnlyFormat = nameof(DateOnlyFormat);

    /// <summary>The syntax identifier for strings containing time format specifiers.</summary>
    public const string TimeOnlyFormat = nameof(TimeOnlyFormat);
}
