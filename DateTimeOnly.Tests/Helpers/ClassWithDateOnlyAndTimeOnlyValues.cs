using System.Text.Json;

namespace System.Tests;

public class ClassWithDateOnlyAndTimeOnlyValues
{
    public JsonDateOnly DateOnly { get; set; }

    public JsonDateOnly? NullableDateOnly { get; set; }

    public JsonTimeOnly TimeOnly { get; set; }

    public JsonTimeOnly? NullableTimeOnly { get; set; }
}
