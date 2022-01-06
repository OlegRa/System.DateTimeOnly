// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System
{
    internal static class DateTimeFormat
    {
        private static readonly DateTimeFormatInfo InvariantFormatInfo = CultureInfo.InvariantCulture.DateTimeFormat;

        private static readonly string[] InvariantAbbreviatedMonthNames = InvariantFormatInfo.AbbreviatedMonthNames;

        private static readonly string[] InvariantAbbreviatedDayNames = InvariantFormatInfo.AbbreviatedDayNames;

        internal static string Format(DateTime dateTime, string format, IFormatProvider? provider) =>
            dateTime.ToString(format, provider);

        internal static bool TryFormat(DateTime dateTime, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            var value = dateTime.ToString(format.ToString(), provider);
            charsWritten = value.Length;
#pragma warning disable PC001 // API not supported on all platforms
            return value.AsSpan().TryCopyTo(destination);
#pragma warning restore PC001 // API not supported on all platforms
#else
            return dateTime.TryFormat(destination, out charsWritten, format, provider);
#endif
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        internal static bool IsValidCustomDateFormat(ReadOnlySpan<char> format, bool throwOnError)
        {
            int i = 0;

            while (i < format.Length)
            {
                switch (format[i])
                {
                    case '\\':
                        if (i == format.Length - 1)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException(SR.Format_InvalidString);
                            }

                            return false;
                        }

                        i += 2;
                        break;

                    case '\'':
                    case '"':
                        char quoteChar = format[i++];
                        while (i < format.Length && format[i] != quoteChar)
                        {
                            i++;
                        }

                        if (i >= format.Length)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException(SR.Format(SR.Format_BadQuote, quoteChar));
                            }

                            return false;
                        }

                        i++;
                        break;

                    case ':':
                    case 't':
                    case 'f':
                    case 'F':
                    case 'h':
                    case 'H':
                    case 'm':
                    case 's':
                    case 'z':
                    case 'K':
                        // reject non-date formats
                        if (throwOnError)
                        {
                            throw new FormatException(SR.Format_InvalidString);
                        }

                        return false;

                    default:
                        i++;
                        break;
                }
            }

            return true;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        internal static bool IsValidCustomTimeFormat(ReadOnlySpan<char> format, bool throwOnError)
        {
            int length = format.Length;
            int i = 0;

            while (i < length)
            {
                switch (format[i])
                {
                    case '\\':
                        if (i == length - 1)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException(SR.Format_InvalidString);
                            }

                            return false;
                        }

                        i += 2;
                        break;

                    case '\'':
                    case '"':
                        char quoteChar = format[i++];
                        while (i < length && format[i] != quoteChar)
                        {
                            i++;
                        }

                        if (i >= length)
                        {
                            if (throwOnError)
                            {
                                throw new FormatException(SR.Format(SR.Format_BadQuote, quoteChar));
                            }

                            return false;
                        }

                        i++;
                        break;

                    case 'd':
                    case 'M':
                    case 'y':
                    case '/':
                    case 'z':
                    case 'k':
                        if (throwOnError)
                        {
                            throw new FormatException(SR.Format_InvalidString);
                        }

                        return false;

                    default:
                        i++;
                        break;
                }
            }

            return true;
        }

        //   012345678901234567890123456789012
        //   ---------------------------------
        //   05:30:45.7680000
        internal static bool TryFormatTimeOnlyO(int hour, int minute, int second, long fraction, Span<char> destination)
        {
            if (destination.Length < 16)
            {
                return false;
            }

            WriteTwoDecimalDigits((uint)hour, destination, 0);
            destination[2] = ':';
            WriteTwoDecimalDigits((uint)minute, destination, 3);
            destination[5] = ':';
            WriteTwoDecimalDigits((uint)second, destination, 6);
            destination[8] = '.';
#pragma warning disable PC001 // API not supported on all platforms
            WriteDigits((uint)fraction, destination.Slice(9, 7));
#pragma warning restore PC001 // API not supported on all platforms

            return true;
        }

        //   012345678901234567890123456789012
        //   ---------------------------------
        //   05:30:45
        internal static bool TryFormatTimeOnlyR(int hour, int minute, int second, Span<char> destination)
        {
            if (destination.Length < 8)
            {
                return false;
            }

            WriteTwoDecimalDigits((uint)hour, destination, 0);
            destination[2] = ':';
            WriteTwoDecimalDigits((uint)minute, destination, 3);
            destination[5] = ':';
            WriteTwoDecimalDigits((uint)second, destination, 6);

            return true;
        }

        // Roundtrippable format. One of
        //   012345678901234567890123456789012
        //   ---------------------------------
        //   2017-06-12
        internal static bool TryFormatDateOnlyO(int year, int month, int day, Span<char> destination)
        {
            if (destination.Length < 10)
            {
                return false;
            }

            WriteFourDecimalDigits((uint)year, destination, 0);
            destination[4] = '-';
            WriteTwoDecimalDigits((uint)month, destination, 5);
            destination[7] = '-';
            WriteTwoDecimalDigits((uint)day, destination, 8);
            return true;
        }

        // Rfc1123
        //   01234567890123456789012345678
        //   -----------------------------
        //   Tue, 03 Jan 2017
        internal static bool TryFormatDateOnlyR(DayOfWeek dayOfWeek, int year, int month, int day, Span<char> destination)
        {
            if (destination.Length < 16)
            {
                return false;
            }

            string dayAbbrev = InvariantAbbreviatedDayNames[(int)dayOfWeek];
            Debug.Assert(dayAbbrev.Length == 3);

            string monthAbbrev = InvariantAbbreviatedMonthNames[month - 1];
            Debug.Assert(monthAbbrev.Length == 3);

            destination[0] = dayAbbrev[0];
            destination[1] = dayAbbrev[1];
            destination[2] = dayAbbrev[2];
            destination[3] = ',';
            destination[4] = ' ';
            WriteTwoDecimalDigits((uint)day, destination, 5);
            destination[7] = ' ';
            destination[8] = monthAbbrev[0];
            destination[9] = monthAbbrev[1];
            destination[10] = monthAbbrev[2];
            destination[11] = ' ';
            WriteFourDecimalDigits((uint)year, destination, 12);
            return true;
        }

        /// <summary>
        /// Writes a value [ 00 .. 99 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTwoDecimalDigits(uint value, Span<char> destination, int offset)
        {
            Debug.Assert(value <= 99);

            uint temp = '0' + value;
            value /= 10;
            destination[offset + 1] = (char)(temp - (value * 10));
            destination[offset] = (char)('0' + value);
        }

        /// <summary>
        /// Writes a value [ 0000 .. 9999 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteFourDecimalDigits(uint value, Span<char> buffer, int startingIndex)
        {
            Debug.Assert(value <= 9999);

            uint temp = '0' + value;
            value /= 10;
            buffer[startingIndex + 3] = (char)(temp - (value * 10));

            temp = '0' + value;
            value /= 10;
            buffer[startingIndex + 2] = (char)(temp - (value * 10));

            temp = '0' + value;
            value /= 10;
            buffer[startingIndex + 1] = (char)(temp - (value * 10));

            buffer[startingIndex] = (char)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigits(ulong value, Span<char> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (char)('0' + value);
        }
    }
}