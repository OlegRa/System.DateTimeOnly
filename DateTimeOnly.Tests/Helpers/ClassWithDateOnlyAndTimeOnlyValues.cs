namespace System.Tests;

public class ClassWithDateOnlyAndTimeOnlyValues
{
    public Text.Json.DateOnly DateOnly { get; set; }

    public Text.Json.DateOnly? NullableDateOnly { get; set; }

    public Text.Json.TimeOnly TimeOnly { get; set; }

    public Text.Json.TimeOnly? NullableTimeOnly { get; set; }
}
