using Xunit;
using Xunit.Sdk;

namespace System.Tests;

[AttributeUsage(AttributeTargets.Method)]
[XunitTestCaseDiscoverer("System.Tests.ConditionalFactDiscoverer", "DateTimeOnly.Tests")]
public sealed class ConditionalFactAttribute(
    string conditionalMemberName) : FactAttribute
{
    // ReSharper disable once UnusedMember.Global
    public string ConditionalMemberName { get; } = conditionalMemberName;
}
