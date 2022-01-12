using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace System.Tests
{
    // ReSharper disable once UnusedType.Global
    public sealed class ConditionalFactDiscoverer : FactDiscoverer
    {
        public ConditionalFactDiscoverer(
            IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
        }

        public override IEnumerable<IXunitTestCase> Discover(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            IEnumerable<IXunitTestCase> testCases = base.Discover(discoveryOptions, testMethod, factAttribute);

            if (factAttribute.GetConstructorArguments().SingleOrDefault() is String conditionMemberName)
            {
                var typeInfo = testMethod.Method?.ToRuntimeMethod().DeclaringType?.GetTypeInfo();
                var methodInfo = typeInfo?.GetDeclaredProperty(conditionMemberName)?.GetMethod;
                if (methodInfo?.Invoke(null, null) is false)
                {
                    return testCases.Select(_ => new SkippedTestCase(_, conditionMemberName));
                }
            }

            return testCases;
        }
    }
}