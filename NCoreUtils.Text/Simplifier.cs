using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NCoreUtils.Text
{
    public class Simplifier : ISimplifier
    {
        public static ISimplifier Default { get; } = new Simplifier('-', CharacterSimplifiers.Russian, CharacterSimplifiers.Hungarian);

        static string GetEscapeSequence(char c) => "\\u" + ((int)c).ToString("X4");

        readonly Regex _delimiters;

        readonly Regex _nonChars;

        readonly ImmutableArray<(Regex regex, ICharacterSimplifier simplifier)> _simplifiableCharacters;

        public char Delimiter { get; }

        public Simplifier(char delimiter, IEnumerable<ICharacterSimplifier> characterSimplifiers)
        {
            if (characterSimplifiers == null)
            {
                throw new ArgumentNullException(nameof(characterSimplifiers));
            }
            Delimiter = delimiter;
            _simplifiableCharacters = characterSimplifiers
                .Select(simplifier => (new Regex($"[{String.Join("|", simplifier.Keys.Select(key => key.ToString()))}]", RegexOptions.Compiled), simplifier))
                .ToImmutableArray();
            var ud = GetEscapeSequence(delimiter);
            _delimiters = new Regex($"{ud}+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            _nonChars = new Regex($"[^a-z0-9{ud}]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public Simplifier(char delimiter, params ICharacterSimplifier[] characterSimplifiers)
            : this(delimiter, (IEnumerable<ICharacterSimplifier>)characterSimplifiers)
        { }

        public string Simplify(string source)
        {
            // convert all language specific convertable characters.
            var chsimplified = _simplifiableCharacters.Aggregate(source.ToLower(CultureInfo.CurrentCulture), (input, s) => s.regex.Replace(input, match => s.simplifier[match.Value[0]]));
            // eliminate all non-converted characters except a-z0-9 + delemiter.
            var onlyChars = _nonChars.Replace(chsimplified, Delimiter.ToString());
            // ensure single delimiter is used and no delimiter is present at the beginning or at the end.
            var trimmed = _delimiters.Replace(onlyChars, Delimiter.ToString()).Trim(Delimiter);
            return trimmed;
        }
    }
}