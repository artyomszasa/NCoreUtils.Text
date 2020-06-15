using System.Collections.Generic;
#if NETCOREAPP3_0 || NETCOREAPP3_1
using System.Text;
#endif

namespace NCoreUtils.Text
{
    public interface IRuneSimplifier
    {
        IReadOnlyCollection<Rune> Keys { get; }

        string this[Rune key] { get; }
    }
}