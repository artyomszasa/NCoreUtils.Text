using System;

namespace NCoreUtils.Text.Internal
{
    public class LibicuDecomposer : IDecomposer
    {
        private readonly LibicuWrapper _icu;

        public LibicuDecomposer(ILibicu icu)
            => _icu = new LibicuWrapper(icu ?? throw new ArgumentNullException(nameof(icu)));

        public bool TryDecompose(int unicodeScalar, Span<char> decomposition, out int written)
            => _icu.TryDecompose(unicodeScalar, decomposition, out written);
    }
}