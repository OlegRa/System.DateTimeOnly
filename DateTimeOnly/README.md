[![Build](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml/badge.svg)](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml)
[![Codacy Grade](https://app.codacy.com/project/badge/Grade/37aac9b569e347d591f1648ff2793092)](https://app.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Codacy Coverage](https://app.codacy.com/project/badge/Coverage/37aac9b569e347d591f1648ff2793092)](https://app.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)
[![PVS-Studio](https://img.shields.io/badge/PVS--Studio-0-blue?logo=opensourceinitiative&logoColor=white&logoWidth=16)](https://pvs-studio.com/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source)
[![Sponsors](https://img.shields.io/github/sponsors/OlegRa?logo=github)](https://github.com/sponsors/OlegRa)

# ![Logo](https://user-images.githubusercontent.com/4800940/174981624-cb9d6acd-ac30-4d46-9118-81425dd4d0fe.png) Portable.System.DateTimeOnly

The .NET 6 introduced two new data types [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) that can be used in various scenarios. If you want to use the same types in the .NET versions below to .NET 6 you can use this library in form of the NuGet package. You can also use it in your own NuGet package if you want to provide an API that uses these types and make it compatible with all currently supported .NET versions at the same time.

## String Parsing Performance

The original version of this package used the simplified string parsing approach - it just reuses the default [`System.DateTime`](https://docs.microsoft.com/dotnet/api/system.datetime) parsing logic as is. It works well in most use cases but parsing behavior was not matched 100% to the .NET 6 [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types. Their raise the [`System.FormatException`](https://docs.microsoft.com/dotnet/api/system.formatexception) exception in case if the input string contains the time part for the [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) or the date part in the case of [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) types.

Unfortunately, this behavior in .NET 6 implementation depends heavily on an internal parser that provides a lot more information about parsing results than a public version of parsing API. Porting this parser required porting a lot of culture-specific code and looks unpractical for such a simple package. So another approach selected for solving this issue - double parsing using [`System.DateTime.TryParseExact`](https://docs.microsoft.com/dotnet/api/system.datetime.tryparseexact) method with proper formatting strings.

This approach, of course, has an obvious disadvantage - in some cases, we have to parse the same string twice. If performance is critical for you and you are OK with the _incorrect_ parsing results in some rare cases you can restore the original behavior using the application context switch named `Portable.System.DateTimeOnly.UseFastParsingLogic`. Check the [`FastStrictParsingLogicTests`](https://github.com/OlegRa/System.DateTimeOnly/blob/master/DateTimeOnly.Tests/FastStrictParsingLogicTests.cs) for more examples for this issue.

## The `System.Text.Json` Support

You can use helper NuGet package [`Portable.System.DateTimeOnly.Json`](https://www.nuget.org/packages/Portable.System.DateTimeOnly.Json) with the [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) for enabling cross-framework JSON serialization of [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types in your code.

## Contributors

Thanks a lot for all contributors. See the full list of project supporters in the [CONTRIBUTORS](https://github.com/OlegRa/System.DateTimeOnly/blob/master/CONTRIBUTORS.md) file.

## SAST Tools

This project uses [PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.
