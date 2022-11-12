using System;
using System.Buffers;

namespace NCoreUtils;

public static class StringSimplifierExtensions
{
    public static int Simplify(this IStringSimplifier simplifier, ReadOnlySpan<char> source, Span<char> destination)
        => simplifier.TrySimplify(source, destination, out var written)
            ? written
            : throw new ArgumentException("Buffer is insufficient.", nameof(destination));

    public static string Simplify(this IStringSimplifier simplifier, string source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        if (source.Length == 0)
        {
            return string.Empty;
        }
        var maxSize = simplifier.GetMaxCharCount(source.Length);
        string result;
        // NOTE: stack allocation only used when buffer size < 32k.
        if (maxSize <= 16 * 1024)
        {
            Span<char> buffer = stackalloc char[maxSize];
            var written = simplifier.Simplify(source.AsSpan(), buffer);
#if NETSTANDARD2_0
            result = buffer[..written].ToString();
#else
            result = new string(buffer[..written]);
#endif
        }
        else
        {
            var buffer = ArrayPool<char>.Shared.Rent(maxSize);
            try
            {
                var written = simplifier.Simplify(source.AsSpan(), buffer.AsSpan());
                result = new string(buffer, 0, written);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        return result;
    }
}