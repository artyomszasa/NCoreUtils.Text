using System.Collections.Generic;
#if !NETSTANDARD2_1
using System.Text;
#endif

namespace NCoreUtils.Text;

public interface IRuneSimplifier
{
    IReadOnlyCollection<Rune> Keys { get; }

    string this[Rune key] { get; }
}