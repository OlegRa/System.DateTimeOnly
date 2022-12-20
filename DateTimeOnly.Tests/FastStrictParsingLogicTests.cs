using Xunit;

namespace System.Tests
{
    public sealed class FastStrictParsingLogicTests
    {
        private const string DateOnlyFormat = "d";

        private const string TimeOnlyFormat = "t";

        [Fact]
        public static void DateOnlyParseWorkedTest() =>
            DateOnlyParseWorked(DateTime.Today, DateOnlyFormat);

        [Fact]
        public static void MidnightDateOnlyParseFailedTest() =>
            DateOnlyParseFailed(DateTime.Today, TimeOnlyFormat);

        [Fact]
        public static void MidnightTimeOnlyParseWorkedTest() =>
            TimeOnlyParseWorked(DateTime.MinValue, TimeOnlyFormat);

        [Fact]
        public static void DateTimeMinValueTimeOnlyParseFailedTest() =>
            TimeOnlyParseFailed(DateTime.MinValue, DateOnlyFormat);

        [Fact]
        public static void MidnightDateOnlyParseWorkedTest() => 
            RunInFastParsingLogicContext(
                (dt, format) => DateOnlyParseWorked(dt, format, DateTime.MinValue),
                DateTime.Today, TimeOnlyFormat);

        [Fact]
        public static void DateTimeMinValueDateOnlyParseWorkedTest() => 
            RunInFastParsingLogicContext(DateOnlyParseWorked, DateTime.MinValue, TimeOnlyFormat);

        [Fact]
        public static void DateTimeMinValueTimeOnlyParseWorkedTest() => 
            RunInFastParsingLogicContext(TimeOnlyParseWorked,DateTime.MinValue, DateOnlyFormat);

        private static void RunInFastParsingLogicContext(Action<DateTime, string> testMethod, DateTime dt, string format)
        {
            const string appContextSwitchName = "Portable.System.DateTimeOnly.UseFastParsingLogic";

            AppContext.SetSwitch(appContextSwitchName, true);
            try
            {
                testMethod(dt, format);
            }
            finally
            {
                AppContext.SetSwitch(appContextSwitchName, false);
            }
        }

        private static void TimeOnlyParseWorked(DateTime dt, string format)
        {
            var formatted = dt.ToString(format);
            Assert.Equal(TimeOnly.FromDateTime(dt), TimeOnly.Parse(formatted));
            Assert.Equal(TimeOnly.FromDateTime(dt), TimeOnly.Parse(formatted.AsSpan()));
            Assert.True(TimeOnly.TryParse(formatted, out _));
            Assert.True(TimeOnly.TryParse(formatted.AsSpan(), out _));
        }

        private static void DateOnlyParseWorked(DateTime dt, string format) => 
            DateOnlyParseWorked(dt, format, dt);

        private static void DateOnlyParseWorked(DateTime dt, string format, DateTime @default)
        {
            var formatted = dt.ToString(format);
            Assert.Equal(DateOnly.FromDateTime(@default), DateOnly.Parse(formatted));
            Assert.Equal(DateOnly.FromDateTime(@default), DateOnly.Parse(formatted.AsSpan()));
            Assert.True(DateOnly.TryParse(formatted, out _));
            Assert.True(DateOnly.TryParse(formatted.AsSpan(), out _));
        }

        private static void TimeOnlyParseFailed(DateTime dt, string format)
        {
            var formatted = dt.ToString(format);
            Assert.Throws<FormatException>(() => TimeOnly.Parse(formatted));
            Assert.Throws<FormatException>(() => TimeOnly.Parse(formatted.AsSpan()));
            Assert.False(TimeOnly.TryParse(formatted, out _));
            Assert.False(TimeOnly.TryParse(formatted.AsSpan(), out _));
        }

        private static void DateOnlyParseFailed(DateTime dt, string format)
        {
            var formatted = dt.ToString(format);
            Assert.Throws<FormatException>(() => DateOnly.Parse(formatted));
            Assert.Throws<FormatException>(() => DateOnly.Parse(formatted.AsSpan()));
            Assert.False(DateOnly.TryParse(formatted, out _));
            Assert.False(DateOnly.TryParse(formatted.AsSpan(), out _));
        }
    }
}
