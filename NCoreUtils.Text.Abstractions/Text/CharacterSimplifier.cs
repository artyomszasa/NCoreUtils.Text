using System;
using System.Collections.Generic;

namespace NCoreUtils.Text
{
    [Obsolete("CharacterSimplifier will be removed in future versions, use RuneSiplifier instead.")]
    public static class CharacterSimplifier
    {
        private sealed class ExplicitCharacterSimplifier : ICharacterSimplifier
        {
            IReadOnlyCollection<char> ICharacterSimplifier.Keys => Keys;

            public HashSet<char> Keys { get; }

            public Dictionary<char, string> Mapping { get; }

            public string this[char key] => Mapping[key];

            public ExplicitCharacterSimplifier(IEnumerable<KeyValuePair<char, string>> mapping)
            {
                #if NETSTANDARD2_1
                Mapping = new Dictionary<char, string>(mapping);
                #else
                var dictionary = new Dictionary<char, string>();
                foreach (var kv in mapping)
                {
                    dictionary.Add(kv.Key, kv.Value);
                }
                Mapping = dictionary;
                #endif
                Keys = new HashSet<char>(Mapping.Keys);
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
