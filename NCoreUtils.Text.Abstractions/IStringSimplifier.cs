using System;

namespace NCoreUtils.Text
{
    public interface IStringSimplifier
    {
        char Delimiter { get; }

        int GetMaxCharCount(int sourceCharCount);

        bool TrySimplify(ReadOnlySpan<char> source, Span<char> destination, out int written);
    }
}