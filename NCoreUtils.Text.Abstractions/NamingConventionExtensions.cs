using System;
using System.Buffers;

namespace NCoreUtils;

public static class NamingConventionExtensions
{
    public static int Apply(this INamingConvention convention, ReadOnlySpan<char> source, Span<char> destination)
        => convention.TryApply(source, destination, out var written)
            ? written
            : throw new ArgumentException("Buffer is insufficient.", nameof(destination));

    public static string Apply(this INamingConvention convention, string source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(source);
#else
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif
        if (source.Length == 0)
        {
            return string.Empty;
        }
        var maxSize = convention.GetMaxCharCount(source.Length);
        string result;
        // NOTE: stack allocation only used when buffer size < 16k.
        if (maxSize <= 16 * 1024)
        {
            Span<char> buffer = stackalloc char[maxSize];
            var written = convention.Apply(source.AsSpan(), buffer);
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
                var written = convention.Apply(source.AsSpan(), buffer.AsSpan());
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