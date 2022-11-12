using System;

namespace NCoreUtils.Text.Internal;

public interface IDecomposer
{
    bool TryDecompose(int unicodeScalar, Span<char> decomposition, out int written);
}