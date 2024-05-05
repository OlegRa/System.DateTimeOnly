using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// This helper wrapper structure can be used as an in-place replacement for the <see cref="DateOnly"/> class with the
/// <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library source code generators.
/// </summary>
[JsonConverter(typeof(JsonDateOnlyConverter))]
public readonly record struct DateOnly
{
    private readonly System.DateOnly _value;

    private DateOnly(System.DateOnly value) => _value = value;

    /// <inheritdoc />
    public override string ToString() => _value.ToString();

    /// <summary>
    /// Implicitly converts a <paramref name="value"/> into a <see cref="System.DateOnly"/> struct instance.
    /// </summary>
    /// <param name="value">A <see cref="DateOnly"/> struct instance.</param>
    /// <returns>A <see cref="System.DateOnly"/> struct instance.</returns>
    public static implicit operator System.DateOnly(DateOnly value) => value._value;

    /// <summary>
    /// Implicitly converts a <paramref name="value"/> into a <see cref="DateOnly"/> struct instance.
    /// </summary>
    /// <param name="value">A <see cref="System.DateOnly"/> struct instance.</param>
    /// <returns>A <see cref="DateOnly"/> struct instance.</returns>
    public static implicit operator DateOnly(System.DateOnly value) => new(value);
}
