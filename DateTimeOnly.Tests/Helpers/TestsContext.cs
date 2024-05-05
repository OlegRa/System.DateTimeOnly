using System.Text.Json.Serialization;

namespace System.Tests;

[JsonSerializable(typeof(ClassWithDateOnlyAndTimeOnlyValues))]
internal partial class TestsContext : JsonSerializerContext;
