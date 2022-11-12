using System;
#if !NETSTANDARD2_1
using System.Text;
#endif

namespace NCoreUtils.Text;

public static class SpanBuilderRuneExtensions
{
    public static bool TryAppend(this ref SpanBuilder builder, Rune rune)
    {
        Span<char> buffer = stackalloc char[4];
        var size = rune.EncodeToUtf16(buffer);
        return builder.TryAppend(buffer[..size]);
    }
}