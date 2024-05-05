using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace System.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
#pragma warning disable IDE0079
[SuppressMessage("ReSharper", "UseRawString")]
#pragma warning restore IDE0079
public sealed class DateOnlyConverterTests
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerOptions.Default);

    static DateOnlyConverterTests() => JsonSerializerOptions.Converters.Add(new DateOnlyConverter());

    [Theory]
    [InlineData("1970-01-01")]
    [InlineData("2002-02-13")]
    [InlineData("2022-05-10")]
    [InlineData("\\u0032\\u0030\\u0032\\u0032\\u002D\\u0030\\u0035\\u002D\\u0031\\u0030", "2022-05-10")]
    [InlineData("0001-01-01")] // DateOnly.MinValue
    [InlineData("9999-12-31")] // DateOnly.MaxValue
    public static void DateOnly_Read_Success(string json, string? actual = null)
    {
        DateOnly value = JsonSerializer.Deserialize<DateOnly>($"\"{json}\"", JsonSerializerOptions);
        Assert.Equal(DateOnly.Parse(actual ?? json), value);
    }

    [Theory]
    [InlineData("1970-01-01")]
    [InlineData("2002-02-13")]
    [InlineData("2022-05-10")]
    [InlineData("\\u0032\\u0030\\u0032\\u0032\\u002D\\u0030\\u0035\\u002D\\u0031\\u0030", "2022-05-10")]
    [InlineData("0001-01-01")] // DateOnly.MinValue
    [InlineData("9999-12-31")] // DateOnly.MaxValue
    public static void DateOnly_ReadDictionaryKey_Success(string json, string? actual = null)
    {
        Dictionary<DateOnly, int> expectedDict = new() { [DateOnly.Parse(actual ?? json)] = 0 };
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Dictionary<DateOnly, int> actualDict = JsonSerializer.Deserialize<Dictionary<DateOnly, int>>(
            $@"{{""{json}"":0}}", JsonSerializerOptions);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.Equal(expectedDict, actualDict);
    }

    [Fact]
    public static void DateOnly_Read_Nullable_Tests()
    {
        DateOnly? value = JsonSerializer.Deserialize<DateOnly?>("null");
        Assert.False(value.HasValue);

        value = JsonSerializer.Deserialize<DateOnly?>("\"2022-05-10\"", JsonSerializerOptions);
        Assert.True(value.HasValue);
        Assert.Equal(DateOnly.Parse("2022-05-10"), value);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DateOnly>("null"));
    }

    [Theory]
    [InlineData("05/10/2022")] // 'd' Format
    [InlineData("Tue, 10 May 2022")] // 'r' Format
    [InlineData("\t2022-05-10")] // Otherwise valid but has invalid json character
    [InlineData("\\t2022-05-10")] // Otherwise valid but has leading whitespace
    [InlineData("2022-05-10   ")] // Otherwise valid but has trailing whitespace
    // Fail on arbitrary ISO dates
    [InlineData("2022-05-10T20:53:01")]
    [InlineData("2022-05-10T20:53:01.3552286")]
    [InlineData("2022-05-10T20:53:01.3552286+01:00")]
    [InlineData("2022-05-10T20:53Z")]
    [InlineData("\\u0030\\u0035\\u002F\\u0031\\u0030\\u002F\\u0032\\u0030\\u0032\\u0032")]
    [InlineData("00:00:01")]
    [InlineData("23:59:59")]
    [InlineData("23:59:59.00000009")]
    [InlineData("1.00:00:00")]
    [InlineData("1:2:00:00")]
    [InlineData("+00:00:00")]
    [InlineData("1$")]
    [InlineData("-2020-05-10")]
    [InlineData("0000-12-31")] // DateOnly.MinValue - 1
    [InlineData("10000-01-01")] // DateOnly.MaxValue + 1
    [InlineData("1234", false)]
    [InlineData("{}", false)]
    [InlineData("[]", false)]
    [InlineData("true", false)]
    [InlineData("null", false)]
    public static void DateOnly_Read_Failure(string json, bool addQuotes = true)
    {
        if (addQuotes)
        {
            json = $"\"{json}\"";
        }

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DateOnly>(json, JsonSerializerOptions));
    }

    [Theory]
    [InlineData("1970-01-01")]
    [InlineData("2002-02-13")]
    [InlineData("2022-05-10")]
    [InlineData("0001-01-01")] // DateOnly.MinValue
    [InlineData("9999-12-31")] // DateOnly.MaxValue
    public static void DateOnly_Write_Success(string value)
    {
        DateOnly ts = DateOnly.Parse(value);
        string json = JsonSerializer.Serialize(ts, JsonSerializerOptions);
        Assert.Equal($"\"{value}\"", json);
    }

    [Theory]
    [InlineData("1970-01-01")]
    [InlineData("2002-02-13")]
    [InlineData("2022-05-10")]
    [InlineData("0001-01-01")] // DateOnly.MinValue
    [InlineData("9999-12-31")] // DateOnly.MaxValue
    public static void DateOnly_WriteDictionaryKey_Success(string value)
    {
        Dictionary<DateOnly, int> dict = new() { [DateOnly.Parse(value)] = 0 };
        string json = JsonSerializer.Serialize(dict, JsonSerializerOptions);
        Assert.Equal($@"{{""{value}"":0}}", json);
    }
}
