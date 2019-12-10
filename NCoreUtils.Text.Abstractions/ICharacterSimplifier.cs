using System.Collections.Generic;

namespace NCoreUtils.Text
{
    public interface ICharacterSimplifier
    {
        IReadOnlyCollection<char> Keys { get; }

        string this[char key] { get; }
    }
}