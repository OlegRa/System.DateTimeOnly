using Xunit;
using Xunit.Sdk;

namespace System.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    [XunitTestCaseDiscoverer("System.Tests.ConditionalFactDiscoverer", "DateTimeOnly.Tests")]
    public sealed class ConditionalFactAttribute : FactAttribute
    {
        public ConditionalFactAttribute(
            String conditionalMemberName) =>
            ConditionalMemberName = conditionalMemberName;

        // ReSharper disable once MemberCanBePrivate.Global
        public String ConditionalMemberName { get; }
    }
}
