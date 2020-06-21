using System;
using System.Collections.Generic;

namespace NCoreUtils.Text
{
    [Obsolete("ICharacterSimplifier will be removed in future versions, use IRuneSiplifier instead.")]
    public interface ICharacterSimplifier
    {
        IReadOnlyCollection<char> Keys { get; }

        string this[char key] { get; }
    }
}