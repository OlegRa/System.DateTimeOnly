using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace System.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("ReSharper", "UseRawString")]
public sealed class TimeOnlyConverterTests
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerOptions.Default);

    static TimeOnlyConverterTests() => JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());


    [Theory]
    [InlineData("23:59:59")]
    [InlineData("23:59:59.9", "23:59:59.9000000")]
    [InlineData("02:48:05.4775807")]
    [InlineData("02:48:05.4775808")]
    [InlineData("\\u0032\\u0033\\u003A\\u0035\\u0039\\u003A\\u0035\\u0039", "23:59:59")]
    [InlineData("00:00:00.0000000", "00:00:00")] // TimeOnly.MinValue
    [InlineData("23:59:59.9999999")] // TimeOnly.MaxValue
    public static void TimeOnly_Read_Success(string json, string? actual = null)
    {
        TimeOnly value = JsonSerializer.Deserialize<TimeOnly>($"\"{json}\"", JsonSerializerOptions);
        Assert.Equal(TimeOnly.Parse(actual ?? json), value);
    }

    [Fact]
    public static void TimeOnly_Read_Nullable_Tests()
    {
        TimeOnly? value = JsonSerializer.Deserialize<TimeOnly?>("null", JsonSerializerOptions);
        Assert.False(value.HasValue);

        value = JsonSerializer.Deserialize<TimeOnly?>("\"23:59:59\"", JsonSerializerOptions);
        Assert.True(value.HasValue);
        Assert.Equal(TimeOnly.Parse("23:59:59"), value);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TimeOnly>("null", JsonSerializerOptions));
    }

    [Theory]
    [InlineData("00:00")]
    [InlineData("23:59")]
    [InlineData("\t23:59:59")] // Otherwise valid but has invalid json character
    [InlineData("\\t23:59:59")] // Otherwise valid but has leading whitespace
    [InlineData("23:59:59   ")] // Otherwise valid but has trailing whitespace
    [InlineData("\\u0032\\u0034\\u003A\\u0030\\u0030\\u003A\\u0030\\u0030")]
    [InlineData("00:60:00")]
    [InlineData("00:00:60")]
    [InlineData("-00:00:00")]
    [InlineData("00:00:00.00000009")]
    [InlineData("900000000.00:00:00")]
    [InlineData("1.00:00:00")]
    [InlineData("0.00:00:00")]
    [InlineData("1:00:00")] // 'g' Format
    [InlineData("1:2:00:00")] // 'g' Format
    [InlineData("+00:00:00")]
    [InlineData("2021-06-18")]
    [InlineData("1$")]
    [InlineData("-00:00:00.0000001")] // TimeOnly.MinValue - 1
    [InlineData("24:00:00.0000000")] // TimeOnly.MaxValue + 1
    [InlineData("10675199.02:48:05.4775807")] // TimeSpan.MaxValue
    [InlineData("-10675199.02:48:05.4775808")] // TimeSpan.MinValue
    [InlineData("1234", false)]
    [InlineData("{}", false)]
    [InlineData("[]", false)]
    [InlineData("true", false)]
    [InlineData("null", false)]
    public static void TimeOnly_Read_Failure(string json, bool addQuotes = true)
    {
        if (addQuotes)
            json = $"\"{json}\"";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TimeOnly>(json, JsonSerializerOptions));
    }
    
    [Theory]
    [InlineData("1:59:59", "01:59:59")]
    [InlineData("23:59:59")]
    [InlineData("23:59:59.9", "23:59:59.9000000")]
    [InlineData("00:00:00")] // TimeOnly.MinValue
    [InlineData("23:59:59.9999999")] // TimeOnly.MaxValue
    public static void TimeOnly_Write_Success(string value, string? expectedValue = null)
    {
        TimeOnly ts = TimeOnly.Parse(value);
        string json = JsonSerializer.Serialize(ts, JsonSerializerOptions);
        Assert.Equal($"\"{expectedValue ?? value}\"", json);
    }
}
