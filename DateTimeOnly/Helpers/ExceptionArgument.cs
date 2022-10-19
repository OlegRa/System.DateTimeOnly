#if !NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;

namespace System
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum ExceptionArgument
    {
        s,
        format
    }
}
#endif