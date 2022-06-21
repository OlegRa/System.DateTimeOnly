using System.Globalization;
using Xunit;

namespace System.Tests
{
    public sealed class ArgumentValidationTests
    {
        private const String InputArgumentName = "s";

        private const String StyleArgumentName = "style";

        private const String FormatArgumentName = "format";

        private static readonly DateOnly DateOnlyValue = new ();

        private static readonly String DateOnlyStringValue = DateOnlyValue.ToString();

        private static readonly TimeOnly TimeOnlyValue = new ();

        private static readonly String TimeOnlyStringValue = TimeOnlyValue.ToString();

#pragma warning disable CS8625
        private static readonly String NullString = null;

        private static readonly String[] NullStringArray = null;
#pragma warning restore CS8625

        [Fact]
        public void DateOnlyToStringWorkedTest()
        {
            Assert.Equal(DateOnlyStringValue, DateOnlyValue.ToShortDateString());
            Assert.NotEqual(DateOnlyStringValue, DateOnlyValue.ToLongDateString());
        }

        [Fact]
        public void DateOnlyToStringValidationWorkedTest()
        {
            Assert.Equal(DateOnlyStringValue, DateOnlyValue.ToString(string.Empty));
            Assert.Equal(DateOnlyStringValue, DateOnlyValue.ToString(NullString));

            Assert.Throws<FormatException>(() => DateOnlyValue.ToString("K"));

            Assert.Throws<FormatException>(() => DateOnlyValue.ToString(@"MM'-'\"));
            Assert.Throws<FormatException>(() => DateOnlyValue.ToString(@"\MM'"));
        }

        [Fact]
        public void DateOnlyTryFormatValidationWorkedTest()
        {
            Assert.Throws<FormatException>(() => DateOnlyValue.TryFormat(Span<char>.Empty, out _, "X"));
            Assert.Throws<FormatException>(() => DateOnlyValue.TryFormat(Span<char>.Empty, out _, "KK"));
        }

        [Fact]
        public void DateOnlyParseValidationWorkedTest()
        {
            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => DateOnly.Parse(NullString, CultureInfo.InvariantCulture)).ParamName);

            Assert.Throws<FormatException>(() => DateOnly.Parse("aa-bb-cc"));
        }

