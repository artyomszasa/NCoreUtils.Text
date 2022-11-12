using System;

namespace NCoreUtils;

public interface INamingConvention
{
    int GetMaxCharCount(int sourceCharCount);

    bool TryApply(ReadOnlySpan<char> source, Span<char> destination, out int written);
}