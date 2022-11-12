using System;

namespace NCoreUtils.Text.Internal;

public static class LibicuWrapperExtensions
{
    public static unsafe bool TryDecompose(this in LibicuWrapper inst, int value, Span<char> decomposition, out int written)
    {
        var size = inst.Decompose(value, decomposition, out var err);
        if (size > 0 && err.IsSuccess())
        {
            written = size;
            return true;
        }
        if (size <= 0 || err == UErrorCode.BUFFER_OVERFLOW_ERROR)
        {
            written = 0;
            return false;
        }
        throw new LibicuException(err);
    }
}