using System.Collections.Generic;
using System.Collections.Immutable;

namespace NCoreUtils.Text
{
    public static class CharacterSimplifier
    {
        sealed class ExplicitCharacterSimplifier : ICharacterSimplifier
        {
            IReadOnlyCollection<char> ICharacterSimplifier.Keys => Keys;

            public ImmutableHashSet<char> Keys { get; }

            public ImmutableDictionary<char, string> Mapping { get; }

            public string this[char key] => Mapping[key];

            public ExplicitCharacterSimplifier(IEnumerable<KeyValuePair<char, string>> mapping)
            {
                Mapping = ImmutableDictionary.CreateRange(mapping);
                Keys = Mapping.Keys.ToImmutableHashSet();
            }
        }

        public static ICharacterSimplifier FromMapping(IEnumerable<KeyValuePair<char, string>> mapping)
        {
            if (mapping == null)
            {
                throw new System.ArgumentNullException(nameof(mapping));
            }
            return new ExplicitCharacterSimplifier(mapping);
        }
    }
}