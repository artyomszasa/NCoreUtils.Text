using System;

namespace NCoreUtils
{
    public interface IStringSimplifier
    {
        char Delimiter { get; }

        int GetMaxCharCount(int sourceCharCount);

        bool TrySimplify(ReadOnlySpan<char> source, Span<char> destination, out int written);
    }
}