using Xunit;
using Xunit.Sdk;

namespace System.Tests;

[AttributeUsage(AttributeTargets.Method)]
[XunitTestCaseDiscoverer("System.Tests.ConditionalFactDiscoverer", "DateTimeOnly.Tests")]
public sealed class ConditionalFactAttribute : FactAttribute
{
    public ConditionalFactAttribute(
        string conditionalMemberName) =>
        ConditionalMemberName = conditionalMemberName;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string ConditionalMemberName { get; }
}
