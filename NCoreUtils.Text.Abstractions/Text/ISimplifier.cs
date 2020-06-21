using System;

namespace NCoreUtils.Text
{
    [Obsolete("ISimplifier will be removed in future versions, use IStringSiplifier instead.")]
    public interface ISimplifier
    {
        char Delimiter { get; }

        string Simplify(string source);
    }
}