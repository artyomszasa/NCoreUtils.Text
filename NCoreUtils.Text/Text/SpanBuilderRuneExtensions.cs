using System;
#if !NETSTANDARD2_1
using System.Text;
#endif

namespace NCoreUtils.Text
{
    internal static class SpanBuilderRuneExtensions
    {
        public static bool TryAppend(this ref SpanBuilder builder, Rune rune)
        {
            Span<char> buffer = stackalloc char[4];
            var size = rune.EncodeToUtf16(buffer);
            ReadOnlySpan<char> slice = buffer.Slice(0, size);
            return builder.TryAppend(slice);
        }
    }
}