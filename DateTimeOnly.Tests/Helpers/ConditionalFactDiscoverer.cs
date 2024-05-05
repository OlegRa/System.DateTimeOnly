using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace System.Tests;

// ReSharper disable once UnusedType.Global
public sealed class ConditionalFactDiscoverer(
    IMessageSink diagnosticMessageSink) : FactDiscoverer(diagnosticMessageSink)
{
    public override IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        IEnumerable<IXunitTestCase> testCases = base.Discover(discoveryOptions, testMethod, factAttribute);

        if (factAttribute.GetConstructorArguments().SingleOrDefault() is not string conditionMemberName)
        {
            return testCases;
        }

        TypeInfo? typeInfo = testMethod.Method?.ToRuntimeMethod().DeclaringType?.GetTypeInfo();
        MethodInfo? methodInfo = typeInfo?.GetDeclaredProperty(conditionMemberName)?.GetMethod;

        return methodInfo?.Invoke(null, null) is false
            ? testCases.Select(testCase => new SkippedTestCase(testCase, conditionMemberName))
            : testCases;
    }
}
