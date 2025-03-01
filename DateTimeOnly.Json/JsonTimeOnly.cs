using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// This helper wrapper structure can be used as an in-place replacement for the <see cref="System.TimeOnly"/> class with the
/// <see href="https://docs.microsoft.com/dotnet/api/system.text.json">System.Text.Json</see> library source code generators.
/// </summary>
[JsonConverter(typeof(JsonTimeOnlyConverter))]
public readonly record struct JsonTimeOnly
{
    private readonly TimeOnly _value;

    private JsonTimeOnly(TimeOnly value) => _value = value;

    /// <inheritdoc />
    public override string ToString() => _value.ToString();

    /// <summary>
    /// Implicitly converts a <paramref name="value"/> into a <see cref="System.TimeOnly"/> struct instance.
    /// </summary>
    /// <param name="value">A <see cref="TimeOnly"/> struct instance.</param>
    /// <returns>A <see cref="System.TimeOnly"/> struct instance.</returns>
    public static implicit operator TimeOnly(JsonTimeOnly value) => value._value;

    /// <summary>
    /// Implicitly converts a <paramref name="value"/> into a <see cref="TimeOnly"/> struct instance.
    /// </summary>
    /// <param name="value">A <see cref="System.TimeOnly"/> struct instance.</param>
    /// <returns>A <see cref="TimeOnly"/> struct instance.</returns>
    public static implicit operator JsonTimeOnly(TimeOnly value) => new(value);
}
