// ReSharper disable once CheckNamespace
namespace System.Tests;

internal static class DateTimeExtensions
{
    internal static DateTime Create(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond) =>
        new DateTime(year, month, day, hour, minute, second, millisecond)
            .Add(new TimeSpan(microsecond * Diagnostics.TimeSpan.TicksPerMicrosecond));

    /// <summary>
    /// The microseconds component, expressed as a value between 0 and 999.
    /// </summary>
    internal static int Microsecond(this DateTime dateTime) => (int)(dateTime.Ticks / Diagnostics.TimeSpan.TicksPerMicrosecond % 1000);

    /// <summary>
    /// The nanoseconds component, expressed as a value between 0 and 900 (in increments of 100 nanoseconds).
    /// </summary>
    internal static int Nanosecond(this DateTime dateTime) => (int)(dateTime.Ticks % Diagnostics.TimeSpan.TicksPerMicrosecond) * 100;
}
