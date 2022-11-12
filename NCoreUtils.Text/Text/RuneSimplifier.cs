using System;
using System.Collections.Generic;
#if !NETSTANDARD2_1
using System.Text;
#endif


namespace NCoreUtils.Text;

public static class RuneSimplifier
{
    sealed class ExplicitRuneSimplifier : IRuneSimplifier
    {
        IReadOnlyCollection<Rune> IRuneSimplifier.Keys => Keys;

        public HashSet<Rune> Keys { get; }

        public Dictionary<Rune, string> Mapping { get; }

        public string this[Rune key] => Mapping[key];

        public ExplicitRuneSimplifier(IEnumerable<KeyValuePair<Rune, string>> mapping, bool merge)
        {
            if (merge)
            {
                var dictionary = new Dictionary<Rune, string>();
                foreach (var kv in mapping)
                {
                    dictionary[kv.Key] = kv.Value;
                }
                Mapping = dictionary;
            }
            else
            {
                #if NETSTANDARD2_1
                Mapping = new Dictionary<Rune, string>(mapping);
                #else
                var dictionary = new Dictionary<Rune, string>();
                foreach (var kv in mapping)
                {
                    dictionary.Add(kv.Key, kv.Value);
                }
                Mapping = dictionary;
                #endif
            }
            Keys = new HashSet<Rune>(Mapping.Keys);
        }
    }

    public static IRuneSimplifier FromMapping(IEnumerable<KeyValuePair<Rune, string>> mapping, bool merge)
    {
        if (mapping == null)
        {
            throw new ArgumentNullException(nameof(mapping));
        }
        return new ExplicitRuneSimplifier(mapping, merge);
    }
}