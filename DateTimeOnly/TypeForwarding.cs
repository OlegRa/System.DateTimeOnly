using System;
using System.Runtime.CompilerServices;

#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(DateOnly))]
[assembly: TypeForwardedTo(typeof(TimeOnly))]
[assembly: TypeForwardedTo(typeof(ISpanFormattable))]
#endif