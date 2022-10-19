// ReSharper disable once CheckNamespace

#if !NET6_0_OR_GREATER
namespace System.Buffers
{
    internal delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);
}
#endif