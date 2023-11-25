using System;

namespace NCoreUtils.Text.Internal;

public class LibicuDecomposer(ILibicu icu) : IDecomposer
{
    private readonly LibicuWrapper _icu = new(icu ?? throw new ArgumentNullException(nameof(icu)));

    public bool TryDecompose(int unicodeScalar, Span<char> decomposition, out int written)
        => _icu.TryDecompose(unicodeScalar, decomposition, out written);
}