using System;
using System.Collections.Generic;
using System.Linq;
#if NETCOREAPP3_0 || NETCOREAPP3_1
using System.Text;
#endif


namespace NCoreUtils.Text
{
    [Obsolete("Simplifier will be removed in future versions, use StringSiplifier instead.")]
    public class Simplifier : ISimplifier
    {
        public static ISimplifier Default { get; } = new Simplifier('-', CharacterSimplifiers.Russian, CharacterSimplifiers.Hungarian);

        private static IRuneSimplifier ToRuneSimplifier(ICharacterSimplifier characterSimplifier)
        {
            var kvs = new List<KeyValuePair<Rune, string>>(characterSimplifier.Keys.Count);
            foreach (var k in characterSimplifier.Keys)
            {
                kvs.Add(new KeyValuePair<Rune, string>(new Rune(k), characterSimplifier[k]));
            }
            return RuneSimplifier.FromMapping(kvs);
        }

        private readonly StringSimplifier _simplifier;

        public char Delimiter => _simplifier.Delimiter;

        public Simplifier(char delimiter, IEnumerable<ICharacterSimplifier> characterSimplifiers)
        {
            _simplifier = new StringSimplifier(delimiter, characterSimplifiers.Select(ToRuneSimplifier));
        }

        public Simplifier(char delimiter, params ICharacterSimplifier[] characterSimplifiers)
            : this(delimiter, (IEnumerable<ICharacterSimplifier>)characterSimplifiers)
        { }

        public string Simplify(string source)
            => _simplifier.Simplify(source);
    }
}