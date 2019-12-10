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

        static bool IsSimple(char ch)
            => ('a' <= ch && 'z' >= ch) || ('0' <= ch && '9' >= ch);

        readonly ImmutableDictionary<char, string> _map;

        public char Delimiter { get; }

        public Simplifier(char delimiter, IEnumerable<ICharacterSimplifier> characterSimplifiers)
        {
            if (characterSimplifiers == null)
            {
                throw new ArgumentNullException(nameof(characterSimplifiers));
            }
            var mapBuilder = ImmutableDictionary.CreateBuilder<char, string>();
            foreach (var characterSimplifier in characterSimplifiers)
            {
                foreach (var key in characterSimplifier.Keys)
                {
                    mapBuilder[key] = characterSimplifier[key];
                }
            }
            _map = mapBuilder.ToImmutable();
            Delimiter = delimiter;
        }

        public Simplifier(char delimiter, params ICharacterSimplifier[] characterSimplifiers)
            : this(delimiter, (IEnumerable<ICharacterSimplifier>)characterSimplifiers)
        { }

        void Simplify(ref SpanBuilder builder, string source)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var src = source.AsSpan();
            var isDelimiter = false;
            var srcLength = src.Length;
            var lastIndex = srcLength - 1;
            for (var i = 0; i < srcLength; ++i)
            {
                var ch = currentCulture.TextInfo.ToLower(src[i]);
                if (_map.TryGetValue(ch, out var replacement))
                {
                    builder.Append(replacement);
                    isDelimiter = false;
                }
                else
                {
                    if (!IsSimple(ch))
                    {
                        ch = Delimiter;
                    }
                    if (Delimiter == ch)
                    {
                        if (builder.Length != 0 && i < lastIndex && !isDelimiter)
                        {
                            builder.Append(ch);
                            isDelimiter = true;
                        }
                    }
                    else
                    {
                        builder.Append(ch);
                        isDelimiter = false;
                    }
                }
            }
        }

        public string Simplify(string source)
        {
            if (source.Length <= 4096)
            {
                Span<char> buffer = stackalloc char[8192];
                SpanBuilder builder = new SpanBuilder(buffer);
                Simplify(ref builder, source);
                var length = builder.Length;
                while (length > 0 && buffer[length - 1] == Delimiter)
                {
                    --length;
                }
                return buffer.Slice(0, length).ToString();
            }
            {
                Span<char> buffer = new char[source.Length * 2];
                SpanBuilder builder = new SpanBuilder(buffer);
                Simplify(ref builder, source);
                var length = builder.Length;
                while (length > 0 && buffer[length - 1] == Delimiter)
                {
                    --length;
                }
                return buffer.Slice(0, length).ToString();
            }
        }
    }
}