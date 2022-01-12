using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace System.Tests
{
    internal sealed class SkippedTestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        private readonly IXunitTestCase _testCase;

        private readonly String _skippedReason;

        public SkippedTestCase()
        {
            _skippedReason = String.Empty;
            _testCase = this;
        }

        internal SkippedTestCase(
            IXunitTestCase testCase, String skippedReason)
        {
            _skippedReason = skippedReason;
            _testCase = testCase;
        }

        public Int32 Timeout => _testCase.Timeout;

        public String SkipReason => _skippedReason;

        public String UniqueID => _testCase.UniqueID;

        public IMethodInfo Method => _testCase.Method;

        public String DisplayName => _testCase.DisplayName;

        public Exception InitializationException => _testCase.InitializationException;

        public ISourceInformation SourceInformation
        {
            get => _testCase.SourceInformation;
            set => _testCase.SourceInformation = value;
        }

        public ITestMethod TestMethod => _testCase.TestMethod;

        public Dictionary<String, List<String>> Traits => _testCase.Traits;

        public Object[] TestMethodArguments => _testCase.TestMethodArguments;

        public void Deserialize(IXunitSerializationInfo info) => _testCase.Deserialize(info);

        public Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            Object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource) =>
            new XunitTestCaseRunner(
                this, DisplayName, _skippedReason, constructorArguments,
                TestMethodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();

        public void Serialize(IXunitSerializationInfo info) => _testCase.Serialize(info);
    }
}