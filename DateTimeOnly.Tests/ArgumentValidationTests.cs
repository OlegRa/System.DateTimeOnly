using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace System.Tests;

[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
public sealed class ArgumentValidationTests
{
    private const string InputArgumentName = "s";

    private const string StyleArgumentName = "style";

    private const string FormatArgumentName = "format";

    private static readonly DateOnly DateOnlyValue = new ();

    private static readonly string DateOnlyStringValue = DateOnlyValue.ToString();

    private static readonly TimeOnly TimeOnlyValue = new ();

    private static readonly string TimeOnlyStringValue = TimeOnlyValue.ToString();

#pragma warning disable CS8625
    private static readonly string NullString = null;

    private static readonly string[] NullStringArray = null;
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
        Assert.Throws<FormatException>(() => DateOnlyValue.TryFormat(Span<char>.Empty, out _, "X".AsSpan()));
        Assert.Throws<FormatException>(() => DateOnlyValue.TryFormat(Span<char>.Empty, out _, "KK".AsSpan()));
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
            (string?)null, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateOnly dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParse("aa-bb-cc".AsSpan(), CultureInfo.InvariantCulture, out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParse("aa-bb-cc", CultureInfo.InvariantCulture, out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParse("aa-bb-cc", out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParse("aa-bb-cc".AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParse("aa-bb-cc", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateOnly)).ParamName);
    }

    [Fact]
    public void DateOnlyParseExactValidationWorkedTest()
    {
        Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
            () => DateOnly.ParseExact(NullString, string.Empty, CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
            () => DateOnly.ParseExact(NullString, [], CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.ParseExact([], ReadOnlySpan<char>.Empty,
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

        Assert.Equal(FormatArgumentName, Assert.Throws<ArgumentNullException>(
            () => DateOnly.ParseExact(string.Empty, NullString, CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.ParseExact(string.Empty, NullStringArray, CultureInfo.InvariantCulture)).ParamName);

        Assert.Throws<FormatException>(() => DateOnly.ParseExact(
            string.Empty, [string.Empty], CultureInfo.InvariantCulture));

        // invalid style must take priority over a bad formats entry, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.ParseExact(
                string.Empty, [string.Empty], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

        // malformed format string (unterminated literal / dangling escape) - approximates upstream's
        // Argument_BadFormatSpecifier failure, since this backport can't detect it the same way upstream does
        Assert.Throws<FormatException>(
            () => DateOnly.ParseExact(string.Empty, @"MM'-'\", CultureInfo.InvariantCulture));

        Assert.Throws<FormatException>(
            () => DateOnly.ParseExact(string.Empty, @"\MM'", CultureInfo.InvariantCulture));

        // a malformed trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(
            () => DateOnly.ParseExact(
                DateOnlyValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), ["yyyy-MM-dd", @"MM'-'\"], CultureInfo.InvariantCulture));

        // invalid style must take priority over a malformed format, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.ParseExact(string.Empty, @"MM'-'\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);
    }

    [Fact]
    public void DateOnlyTryParseExactValidationWorkedTest()
    {
        Assert.False(DateOnly.TryParseExact(
            null, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateOnly dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParseExact(
            (string?)null, [], CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParseExact(
            string.Empty, NullString, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.False(DateOnly.TryParseExact(
            string.Empty, NullStringArray, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateOnly));
        Assert.Equal(default, dateOnly);

        Assert.Throws<FormatException>(() => DateOnly.TryParseExact(
            [], [string.Empty], CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out dateOnly));

        Assert.False(DateOnly.TryParseExact(
            [], Array.Empty<string>(), CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out dateOnly));
        Assert.Equal(default, dateOnly);

        // a bad trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(() => DateOnly.ParseExact(
            DateOnlyValue.ToString("yyyy-MM-dd"), ["yyyy-MM-dd", string.Empty], CultureInfo.InvariantCulture));

        Assert.Throws<FormatException>(() => DateOnly.TryParseExact(
            DateOnlyValue.ToString("yyyy-MM-dd").AsSpan(), ["yyyy-MM-dd", string.Empty], CultureInfo.InvariantCulture,
            DateTimeStyles.None, out dateOnly));

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParseExact(
                string.Empty, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParseExact(
                new ReadOnlySpan<char>(), string.Empty.AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParseExact(
                string.Empty, [string.Empty], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateOnly)).ParamName);

        // malformed format string - TryParseExact must now throw instead of silently returning false
        Assert.Throws<FormatException>(
            () => DateOnly.TryParseExact(string.Empty, @"MM'-'\", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOnly));

        Assert.Throws<FormatException>(
            () => DateOnly.TryParseExact(string.Empty.AsSpan(), @"\MM'".AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOnly));

        // a malformed trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(
            () => DateOnly.TryParseExact(
                DateOnlyValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), ["yyyy-MM-dd", @"MM'-'\"], CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dateOnly));

        // invalid style must take priority over a malformed format, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => DateOnly.TryParseExact(string.Empty, @"MM'-'\", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateOnly)).ParamName);

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
        Assert.Throws<FormatException>(() => TimeOnlyValue.TryFormat(Span<char>.Empty, out _, "X".AsSpan()));
        Assert.Throws<FormatException>(() => TimeOnlyValue.TryFormat(Span<char>.Empty, out _, "kk".AsSpan()));
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
            (string?)null, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out TimeOnly timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParse("aa:bb".AsSpan(), CultureInfo.InvariantCulture, out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParse("aa:bb", CultureInfo.InvariantCulture, out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParse("aa:bb", out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParse("aa:bb".AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timeOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParse("aa:bb", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timeOnly)).ParamName);
    }

    [Fact]
    public void TimeOnlyParseExactValidationWorkedTest()
    {
        Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
            () => TimeOnly.ParseExact(NullString, string.Empty, CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(InputArgumentName, Assert.Throws<ArgumentNullException>(
            () => TimeOnly.ParseExact(NullString, [], CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.ParseExact(new ReadOnlySpan<char>(), new ReadOnlySpan<char>(),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

        Assert.Equal(FormatArgumentName, Assert.Throws<ArgumentNullException>(
            () => TimeOnly.ParseExact(string.Empty, NullString, CultureInfo.InvariantCulture)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.ParseExact(string.Empty, NullStringArray, CultureInfo.InvariantCulture)).ParamName);

        Assert.Throws<FormatException>(() => TimeOnly.ParseExact(
            string.Empty, [string.Empty], CultureInfo.InvariantCulture));

        // invalid style must take priority over a bad formats entry, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.ParseExact(
                string.Empty, [string.Empty], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);

        // malformed format string (unterminated literal / dangling escape) - approximates upstream's
        // Argument_BadFormatSpecifier failure, since this backport can't detect it the same way upstream does
        Assert.Throws<FormatException>(
            () => TimeOnly.ParseExact(string.Empty, @"mm'-'\", CultureInfo.InvariantCulture));

        Assert.Throws<FormatException>(
            () => TimeOnly.ParseExact(string.Empty, @"\mm'", CultureInfo.InvariantCulture));

        // a malformed trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(
            () => TimeOnly.ParseExact(
                TimeOnlyValue.ToString("HH:mm:ss", CultureInfo.InvariantCulture), ["HH:mm:ss", @"mm'-'\"], CultureInfo.InvariantCulture));

        // invalid style must take priority over a malformed format, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.ParseExact(string.Empty, @"mm'-'\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)).ParamName);
    }

    [Fact]
    public void TimeOnlyTryParseExactValidationWorkedTest()
    {
        Assert.False(TimeOnly.TryParseExact(
            null, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out TimeOnly timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParseExact(
            (string?)null, [], CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParseExact(
            string.Empty, NullString, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.False(TimeOnly.TryParseExact(
            string.Empty, NullStringArray, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out timeOnly));
        Assert.Equal(default, timeOnly);

        Assert.Throws<FormatException>(() => TimeOnly.TryParseExact(
            [], [string.Empty], CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out timeOnly));

        Assert.False(TimeOnly.TryParseExact(
            [], Array.Empty<string>(), CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces, out timeOnly));
        Assert.Equal(default, timeOnly);

        // a bad trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(() => TimeOnly.ParseExact(
            TimeOnlyValue.ToString("HH:mm:ss", CultureInfo.InvariantCulture), ["HH:mm:ss", string.Empty], CultureInfo.InvariantCulture));

        Assert.Throws<FormatException>(() => TimeOnly.TryParseExact(
            TimeOnlyValue.ToString("HH:mm:ss", CultureInfo.InvariantCulture).AsSpan(), ["HH:mm:ss", string.Empty], CultureInfo.InvariantCulture,
            DateTimeStyles.None, out timeOnly));

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParseExact(
                string.Empty, string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out timeOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParseExact(
                new ReadOnlySpan<char>(), string.Empty.AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out timeOnly)).ParamName);

        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParseExact(
                string.Empty, [string.Empty], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out timeOnly)).ParamName);

        // malformed format string - TryParseExact must now throw instead of silently returning false
        Assert.Throws<FormatException>(
            () => TimeOnly.TryParseExact(string.Empty, @"mm'-'\", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeOnly));

        Assert.Throws<FormatException>(
            () => TimeOnly.TryParseExact(string.Empty.AsSpan(), @"\mm'".AsSpan(), CultureInfo.InvariantCulture, DateTimeStyles.None, out timeOnly));

        // a malformed trailing entry must be caught even if an earlier format would have matched
        Assert.Throws<FormatException>(
            () => TimeOnly.TryParseExact(
                TimeOnlyValue.ToString("HH:mm:ss", CultureInfo.InvariantCulture), ["HH:mm:ss", @"mm'-'\"], CultureInfo.InvariantCulture,
                DateTimeStyles.None, out timeOnly));

        // invalid style must take priority over a malformed format, not the other way around
        Assert.Equal(StyleArgumentName, Assert.Throws<ArgumentException>(
            () => TimeOnly.TryParseExact(string.Empty, @"mm'-'\", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out timeOnly)).ParamName);

        Assert.False(TimeOnly.TryParseExact("aa-bb-cc", "dd-MM-YY", out timeOnly));
        Assert.Equal(default, timeOnly);
    }
}