        [Fact]
        public void DateOnlyTryParseValidationWorkedTest()
        {
            Assert.False(DateOnly.TryParse(
                null, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParse("aa-bb-cc", out dateOnly));
            Assert.Equal(default, dateOnly);
        }

        [Fact]
        public void DateOnlyParseExactValidationWorkedTest()
        {
            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => DateOnly.ParseExact(NullString, string.Empty, CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => DateOnly.ParseExact(NullString, Array.Empty<string>(), CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
                () => DateOnly.ParseExact(ReadOnlySpan<char>.Empty, ReadOnlySpan<char>.Empty,
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

            Assert.Equal(FormatArgumentName, Assert.Throws<ArgumentNullException>(
                () => DateOnly.ParseExact(string.Empty, NullString, CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
                () => DateOnly.ParseExact(string.Empty, NullStringArray, CultureInfo.InvariantCulture)).ParamName);

            Assert.Throws<FormatException>(() => DateOnly.ParseExact(
                string.Empty, new []{ String.Empty }, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void DateOnlyTryParseExactValidationWorkedTest()
        {
            Assert.False(DateOnly.TryParseExact(
                null, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                null, Array.Empty<string>(), CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                string.Empty, NullString, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                string.Empty, NullStringArray, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                ReadOnlySpan<char>.Empty, new [] { string.Empty }, CultureInfo.InvariantCulture, 
                DateTimeStyles.AllowWhiteSpaces, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                ReadOnlySpan<char>.Empty, Array.Empty<string>(), CultureInfo.InvariantCulture, 
                DateTimeStyles.AllowWhiteSpaces, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                string.Empty, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact(
                new ReadOnlySpan<char>(), string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateOnly));
            Assert.Equal(default, dateOnly);

            Assert.False(DateOnly.TryParseExact("aa-bb-cc", "dd-MM-YY", out dateOnly));
            Assert.Equal(default, dateOnly);
        }

        [Fact]
        public void TimeOnlyToStringWorkedTest()
        {
            Assert.Equal(TimeOnlyStringValue, TimeOnlyValue.ToShortTimeString());
            Assert.NotEqual(TimeOnlyStringValue, TimeOnlyValue.ToLongTimeString());

#pragma warning disable CS8600
            Assert.Equal(TimeOnlyStringValue, TimeOnlyValue.ToString((IFormatProvider)null));
#pragma warning restore CS8600
        }

        [Fact]
        public void TimeOnlyCompareToValidationWorkedTest() => 
            Assert.Throws<ArgumentException>(() => TimeOnlyValue.CompareTo(DateOnlyValue));

        [Fact]
        public void TimeOnlyToStringValidationWorkedTest()
        {
            Assert.Equal(TimeOnlyStringValue, TimeOnlyValue.ToString(string.Empty));
            Assert.Equal(TimeOnlyStringValue, TimeOnlyValue.ToString(NullString));

            Assert.Throws<FormatException>(() => TimeOnlyValue.ToString("k"));

            Assert.Throws<FormatException>(() => TimeOnlyValue.ToString(@"mm'-'\"));
            Assert.Throws<FormatException>(() => TimeOnlyValue.ToString(@"\mm'"));
        }
        
        [Fact]
        public void TimeOnlyTryFormatValidationWorkedTest()
        {
            Assert.Throws<FormatException>(() => TimeOnlyValue.TryFormat(Span<char>.Empty, out _, "X"));
            Assert.Throws<FormatException>(() => TimeOnlyValue.TryFormat(Span<char>.Empty, out _, "kk"));
        }

        [Fact]
        public void TimeOnlyParseValidationWorkedTest()
        {
            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => TimeOnly.Parse(NullString, CultureInfo.InvariantCulture)).ParamName);

            Assert.Throws<FormatException>(() => TimeOnly.Parse("aa:bb"));
        }

        [Fact]
        public void TimeOnlyTryParseValidationWorkedTest()
        {
            Assert.False(TimeOnly.TryParse(
                null, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParse("aa:bb", out timeOnly));
            Assert.Equal(default, timeOnly);
        }

        [Fact]
        public void TimeOnlyParseExactValidationWorkedTest()
        {
            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => TimeOnly.ParseExact(NullString, string.Empty, CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
                () => TimeOnly.ParseExact(NullString, Array.Empty<string>(), CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
                () => TimeOnly.ParseExact(new ReadOnlySpan<char>(), new ReadOnlySpan<char>(),
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

            Assert.Equal(FormatArgumentName, Assert.Throws<ArgumentNullException>(
                () => TimeOnly.ParseExact(string.Empty, NullString, CultureInfo.InvariantCulture)).ParamName);

            Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
                () => TimeOnly.ParseExact(string.Empty, NullStringArray, CultureInfo.InvariantCulture)).ParamName);

            Assert.Throws<FormatException>(() => TimeOnly.ParseExact(
                string.Empty, new []{ String.Empty }, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void TimeOnlyTryParseExactValidationWorkedTest()
        {
            Assert.False(TimeOnly.TryParseExact(
                null, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                null, Array.Empty<string>(), CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                string.Empty, NullString, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                string.Empty, NullStringArray, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                ReadOnlySpan<char>.Empty, new []{ string.Empty }, CultureInfo.InvariantCulture, 
                DateTimeStyles.AllowWhiteSpaces, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                ReadOnlySpan<char>.Empty, Array.Empty<string>(), CultureInfo.InvariantCulture, 
                DateTimeStyles.AllowWhiteSpaces, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                string.Empty, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact(
                new ReadOnlySpan<char>(), string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out timeOnly));
            Assert.Equal(default, timeOnly);

            Assert.False(TimeOnly.TryParseExact("aa-bb-cc", "dd-MM-YY", out timeOnly));
            Assert.Equal(default, timeOnly);
        }
    }
}
