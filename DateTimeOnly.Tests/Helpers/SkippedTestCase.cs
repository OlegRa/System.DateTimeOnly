using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace System.Tests;

internal sealed class SkippedTestCase : LongLivedMarshalByRefObject, IXunitTestCase
{
    private readonly IXunitTestCase _testCase;

    // ReSharper disable once UnusedMember.Global
    public SkippedTestCase()
    {
        SkipReason = string.Empty;
        _testCase = this;
    }

    internal SkippedTestCase(
        IXunitTestCase testCase, string skippedReason)
    {
        SkipReason = skippedReason;
        _testCase = testCase;
    }

    public string SkipReason { get; }

    public int Timeout => _testCase.Timeout;

    public string UniqueID => _testCase.UniqueID;

    public IMethodInfo Method => _testCase.Method;

    public string DisplayName => _testCase.DisplayName;

    public Exception InitializationException => _testCase.InitializationException;

    public ISourceInformation SourceInformation
    {
        get => _testCase.SourceInformation;
        set => _testCase.SourceInformation = value;
    }

    public ITestMethod TestMethod => _testCase.TestMethod;

    public Dictionary<string, List<string>> Traits => _testCase.Traits;

    public object[] TestMethodArguments => _testCase.TestMethodArguments;

    public void Deserialize(IXunitSerializationInfo info) => _testCase.Deserialize(info);

    public Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource) =>
        new XunitTestCaseRunner(
            this, DisplayName, SkipReason, constructorArguments,
            TestMethodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();

    public void Serialize(IXunitSerializationInfo info) => _testCase.Serialize(info);
}
