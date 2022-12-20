using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics;

internal static class DateTime
{
    // Number of 100ns ticks per time unit
    private const int MicrosecondsPerMillisecond = 1000;
    private const long TicksPerMicrosecond = 10;
    private const long TicksPerMillisecond = TicksPerMicrosecond * MicrosecondsPerMillisecond;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private const long TicksPerMinute = TicksPerSecond * 60;
    private const long TicksPerHour = TicksPerMinute * 60;
    private const long TicksPerDay = TicksPerHour * 24;

    // Number of milliseconds per time unit
    private const int MillisPerSecond = 1000;

    // Number of days in a non-leap year
    private const int DaysPerYear = 365;
    // Number of days in 4 years
    private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
    // Number of days in 100 years
    private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
    // Number of days in 400 years
    private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

    // Number of days from 1/1/0001 to 12/31/9999
    private const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059

    private const long MaxTicks = DaysTo10000 * TicksPerDay - 1;

    internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond)
    {
        var ticks = TimeToTicks(hour, minute, second);

        if ((uint)millisecond >= MillisPerSecond)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecond),
                SR.Format(SR.ArgumentOutOfRange_Range, 0, MillisPerSecond - 1));
        }

        ticks += (uint)millisecond * (uint)TicksPerMillisecond;

        Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");

        return ticks;
    }
        
    internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond, int microsecond)
    {
        var ticks = TimeToTicks(hour, minute, second, millisecond);

        if ((uint)microsecond >= MicrosecondsPerMillisecond)
        {
            throw new ArgumentOutOfRangeException(nameof(microsecond),
                SR.Format(SR.ArgumentOutOfRange_Range, 0, MicrosecondsPerMillisecond - 1));
        }

        ticks += (uint)microsecond * (uint)TicksPerMicrosecond;

        Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");

        return ticks;
    }

    // Return the tick count corresponding to the given hour, minute, second.
    // Will check the if the parameters are valid.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong TimeToTicks(int hour, int minute, int second)
    {
        if ((uint)hour >= 24 || (uint)minute >= 60 || (uint)second >= 60)
        {
            ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
        }

        var totalSeconds = hour * 3600 + minute * 60 + second;
        return (uint)totalSeconds * (ulong)TicksPerSecond;
    }

    internal static int Microseconds(this TimeSpan timeSpan) => (int)(timeSpan.Ticks / TicksPerMicrosecond % 1000);

    internal static int Nanoseconds(this TimeSpan timeSpan) => (int)(timeSpan.Ticks % TicksPerMicrosecond * 100);
}
