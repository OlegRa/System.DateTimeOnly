[![Build and Release](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml/badge.svg)](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml)
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-1-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/37aac9b569e347d591f1648ff2793092)](https://www.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=github.com&amp)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/37aac9b569e347d591f1648ff2793092)](https://www.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=github.com)
[![Nuget](https://img.shields.io/nuget/v/Portable.System.DateTimeOnly?logo=NuGet)](https://www.nuget.org/packages/Portable.System.DateTimeOnly)
[![Nuget](https://img.shields.io/nuget/dt/Portable.System.DateTimeOnly?logo=NuGet)](https://www.nuget.org/stats/packages/Portable.System.DateTimeOnly?groupby=Version)
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-13-orange.svg?style=flat-square)](#contributors)
<!-- ALL-CONTRIBUTORS-BADGE:END -->

# ![Logo](https://user-images.githubusercontent.com/4800940/174981624-cb9d6acd-ac30-4d46-9118-81425dd4d0fe.png) Portable.System.DateTimeOnly

The .NET 6 introduced two new data types [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) that can be used in various scenarios. If you want to use the same types in the .NET versions below to .NET 6 you can use this library in form of the NuGet package. You can also use it in your own NuGet package if you want to provide an API that uses these types and make it compatible with all currently supported .NET versions at the same time.

## String Parsing Performance

The original version of this package used the simplified string parsing approach - it just reuses the default [`System.DateTime`](https://docs.microsoft.com/dotnet/api/system.datetime) parsing logic as is. It works well in most use cases but parsing behavior was not matched 100% to the .NET 6 [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types. Their raise the [`System.FormatException`](https://docs.microsoft.com/dotnet/api/system.formatexception) exception in case if the input string contains the time part for the [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) or the date part in the case of [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) types.

Unfortunately, this behavior in .NET 6 implementation depends heavily on an internal parser that provides a lot more information about parsing results than a public version of parsing API. Porting this parser required porting a lot of culture-specific code and looks unpractical for such a simple package. So another approach selected for solving this issue - double parsing using [`System.DateTime.TryParseExact`](https://docs.microsoft.com/dotnet/api/system.datetime.tryparseexact) method with proper formatting strings.

This approach, of course, has an obvious disadvantage - in some cases, we have to parse the same string twice. If performance is critical for you and you are OK with the _incorrect_ parsing results in some rare cases you can restore the original behavior using the application context switch named `Portable.System.DateTimeOnly.UseFastParsingLogic`. Check the [`FastStrictParsingLogicTests`](https://github.com/OlegRa/System.DateTimeOnly/blob/master/DateTimeOnly.Tests/FastStrictParsingLogicTests.cs) for more examples for this issue.

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center"><a href="https://github.com/Chryssie"><img src="https://avatars.githubusercontent.com/u/10442662?v=4?s=100" width="100px;" alt="Chryssie Ta"/><br /><sub><b>Chryssie Ta</b></sub></a><br /><a href="https://github.com/OlegRa/System.DateTimeOnly/commits?author=Chryssie" title="Code">💻</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->
