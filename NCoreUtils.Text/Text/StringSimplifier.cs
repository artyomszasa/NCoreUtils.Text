using System;
using System.Collections.Generic;
using System.Globalization;
using NCoreUtils.Text.Internal;
#if NETCOREAPP3_0 || NETCOREAPP3_1
using System.Text;
#endif

namespace NCoreUtils.Text
{
    public class StringSimplifier : IStringSimplifier
    {
        private const int CharA = (int)'a';
        private const int CharZ = (int)'z';
        private const int Char0 = (int)'0';
        private const int Char9 = (int)'9';

        private static bool IsSimple(Rune rune)
            => (CharA <= rune.Value && CharZ >= rune.Value) || (Char0 <= rune.Value && Char9 >= rune.Value);

        public static StringSimplifier Default { get; } = new StringSimplifier('-', RuneSimplifiers.Russian, RuneSimplifiers.German);

        private readonly Dictionary<Rune, string> _map;

        public char Delimiter { get; }

        public StringSimplifier(char delimiter, IEnumerable<IRuneSimplifier> runeSimplifiers)
        {
            if (runeSimplifiers is null)
            {
                throw new System.ArgumentNullException(nameof(runeSimplifiers));
            }
            var map = new Dictionary<Rune, string>();
            foreach (var runeSimplifier in runeSimplifiers)
            {
                foreach (var key in runeSimplifier.Keys)
                {
                    map[key] = runeSimplifier[key];
                }
            }
            _map = map;
            Delimiter = delimiter;
        }

        public StringSimplifier(char delimiter, params IRuneSimplifier[] runeSimplifiers)
            : this(delimiter, (IEnumerable<IRuneSimplifier>)runeSimplifiers)
        { }

        private bool TrySimplifyRune(ref SpanBuilder builder, ref bool isDelimiter, Rune rune)
        {
            // exclude non-spacing marks!
            if (Rune.GetUnicodeCategory(rune) != UnicodeCategory.NonSpacingMark)
            {
                var r = Rune.ToLowerInvariant(rune);
                if (!IsSimple(r))
                {
                    if (!isDelimiter)
                    {
                        if (!builder.TryAppend(Delimiter)) { return false; }
                        isDelimiter = true;
                    }
                }
                else
                {
                    if (!builder.TryAppend(r)) { return false; }
                    isDelimiter = false;
                }
            }
            return true;
        }

        /// <summary>
        /// Attempts to write simplified information to the specified span builder.
        /// <para>
        /// Buffer MAY contain trailing delimiter!!
        /// </para>
        /// </summary>
        /// <param name="builder">Span builder to use.</param>
        /// <param name="source">Source.</param>
        /// <returns>
        /// <c>true</c> if span builder had sufficient space to store the simplifed string, <c>false</c> otherwise.
        /// </returns>
        private bool TrySimplify(ref SpanBuilder builder, ReadOnlySpan<char> source)
        {
            Span<char> decomposition = stackalloc char[8];
            var isDelimiter = true;
            var normalizer = new NativeNormalizer(NormalizationForms.FormD);
            foreach (var rune in source.EnumerateRunes())
            {
                if (_map.TryGetValue(rune, out var replacement))
                {
                    foreach (var ch in replacement)
                    {
                        if (!builder.TryAppend(char.ToLowerInvariant(ch))) { return false; }
                    }
                    isDelimiter = false;
                }
                else
                {
                    if (normalizer.TryDecompose(rune.Value, decomposition, out var decompositionLength))
                    {
                        ReadOnlySpan<char> decomposed = decomposition.Slice(0, decompositionLength);
                        foreach (var subrune in decomposed.EnumerateRunes())
                        {
                            if (!TrySimplifyRune(ref builder, ref isDelimiter, subrune)) { return false; }
                        }
                    }
                    else
                    {
                        if (!TrySimplifyRune(ref builder, ref isDelimiter, rune)) { return false; }
                    }
                }
            }
            return true;
        }

        public int GetMaxCharCount(int sourceCharCount)
            => sourceCharCount;

        public bool TrySimplify(ReadOnlySpan<char> source, Span<char> destination, out int written)
        {
            var builder = new SpanBuilder(destination);
            if (TrySimplify(ref builder, source))
            {
                var size = builder.Length;
                while (size != 0 && destination[size - 1] == Delimiter)
                {
                    --size;
                }
                written = size;
                return true;
            }
            written = default;
            return false;
        }
    }
}