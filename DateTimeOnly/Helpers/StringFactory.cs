using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics
{
    internal static class StringFactory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Create<TState>(int length, TState state, Buffers.SpanAction<char, TState> action)
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            var destination = new Span<char>(new char[length]);
            action(destination, state);
            return destination.ToString();
#else
            return string.Create(length, state, (destination, state) => action(destination, state));
#endif
        }
    }
}
