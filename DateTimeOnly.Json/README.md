[![Build](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml/badge.svg)](https://github.com/OlegRa/System.DateTimeOnly/actions/workflows/release.yml)
[![Codacy Grade](https://app.codacy.com/project/badge/Grade/37aac9b569e347d591f1648ff2793092)](https://app.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Codacy Coverage](https://app.codacy.com/project/badge/Coverage/37aac9b569e347d591f1648ff2793092)](https://app.codacy.com/gh/OlegRa/System.DateTimeOnly/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)
[![PVS-Studio](https://img.shields.io/badge/PVS--Studio-0-blue?logo=opensourceinitiative&logoColor=white&logoWidth=16)](https://pvs-studio.com/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source)
[![Sponsors](https://img.shields.io/github/sponsors/OlegRa?logo=github)](https://github.com/sponsors/OlegRa)

# ![Logo](https://user-images.githubusercontent.com/4800940/174981624-cb9d6acd-ac30-4d46-9118-81425dd4d0fe.png) Portable.System.DateTimeOnly.Json

The [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) NuGet package contains the built-in support for the [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) types serialization/deserialization. Of course, the backported versions of the same types from the [`Portable.System.DateTimeOnly`](https://www.nuget.org/packages/Portable.System.DateTimeOnly/) NuGet package are not supported by default by this library. If you already use the portable versions of these types in your code and want to make them compatible with the [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) you can implement custom converters or use this package for cross-framework compatible approach.

## Reflection-based Serialization

The package provides two concrete implementations of the [`System.Text.Json.Serialization.JsonConverter<T>`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverter-1) class: `System.Text.Json.DateOnlyConverter` and `System.Text.Json.TimeOnlyConverter` for converting [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types. You can use this converter as usual [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) converters: with the [`System.Text.Json.Serialization.JsonConverterAttribute`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute) attribute or [`System.Text.Json.JsonSerializerOptions.Converters`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.converters) property.

The main disadvantage of this "direct" approach for configuring [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types handling by the [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) library is that in this case you'll always use the backported converters even if your code runs on .NET 6 and later. The package also provides two helper attributes `System.Text.Json.DateOnlyConverterAttribute` and `System.Text.Json.TimeOnlyConverterAttribute` for mitigating this problem. These attributes use built-in converters on .NET 6 and later and fall back to the backported one in other cases.

## Source Generators Workaround

Unfortunately, attributes-based approach will not work if you'll try to use [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and/or [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types with the [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) source generators. The only way to make generated code compilable is to use any type names not equal to [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and/or [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) available in object graph processed by the source generator.

The package provides two helper types: `System.Text.Json.JsonDateOnly` and `System.Text.Json.JsonTimeOnly` to solve this problem. These types have no public properties, but they provide implicit conversion operations from/to the [`System.DateOnly`](https://docs.microsoft.com/dotnet/api/system.dateonly) and [`System.TimeOnly`](https://docs.microsoft.com/dotnet/api/system.timeonly) data types respectively. You can use them safely in your JSON-mapping objects with any kind of serialization available in the [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) library. These types have custom [`System.Text.Json.Serialization.JsonConverterAttribute`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute) applied with helper converters that delegate work to `System.Text.Json.DateOnlyConverter` and `System.Text.Json.TimeOnlyConverter` types without code duplication.

## Contributors

Thanks a lot for all contributors. See the full list of project supporters in the [CONTRIBUTORS](https://github.com/OlegRa/System.DateTimeOnly/blob/master/CONTRIBUTORS.md) file.

## SAST Tools

This project uses [PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.
