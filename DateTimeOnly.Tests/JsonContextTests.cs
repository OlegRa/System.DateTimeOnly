using System.Text.Json;
using Xunit;

namespace System.Tests;

public sealed class JsonContextTests
{
    [Fact]
    public void ClassWithDateOnlyAndTimeOnlyValues_Roundtrip()
    {
        RunTest(new ClassWithDateOnlyAndTimeOnlyValues
        {
            DateOnly = DateOnly.Parse("2022-05-10"),
            NullableDateOnly = DateOnly.Parse("2022-05-10"),

            TimeOnly = TimeOnly.Parse("21:51:51"),
            NullableTimeOnly = TimeOnly.Parse("21:51:51")
        });

        RunTest(new ClassWithDateOnlyAndTimeOnlyValues());
    }

    private static void RunTest(ClassWithDateOnlyAndTimeOnlyValues expected)
    {
        string json = JsonSerializer.Serialize(
            expected, TestsContext.Default.ClassWithDateOnlyAndTimeOnlyValues);
        ClassWithDateOnlyAndTimeOnlyValues? actual = JsonSerializer.Deserialize(
            json, TestsContext.Default.ClassWithDateOnlyAndTimeOnlyValues);

        Assert.NotNull(actual);

        Assert.Equal((DateOnly)expected.DateOnly, (DateOnly)actual.DateOnly);
        Assert.Equal((DateOnly?)expected.NullableDateOnly, (DateOnly?)actual.NullableDateOnly);

        Assert.Equal((TimeOnly)expected.TimeOnly, (TimeOnly)actual.TimeOnly);
        Assert.Equal((TimeOnly?)expected.NullableTimeOnly, (TimeOnly?)actual.NullableTimeOnly);
    }
}
