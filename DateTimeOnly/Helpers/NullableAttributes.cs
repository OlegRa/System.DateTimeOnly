// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>Specifies that when a method returns <see cref="ReturnValue"/>, the parameter may be null even if the corresponding type disallows it.</summary>
/// <remarks>Initializes the attribute with the specified return value condition.</remarks>
/// <param name="returnValue">
/// The return value condition. If the method returns this value, the associated parameter may be null.
/// </param>
[ExcludeFromCodeCoverage]
[AttributeUsage (AttributeTargets.Parameter)]
internal sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute
{

    /// <summary>Gets the return value condition.</summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool ReturnValue { get; } = returnValue;
}

/// <summary>Specifies that when a method returns <see cref="ReturnValue"/>, the parameter will not be null even if the corresponding type allows it.</summary>
/// <remarks>Initializes the attribute with the specified return value condition.</remarks>
/// <param name="returnValue">
/// The return value condition. If the method returns this value, the associated parameter will not be null.
/// </param>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{

    /// <summary>Gets the return value condition.</summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool ReturnValue { get; } = returnValue;
}

/// <summary>Applied to a method that will never return under any circumstance.</summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal sealed class DoesNotReturnAttribute : Attribute;
